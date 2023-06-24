using System.Text;
using DGrok.Framework;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Core
{
    public static class DelphiParser
    {
        public static string ParsePas(string simpleName, SourceText sourceText)
        {
            var options = new CodeBaseOptions();
            var defines = options.CreateCompilerDefines();
            var loader = new FileLoader(true);
            var cb = new CodeBase(defines, loader);
            var sourceCode = sourceText.ToString();
            cb.AddFileExpectingSuccess(simpleName, sourceCode);

            var bld = new StringBuilder();
            foreach (var error in cb.Errors)
            {
                bld.AppendLine($" /* {error.Name} / {error.FileName} / {error.Content} */ ");
            }
            return bld.ToString();
        }
    }
}