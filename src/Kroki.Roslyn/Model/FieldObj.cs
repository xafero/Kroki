using Kroki.Core.API;
using Kroki.Core.Code;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    public sealed class FieldObj : CompileObj<MemberDeclarationSyntax>
    {
        public FieldObj(string name, string? value = null)
        {
            Visibility = Visibility.Public;
            FieldType = "object";
            IsStatic = false;
            IsReadOnly = false;
            Name = name;
            if (value != null)
                (FieldType, Value) = Values.Parse(value);
        }

        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public bool IsReadOnly { get; set; }
        public string FieldType { get; set; }
        public string Name { get; }
        public object? Value { get; set; }

        public override MemberDeclarationSyntax Create()
        {
            var vd = VariableDeclarator(Name);
            var eq = Value == null ? null : EqualsValueClause(Express.AsValue(Value));
            if (eq != null)
                vd = vd.WithInitializer(eq);
            return FieldDeclaration(VariableDeclaration(ParseTypeName(FieldType)).AddVariables(vd))
                .AddModifiers(Visibility.AsModifier(IsStatic, IsReadOnly));
        }
    }
}