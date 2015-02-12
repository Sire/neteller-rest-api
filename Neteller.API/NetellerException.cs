using System;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;

namespace Neteller.API
{

	/// <summary>
	/// Exception from the Neteller REST API
	/// </summary>
	public class NetellerException : Exception, ISerializable
	{

		private static NLog.Logger logger = LogManager.GetCurrentClassLogger();		

		/// <summary>
		/// The entire API response from Neteller
		/// </summary>
		public IRestResponse Response { get; set; }

		/// <summary>
		/// A user-friendly error message if this is not a totally unexpected exception. For instance when the account doesn't contain enough money for payment.
		/// </summary>
		public string UserFriendlyErrorMessage { get; private set; }

		/// <summary>
		/// Is this just a warning or is it truly an unexpected exception? Warnings don't get logged as fatal errors.
		/// </summary>
		public bool Warning { get; private set; }

		public NetellerException()
		{
			// Add implementation.
		}
		
		public NetellerException(string message) : base(message) {
		}

		public NetellerException(string message, Exception inner) : base(message, inner)
		{
		}

		public NetellerException(string message, Exception inner, IRestResponse response)
			: base(message, inner)
		{
			Response = response;
			SetUserFriendlyErrorMessage(response);
		}

		public NetellerException(string message, IRestResponse response) : base(message)
		{
			Response = response;
			SetUserFriendlyErrorMessage(response);
		}

		// This constructor is needed for serialization.
		protected NetellerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// Add implementation.
		}

		private void SetUserFriendlyErrorMessage(IRestResponse response)
		{
			UserFriendlyErrorMessage = "";
			string content = response.Content;

			//string content = @"{   ""error"": {     ""code"": ""20020"",     ""message"": ""Insufficient balance""   } }";

			string code = ""; 
			string errorMessage = "";

			//set clear error messages for our users

			if (response.StatusCode == HttpStatusCode.PaymentRequired)
			{
				GetErrorCodeAndMessage(content, out code, out errorMessage);
				if (code == "20020")
				{
					//original: "Insufficient balance"
					this.UserFriendlyErrorMessage = "You have insufficient balance in your Neteller account to complete the transaction. Please deposit funds and try again.";
					this.Warning = true; //warning = we don't log a fatal error
				}
			}
			else {
				//the errorMessage returned is better than no error message at all usually
				if (!string.IsNullOrEmpty(errorMessage))
					this.UserFriendlyErrorMessage = errorMessage;
			}

		}

		private void GetErrorCodeAndMessage(string content, out string code, out string errorMessage) {
			code = "";
			errorMessage = "";

			try
			{
				dynamic obj = JObject.Parse(content);
				dynamic error = obj["error"];

				if (error != null)
				{
					if (error["code"] != null)
						code = (string)error["code"];
					if (error["message"] != null)
						errorMessage = (string)error["message"];
				}
			}
			catch {
				logger.Fatal("Could not extract code and message from Neteller exception: " + content);
			}
		}
	}


}
