using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Code
{
    public static class Construct
    {
        public static StatementSyntax AsStat(this ExpressionSyntax es)
        {
            return ExpressionStatement(es);
        }
    }
}