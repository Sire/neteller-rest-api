using System;
using System.Collections.Generic;
using Neteller.API.Helper;

namespace Neteller.API
{
	public class Languages
	{

		private static List<string> languages = new List<string>()
		{
			"da_DK",
			"de_DE",
			"el_GR",
			"en_US",
			"es_ES",
			"fr_FR",
			"it_IT",
			"ja_JP",
			"ko_KR",
			"no_NO",
			"pl_PL",
			"pt_PT",
			"ru_RU",
			"sv_SE",
			"tr_TR",
		};


		private static string m_Default = "en_US";
		public static string Default {
			get { return m_Default; }
			set { m_Default = value;  }
		}

		/// <summary>
		/// Returns a corresponding Neteller language code if exists, otherwise returns English as default.
		/// Example usage getting language from user web browser:
		/// <c>
		/// var context = HttpContext.Current;
		/// if (context != null) {
		/// 	if (context.Request != null && context.Request.UserLanguages != null && context.Request.UserLanguages.Length > 0) {
		/// 		return Languages.GetFromCode(context.Request.UserLanguages);
		/// 	}
		/// }
		/// return Languages.English;
		/// </c>
		/// </summary>
		/// <param name="langCodes">Prioritized list of accepted languange codes</param>
		/// <returns></returns>
		public static string GetFromCode(string[] langCodes)
		{
			foreach (var langCodeWithPriority in langCodes) {
				string langCode = langCodeWithPriority.TrimAfterAndIncluding(";"); //remove possible ;q=1.0 priority info from web browser request
				langCode = langCode.Replace("-", "_"); //for some reason Neteller uses a non standard underscore separator
				//case insensitive
				string netellerLangCode = languages.Find(s => s.StartsWith(langCode, StringComparison.InvariantCultureIgnoreCase));
				if (!string.IsNullOrEmpty(netellerLangCode))
					return netellerLangCode;
			}


			return Default;
		}
	}
}
