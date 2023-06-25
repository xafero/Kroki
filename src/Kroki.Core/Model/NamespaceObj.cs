using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    internal sealed class NamespaceObj : CompileObj<NamespaceDeclarationSyntax>, IHasMembers
    {
        public NamespaceObj(string name)
        {
            Name = name;
            Members = new List<CompileObj<MemberDeclarationSyntax>>();
        }

        public string Name { get; }
        public List<CompileObj<MemberDeclarationSyntax>> Members { get; }

        public override NamespaceDeclarationSyntax Create()
            => NamespaceDeclaration(ParseName(Name))
                .AddMembers(Members.AsArray());

        public override string ToString()
        {
            var ns = Create();
            using var writer = new StringWriter();
            ns.NormalizeWhitespace().WriteTo(writer);
            return writer.ToString();
        }
    }
}