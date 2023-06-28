using System;
using System.Collections.Generic;
using System.Linq;
using Kroki.Core.API;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Code
{
    public static class Coding
    {
        public static LocalDeclarationStatementSyntax Assign(string type, string name, object value,
            bool isConst = false)
        {
            var lds = LocalDeclarationStatement(VariableDeclaration(ParseTypeName(type))
                .AddVariables(VariableDeclarator(name).WithInitializer(EqualsValueClause(Expr(value)))));
            return isConst ? lds.AddModifiers(Visibility.None.AsModifier(isConst: isConst)) : lds;
        }

        public static ExpressionStatementSyntax Assign(string name, object value)
            => ExpressionStatement(AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression, IdentifierName(name), Expr(value)));

        public static AssignmentExpressionSyntax Assign3(ExpressionSyntax name, ExpressionSyntax value)
            => AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, name, value);

        public static ReturnStatementSyntax Return(object? value = null)
        {
            var ex = value == null ? null : Expr(value);
            return ReturnStatement(ex);
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

        private static ExpressionSyntax GetSingle(ExpressionSyntax prefix)
        {
            return GetMultiple(prefix).First();
        }

        private static SeparatedSyntaxList<ExpressionSyntax> GetMultiple(ExpressionSyntax prefix)
        {
            var a = (prefix as ImplicitArrayCreationExpressionSyntax)?.Initializer.Expressions;
            return a ?? SeparatedList(new[] { prefix });
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
            }
            throw new InvalidOperationException($"{prefix} / {statement} ({statement.GetType()}) ?");
        }

        public static TryStatementSyntax Try(IEnumerable<StatementSyntax> @try,
            IEnumerable<StatementSyntax>? @finally = null)
        {
            var block = Block(@try);
            var catches = List<CatchClauseSyntax>();
            var fin = @finally == null ? null : FinallyClause(Block(@finally));
            return TryStatement(block, catches, fin);
        }

        public static ForStatementSyntax For(string loop, string start, string end, bool isDown,
            IEnumerable<StatementSyntax> s)
        {
            VariableDeclarationSyntax? declaration = null;
            var ini = SeparatedList<ExpressionSyntax>(new[]
            {
                AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(loop),
                    LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(start)))
            });
            var cond = BinaryExpression(
                isDown ? SyntaxKind.GreaterThanOrEqualExpression : SyntaxKind.LessThanOrEqualExpression,
                IdentifierName(loop), ParseExpression(end));
            var inc = SeparatedList<ExpressionSyntax>(new[]
            {
                PostfixUnaryExpression(isDown
                    ? SyntaxKind.PostDecrementExpression
                    : SyntaxKind.PostIncrementExpression, IdentifierName(loop))
            });
            var statement = Block(s);
            return ForStatement(declaration, ini, cond, inc, statement);
        }

        public static InvocationExpressionSyntax Invoke2(string owner, string method, IEnumerable<object> a)
        {
            Mapping.Replace(ref owner, ref method);

            var ar = a.Select(t => Argument(Expr(t)));
            var args = ArgumentList(SeparatedList(ar));
            ExpressionSyntax acc = string.IsNullOrWhiteSpace(owner)
                ? IdentifierName(method)
                : MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(owner), IdentifierName(method));
            return InvocationExpression(acc, args);
        }

        public static ExpressionStatementSyntax Invoke(string owner, string method, IEnumerable<ArgumentSyntax> a)
        {
            Mapping.Replace(ref owner, ref method);
            var args = ArgumentList(SeparatedList(a));
            return ExpressionStatement(InvocationExpression(MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression, IdentifierName(owner),
                IdentifierName(method)), args));
        }

        public static ArgumentSyntax Arg(string text)
        {
            if (text.StartsWith("'") && text.EndsWith("'"))
                text = text.Trim('\'');
            return Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(text)));
        }

        public static ArgumentSyntax ArgN(string text)
        {
            return Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(text)));
        }

        public static IEnumerable<ArgumentSyntax> Arg(this IEnumerable<StatementSyntax> stat)
        {
            foreach (var expr in stat.OfType<ExpressionStatementSyntax>())
                yield return Argument(expr.Expression);
        }

        public static string? GetQuote(string text)
            => text.StartsWith("'") && text.EndsWith("'") ? text.Trim('\'') : null;

        public static ExpressionSyntax Expr(object obj)
        {
            switch (obj)
            {
                case ExpressionSyntax e:
                    return e;
                case bool b:
                    return ExprBool(b);
                case string s:
                    return ExprStr(s);
            }
            throw new InvalidOperationException($"{obj} ({obj.GetType()})");
        }

        public static ExpressionSyntax ExprBool(bool b)
        {
            return LiteralExpression(b ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
        }

        public static ExpressionSyntax ExprNum(string text)
        {
            return LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(text));
        }

        public static ExpressionSyntax ExprNull()
        {
            return LiteralExpression(SyntaxKind.NullLiteralExpression);
        }

        public static ExpressionSyntax ExprDefault()
        {
            return LiteralExpression(SyntaxKind.DefaultLiteralExpression);
        }

        public static BreakStatementSyntax Break()
        {
            return BreakStatement();
        }

        public static ExpressionSyntax ExprStr(string s)
        {
            if (GetQuote(s) is { } sl)
            {
                return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(sl));
            }
            return IdentifierName(s);
        }

        public static ParenthesizedExpressionSyntax Paren(object value)
        {
            return ParenthesizedExpression(Expr(value));
        }

        public static PrefixUnaryExpressionSyntax Not(object value)
        {
            const SyntaxKind op = SyntaxKind.LogicalNotExpression;
            return PrefixUnaryExpression(op, Expr(value));
        }

        public static BinaryExpressionSyntax Binary(BinaryMode bm, object left, object right)
        {
            SyntaxKind op;
            switch (bm)
            {
                case BinaryMode.Plus:
                    op = SyntaxKind.AddExpression;
                    break;
                case BinaryMode.Minus:
                    op = SyntaxKind.SubtractExpression;
                    break;
                case BinaryMode.Multiply:
                    op = SyntaxKind.MultiplyExpression;
                    break;
                case BinaryMode.Divide:
                    op = SyntaxKind.DivideExpression;
                    break;
                case BinaryMode.BitAnd:
                    op = SyntaxKind.BitwiseAndExpression;
                    break;
                default:
                    throw new InvalidOperationException($"{bm} ?!");
            }
            var leftO = Expr(left);
            var rightO = Expr(right);
            return BinaryExpression(op, leftO, rightO);
        }

        public static ExpressionSyntax Array2(IEnumerable<ExpressionSyntax?> values)
        {
            var v = SeparatedList(values.Select(v => v ?? ExprNull()));
            return ImplicitArrayCreationExpression(InitializerExpression(SyntaxKind.ArrayInitializerExpression, v));
        }

        public static ExpressionSyntax Unary2(UnaryMode bm, ExpressionSyntax left)
        {
            SyntaxKind op;
            switch (bm)
            {
                case UnaryMode.Not:
                    op = SyntaxKind.LogicalNotExpression;
                    break;
                default:
                    throw new InvalidOperationException($"{bm} ?!");
            }
            return PrefixUnaryExpression(op, left);
        }

        public static ExpressionSyntax Binary2(BinaryMode bm, ExpressionSyntax left, ExpressionSyntax right)
        {
            SyntaxKind op;
            switch (bm)
            {
                case BinaryMode.Plus:
                    op = SyntaxKind.AddExpression;
                    break;
                case BinaryMode.Minus:
                    op = SyntaxKind.SubtractExpression;
                    break;
                case BinaryMode.Multiply:
                    op = SyntaxKind.MultiplyExpression;
                    break;
                case BinaryMode.Divide:
                    op = SyntaxKind.DivideExpression;
                    break;
                case BinaryMode.Mod:
                    op = SyntaxKind.ModuloExpression;
                    break;
                case BinaryMode.IntDiv:
                    op = SyntaxKind.DivideExpression;
                    break;
                case BinaryMode.BitAnd:
                    op = SyntaxKind.BitwiseAndExpression;
                    break;
                case BinaryMode.And:
                    op = SyntaxKind.LogicalAndExpression;
                    break;
                case BinaryMode.Or:
                    op = SyntaxKind.LogicalOrExpression;
                    break;
                case BinaryMode.Xor:
                    op = SyntaxKind.ExclusiveOrExpression;
                    break;
                case BinaryMode.Greater:
                    op = SyntaxKind.GreaterThanExpression;
                    break;
                case BinaryMode.GreaterEq:
                    op = SyntaxKind.GreaterThanOrEqualExpression;
                    break;
                case BinaryMode.Less:
                    op = SyntaxKind.LessThanExpression;
                    break;
                case BinaryMode.Equal:
                    op = SyntaxKind.EqualsExpression;
                    break;
                case BinaryMode.EqualNot:
                    op = SyntaxKind.NotEqualsExpression;
                    break;

                case BinaryMode.In:
                    return Invoke3(left, "In", right);
                case BinaryMode.Dot:
                    return Access(left, right);
                case BinaryMode.ColonEq:
                    return Assign3(left, right);
                default:
                    throw new InvalidOperationException($"{bm} ?!");
            }
            return BinaryExpression(op, left, right);
        }

        private static ExpressionSyntax Invoke3(ExpressionSyntax owner, string method, ExpressionSyntax arg)
        {
            var member = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, owner, IdentifierName(method));
            return InvocationExpression(member, ArgumentList(SeparatedList(new[] { Argument(arg) })));
        }

        public static ExpressionSyntax Access(ExpressionSyntax left, ExpressionSyntax right)
        {
            return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, left, (SimpleNameSyntax)right);
        }

        public static StatementSyntax AsStat(this ExpressionSyntax es)
        {
            return ExpressionStatement(es);
        }
    }
}