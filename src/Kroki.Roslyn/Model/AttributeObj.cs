using Kroki.Roslyn.API;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
	public sealed class AttributeObj : CompileObj<AttributeListSyntax>
	{
		public AttributeObj(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public override AttributeListSyntax Create()
		{
			var arg = Attribute(IdentifierName(Name));
			var sep = SeparatedList(new[] { arg });
			var lst = AttributeList(sep);
			return lst;
		}
	}
}