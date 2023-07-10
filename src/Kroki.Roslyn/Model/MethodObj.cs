﻿using System;
using System.Collections.Generic;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Model
{
	public sealed class MethodObj : CompileObj<MemberDeclarationSyntax>, IHasName, IHasParams
	{
		public MethodObj(string name)
		{
			Visibility = Visibility.Public;
			ReturnType = "void";
			IsStatic = false;
			IsAbstract = false;
			IsRump = false;
			Name = name;
			Params = new List<CompileObj<ParameterSyntax>>();
			Attributes = new List<CompileObj<AttributeListSyntax>>();
			Statements = new List<StatementSyntax>();
		}

		public Visibility Visibility { get; set; }
		public bool IsStatic { get; set; }
		public string ReturnType { get; set; }
		public string Name { get; set; }
		public List<CompileObj<ParameterSyntax>> Params { get; }
		public List<CompileObj<AttributeListSyntax>> Attributes { get; }
		public bool IsAbstract { get; set; }
		public bool IsRump { get; set; }
		public List<StatementSyntax> Statements { get; }
		public bool IsOverride { get; set; }
		public bool IsConstructor { get; set; }

		public override MemberDeclarationSyntax Create()
		{
			BaseMethodDeclarationSyntax bm = IsConstructor
				? ConstructorDeclaration(Name)
				: MethodDeclaration(ParseTypeName(ReturnType), Name);
			var method = bm
				.AddModifiers(IsRump
					? Array.Empty<SyntaxToken>()
					: Visibility.AsModifier(IsStatic, isAbstract: IsAbstract, isOverride: IsOverride))
				.AddAttributeLists(Attributes.AsArray())
				.AddParameterListParameters(Params.AsArray());
			method = IsAbstract
				? method.WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
				: method.AddBodyStatements(Statements.ToArray());
			return method;
		}
	}
}