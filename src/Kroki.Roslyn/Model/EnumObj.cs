using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class EnumObj : CompileObj<MemberDeclarationSyntax>, ITypedDef
    {
        public EnumObj(string name)
        {
            Visibility = Visibility.Public;
            Name = name;
        }

        public Visibility Visibility { get; set; }
        public string Name { get; }

        public override MemberDeclarationSyntax Create()
            => EnumDeclaration(Identifier(Name))
                .AddModifiers(Visibility.AsModifier())
                .AddMembers(); // TODO
    }
}