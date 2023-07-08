using Kroki.Roslyn.API;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.Model
{
	internal sealed class TypeAlias : ITypedDef
	{
		public TypeAlias(string name)
		{
			Name = name;
		}

		public string Name { get; }
		public ExpressionSyntax? Target { get; set; }
	}
}