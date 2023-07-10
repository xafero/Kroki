using System.Collections.Generic;
using System.Linq;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
	public sealed class ClassObj : CompileObj<MemberDeclarationSyntax>, ITypeDef
	{
		public ClassObj(string name)
		{
			Visibility = Visibility.Public;
			IsStatic = false;
			Name = name;
			Members = new List<CompileObj<MemberDeclarationSyntax>>();
			Base = new List<TypeSyntax>();
		}

		public Visibility Visibility { get; set; }
		public bool IsStatic { get; set; }
		public string Name { get; }
		public List<CompileObj<MemberDeclarationSyntax>> Members { get; }
		public List<TypeSyntax> Base { get; }
		public bool IsPartial { get; set; }

		public override MemberDeclarationSyntax Create()
		{
			var res = ClassDeclaration(Identifier(Name))
				.AddModifiers(Visibility.AsModifier(IsStatic, isPartial: IsPartial))
				.AddMembers(Members.AsArray());
			if (Base.Count >= 1)
			{
				var bases = Base.Select(SimpleBaseType)
					.Cast<BaseTypeSyntax>().ToArray();
				res = res.AddBaseListTypes(bases);
			}
			return res;
		}
	}
}