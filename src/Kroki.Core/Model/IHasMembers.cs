using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.Model
{
    internal interface IHasMembers
    {
        List<CompileObj<MemberDeclarationSyntax>> Members { get; }
    }
}