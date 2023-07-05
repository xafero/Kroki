using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Kroki.Core.Model;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Core.Util
{
    public static class Helpers
    {
        public static readonly Encoding Utf = Encoding.UTF8;

        public static void TransformComment(List<string> pasLines, TextWriter code)
        {
            var doComment = false;
            var commentLines = new List<string>();
            var pasCopy = pasLines.ToArray();
            for (var i = 0; i < pasCopy.Length; i++)
            {
                var pasLine = pasCopy[i];
                if (pasLine.StartsWith("{") && pasCopy[i + 1].Contains("Source "))
                    doComment = true;
                if (doComment)
                {
                    commentLines.Add(pasLine);
                    pasLines.Remove(pasLine);
                }
                if (pasLine.StartsWith("}"))
                    break;
            }

            foreach (var rawComLine in commentLines)
            {
                var comLine = rawComLine;
                if (comLine is "{" or "}")
                    continue;
                if (comLine.Contains("ae"))
                {
                    var comChars = comLine.ToCharArray();
                    for (var i = 0; i < comChars.Length; i++)
                    {
                        if (comChars[i] == 65533)
                            comChars[i] = 'ä';
                        else
                            continue;
                        comLine = new string(comChars);
                    }
                }
                code.WriteLine($"// {comLine}");
            }
            if (commentLines.Count >= 1)
                code.WriteLine();
        }

        public static Translated Translate(string path, SourceText code,
            string rSpace = "Kroki.Example", bool includeDate = true)
        {
            var simpleName = Path.GetFileName(path);
            var hintName = $"{simpleName}.cs";

            var nowDate = DateTime.Now.ToString("s");
            var genCode = new StringBuilder();
            if (includeDate)
            {
                genCode.AppendLine();
                genCode.AppendLine($"// Generated at {nowDate}");
                genCode.AppendLine();
            }

            TranslateError? error = null;
            string processed;
            try
            {
	            processed = DelphiParser.Parse(simpleName, code, rSpace);
            }
            catch (Exception parseEx)
            {
	            error = new TranslateError(path, code, parseEx, hintName);
	            processed = $"/* {parseEx} */";
            }
			var genText = genCode + processed + Environment.NewLine;
			return new Translated(hintName, genText, error);
        }

        public static SourceText ReadSource(string file)
        {
            var text = File.ReadAllText(file, Utf);
            return LoadSource(text);
        }

        public static SourceText LoadSource(string text)
        {
	        var src = SourceText.From(text, Utf);
	        return src;
        }

		public static string Clean(string text) => text.Replace("\r\n", "\n");
	}
}