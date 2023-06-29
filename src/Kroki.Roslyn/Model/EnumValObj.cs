using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class EnumValObj : CompileObj<EnumMemberDeclarationSyntax>
    {
        public EnumValObj(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public object? Value { get; set; }

        public override EnumMemberDeclarationSyntax Create()
        {
            var ev = EnumMemberDeclaration(Identifier(Name));
            if (Value != null)
            {
                var ee = Express.AsValue(Value);
                ev = ev.WithEqualsValue(EqualsValueClause(ee));
            }
            return ev;
        }
    }
}