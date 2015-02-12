namespace Neteller.API.Model
{

	public class Payment
	{
		public LinkObject customer { get; set; }
		public Transaction transaction { get; set; }
	}
}
