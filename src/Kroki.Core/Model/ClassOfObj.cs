using Kroki.Roslyn.API;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.Model
{
	internal sealed class ClassOfObj : ITypedDef
	{
		public ClassOfObj(string name)
		{
			Name = name;
		}

		public string Name { get; }
		public ExpressionSyntax? Target { get; set; }
	}
}