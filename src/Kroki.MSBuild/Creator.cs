using System;
using System.Collections.Generic;
using System.IO;
using static Kroki.MSBuild.Helper;

namespace Kroki.MSBuild
{
	public static class Creator
	{
		public static Func<string, string, string>? Relative;

		public static string CreateSolution()
		{
			var args = new { Name = "World" };
			return LoadAndRender("Templates.solution.sln", args);
		}

		public static string CreateProject(string? name, IDictionary<string, string> meta,
			string root, ISet<string> sources)
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

			var pLinks = new Dictionary<string, string>();
			foreach (var source in sources)
			{
				var srcName = Path.GetFileName(source);
				var srcPath = Relative!.Invoke(root, source);
				if (srcPath == srcName)
					continue;
				pLinks[srcPath] = srcName;
			}

			var args = new { Meta = pMeta, Links = pLinks };
			return LoadAndRender("Templates.classlib.csproj", args);
		}
	}
}