using Kroki.Roslyn.API;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.Model
{
	internal sealed class RangeOfVal : ITypedDef
	{
		public RangeOfVal(string name)
		{
			Name = name;
		}

		public string Name { get; }
		public ExpressionSyntax? Start { get; set; }
		public ExpressionSyntax? End { get; set; }
	}
}