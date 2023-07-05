using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static Kroki.Core.Util.Helpers;

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
                var rootSpace = context.Compilation.AssemblyName ?? nameof(Kroki);

                var allPasFiles = context
                    .AdditionalFiles
                    .Where(f => f.Path.EndsWith(".pas", StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

                foreach (var someFile in allPasFiles)
                {
                    var sourceText = someFile.GetText(context.CancellationToken);
                    if (sourceText == null)
                        continue;

                    var (hintName, genText, _) = Translate(someFile.Path, sourceText, rootSpace);
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