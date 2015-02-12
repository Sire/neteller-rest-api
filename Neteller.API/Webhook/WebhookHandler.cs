using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Neteller.API.Model;

namespace Neteller.API.Webhook
{
	public class WebhookHandler
	{

		Deserializer deserializer = new Deserializer();

		/// <summary>
		/// Convert the Neteller post data into a WebhookMessage
		/// Example usage: 
		///		var webhook = new WebhookHandler();
		///		WebhookMessage message = webhook.GetMessage(postdata);
		/// </summary>
		/// <param name="postData"></param>
		/// <returns></returns>
		public WebhookMessage GetMessage(string postData)
		{
			return deserializer.FromJson<WebhookMessage>(postData);
		}




		/// <summary>
		/// T should be a class that implements methods whose names match the urls being called, and take one parameter, an HttpListenerContext.
		/// e.g.
		/// urls exercised:  "http://localhost:8080/error"  and "http://localhost:8080/get_list"
		/// 
		/// class MyHandler
		/// {
		///   void error(HttpListenerContext ctx)
		///   {
		///     // do something interesting here
		///   }
		///
		///   void get_list(HttpListenerContext ctx)
		///   {
		///     // do something interesting here
		///   }
		/// }
		/// </summary>
		public static Action<HttpListenerContext> Generic<T>() where T : new()
		{
			return ctx =>
			{
				var methodName = ctx.Request.Url.Segments.Last();
				var method = typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				if(method.IsStatic)
				{
					method.Invoke(null, new object[] { ctx });
				}
				else
				{
					method.Invoke(new T(), new object[] { ctx });
				}
			};
		}

	}
}
