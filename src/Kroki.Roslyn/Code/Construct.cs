﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Roslyn.Code.Express;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Code
{
    public static class Construct
    {
        public static StatementSyntax AsStat(this ExpressionSyntax es)
        {
            return ExpressionStatement(es);
        }

        public static TryStatementSyntax Try(IEnumerable<StatementSyntax> @try,
	        IEnumerable<StatementSyntax>? @finally = null, IEnumerable<Catch>? @catch = null)
        {
	        var block = Block(@try);
	        var catches = List<CatchClauseSyntax>();
	        if (@catch != null)
		        foreach (var item in @catch)
		        {
			        var id = item.Type is { } vt
				        ? item.Name is { } vn
					        ? CatchDeclaration(vt, vn)
					        : CatchDeclaration(vt)
				        : null;
			        var ib = Block(item.Code);
			        catches = catches.Add(CatchClause(id, null, ib));
		        }
	        var fin = @finally == null ? null : FinallyClause(Block(@finally));
	        return TryStatement(block, catches, fin);
        }

        public static IfStatementSyntax If(ExpressionSyntax cond, IEnumerable<StatementSyntax> then,
            IEnumerable<StatementSyntax>? @else = null)
        {
            var el = @else == null ? null : ElseClause(Block(@else));
            return IfStatement(cond, Block(then), el);
        }

        public static WhileStatementSyntax While(ExpressionSyntax cond, IEnumerable<StatementSyntax> loop)
        {
            var block = Block(loop);
            return WhileStatement(cond, block);
        }

        public static DoStatementSyntax Repeat(ExpressionSyntax cond, IEnumerable<StatementSyntax> loop)
        {
            var block = Block(loop);
            var notCond = Not(Paren(cond));
            return DoStatement(block, notCond);
        }

        public static BreakStatementSyntax Break() => BreakStatement();
        public static ContinueStatementSyntax Continue() => ContinueStatement();
        public static ReturnStatementSyntax Return(ExpressionSyntax? value = null) => ReturnStatement(value);

        public static ThrowStatementSyntax Throw(ExpressionSyntax? value = null) => ThrowStatement(value);

        public static SwitchStatementSyntax Switch(ExpressionSyntax cond, (ExpressionSyntax v,
            IEnumerable<StatementSyntax> c)[] tuples)
        {
            var switches = tuples.Select(t =>
            {
                var labels = GetMultiple(t.v).Select(m =>
                {
                    SwitchLabelSyntax sc = CaseSwitchLabel(m);
                    if (t.v.Kind() == SyntaxKind.DefaultLiteralExpression) sc = DefaultSwitchLabel();
                    return sc;
                });
                var sy = List(labels);
                var sst = List(t.c.Concat(new[] { Break() }));
                return SwitchSection(sy, sst);
            }).ToArray();
            var sections = List(switches);
            return SwitchStatement(cond, sections);
        }

        public static BlockSyntax With(ExpressionSyntax prefix, IEnumerable<StatementSyntax> loop)
        {
            var stats = loop.Select(s => Combine(prefix, s)).ToArray();
            return Block(stats);
        }

        private static StatementSyntax Combine(ExpressionSyntax prefix, CSharpSyntaxNode statement)
        {
	        switch (statement)
	        {
		        case ExpressionStatementSyntax ess:
			        return Combine(prefix, ess.Expression);
		        case AssignmentExpressionSyntax aes:
			        var p = GetSingle(prefix);
			        var l = aes.Left;
			        if (l is MemberAccessExpressionSyntax && statement is AssignmentExpressionSyntax aas)
				        return aas.AsStat();
			        var upd = aes.WithLeft(Access(p, l));
			        return upd.AsStat();

		        case IfStatementSyntax iss:
			        return iss;
		        case SwitchStatementSyntax iss:
			        return iss;
		        case TryStatementSyntax tri:
			        return tri;
		        case ForStatementSyntax fsi:
			        return fsi;
                case ForEachStatementSyntax fei:
	                return fei;
                case ReturnStatementSyntax rsi:
	                return rsi;
		        case InvocationExpressionSyntax ies:
	                return ies.AsStat();
                case BlockSyntax bs:
	                return bs;
	        }
	        throw new InvalidOperationException($"{prefix} / {statement} ({statement.GetType()}) ?");
        }

        public static ForStatementSyntax For(string loop, string start, string end, bool isDown,
            IEnumerable<StatementSyntax> statements)
        {
            var ini = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(loop), LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(start)));
            var cond = BinaryExpression(
                isDown ? SyntaxKind.GreaterThanOrEqualExpression : SyntaxKind.LessThanOrEqualExpression,
                IdentifierName(loop), ParseExpression(end));
            var inc = PostfixUnaryExpression(isDown
                ? SyntaxKind.PostDecrementExpression
                : SyntaxKind.PostIncrementExpression, IdentifierName(loop));
            return For(ini, cond, inc, statements);
        }

        public static StatementSyntax For(ExpressionSyntax loop, ExpressionSyntax start,
            ExpressionSyntax end, bool isDown, IEnumerable<StatementSyntax> statements)
        {
            var ini = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, loop, start);
            var kind = isDown ? SyntaxKind.GreaterThanOrEqualExpression : SyntaxKind.LessThanOrEqualExpression;
            var cond = BinaryExpression(kind, loop, end);
            var mode = isDown ? SyntaxKind.PostDecrementExpression : SyntaxKind.PostIncrementExpression;
            var post = PostfixUnaryExpression(mode, loop);
            return For(ini, cond, post, statements);
        }

        public static ForStatementSyntax For(ExpressionSyntax init, ExpressionSyntax cond, ExpressionSyntax post,
            IEnumerable<StatementSyntax> s, VariableDeclarationSyntax? declaration = null)
        {
            var ini = SeparatedList(new[] { init });
            var inc = SeparatedList(new[] { post });
            var statement = Block(s);
            return ForStatement(declaration, ini, cond, inc, statement);
        }

        public static ForEachStatementSyntax ForEach(ExpressionSyntax init,
	        ExpressionSyntax expression, IEnumerable<StatementSyntax> s)
        {
	        var vt = ParseTypeName("var");
	        var id = init.GetFirstToken();
	        var statement = Block(s);
	        return ForEachStatement(vt, id, expression, statement);
        }

        public static StatementSyntax InvokeS(string name, params ArgumentSyntax[] args)
        {
	        var exp = Invoke(Name(name), args);
	        return exp.AsStat();
        }

        public static StatementSyntax Comment(this StatementSyntax statement,
	        string text, bool trail = false)
        {
	        var plain = $"// {text}";
	        var comment = SyntaxFactory.Comment(plain);
	        return trail
		        ? statement.WithTrailingTrivia(comment)
		        : statement.WithLeadingTrivia(comment);
        }
    }
}