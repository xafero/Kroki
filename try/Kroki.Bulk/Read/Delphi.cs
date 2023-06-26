using System;
using System.Collections.Generic;
using System.IO;
using static Kroki.Core.Util.Helpers;

namespace Kroki.Bulk.Read
{
    internal static class Delphi
    {
        public static void ParseRes(string file, string outFile)
        {
            // TODO
        }

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

        public static void ParseRc(string file, string outFile)
        {
            // TODO
        }

        public static void ParseDfm(string file, string outFile)
        {
            // TODO
        }

        public static void ParseDproj(string file, string outFile)
        {
            // TODO
        }

        public static void ParseDpr(string file, string outFile)
        {
            // TODO
        }

        public static void ParseInc(string file, string outFile)
        {
            // TODO
        }

        public static void ParseGroupPrj(string file, string outFile)
        {
            // TODO
        }

        public static void ParseDcr(string file, string outFile)
        {
            // TODO
        }

        public static void ParseBpr(string file, string outFile)
        {
            // TODO
        }

        public static void ParseBpf(string file, string outFile)
        {
            // TODO
        }

        public static void ParseDef(string file, string outFile)
        {
            // TODO
        }

        public static void ParseCbPrj(string file, string outFile)
        {
            // TODO
        }

        public static void ParseBdsPrj(string file, string outFile)
        {
            // TODO
        }

        public static void ParseManifest(string file, string outFile)
        {
            // TODO
        }

        public static void ParseDpk(string file, string outFile)
        {
            // TODO
        }

        public static void ParseBpg(string file, string outFile)
        {
            // TODO
        }

        public static void ParseBpl(string file, string outFile)
        {
            // TODO
        }
    }
}