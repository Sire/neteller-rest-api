namespace Neteller.API.Model {
	public class Fee
	{
		/// <summary>
		/// The name that will display in histor yand invoice details for this fee item. (ie: Setup Fee, Shipping & Handling Fee...etc)
		/// </summary>
		public string feeName { get; set; }

		/// <summary>
		/// The type of fee. (Service_fee)
		/// </summary>
		public string feeType { get; set; }

		/// <summary>
		/// The fee amount that was deducted.
		/// Amount fields reflect the smallest unit of currency
		/// with no decimals. Eg. $25.00 USD should be
		/// formatted as 2500
		/// </summary>
		public int feeAmount { get; set; }

		/// <summary>
		/// The currency the fee was in.
		/// </summary>
		public string feeCurrency { get; set; }
	}
}