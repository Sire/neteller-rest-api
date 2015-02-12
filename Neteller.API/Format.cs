namespace Neteller.API
{
	public class Format
	{
		/// <summary>
		/// The Neteller datetime format. Always UTC.
		/// ISO 8601 with two digit milliseconds (upper case FFF means supressing trailing zeroes). 
		/// This format also handles date strings with no milliseconds at all. Because, magic.
		/// </summary>
		public const string DateTimeUTC = "yyyy-MM-ddTHH:mm:ss.FFFZ";
	}
}
