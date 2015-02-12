using System.Collections.Generic;

namespace Neteller.API.Model
{
	public class Event
	{
		public string mode { get; set; }
		public string eventDate { get; set; }
		public string eventType { get; set; }
		public int attemptNumber { get; set; }
		public List<Link> links { get; set; }
	}
}
