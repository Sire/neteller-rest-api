using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Neteller.API.Model;
using Neteller.API.Webhook;
using NUnit.Framework;
using RestSharp;

namespace Neteller.API.Tests
{

	[TestFixture]
	public class WebhookTests
	{

		[Test]
		public void ParseMessage() {
			string content = @"
				{
					""mode"": ""test"", 
					""id"": ""5359569e-0cb0-4287-ac6a-356826cce032"", 
					""eventDate"": ""2014-10-01T13:48:30Z"", 
					""eventType"": ""subscription_payment_pending_retry"", 
					""attemptNumber"": ""1"", 
					""key"": ""secret"",
					 ""links"": [
					{
					""url"":
					""https://api.neteller.com/v1/subscriptions/234234224/invoices/99102"",
					""rel"": ""invoice"",
					""method"": ""GET""
					}
					]
				}";

			var webhook = new WebhookHandler();
			WebhookMessage message = webhook.GetMessage(content);

			Assert.That(message.Mode, Is.EqualTo("test"));
			Assert.That(message.Id, Is.EqualTo("5359569e-0cb0-4287-ac6a-356826cce032"));
			Assert.That(message.EventDate, Is.EqualTo(DateTime.Parse("2014-10-01T13:48:30", null, DateTimeStyles.AssumeUniversal)));
			//don't test - dependent on current thread locale timezone 
			//locale Assert.That(message.EventDate, Is.EqualTo(new DateTime(2014, 10, 1, 13+2, 48, 30))); //utc +2
			Assert.That(message.EventType, Is.EqualTo(WebhookEventType.SubscriptionPaymentPendingRetry));
			Assert.That(message.AttemptNumber, Is.EqualTo(1));
			Assert.That(message.Key, Is.EqualTo("secret"));

			Assert.That(message.Links, Is.Not.Empty);
			Assert.That(message.Links[0].url, Is.EqualTo("https://api.neteller.com/v1/subscriptions/234234224/invoices/99102"));

		}


		//test from RestSharp

		private const string BASE_URL = "http://localhost:8080/";


		[Test]
		public void RestSharpSimpleServerTest()
		{
			const Method httpMethod = Method.POST;
			using (WebhookServer.Create(BASE_URL, WebhookHandler.Generic<RequestBodyCapturer>()))
			{
				var client = new RestClient(BASE_URL);
				var request = new RestRequest(RequestBodyCapturer.RESOURCE, httpMethod);

				const string contentType = "text/plain";
				const string bodyData = "abc123 foo bar baz BING!";
				request.AddParameter(contentType, bodyData, ParameterType.RequestBody);

				var resetEvent = new ManualResetEvent(false);
				client.ExecuteAsync(request, response => resetEvent.Set());
				resetEvent.WaitOne();

				AssertHasRequestBody(contentType, bodyData);
			}
		}

		private static void AssertHasRequestBody(string contentType, string bodyData)
		{
			Assert.That(contentType, Is.EqualTo(RequestBodyCapturer.CapturedContentType));
			Assert.That(true, Is.EqualTo(RequestBodyCapturer.CapturedHasEntityBody));
			Assert.That(bodyData, Is.EqualTo(RequestBodyCapturer.CapturedEntityBody));
		}

		private class RequestBodyCapturer
		{
			public const string RESOURCE = "Capture";

			public static string CapturedContentType { get; set; }
			public static bool CapturedHasEntityBody { get; set; }
			public static string CapturedEntityBody { get; set; }

			public static void Capture(HttpListenerContext context)
			{
				var request = context.Request;
				CapturedContentType = request.ContentType;
				CapturedHasEntityBody = request.HasEntityBody;
				CapturedEntityBody = StreamToString(request.InputStream);
			}

			private static string StreamToString(Stream stream)
			{
				var streamReader = new StreamReader(stream);
				return streamReader.ReadToEnd();
			}
		}

	}
}
