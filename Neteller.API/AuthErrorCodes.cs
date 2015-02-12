namespace Neteller.API
{
	public class AuthErrorCodes
	{
		/// <summary>
		/// The client is not authorized to request an authorization code using this method
		/// </summary>
		public const string UnauthorizedClient = "unauthorized_client";

		/// <summary>
		/// The resource owner or authorization server denied your request
		/// </summary>
		public const string AccessDenied = "access_denied";

		/// <summary>
		/// The authorization server does not support obtaining an authorization code using this method.
		/// </summary>
		public const string UnsupportedResponseType = "unsupported_response_type";

		/// <summary>
		/// The requested scope is invalid, unknown, or malformed.
		/// </summary>
		public const string InvalidScope = "invalid_scope";

		/// <summary>
		/// The authorization server encountered an unexpected condition that prevented it
		/// from fulfilling the request.(This error code is needed because a 500 Internal Server
		/// Error HTTP status code cannot be returned to the client via an HTTP redirect.)
		/// </summary>
		public const string ServerError = "server_error";

		/// <summary>
		/// The authorization server is currently unable to handle the request due to a
		/// temporary overloading or maintenance of the server.  (This error code is needed
		/// because a 503.  Service Unavailable HTTP status code cannot be returned to the
		/// client via an HTTP redirect.)
		/// </summary>
		public const string TemporarilyUnavailable = "temporarily_unavailable";
	}
}
