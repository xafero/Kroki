using Kroki.Core.API;
using Kroki.Core.Code;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    public sealed class ConstObj : CompileObj<MemberDeclarationSyntax>
    {
        public ConstObj(string name)
        {
            Visibility = Visibility.Public;
            Name = name;
            FieldType = "int";
            Value = "42";
        }

        public ConstObj(FieldObj field)
        {
            Visibility = field.Visibility;
            Name = field.Name;
            FieldType = field.FieldType;
            Value = field.Value ?? "null";

            if (int.TryParse(Value, out _)) FieldType = "int";
        }

        public Visibility Visibility { get; set; }
        public string Name { get; }
        public string FieldType { get; set; }
        public string Value { get; set; }

        public override MemberDeclarationSyntax Create()
        {
            var eq = EqualsValueClause(Coding.ExprNum(Value));
            var vd = VariableDeclaration(ParseTypeName(FieldType))
                .AddVariables(VariableDeclarator(Name).WithInitializer(eq));
            var fd = FieldDeclaration(vd)
                .AddModifiers(Visibility.AsModifier(isConst: true));
            return fd;
        }
    }
}