namespace Neteller.API {

	public class SubscriptionStatus {
		/// <summary>
		/// Subscription has been requested and is pending activation upon the requested start date
		/// </summary>
		public const string Pending = "pending";

		/// <summary>
		/// The subscription is currently active.
		/// </summary>
		public const string Active = "active";

		/// <summary>
		/// The subscription was cancelled by the subscriber or by the merchant application
		/// </summary>
		public const string Cancelled = "cancelled";

		/// <summary>
		/// The subscription has lapsed
		/// </summary>
		public const string Ended = "ended";

		/// <summary>
		/// This status will only be set if the subscription was created with no startDate supplied and the immediate billing failed
		/// </summary>
		public const string Failed = "failed";

	}
}