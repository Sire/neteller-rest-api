namespace Neteller.API
{
	public class WebhookEventType
	{

		/// <summary>
		/// Subscription has moved from an 'pending' to 'active' state
		/// </summary>
		public const string SubscriptionActivated = "subscription_activated";

		/// <summary>
		/// Subscription was cancelled
		/// </summary>
		public const string SubscriptionCancelled = "subscription_cancelled";

		/// <summary>
		/// Subscription is set to be cancelled at the end of the current billing period
		/// </summary>
		public const string SubscriptionCancelledAtPeriodEnd = "subscription_cancelled_at_period_end";

		/// <summary>
		/// Subscription was created
		/// </summary>
		public const string SubscriptionCreated = "subscription_created";

		/// <summary>
		/// Subscription has lapsed and is now ended
		/// </summary>
		public const string SubscriptionEnded = "subscription_ended";

		/// <summary>
		/// Attempts to collect payment for the subscription for the current billing period have failed
		/// </summary>
		public const string SubscriptionPaymentFailed = "subscription_payment_failed";

		/// <summary>
		/// Member has insufficient funds available, NETELLER will continue to try to collect payment for this period
		/// </summary>
		public const string SubscriptionPaymentPendingRetry = "subscription_payment_pending_retry";

		/// <summary>
		/// Payment was successful
		/// </summary>
		public const string SubscriptionPaymentSucceeded = "subscription_payment_succeeded";



	}
}
