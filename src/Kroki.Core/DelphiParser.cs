using System;
using System.Text;
using DGrok.Framework;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Core
{
    public static class DelphiParser
    {
        public static string Parse(string simpleName, SourceText sourceText, string nspName)
        {
            var cb = Parse(simpleName, sourceText);
            var addIt = new StringBuilder();
            var visitor = new CSharpVisitor(nspName);

            foreach (var file in cb.ParsedFiles)
                visitor.Visit(file.Content);

            var code = visitor.ToString();
            return (code + Environment.NewLine + addIt).Trim();
        }

        private static CodeBase Parse(string simpleName, SourceText sourceText)
        {
            var options = new CodeBaseOptions();
            var defines = options.CreateCompilerDefines();
            var loader = new FileLoader(true);
            var cb = new CodeBase(defines, loader);
            var sourceCode = sourceText.ToString();
            cb.AddFileExpectingSuccess(simpleName, sourceCode);
            return cb;
        }
    }
}