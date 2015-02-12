using System;

namespace Neteller.API.Helper
{
	public static class StringHelper
	{

		public static string TrimAfterAndIncluding(this string inputString, string searchFor, bool lastOccurance=false)
		{
			int pos = 0;
			if (!lastOccurance) //default we remove from the first occurrence
				pos = inputString.IndexOf(searchFor, StringComparison.InvariantCultureIgnoreCase);
			else
				pos = inputString.LastIndexOf(searchFor, StringComparison.InvariantCultureIgnoreCase);

			if (pos != -1)
			{
				return inputString.Substring(0, pos).Trim();
			}
			else
				return inputString;
		}

		public static string TrimBeforeAndIncluding(this string inputString, string searchFor)
		{
			int pos = inputString.IndexOf(searchFor, StringComparison.InvariantCultureIgnoreCase);
			if (pos != -1)
			{
				return inputString.Substring(pos + searchFor.Length, inputString.Length - (pos + searchFor.Length)).Trim();
			}
			else
				return inputString;
		}


	}
}
