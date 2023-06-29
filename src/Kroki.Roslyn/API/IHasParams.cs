using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Roslyn.API
{
    public interface IHasParams
    {
        List<CompileObj<ParameterSyntax>> Params { get; }
    }
}