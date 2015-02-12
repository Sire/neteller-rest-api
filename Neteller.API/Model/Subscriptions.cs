#region

using System.Collections.Generic;

#endregion

namespace Neteller.API.Model {
	public class Subscriptions
	{
		public List<SubscriptionThin> list { get; set; }
		public List<Link> links { get; set; }

		//not used anymore (even though docs still say so)
		//public class SubscriptionObject
		//{
		//	public Subscription subscription { get; set; }
		//}
	}
}

