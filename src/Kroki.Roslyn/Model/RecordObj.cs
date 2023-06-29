using System.Collections.Generic;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
    public sealed class RecordObj : CompileObj<MemberDeclarationSyntax>, ITypeDef, IHasParams
    {
        public RecordObj(string name)
        {
            Visibility = Visibility.Public;
            Name = name;
            Params = new List<CompileObj<ParameterSyntax>>();
            Members = new List<CompileObj<MemberDeclarationSyntax>>();
        }

        public Visibility Visibility { get; set; }
        public string Name { get; }
        public List<CompileObj<ParameterSyntax>> Params { get; }
        public List<CompileObj<MemberDeclarationSyntax>> Members { get; }

        public override MemberDeclarationSyntax Create()
            => RecordDeclaration(Token(SyntaxKind.RecordKeyword), Identifier(Name))
                .AddModifiers(Visibility.AsModifier())
                .AddParameterListParameters(Params.AsArray())
                .AddMembers(Members.AsArray());
    }
}