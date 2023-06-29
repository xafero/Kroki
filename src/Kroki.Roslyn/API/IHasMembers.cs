using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Roslyn.API
{
    public interface IHasMembers
    {
        List<CompileObj<MemberDeclarationSyntax>> Members { get; }
    }
}