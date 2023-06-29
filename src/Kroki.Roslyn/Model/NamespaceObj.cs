using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class NamespaceObj : CompileObj<NamespaceDeclarationSyntax>, IHasMembers
    {
        public NamespaceObj(string name)
        {
            Name = name;
            Members = new List<CompileObj<MemberDeclarationSyntax>>();
            Usings = new List<string>
            {
                "System", "System.Collections.Generic", "System.Text"
            };
        }

        public string Name { get; }
        public List<CompileObj<MemberDeclarationSyntax>> Members { get; }
        public List<string> Usings { get; }

        public override NamespaceDeclarationSyntax Create()
            => NamespaceDeclaration(ParseName(Name))
                .AddMembers(Members.AsArray());

        public override string ToString()
        {
            var ns = Create();
            using var writer = new StringWriter();
            var imports = string.Join(Environment.NewLine, Usings.Select(u => $"using {u};"));
            if (imports.Length >= 1)
            {
                writer.WriteLine(imports);
                writer.WriteLine();
            }
            ns.NormalizeWhitespace().WriteTo(writer);
            return writer.ToString();
        }
    }
}