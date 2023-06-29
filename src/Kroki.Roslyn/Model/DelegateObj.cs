using System.Collections.Generic;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class DelegateObj : CompileObj<MemberDeclarationSyntax>, ITypedDef, IHasParams
    {
        public DelegateObj(string name)
        {
            Visibility = Visibility.Public;
            Name = name;
            ReturnType = "void";
            Params = new List<CompileObj<ParameterSyntax>>();
        }

        public Visibility Visibility { get; set; }
        public string Name { get; }
        public string ReturnType { get; set; }
        public List<CompileObj<ParameterSyntax>> Params { get; }

        public override MemberDeclarationSyntax Create()
            => DelegateDeclaration(ParseTypeName(ReturnType), Name)
                .AddModifiers(Visibility.AsModifier())
                .AddParameterListParameters(Params.AsArray());
    }
}