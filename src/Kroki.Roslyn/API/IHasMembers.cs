using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.API
{
    public interface IHasMembers
    {
        List<CompileObj<MemberDeclarationSyntax>> Members { get; }
    }
}