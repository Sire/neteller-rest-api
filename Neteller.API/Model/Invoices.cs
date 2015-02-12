using System.Collections.Generic;

namespace Neteller.API.Model
{

	public class Invoices
	{
		public List<Invoice> list { get; set; }
		public List<Link> links { get; set; }
	}

}
