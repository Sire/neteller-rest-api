#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Neteller.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NLog;
using RestSharp;

#endregion

namespace Neteller.API
{

	public class RestAPI
	{

		static string EndpointProduction = "https://api.neteller.com";
		static string EndpointTest = "https://test.api.neteller.com";

		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

		private NetellerEnvironment _netellerEnvironment;
		public NetellerEnvironment NetellerEnvironment { get { return _netellerEnvironment; } }

		public string EncodedAuthString { get; private set; }

		public RestAPI(NetellerEnvironment e)
		{
			_netellerEnvironment = e;
			try
			{
				EncodedAuthString = GetEncodedAuthString(ClientID, ClientSecret);
			}
			catch (Exception ex) {
				throw new Exception("Could not initialize Neteller API. Missing ClientID and ClientSecret, check Neteller.config file and create it from Neteller.config.sample if missing. Exception: " + ex.Message);
			}
		}

		public static string GetEncodedAuthString(string clientId, string clientSecret) {
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));
		}

		public string Endpoint {
			get {
				if (_netellerEnvironment == NetellerEnvironment.Production)
					return EndpointProduction;
				else
					return EndpointTest;
			}
		}

		public string RedirectUrl
		{
			get
			{
				if (_netellerEnvironment == NetellerEnvironment.Production)
					return Configuration.RedirectUrl;
				else
					return Configuration.RedirectUrlTest;
			}
		}

		public string ClientID {
			get {
				if (_netellerEnvironment == NetellerEnvironment.Production)
					return Configuration.ClientID;
				else
					return Configuration.ClientIDTest;
			}
		}

		public string ClientSecret
		{
			get
			{
				if (_netellerEnvironment == NetellerEnvironment.Production)
					return Configuration.ClientSecret;
				else
					return Configuration.ClientSecretTest;
			}
		}

		/// <summary>
		/// Execute a Requst using default accessToken
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="request"></param>
		/// <returns></returns>
		private T Execute<T>(RestRequest request) where T : new() {
			return Execute<T>(request, GetAccessToken());
		}


		/// <summary>
		/// Execute a request
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="request"></param>
		/// <param name="accessToken">Use the specified accessToken</param>
		/// <returns></returns>
		private T Execute<T>(RestRequest request, string accessToken) where T : new()
		{
			var client = new RestClient(this.Endpoint);

			request.AddHeader("Content-Type", "application/json");
			//charset doesn't seem to be needed
			//request.AddHeader("Content-Type", "application/json;charset=UTF-8");

			request.RequestFormat = DataFormat.Json;

			//all dates from Neteller are formatted using this format, and in UTC
			//will turn all parsed dates into LOCAL TIME, adjusting the hours accordingly.
			request.DateFormat = Format.DateTimeUTC;

			client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "Bearer");

			//log the entire request in XML format.
			logger.Debug(GetRequestDescription(request));
			logger.Debug("Using accessToken " + accessToken);

			var response = client.Execute<T>(request);

			logger.Debug(string.Format("NetellerAPI response: {0} {1}\n{2}", request.Method, request.Resource, response.Content));

			if (response.StatusCode != HttpStatusCode.OK) {

				string netellerMsg = response.Content.Replace("\n", "").Replace("  ", " ");
				try
				{
					// error/message
					netellerMsg = GetErrorMessage(response.Content);
				}
				catch { } //if message is missing, send the entire content

				string message = string.Format("NetellerAPI {0} {1} response failed with status {2}. Message:\n{3}",
					request.Method, request.Resource, response.StatusCode, netellerMsg);
				logger.Error(message); 
				throw new NetellerException(message, response);
			}

			if (response.ErrorException != null)
			{
				const string message = "Error retrieving response.  Check inner details for more info.";
				logger.Fatal(message + "\n" + response.ErrorException.ToString());
				throw new NetellerException(message, response.ErrorException, response);
			}


			return response.Data;
		}

		private string GetRequestDescription(RestRequest request) {
			string parameterString = string.Join("\n", request.Parameters.Select(p => p.Name + "=" + p.Value));
			return string.Format("Neteller request: {0}\nParameters: {1}", request.Resource, parameterString);
		}

		private string GetErrorMessage(string content) {
			dynamic parsedJson = JsonConvert.DeserializeObject(content);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}


		/// <summary>
		/// Get a specific plan
		/// </summary>
		/// <param name="planId"></param>
		/// <returns></returns>
		public Plan GetPlan(string planId)
		{
			var request = new RestRequest("v1/plans/" + planId, Method.GET);
			Plan response = Execute<Plan>(request);
			return response;
		}

		/// <summary>
		/// Get all plans (max 100)
		/// </summary>
		/// <returns></returns>
		public Plans GetPlans()
		{
			var request = new RestRequest("v1/plans/", Method.GET);
			request.AddParameter("limit", "100"); //default 10, max 100
			//FIXME: request.AddParameter("offset", "100"); //to fetch next set of plans
			Plans response = Execute<Plans>(request);
			return response;
		}

		public Plan CreatePlan(Plan plan)
		{
			var request = new RestRequest("v1/plans", Method.POST);
			//note: requestFormat must be set BEFORE addBody otherwise resulting object will be serialized to xml per default
			request.RequestFormat = DataFormat.Json;
			request.AddBody(plan);
			Plan response = Execute<Plan>(request);
			return response;
		}

		public bool DeletePlan(string testPlanID)
		{
			var request = new RestRequest("v1/plans/" + testPlanID, Method.DELETE);
			var response = Execute<object>(request);
			return true;
		}


		public SubscriptionThin CreateSubscription(NewSubscription newSub, string accessToken)
		{

			var request = new RestRequest("v1/subscriptions", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddBody(newSub);
			//send in specified accessToken that has access to creating subscription on customer account
			SubscriptionThin response = Execute<SubscriptionThin>(request, accessToken);
			return response;
		}

		/// <summary>
		/// Get a subscription. Preloading Customer and Plan objects (resource expansion)
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public Subscription GetSubscription(int subscriptionId)
		{
			return GetSubscription<Subscription>(subscriptionId, true);
		}

		/// <summary>
		/// Get a subscription without preloading Customer and Plan objects (no resource expansion)
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public SubscriptionThin GetSubscriptionThin(int subscriptionId)
		{
			return GetSubscription<SubscriptionThin>(subscriptionId, false);
		}

		private T GetSubscription<T>(int subscriptionId, bool resourceExpansion) where T : new()
		{
			var request = new RestRequest("v1/subscriptions/" + subscriptionId, Method.GET);
			if (resourceExpansion)
				request.AddParameter("expand", Plan.ResourceName + "," + Customer.ResourceName);
			T response = Execute<T>(request);
			return response;
		}

		public Subscriptions GetSubscriptions()
		{
			var request = new RestRequest("v1/subscriptions", Method.GET);
			request.AddParameter("limit", "100"); //default 10, max 100
			//FIXME: request.AddParameter("offset", "100"); //to fetch next set of plans

			//Turn on Resource Expansion by default when supported by Neteller
			//request.AddParameter("expand", Plan.ResourceName + "," + Customer.ResourceName);

			Subscriptions response = Execute<Subscriptions>(request);
			return response;
		}

		public Invoices GetInvoices(int subscriptionId)
		{
			//optional parameters: limit, offset
			var request = new RestRequest(string.Format("v1/subscriptions/{0}/invoices", subscriptionId), Method.GET);
			Invoices response = Execute<Invoices>(request);
			return response;
		}

		public Invoice GetInvoice(int subscriptionId, int invoiceId) 
		{
			var request = new RestRequest(string.Format("v1/subscriptions/{0}/invoices/{1}", subscriptionId, invoiceId), Method.GET);
			Invoice response = Execute<Invoice>(request);
			return response;
		}

		/// <summary>
		/// Get a payment using merchants' reference Id
		/// This only applies to manually created Payments as of now, not for Subscriptions.
		/// </summary>
		/// <param name="merchantRefId"></param>
		/// <returns></returns>
		public Payment GetPayment(int merchantRefId)
		{
			var request = new RestRequest("v1/payments/" + merchantRefId, Method.GET);
			request.AddParameter("refType", "merchantRefId");
			Payment response = Execute<Payment>(request);
			return response;
		}

		/// <summary>
		/// Get a payment using Neteller payment Id
		/// </summary>
		/// <param name="paymentId"></param>
		/// <returns></returns>
		public Payment GetPaymentWithNetellerId(string paymentId)
		{
			var request = new RestRequest("v1/payments/" + paymentId, Method.GET);
			Payment response = Execute<Payment>(request);
			return response;
		}

		//doesn't exist (yet?)
		//public Transaction GetTransaction(string transactionId)
		//{
		//	var request = new RestRequest("v1/transactions/" + transactionId, Method.GET);
		//	Transaction response = Execute<Transaction>(request);
		//	return response;
		//}

		public SubscriptionThin CancelSubscription(int subscriptionId)
		{
			//POST  {subscriptionId}/cancel https://api.neteller.com/v1/subscriptions/
			//optional parameter: ?cancelAtPeriodEnd=true
			var request = new RestRequest("v1/subscriptions/" + subscriptionId + "/cancel", Method.POST);
			SubscriptionThin response = Execute<SubscriptionThin>(request);
			return response;
		}
		
		public Customer GetCustomerFromEmail(string email)
		{
			var request = new RestRequest("v1/customers/", Method.GET);
			request.AddParameter("email", email);
			Customer response = Execute<Customer>(request);
			return response;
		}
		public Customer GetCustomerFromCustomerId(string customerId)
		{
			var request = new RestRequest("v1/customers/" + customerId, Method.GET);
			Customer response = Execute<Customer>(request);
			return response;
		}
		public Customer GetCustomerFromAccountId(string accountId)
		{
			var request = new RestRequest("v1/customers/", Method.GET);
			request.AddParameter("accountId", accountId);
			Customer response = Execute<Customer>(request);
			return response;
		}

		/// <summary>
		/// Get a new client_credentials access token
		/// </summary>
		/// <returns></returns>
		public string GetAccessToken()
		{
			string na;
			return GetAccessToken(GrantType.client_credentials, null, out na);
		}

		/// <summary>
		/// Get a new access token (not expecting a refresh token)
		/// </summary>
		/// <param name="grantType"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public string GetAccessToken(GrantType grantType, string token)
		{
			string na;
			return GetAccessToken(grantType, token, out na);
		}

		/// <summary>
		/// Get a new access token (and possibly refresh token)
		/// </summary>
		public string GetAccessToken(GrantType grantType, string token, out string refreshToken) 
		{
			try
			{
				var client = new RestClient(this.Endpoint);
				var request = new RestRequest("v1/oauth2/token", Method.POST);
				refreshToken = "";

				//there should be an easier way of sending basic auth? similar to oauth v1:
				//client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);

				request.AddHeader("Authorization", "Basic " + EncodedAuthString);

				request.AddHeader("Content-Type", "application/json");
				request.AddHeader("Cache-Control", "no-cache");

				switch (grantType) {
					case GrantType.client_credentials: 
						request.AddParameter("grant_type", "client_credentials");
						break;
					case GrantType.refresh_token:
						request.AddParameter("grant_type", "refresh_token");
						request.AddParameter("refresh_token", token);
						break;
					case GrantType.authorization_code:
						request.AddParameter("grant_type", "authorization_code");
						request.AddParameter("code", token);
						request.AddParameter("redirect_url", RedirectUrl);
						break;
				}

				var response = client.Execute(request);

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
					string msg = "Error getting access token from Neteller: " + response.Content;
					logger.Fatal(msg);
					throw new NetellerException(msg, response);
				}

				return GetAccessTokenAndRefreshToken(response.Content, out refreshToken);

			}
			catch (Exception ex) {
				logger.Fatal("Error getting access token from Neteller: " + ex.ToString());
				throw;
			}
		}

		private string GetAccessToken(string content) {
			dynamic obj = JObject.Parse(content);
			string accessToken = (string) obj["accessToken"];
			return accessToken;
		}

		private string GetAccessTokenAndRefreshToken(string content, out string refreshToken)
		{
			dynamic obj = JObject.Parse(content);
			if (obj["refreshToken"] != null)
				refreshToken = (string)obj["refreshToken"];
			else {
				refreshToken = "";
			}
			return (string)obj["accessToken"];
		}


		/// <summary>
		/// Returns a URL used for authenticating the Neteller customer.
		/// If successful, Neteller will then post to RedirectUrl.
		/// </summary>
		/// <param name="paymentTransactionId">Merchant (not Neteller) internal payment/transaction ID for reference</param>
		/// <param name="lang">The language code, see Languages.cs</param>
		/// <returns>URL</returns>
		public string GetAuthenticationUrl(int paymentTransactionId, string lang) {
			//The flow consists of the following steps:
			//The client app requests authorization by redirecting the user to the NETELLER OAuth Dialog. 
			//The member must verify their identity via a hosted authentication page.
			//The member is prompted to authorize the application.  The requested scope (permissions) are displayed and the member must Allow or Deny the authorization.
			//The auth_code is returned if the member authorized the requested scope.  This code will have a limited lifetime for you to complete the callback to redeem the access_token.
			//Now you need to authorize your app.  Call the OAUTH2 token api with a grant type of 'authorization_code and provide the from (4). 
			//If successful, an access_token is returned that can be used to make API requests on the users behalf.

			var client = new RestClient(Endpoint);
			var authRequest = new RestRequest("v1/oauth2/authorize", Method.GET);
			authRequest.AddParameter("client_id", ClientID);

			//The url that the user's browser will be redirected back to if authorization is successful.
			//Redirect URL can also be entered into merchant.neteller.com, but it will then override everything, even if sent here!
			authRequest.AddParameter("redirect_url", this.RedirectUrl);

			authRequest.AddParameter("scope", "subscription_payment");

			//check if "token" (client side flow) is supported now.
			authRequest.AddParameter("response_type", "code");

			authRequest.AddParameter("state", paymentTransactionId.ToString());

			authRequest.AddParameter("lang", lang);

			string url = client.BuildUri(authRequest).ToString();

			return url;
		}

	}


}