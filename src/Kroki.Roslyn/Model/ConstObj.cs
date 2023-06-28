using Kroki.Core.API;
using Kroki.Core.Code;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    public sealed class ConstObj : CompileObj<MemberDeclarationSyntax>
    {
        public ConstObj(string name, string value = "42")
        {
            Visibility = Visibility.Public;
            Name = name;
            (FieldType, Value) = Values.Parse(value);
        }

        public ConstObj(FieldObj field)
        {
            Visibility = field.Visibility;
            Name = field.Name;
            (FieldType, Value) = Values.Parse(field.Value?.ToString() ?? Values.Null);
        }

        public Visibility Visibility { get; set; }
        public string Name { get; }
        public string FieldType { get; set; }
        public object? Value { get; set; }

        public override MemberDeclarationSyntax Create()
        {
            var eq = EqualsValueClause(Express.AsValue(Value));
            var vd = VariableDeclaration(ParseTypeName(FieldType))
                .AddVariables(VariableDeclarator(Name).WithInitializer(eq));
            var fd = FieldDeclaration(vd)
                .AddModifiers(Visibility.AsModifier(isConst: true));
            return fd;
        }
    }
}