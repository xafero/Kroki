using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Generator
{
    [Generator]
    public class CSharpGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            Debug.WriteLine(nameof(GeneratorInitializationContext));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // if (!Debugger.IsAttached) Debugger.Launch();

            try
            {
                var namespaceName = context.Compilation?.AssemblyName ?? nameof(Kroki);

                var allPasFiles = context
                    .AdditionalFiles
                    .Where(f => f.Path.EndsWith(".pas", StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

                foreach (var someFile in allPasFiles)
                {
                    var sourceText = someFile.GetText(context.CancellationToken);
                    if (sourceText == null)
                        continue;

                    var single = Path.GetFileName(someFile.Path);
                    var hintName = $"{single}.gen.cs";
                    var delphiCode = sourceText.ToString();

                    var nowDate = DateTime.Now.ToString("s");
                    var genCode = new StringBuilder();
                    genCode.AppendLine();
                    genCode.AppendLine("// Generated at " + nowDate);
                    genCode.AppendLine("// " + delphiCode.Length + " characters?!");
                    genCode.AppendLine();

                    var genText = genCode.ToString();
                    context.AddSource(hintName, SourceText.From(genText, Encoding.UTF8));
                }
            }
            catch (Exception ex)
            {
                var message = $"Exception: {ex.Message} - {ex.StackTrace}";
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("SI0000",
                    message, message, nameof(Generator), DiagnosticSeverity.Error,
                    isEnabledByDefault: true), Location.None));
            }
        }
    }
}