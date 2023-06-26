using Kroki.Core.API;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    internal sealed class FieldObj : CompileObj<MemberDeclarationSyntax>
    {
        public FieldObj(string name)
        {
            Visibility = Visibility.Public;
            FieldType = "object";
            IsStatic = false;
            IsReadOnly = false;
            Name = name;
        }

        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public bool IsReadOnly { get; set; }
        public string FieldType { get; set; }
        public string Name { get; }

        public override MemberDeclarationSyntax Create()
            => FieldDeclaration(VariableDeclaration(ParseTypeName(FieldType))
                    .AddVariables(VariableDeclarator(Name)))
                .AddModifiers(Visibility.AsModifier(IsStatic, IsReadOnly));
    }
}