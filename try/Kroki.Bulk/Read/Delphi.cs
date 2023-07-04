using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Kroki.Core.Util.Helpers;

namespace Kroki.Bulk.Read
{
    internal static class Delphi
    {
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

            Console.WriteLine($"   * Converted to '{outFile}'...");
        }

		public static void ParseDfm(string file, string outFile)
        {
	        outFile = outFile.Replace(".dfm", ".d.cs");
	        ParsePascal(file, outFile);
        }

        public static void ParseDpr(string file, string outFile)
        {
	        outFile = outFile.Replace(".dpr", ".p.cs");
	        ParsePascal(file, outFile);
        }

        public static void ParseGroupPrj(string file, string outFile)
        {
	        var gp = Groups.ReadProjects(file);
	        var path = $"{outFile}.txt";
	        var lines = new[] { gp.Root, string.Empty }.Concat(gp.Links);
	        File.WriteAllLines(path, lines, Encoding.UTF8);
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
	        var prj = Groups.ReadProject(file);
	        var path = $"{outFile}.txt";
	        var lines = new[] { prj.Root, string.Empty }.Concat(prj.Links)
		        .Concat(new[] { string.Empty }).Concat(prj.Sources).Concat(new[] { string.Empty })
		        .Concat(prj.Meta.Select(p => $"{p.Key} = {p.Value}"));
	        File.WriteAllLines(path, lines, Encoding.UTF8);
        }
    }
}