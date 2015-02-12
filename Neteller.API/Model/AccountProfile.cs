using System.Collections.Generic;

namespace Neteller.API.Model {
	public class AccountProfile
	{
		public string email { get; set; }
		public string accountId { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string address3 { get; set; }
		public string city { get; set; }

		/// <summary>
		/// Two-letter country code
		/// </summary>
		public string country { get; set; }

		/// <summary>
		/// For example state in the US
		/// </summary>
		public string countrySubdivisionCode { get; set; }

		public string postCode { get; set; }
		public List<ContactDetail> contactDetails { get; set; }
		public string gender { get; set; }
		public DateOfBirth dateOfBirth { get; set; }
		public AccountPreferences accountPreferences { get; set; }
	}
}
