using System.Collections.Generic;

namespace Neteller.API.Model
{
	public class Error
	{
		public string code { get; set; }
		public string message { get; set; }
		public List<FieldError> fieldErrors { get; set; }
	}

	public class FieldError
	{
		public string field { get; set; }
		public string error { get; set; }
	}
}
