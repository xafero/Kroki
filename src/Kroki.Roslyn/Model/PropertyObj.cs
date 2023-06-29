using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class PropertyObj : CompileObj<MemberDeclarationSyntax>
    {
        public PropertyObj(string name)
        {
            Visibility = Visibility.Public;
            PropType = "object";
            IsStatic = false;
            Name = name;
        }

        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public string PropType { get; set; }
        public string Name { get; }

        public override MemberDeclarationSyntax Create()
            => PropertyDeclaration(ParseTypeName(PropType), Name)
                .AddModifiers(Visibility.AsModifier(IsStatic))
                .AddAccessorListAccessors(MakeAuto());

        private AccessorDeclarationSyntax[] MakeAuto()
        {
            return new[]
            {
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            };
        }
    }
}