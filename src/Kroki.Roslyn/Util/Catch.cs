using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Roslyn.Util
{
	public record Catch(TypeSyntax? Type, SyntaxToken? Name,
		IEnumerable<StatementSyntax> Code);
}