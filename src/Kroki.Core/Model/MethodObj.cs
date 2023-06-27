using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Kroki.Core.API;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    public sealed class MethodObj : CompileObj<MemberDeclarationSyntax>, IHasName
    {
        public MethodObj(string name)
        {
            Visibility = Visibility.Public;
            ReturnType = "void";
            IsStatic = false;
            IsAbstract = false;
            Name = name;
            Params = new List<CompileObj<ParameterSyntax>>();
            Statements = new List<StatementSyntax>();
        }

        public Visibility Visibility { get; set; }
        public bool IsStatic { get; set; }
        public string ReturnType { get; set; }
        public string Name { get; set; }
        public List<CompileObj<ParameterSyntax>> Params { get; }
        public bool IsAbstract { get; set; }
        public List<StatementSyntax> Statements { get; }

        public override MemberDeclarationSyntax Create()
        {
            var method = MethodDeclaration(ParseTypeName(ReturnType), Name)
                .AddModifiers(Visibility.AsModifier(IsStatic, isAbstract: IsAbstract))
                .AddParameterListParameters(Params.AsArray());
            method = IsAbstract
                ? method.WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                : method.AddBodyStatements(Statements.ToArray());
            return method;
        }
    }
}