using System;
using System.Collections.Generic;

namespace Neteller.API.Model {
	public class Transaction
	{

		/// <summary>
		/// The external merchant transaction id that uniquely identifies this transaction within the merchant system.
		/// </summary>
		public string merchantRefId { get; set; }

		public int amount { get; set; }
		public string currency { get; set; }
		public string id { get; set; }
		public string createDate { get; set; }
		public string updateDate { get; set; }
		public string status { get; set; }
		public List<Fee> fees { get; set; }
	}
}