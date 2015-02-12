using System;
using RestSharp.Deserializers;

namespace Neteller.API.Model
{
	public class Invoice
	{

		[DeserializeAs(Name = "invoiceNumber")]
		public int InvoiceNumber { get; set; }

		[DeserializeAs(Name = "invoiceDate")]
		public DateTime InvoiceDate { get; set; }

		[DeserializeAs(Name = "invoiceType")]
		public string InvoiceType { get; set; }

		[DeserializeAs(Name = "customer")]
		public LinkObject Customer { get; set; }

		[DeserializeAs(Name = "subscription")]
		public LinkObject Subscription { get; set; }

		[DeserializeAs(Name = "status")]
		public string Status { get; set; }

		[DeserializeAs(Name = "periodStartDate")]
		public DateTime PeriodStartDate { get; set; }

		[DeserializeAs(Name = "periodEndDate")]
		public DateTime PeriodEndDate { get; set; }

		/// <summary>
		/// The total amount due reported in the smallest unit of currency (no decimals)
		/// </summary>
		[DeserializeAs(Name = "totalAmount")]
		public int TotalAmount { get; set; }
		
		[DeserializeAs(Name = "currency")]
		public string Currency { get; set; }
		
		[DeserializeAs(Name = "retryCount")]
		public int RetryCount { get; set; }

		[DeserializeAs(Name = "nextRetryDate")]
		public DateTime NextRetryDate { get; set; }

	}

}
