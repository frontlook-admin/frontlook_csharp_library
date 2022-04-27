using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace frontlook_csharp_library.FL_General
{
	public static class FL_JavaScriptSerializer
	{
		public static HtmlString FL_SerializeObjectForHtml<T>(this T value)
		{
			using (var stringWriter = new StringWriter())
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					// Let's use camelCasing as is common practice in JavaScript
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				};

				// We don't want quotes around object names
				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, value);

				return new HtmlString(stringWriter.ToString());
			}
		}

		public static string FL_SerializeObject<T>(this T value)
		{
			using (var stringWriter = new StringWriter())
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					// Let's use camelCasing as is common practice in JavaScript
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				};

				// We don't want quotes around object names
				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, value);

				return (stringWriter.ToString());
			}
		}

		public static string FL_SerializeSchemaObject<T>(this T value)
		{
			var generator = new JsonSchemaGenerator();

			var schema = generator.Generate(typeof(T));

			using (var stringWriter = new StringWriter())
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					// Let's use camelCasing as is common practice in JavaScript
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				};

				// We don't want quotes around object names
				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, schema);

				return (stringWriter.ToString());
			}
		}
	}
}