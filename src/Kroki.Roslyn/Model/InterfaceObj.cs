using System.Collections.Generic;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class InterfaceObj : CompileObj<MemberDeclarationSyntax>, ITypeDef
    {
	    public InterfaceObj(string name)
        {
            Visibility = Visibility.Public;
            Name = name;
            Members = new List<CompileObj<MemberDeclarationSyntax>>();
        }

        public Visibility Visibility { get; set; }
        public string Name { get; }
        public List<CompileObj<MemberDeclarationSyntax>> Members { get; }

        public override MemberDeclarationSyntax Create()
            => InterfaceDeclaration(Identifier(Name))
                .AddModifiers(Visibility.AsModifier())
                .AddMembers(Members.AsArray());
    }
}