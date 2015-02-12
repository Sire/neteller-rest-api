using System;
using System.Globalization;
using System.IO;
using Neteller.API.Model;
using NUnit.Framework;

namespace Neteller.API.Tests
{

	/// <summary>
	/// Use real Neteller json responses and make sure they get deserialized into classes properly
	/// </summary>
	[TestFixture]
	public class DeserializationTests
	{


		//FIXME: build these tests.

		Deserializer deserializer = new Deserializer();
		private string GetExampleResponseFilename(string filename) {
			return Path.Combine("ExampleResponse", filename);
		}

		[Test]
		public void Customer()
		{
			var customer = deserializer.FromFile<Customer>(GetExampleResponseFilename("Customer.json"));
		}
		[Test]
		public void Invoice()
		{
			var invoice = deserializer.FromFile<Invoice>(GetExampleResponseFilename("Invoice.json"));
		}
		[Test]
		public void Invoices()
		{
			var invoices = deserializer.FromFile<Invoices>(GetExampleResponseFilename("Invoices.json"));
		}
		[Test]
		public void Plans()
		{
			var plans = deserializer.FromFile<Plans>(GetExampleResponseFilename("Plans.json"));
		}
		[Test]
		public void Subscription()
		{
			var subscription = deserializer.FromFile<Subscription>(GetExampleResponseFilename("Subscription.json"));
		}
		[Test]
		public void Subscriptions()
		{
			var subscriptions = deserializer.FromFile<Subscriptions>(GetExampleResponseFilename("Subscriptions.json"));
		}


		[Test]
		public void OLDSubscriptionPlans() {
			//var subscriptions = deserializer.FromFile<Subscriptions>(GetExampleResponseFilename("Subscriptions.json"));
			//Assert.That(subscriptions.list.Count, Is.EqualTo(3));

			//var sub = subscriptions.list[0];

			//Assert.That(sub.SubscriptionId, Is.EqualTo(26));

			////Assert.That(sub.Plan.link.url, Is.EqualTo("???/TestPlan1"));
			////Assert.That(sub.Customer.link.url, Is.StringContaining("???"));

			//Assert.That(sub.Status, Is.EqualTo(SubscriptionStatus.Pending));
			////requires JSonDeserializer is set

			////both .StartDate and DateTime.Parse turns this into LOCAL TIME
			//Assert.That(sub.StartDate, Is.EqualTo(DateTime.Parse("2014-10-08T12:36:32Z")));

			////convert time to UTC and compare
			//Assert.That(sub.StartDate.ToUniversalTime(), Is.EqualTo(DateTime.Parse("2014-10-08T12:36:32Z",null, DateTimeStyles.AdjustToUniversal)));

			//Assert.That(sub.EndDate, Is.EqualTo(DateTime.Parse("2014-10-22T12:36:32Z")));
			//Assert.That(sub.CancelAtPeriodEnd, Is.EqualTo(false));

			//sub = subscriptions.list[1];
			//Assert.That(sub.CurrentPeriodStart, Is.EqualTo(DateTime.Parse("2014-10-01T12:36:05Z")));
			//Assert.That(sub.CurrentPeriodEnd, Is.EqualTo(DateTime.Parse("2014-10-08T12:36:05Z")));
	
		}


		[Test]
		public void OLDInvoices()
		{
			//var invoices = deserializer.FromFile<Invoices>(GetExampleResponseFilename("Invoices.json"));
			//Assert.That(invoices.list.Count, Is.EqualTo(1));
			//Assert.That(invoices.links.Count, Is.EqualTo(1));
			//var invoice = invoices.list[0];
			//Assert.That(invoice.InvoiceNumber, Is.EqualTo(7));
			//Assert.That(invoice.InvoiceDate.ToUniversalTime(), Is.EqualTo(new DateTime(2014,10,01,12,36,05, DateTimeKind.Utc)));
			//Assert.That(invoice.Subscription.link.url, Is.StringContaining("25"));
			//Assert.That(invoice.Status, Is.EqualTo("paid"));
			//Assert.That(invoice.NextRetryDate, Is.EqualTo(DateTime.MinValue));
		}



	}
}
