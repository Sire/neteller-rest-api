#region Using
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using Neteller.API.Model;
using Newtonsoft.Json.Linq;
using NLog;
using NUnit.Framework;
using RestSharp;

#endregion

namespace Neteller.API.Tests
{
	[TestFixture]
	[Category("DontRunOnTeamcity")]
	class NetellerApiTests
	{
		RestAPI api = null;
		private static Logger logger = LogManager.GetCurrentClassLogger();

		[SetUp]
		public void SetUp()
		{
			api = new RestAPI(NetellerEnvironment.Test);

			//always use same culture to avoid formatting differences
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
		}

		[Test]
		public void GetAccessTokenTest()
		{
			var token = api.GetAccessToken();
			Console.WriteLine(token);
			Assert.That(token.ToLower(), Is.Not.StringContaining("error"));
			Assert.That(token.Length, Is.GreaterThanOrEqualTo(30));
		}

		[Test]
		public void GetAllSubscriptions()
		{
			//note: throws an exception if no subscriptions exist, by design of Neteller.

			NetellerEnvironment[] environments = new[] { NetellerEnvironment.Test, NetellerEnvironment.Production };
			foreach (var environment in environments)
			{

				api = new RestAPI(environment);

				Console.WriteLine("Subscriptions in " + environment);
				Console.WriteLine("===============================");


				var subs = api.GetSubscriptions();
				foreach (var subscription in subs.list.ToList())
				{

					//with resource expansion (not supported by Neteller on this function yet!)
					//string customerId = subscription.Customer.id;
					//string planId = subscription.Plan.planId;

					//wihtout resource expansion
					string customerId = subscription.Customer.link.url.Substring(subscription.Customer.link.url.LastIndexOf("/") + 1);
					string planId = subscription.Plan.link.url.Substring(subscription.Plan.link.url.LastIndexOf("/") + 1);

					//Customer customer = api.GetCustomerFromCustomerId(customerId);
					//string customerInfo = string.Format("Email {0} Name {1} {2}", customer.accountProfile.email, customer.accountProfile.firstName, customer.accountProfile.lastName);
					string customerInfo = string.Format("ID {0}", customerId);

					Console.WriteLine("Subscription ID {0,-3} Status {1} Start {2} End {3} Customer {4} PlanID: {5}",
						subscription.SubscriptionId,
						subscription.Status,
						subscription.StartDate,
						subscription.EndDate,
						customerInfo,
						planId
					);
				}

				Assert.That(subs.list, Is.Not.Empty);

			}
		}

		[Test]
		public void GetAllSubscriptionsPlans() {

			NetellerEnvironment[] environments = new[] { NetellerEnvironment.Test, NetellerEnvironment.Production };

			foreach (var environment in environments) {

				api = new RestAPI(environment);

				Console.WriteLine("Plans in " + environment);
				Console.WriteLine("===============================");

				Plans plans = api.GetPlans();

				plans.list.Sort((list, planList) => { return list.planName.CompareTo(planList.planName); });

				foreach (var list in plans.list.ToList()) {
					Console.WriteLine("PlanID {0}. Status: {7}. Amount {2} {3} Interval {4} count {5} type {6}. Name: {1}",
					list.planId,
					list.planName,
					list.amount / 100,
					list.currency,
					list.interval,
					list.intervalCount,
					list.intervalType,
					list.status
					);
				}
			
				Assert.That(plans.list, Is.Not.Empty);

			}
			
		}

		[Test]
		public void CreateSubscriptionPlanThenDelete()
		{
			string testPlanID = "TestPlanForUnitTestDeleteMe";
			var plan = new Plan()
			{
				planId = testPlanID,
				planName = "Test plan for Unit Test. Should be deleted.",
				interval = 1, intervalType = IntervalType.Weekly, intervalCount = 1,
				amount = 100, currency = "EUR"
			};
			var plan1 = CreatePlan(plan);
			Assert.That(plan1.planId, Is.EqualTo(testPlanID));
			var plan2 = api.GetPlan(testPlanID);
			Assert.That(plan1.planId, Is.EqualTo(plan2.planId));

			DeletePlan(testPlanID);
		}

		private Plan CreatePlan(Plan plan) {
			var plan1 = api.CreatePlan(plan);
			Assert.That(plan1.status, Is.EqualTo(SubscriptionStatus.Active));
			return plan1;
		}

		private void DeletePlan(string planId) {
			var success = api.DeletePlan(planId);
			Assert.That(success, Is.True);
		}


		[Test]
		public void CreateSubscription()
		{
			string testPlanID = "TestPlanForUnitTest";
			bool planExists = false;
			try
			{
				var plan = api.GetPlan(testPlanID);
				planExists = true;
			}
			catch (NetellerException ex) {
				if (!ex.Message.Contains("NotFound"))
					throw;
				Console.WriteLine("Plan already exist");
			}

			if (!planExists)
			{
				Console.WriteLine("Creating plan");
				var plan = new Plan()
				{
					planId = testPlanID,
					planName = "Test plan for Unit Test.",
					interval = 1,
					intervalType = IntervalType.Weekly,
					intervalCount = 1,
					amount = 100,
					currency = "EUR"
				};
				plan = CreatePlan(plan);
				Assert.That(plan.status, Is.EqualTo(SubscriptionStatus.Active));
			}

			//FIXME: create payment to a subscription
			//no way to create this non-interactively at the moment :(

			//Console.WriteLine("Creating subscription");

			//NewSubscription newSub = new NewSubscription()
			//{
			//	accountProfile = new AccountProfileLite() { email = "TEST" },
			//	planId = testPlanID,
			//	startDate = string.Empty //leave empty (not null) to start immediately!
			//};

			//api.CreateSubscription(newSub, accessToken);
		}




		[Test] [Ignore("Mock api.GetPlan!")]
		public void GetPlan()
		{
			Plan plan = api.GetPlan("plan that exists");
			Assert.That(plan, Is.Not.Null);
			Assert.That(plan.status, Is.EqualTo(SubscriptionStatus.Active));
			Assert.That(plan.amount, Is.EqualTo(100));
		}

		[Test]
		public void GetNonExistingPlan() {
			NetellerException exception = Assert.Throws<NetellerException>(() =>
			{
				api.GetPlan("non-existing-plan");
			});
			Assert.That(exception.Response, Is.Not.Null);
			Assert.That(exception.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

			Plan plan = ((RestResponse<Plan>)(exception.Response)).Data;
			Assert.That(plan, Is.Not.Null);
			Assert.That(plan.planId, Is.Null);
		}



		//FIXME: 
		//Added ability to list subscriptions and subscription plans
		//Added invoice lookup
		//Added webhooks
		//Added OAUTH2 authorization grant
		//Added account, balance resources
		//Added account lookup functionality
		//Added new scopes: subscription_payment, account_enhanced_profile, account_contacts,
		//account_balance
		//Modified create subscription to require scope: subscription_payment
		//>> Added Quick Signup functionality



		[Test]
		public void AuthenticateWithOAuthNeteller()
		{

			string EncodedAuthString = RestAPI.GetEncodedAuthString(api.ClientID, api.ClientSecret);
			var client = new RestClient(api.Endpoint);
			var request = new RestRequest("v1/oauth2/token", Method.POST);

			//should be better ways of sending basic auth, like oauth v1:
			//client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
			request.AddHeader("Authorization", "Basic " + EncodedAuthString);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Cache-Control", "no-cache");
			request.AddParameter("grant_type", "client_credentials");

			var response = client.Execute(request);

			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

			string result = response.Content;
			if (
				result.Contains("error") ||
				result.Contains("invalid_request") ||
				result.Contains("invalid_client") ||
				result.Contains("invalid_grant") ||
				result.Contains("invalid") ||
				result.Contains("unauthorized_client") ||
				result.Contains("unsupported_grant_type") ||
				result.Contains("invalid_scope")
			)
			{
				//logger.Fatal("Error getting access token from Neteller: " + result);
				throw new Exception("failed");
			}

			dynamic obj = JObject.Parse(response.Content);
			string accessToken = (string)obj["accessToken"];
			Assert.NotNull(accessToken);
		}


	}
}