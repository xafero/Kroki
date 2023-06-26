using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.API
{
    internal interface IHasMembers
    {
        List<CompileObj<MemberDeclarationSyntax>> Members { get; }
    }
}