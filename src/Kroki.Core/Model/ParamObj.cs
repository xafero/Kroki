using Kroki.Core.API;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    public sealed class ParamObj : CompileObj<ParameterSyntax>
    {
        public ParamObj(string name)
        {
            Type = "object";
            Name = name;
        }

        public string Type { get; set; }
        public string Name { get; }

        public override ParameterSyntax Create()
            => Parameter(ParseToken(Name))
                .WithType(ParseTypeName(Type));
    }
}