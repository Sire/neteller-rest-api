namespace Neteller.API.Model
{
	public class Token
	{
		public string tokenType { get; set; }
		public int expiresIn { get; set; }
		public string accessToken { get; set; }
		public string refreshToken { get; set; }
	}
}
