using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Kroki.Bulk.Read
{
	internal static class Groups
	{
		public static OneProj ReadProject(string file)
		{
			using var stream = File.OpenRead(file);
			var doc = XDocument.Load(stream);
			var nsp = doc.Root!.GetDefaultNamespace();
			var tmp = "MainSource";
			var versionMeta = doc.Descendants(nsp.GetName("VersionInfoKeys"))
				.Concat(doc.Descendants(nsp.GetName("VersionInfo")))
				.Select(p => (k: p.Attribute("Name")?.Value, v: p.Value))
				.Where(p => p.k != null).GroupBy(p => p.k)
				.ToImmutableSortedDictionary(k => k.Key!, v => v.First().v);
			var mainSrc = doc.Descendants(nsp.GetName(tmp))
				.Select(p => p.Value)
				.Where(p => p != tmp)
				.ToArray();
			var codeSrc = doc.Descendants(nsp.GetName("DelphiCompile"))
				.Concat(doc.Descendants(nsp.GetName("DCCReference")))
				.Select(p => p.Attribute("Include")!.Value)
				.Concat(mainSrc)
				.ToArray();
			var dccPathA = doc.Descendants(nsp.GetName("DCC_UnitSearchPath"))
				.Concat(doc.Descendants(nsp.GetName("DCC_ResourcePath")))
				.Concat(doc.Descendants(nsp.GetName("DCC_ObjPath")))
				.Concat(doc.Descendants(nsp.GetName("DCC_IncludePath")))
				.SelectMany(p => p.Value.Split(';').Select(q => q.TrimEnd('\\')))
				.ToArray();
			var src = ResolveFull(file, codeSrc, out var dir,
				new[] { ".pas", ".dpr" }, new[] { ".cs", ".cs" });
			var paths = ResolveFull(file, dccPathA, out _,
				new string[] { }, new string[] { });
			return new OneProj(dir, versionMeta, src, paths);
		}

		public static GroupProj ReadProjects(string file)
		{
			using var stream = File.OpenRead(file);
			var doc = XDocument.Load(stream);
			var nsp = doc.Root!.GetDefaultNamespace();
			var projects = doc.Descendants(nsp.GetName("Projects"))
				.Select(p => p.Attribute("Include")!.Value)
				.ToArray();
			var msBuilds = doc.Descendants(nsp.GetName("MSBuild"))
				.Select(p => p.Attribute("Projects")!.Value)
				.ToArray();
			var includes = ResolveFull(file, projects.Concat(msBuilds), out var dir,
				new[] { ".dproj" }, new[] { ".csproj" });
			return new GroupProj(dir, includes);
		}

		internal static ISet<string> ResolveFull(string file, IEnumerable<string> refs,
			out string dirPath, string[] oldExt, string[] newExt)
		{
			dirPath = Path.GetDirectoryName(file)!;
			var includes = new SortedSet<string>();
			foreach (var project in refs)
			{
				var fullPath = Path.Combine(dirPath, project);
				foreach (var (oe, ne) in oldExt.Zip(newExt))
					fullPath = fullPath.Replace(oe, ne);
				fullPath = Path.GetFullPath(fullPath);
				includes.Add(fullPath);
			}
			return includes;
		}

		public static OnePackage ReadPackage(string file)
		{
			var lines = File.ReadAllLines(file, Encoding.UTF8);
			var files = lines.Select(l => l.Split("in '", 2))
				.Where(l => l.Length == 2).Select(l => l.Last()
					.Split('\'', 2).First()).ToArray();
			var includes = ResolveFull(file, files, out var dir,
				new[] { "pas" }, new[] { "cs" });
			return new OnePackage(dir, includes);
		}
	}
}