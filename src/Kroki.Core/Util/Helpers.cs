using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Core.Util
{
    public static class Helpers
    {
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

        public static (string file, string content) Translate(string path, SourceText code, string rSpace)
        {
            var simpleName = Path.GetFileName(path);
            var hintName = $"{simpleName}.cs";

            var nowDate = DateTime.Now.ToString("s");
            var genCode = new StringBuilder();
            genCode.AppendLine();
            genCode.AppendLine($"// Generated at {nowDate}");
            genCode.AppendLine();

            string processed;
            try
            {
                processed = DelphiParser.Parse(simpleName, code, rSpace);
            }
            catch (Exception parseEx)
            {
                processed = $"/* {parseEx} */";
            }
            var genText = genCode + processed + Environment.NewLine;
            return (hintName, genText);
        }
    }
}