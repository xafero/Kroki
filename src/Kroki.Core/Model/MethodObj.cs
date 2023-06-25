using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    internal sealed class MethodObj : CompileObj<MemberDeclarationSyntax>
    {
        public MethodObj(string name)
        {
            Visibility = Visibility.Public;
            ReturnType = "void";
            IsStatic = false;
            Name = name;
            Params = new List<CompileObj<ParameterSyntax>>();
        }

        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public string ReturnType { get; }
        public string Name { get; }
        public List<CompileObj<ParameterSyntax>> Params { get; }

        public override MemberDeclarationSyntax Create()
            => MethodDeclaration(ParseTypeName(ReturnType), Name)
                .AddModifiers(Visibility.AsModifier(IsStatic))
                .AddParameterListParameters(Params.AsArray())
                .WithBody(Block());
    }
}