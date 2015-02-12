using System.IO;
using RestSharp;
using RestSharp.Deserializers;

namespace Neteller.API
{
	public class Deserializer
	{
		public T FromFile<T>(string filename)
		{
			var content = File.ReadAllText(filename);
			var response = new RestResponse { Content = content };
			var json = new JsonDeserializer();
			//Important to parse Neteller date correctly from UTC
			json.DateFormat = Format.DateTimeUTC;
			var output = json.Deserialize<T>(response);
			return output;
		}

		public T FromJson<T>(string content)
		{
			var response = new RestResponse { Content = content };
			var json = new JsonDeserializer();
			//Important to parse Neteller date correctly from UTC
			json.DateFormat = Format.DateTimeUTC;
			var output = json.Deserialize<T>(response);
			return output;
		}


	}
}
