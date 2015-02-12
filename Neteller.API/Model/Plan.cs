namespace Neteller.API.Model
{
	/// <summary>
	/// Neteller Subscription Plan
	/// Will be serialized to JSON so exact names are cruicial (or add json serialization attributes).
	/// Creating a subscription plan allows you to define a plan for having automatic payments transferred to your merchant
	/// account on a fixed schedule.
	/// </summary>
	public class Plan
	{

		public const string ResourceName = "plan"; //case sensitive

		/// <summary>
		/// Unique subscription plan ID. Max length 50.
		/// </summary>
		public string planId { get;  set; }

		/// <summary>
		/// Description. Max length 200.
		/// </summary>
		public string planName { get;  set; }

		/// <summary>
		/// The number of intervals between each billing attempt.
		/// </summary>
		public int interval { get;  set; }

		/// <summary>
		/// The frequency that the plan member will be billed (weekly, monthly, yearly)
		/// </summary>
		public string intervalType { get;  set; }

		/// <summary>
		/// The length of the contract in intervals.  (ie: to establish a 1
		/// year subscription plan with quarterly charges, then interval =
		/// 3, intervalType = 'monthly' and intervalCount = 4.
		/// </summary>
		public int intervalCount { get;  set; }

		/// <summary>
		/// The amount to bill for each re-occurence.  Amount should
		/// be in the smallest unit of currency with no decimals.  (ie:
		/// 14.99 EUR would be 1499)
		/// </summary>
		public int amount { get;  set; }

		/// <summary>
		/// The currency of the amount to be billed.
		/// </summary>
		public string currency { get;  set; }

		/// <summary>
		/// The status of the plan: active, deleted (cancelled?)
		/// </summary>
		public string status { get;  set; }

	}

}
