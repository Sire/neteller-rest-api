using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Neteller.API.Model
{
	public class WebhookMessage
	{

		[DeserializeAs(Name = "mode")]
		public string Mode { get; set; }

		[DeserializeAs(Name = "id")]
		public string Id { get; set; }

		[DeserializeAs(Name = "eventDate")]
		public DateTime EventDate { get; set; }

		[DeserializeAs(Name = "eventType")]
		public string EventType { get; set; }

		[DeserializeAs(Name = "attemptNumber")]
		public int AttemptNumber { get; set; }

		[DeserializeAs(Name = "key")]
		public string Key { get; set; }

		[DeserializeAs(Name = "links")]
		public List<Link> Links { get; set; }
	
	}
}
