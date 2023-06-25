using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    internal sealed class ClassObj : CompileObj<MemberDeclarationSyntax>
    {
        public ClassObj(string name)
        {
            Visibility = Visibility.Public;
            IsStatic = false;
            Name = name;
            Members = new List<CompileObj<MemberDeclarationSyntax>>();
        }

        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public string Name { get; }
        public List<CompileObj<MemberDeclarationSyntax>> Members { get; }

        public override MemberDeclarationSyntax Create()
            => ClassDeclaration(Identifier(Name)).AddModifiers(Visibility.AsModifier(IsStatic))
                .AddMembers(Members.AsArray());
    }
}