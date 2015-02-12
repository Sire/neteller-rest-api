#region

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#endregion

namespace Neteller.API {

	/// <summary>
	/// Neteller Configuration, read from Neteller.config file
	/// </summary>
	public class Configuration {

		public Configuration() {
		}

		/// <summary>
		/// Your unique Neteller App Client ID
		/// </summary>
		public static string ClientID { get { return GetSettings("ClientID", "Neteller"); } }

		/// <summary>
		/// Your client secret
		/// </summary>
		public static string ClientSecret { get { return GetSettings("ClientSecret", "Neteller"); } }

		/// <summary>
		/// Your unique Neteller App Client ID for testing
		/// </summary>
		public static string ClientIDTest { get { return GetSettings("ClientIDTest", "Neteller"); } }

		/// <summary>
		/// Your client secret for testing
		/// </summary>
		public static string ClientSecretTest { get { return GetSettings("ClientSecretTest", "Neteller"); } }



		/// <summary>
		/// Neteller will post the authorization code to this URL after successful authentication of customer. Production URL.
		/// </summary>
		public static string RedirectUrl { get { return GetSettings("RedirectUrl", "Neteller"); } }

		/// <summary>
		/// Neteller will post the authorization code to this URL after successful authentication of customer. URL when running in test/sandbox.
		/// </summary>
		public static string RedirectUrlTest { get { return GetSettings("RedirectUrlTest", "Neteller"); } }





		//http://stackoverflow.com/a/283917
		public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public static string ExecutingAssembly
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return new FileInfo(path).FullName;
			}
		}


		/// <summary>
		/// Get an appsetting. Throws an error if settings does not exist.
		/// </summary>
		public static string GetSettings(string key, string sectionName = "appSettings")
		{
			NameValueCollection settings = (NameValueCollection)ConfigurationManager.GetSection(sectionName);

			if (settings != null)
				if (settings[key] != null)
					return settings[key];
			throw new Exception(sectionName + "/" + key + " missing from config file in assembly (DOUBLE CHECK THIS PATH): " + ExecutingAssembly);
		}

		/// <summary>
		/// Get an appsetting but does NOT throw an error if settings does not exist.
		/// </summary>
		public static bool TryGetSettings(string key, out string value)
		{
			try
			{
				value = GetSettings(key);
				return true;
			}
			catch {
				value = "";
				return false;
			}
		}


	}
}
