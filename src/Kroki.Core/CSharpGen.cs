using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core
{
    public static class CSharpGen
    {
        public static ClassDeclarationSyntax CreateClass(string name)
        {
            return ClassDeclaration(Identifier(name)).AddModifiers(Token(SyntaxKind.PublicKeyword));
        }

        public static NamespaceDeclarationSyntax CreateNamespace(string name,
            IEnumerable<MemberDeclarationSyntax> members)
        {
            return NamespaceDeclaration(ParseName(name)).AddMembers(members.ToArray());
        }

        public static string WriteToStr(NamespaceDeclarationSyntax ns)
        {
            using var writer = new StringWriter();
            ns.NormalizeWhitespace().WriteTo(writer);
            return writer.ToString();
        }

        public static MethodDeclarationSyntax CreateMain()
        {
            return MethodDeclaration(ParseTypeName("void"), "Main")
                .AddParameterListParameters(Parameter(ParseToken("args"))
                    .WithType(ParseTypeName("string[]")))
                .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword))
                .WithBody(Block());
        }
    }
}