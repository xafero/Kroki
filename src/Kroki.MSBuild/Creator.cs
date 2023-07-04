using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Scriban;

namespace Kroki.MSBuild
{
	public static class Creator
	{
		private static string LoadTemplate(string shortName)
		{
			var type = typeof(Creator);
			var name = $"{type.Namespace}.{shortName}.txt";
			using var stream = type.Assembly.GetManifestResourceStream(name)!;
			using var reader = new StreamReader(stream, Encoding.UTF8);
			return reader.ReadToEnd();
		}

		private static string LoadAndRender(string shortName, object model)
		{
			var text = LoadTemplate(shortName);
			var template = Template.Parse(text);
			var result = template.Render(model);
			return result;
		}

		public static string CreateSolution()
		{
			var args = new { Name = "World" };
			return LoadAndRender("Templates.solution.sln", args);
		}

		public static string CreateProject(string? name, IDictionary<string, string> meta)
		{
			var pMeta = new Dictionary<string, string>();
			if (!string.IsNullOrWhiteSpace(name))
				pMeta["Product"] = name!;
			if (meta.TryGoodValue("InternalName", out var it))
				pMeta["AssemblyTitle"] = it;
			if (meta.TryGoodValue("Author", out var au))
				pMeta["Authors"] = au;
			if (meta.TryGoodValue("ProductName", out var pn))
				pMeta["Product"] = pn;
			if (meta.TryGoodValue("CompanyName", out var cn))
				pMeta["Company"] = cn;
			if (meta.TryGoodValue("LegalCopyright", out var lc))
				pMeta["Copyright"] = lc;
			if (meta.TryGoodValue("FileDescription", out var fd))
				pMeta["Description"] = fd;
			if (meta.TryGoodValue("FileVersion", out var fv))
				pMeta["AssemblyVersion"] = fv;
			if (meta.TryGoodValue("ProductVersion", out var pv))
				pMeta["InformationalVersion"] = pv;
			if (meta.TryGoodValue("OriginalFilename", out var an))
				pMeta["AssemblyName"] = an.Replace(".exe", string.Empty)
					.Replace(".dll", string.Empty);

			var args = new { Meta = pMeta };
			return LoadAndRender("Templates.classlib.csproj", args);
		}

		public static void Write(string path, string text)
		{
			File.WriteAllText(path, text, Encoding.UTF8);
		}

		private static bool TryGoodValue(this IDictionary<string, string> dict,
			string key, out string value)
			=> dict.TryGetValue(key, out value) && !string.IsNullOrWhiteSpace(value);
	}
}