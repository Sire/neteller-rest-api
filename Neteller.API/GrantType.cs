namespace Neteller.API
{
	/// <summary>
	/// The different 
	/// </summary>
	public enum GrantType
	{
		/// <summary>
		/// User authenticating
		/// </summary>
		client_credentials,
		
		/// <summary>
		/// After user authentication, get access token with auth_code
		/// </summary>
		authorization_code,

		/// <summary>
		/// Get access token by refreshing a previous authentication by user
		/// </summary>
		refresh_token
	}
}
