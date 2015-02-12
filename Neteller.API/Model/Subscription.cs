#region

using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

#endregion

namespace Neteller.API.Model
{

	/// <summary>
	/// Subscription without Customer and Plan objects preloaded (no resource expansion)
	/// </summary>
	public class SubscriptionThin : SubscriptionBase
	{
		[DeserializeAs(Name = "plan")]
		public LinkObject Plan { get; set; }

		[DeserializeAs(Name = "customer")]
		public LinkObject Customer { get; set; }
	}

	/// <summary>
	/// Subscription object with Customer and Plan objects preloaded (resource expansion)
	/// </summary>
	public class Subscription : SubscriptionBase
	{
		[DeserializeAs(Name = "plan")]
		public Plan Plan { get; set; }

		[DeserializeAs(Name = "customer")]
		public Customer Customer { get; set; }
	}

	public abstract class SubscriptionBase
	{

		[DeserializeAs(Name = "subscriptionId")]
		public string SubscriptionId { get; set; }

		[DeserializeAs(Name = "status")]
		public string Status { get; set; }

		/// <summary>
		/// An optional start date for the subscription.  If the subscription is to start immediately, this should be left blank and enrollment will immediately 
		/// attempt to issue the initial payment.  If the initial payment fails then an error will be returned and the subscription will not be started.
		/// </summary>
		[DeserializeAs(Name = "startDate")]
		public DateTime StartDate { get; set; }

		[DeserializeAs(Name = "endDate")]
		public DateTime EndDate { get; set; }

		[DeserializeAs(Name = "termEndDate")]
		public DateTime TermEndDate { get; set; }

		[DeserializeAs(Name = "currentPeriodStart")]
		public DateTime CurrentPeriodStart { get; set; }

		[DeserializeAs(Name = "currentPeriodEnd")]
		public DateTime CurrentPeriodEnd { get; set; }

		[DeserializeAs(Name = "cancelAtPeriodEnd")]
		public bool CancelAtPeriodEnd { get; set; }

		[DeserializeAs(Name = "cancelDate")]
		public DateTime CancelDate { get; set; }

		[DeserializeAs(Name = "lastCompletedPaymentDate")]
		public DateTime LastCompletedPaymentDate { get; set; }
	}


	/// <summary>
	/// Only used when creating a subscription
	/// </summary>
	public class NewSubscription
	{
		public string planId { get; set; }
		public AccountProfileLite accountProfile { get; set; }
		public string startDate { get; set; }
	}
}
