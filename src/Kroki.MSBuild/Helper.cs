using System.Collections.Generic;
using System.IO;
using System.Text;
using Scriban;

namespace Kroki.MSBuild
{
	public static class Helper
	{
		private static string LoadTemplate(string shortName)
		{
			var type = typeof(Helper);
			var name = $"{type.Namespace}.{shortName}.txt";
			using var stream = type.Assembly.GetManifestResourceStream(name)!;
			using var reader = new StreamReader(stream, Encoding.UTF8);
			return reader.ReadToEnd();
		}

		internal static string LoadAndRender(string shortName, object model)
		{
			var text = LoadTemplate(shortName);
			var template = Template.Parse(text);
			var result = template.Render(model);
			return result;
		}

		public static void Write(string path, string text)
		{
			File.WriteAllText(path, text, Encoding.UTF8);
		}

		internal static bool TryGoodValue(this IDictionary<string, string> dict,
			string key, out string value)
			=> dict.TryGetValue(key, out value) && !string.IsNullOrWhiteSpace(value);
	}
}