#region

using System.Collections.Generic;

#endregion

namespace Neteller.API.Model {
	public class Plans
	{
		public List<Plan> list { get; set; }
		public List<Link> links { get; set; }
	}
}