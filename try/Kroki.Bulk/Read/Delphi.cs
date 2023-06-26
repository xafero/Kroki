using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Bulk.Read
{
    internal static class Delphi
    {
        private static readonly Encoding Utf = Encoding.UTF8;

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

            const string rootSpace = "Kroki.Example";
            Helpers.TransformComment(pasLines, code);

            var text = File.ReadAllText(file, Utf);
            var src = SourceText.From(text, Utf);

            var (_, csCode) = Helpers.Translate(file, src, rootSpace);
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