using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neteller.API.Model
{
	public class Customer
	{

		public const string ResourceName = "customer";

		public string id { get; set; }
		public AccountProfile accountProfile { get; set; }
		public string verificationLevel { get; set; }
	}
}
