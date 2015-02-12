using System;
using System.Globalization;
using NUnit.Framework;

namespace Neteller.API.Tests
{

	[TestFixture]
	class DateTimeTests
	{

		[Test]
		public void FormatTest()
		{

			DateTime timeUtc = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			string timeString = "2001-01-01T00:00:00Z"; //example time string from neteller

			//ParseExact automatically turns UTC into local time
			DateTime timeParsedLocal = DateTime.ParseExact(timeString, Format.DateTimeUTC, null);

			Assert.That(timeUtc.Kind, Is.EqualTo(DateTimeKind.Utc));
			Assert.That(timeParsedLocal.Kind, Is.EqualTo(DateTimeKind.Local));

			Assert.That(timeUtc, Is.Not.EqualTo(timeParsedLocal));
			Assert.That(timeUtc, Is.EqualTo(timeParsedLocal.ToUniversalTime()));

			string timeStringReversed = timeUtc.ToUniversalTime().ToString("u").Replace(" ", "T");
			Assert.That(timeStringReversed, Is.EqualTo(timeString));

			DateTime localTime = timeUtc.ToLocalTime();
			Assert.That(localTime, Is.Not.EqualTo(timeUtc));

			Assert.That(localTime.ToUniversalTime(), Is.EqualTo(timeUtc));
		}

		[Test]
		public void MillisecondFormatTest()
		{
			string formatWithMs = "yyyy-MM-ddTHH:mm:ss.FFFZ";
			string formatWithoutMs = "yyyy-MM-ddTHH:mm:ssZ";

			DateTime date;

			//Neteller uses TWO datetime formats, sometimes with milliseconds sometimes not :(
			string dateStringWithMs = "2014-11-10T11:48:45.54Z";
			string dateStringWithoutMs = "2014-11-10T11:48:45Z";

			Assert.That(DateTime.TryParseExact(dateStringWithMs, formatWithMs, null, DateTimeStyles.None, out date), Is.True);
			Assert.That(date.Millisecond, Is.EqualTo(540)); //always 3 digits

			Assert.That(DateTime.TryParseExact(dateStringWithoutMs, formatWithoutMs, null, DateTimeStyles.None, out date), Is.True);
			Assert.That(date.Millisecond, Is.EqualTo(0));

			//format string must work even when no milliseconds are sent
			Assert.That(DateTime.TryParseExact(dateStringWithoutMs, formatWithMs, null, DateTimeStyles.None, out date), Is.True);
			Assert.That(date.Millisecond, Is.EqualTo(0));
		}
	}
}
