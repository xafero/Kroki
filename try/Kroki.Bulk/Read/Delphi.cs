using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kroki.MSBuild;
using static Kroki.Core.Util.Helpers;

namespace Kroki.Bulk.Read
{
	internal static class Delphi
	{
		internal static Action<string>? Log;

		public static void ParsePas(string file, string outFile)
		{
			outFile = outFile.Replace(".pas", ".cs");
			ParsePascal(file, outFile);
		}

		private static void ParsePascal(string file, string outFile)
		{
			var pasLines = new List<string>(File.ReadAllLines(file, Utf));

			using var stream = File.Create(outFile);
			using var code = new StreamWriter(stream, Utf);

			TransformComment(pasLines, code);

			var src = ReadSource(file);

			var (_, csCode) = Translate(file, src);
			code.Write(csCode);

			Log!.Invoke($"   * Converted to '{outFile}'...");
		}

		public static void ParseDfm(string file, string outFile)
		{
			outFile = outFile.Replace(".dfm", ".d.cs");
			ParsePascal(file, outFile);
		}

		public static void ParseDpr(string file, string outFile)
		{
			outFile = outFile.Replace(".dpr", ".cs");
			ParsePascal(file, outFile);
		}

		public static void ParseGroupPrj(string file, string outFile)
		{
			var gp = Groups.ReadProjects(file);
			var path = outFile.Replace(".groupproj", ".sln");
			var sln = Creator.CreateSolution(gp.Root, gp.Links);
			Helper.Write(path, sln);
		}

		public static void ParseDpk(string file, string outFile)
		{
			var gp = Groups.ReadPackage(file);
			var path = $"{outFile}.txt";
			var lines = new[] { gp.Root, string.Empty }.Concat(gp.Files);
			File.WriteAllLines(path, lines, Encoding.UTF8);
		}

		public static void ParseDproj(string file, string outFile)
		{
			var op = Groups.ReadProject(file);
			var path = outFile.Replace(".dproj", ".csproj");
			var name = Path.GetFileNameWithoutExtension(path);
			var prj = Creator.CreateProject(name, op.Meta, op.Root, op.Sources);
			Helper.Write(path, prj);
		}

		static Delphi()
		{
			Creator.Relative = Path.GetRelativePath;
		}
	}
}