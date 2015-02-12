using System;
using System.Net;
using System.Threading;

namespace Neteller.API.Webhook
{

	/// <summary>
	/// This is the SimpleServer from RestSharp project, should only be used with unit tests, not in production.
	/// </summary>
	public class WebhookServer : IDisposable
	{
		readonly HttpListener _listener;
		readonly Action<HttpListenerContext> _handler;
		Thread _processor;

		public static WebhookServer Create(string url, Action<HttpListenerContext> handler, AuthenticationSchemes authenticationSchemes = AuthenticationSchemes.Anonymous)
		{
			var listener = new HttpListener
			{
				Prefixes = { url },
				AuthenticationSchemes = authenticationSchemes
			};
			var server = new WebhookServer(listener, handler);
			server.Start();
			return server;
		}

		WebhookServer(HttpListener listener, Action<HttpListenerContext> handler)
		{
			_listener = listener;
			_handler = handler;
		}

		public void Start()
		{
			if (!_listener.IsListening)
			{
				_listener.Start();

				_processor = new Thread(() =>
				{
					var context = _listener.GetContext();
					_handler(context);
					context.Response.Close();
				}) { Name = "WebhookServer" };
				_processor.Start();
			}
		}

		public void Dispose()
		{
			_processor.Abort();
			_listener.Stop();
			_listener.Close();
		}
	}
}
