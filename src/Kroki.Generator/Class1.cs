using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Generator
{
    public class Class1
    {
        private static ClassDeclarationSyntax CreateClass(string name)
            => ClassDeclaration(Identifier(name)).AddModifiers(Token(SyntaxKind.PublicKeyword));

        public void Try1()
        {
            var types = new[] { "Hello1", "Hello2" };

            var members = types.Select(CreateClass).Cast<MemberDeclarationSyntax>().ToArray();

            var ns = NamespaceDeclaration(ParseName("CodeGen")).AddMembers(members);

            using var streamWriter = new StreamWriter(@"generated.cs", false);
            ns.NormalizeWhitespace().WriteTo(streamWriter);
        }
    }
}