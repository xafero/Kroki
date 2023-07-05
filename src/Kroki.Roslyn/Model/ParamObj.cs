using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
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
            => Parameter(ParseToken(Names.CleanName(Name)))
                .WithType(ParseTypeName(Type));
    }
}