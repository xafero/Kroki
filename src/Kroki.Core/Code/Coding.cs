using System;
using System.Collections.Generic;
using System.Linq;
using Kroki.Core.API;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Code
{
    public static class Coding
    {
        public static LocalDeclarationStatementSyntax Assign(string type, string name, object value)
            => LocalDeclarationStatement(VariableDeclaration(ParseTypeName(type))
                .AddVariables(VariableDeclarator(name)
                    .WithInitializer(EqualsValueClause(Expr(value)))));

        public static ExpressionStatementSyntax Assign(string name, object value)
            => ExpressionStatement(AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression, IdentifierName(name), Expr(value)));

        public static ReturnStatementSyntax Return(object? value = null)
        {
            var ex = value == null ? null : Expr(value);
            return ReturnStatement(ex);
        }

        public static StatementSyntax For(string loop, string start, string end, bool isDown,
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

        private static string? GetQuote(string text)
            => text.StartsWith("'") && text.EndsWith("'") ? text.Trim('\'') : null;

        public static ExpressionSyntax Expr(object obj)
        {
            switch (obj)
            {
                case ExpressionSyntax e:
                    return e;
                case bool b:
                    return LiteralExpression(b
                        ? SyntaxKind.TrueLiteralExpression
                        : SyntaxKind.FalseLiteralExpression);
                case string s:
                    if (GetQuote(s) is { } sl)
                    {
                        return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(sl));
                    }
                    return IdentifierName(s);
            }
            throw new InvalidOperationException($"{obj} ({obj.GetType()})");
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

        public static StatementSyntax AsStat(this ExpressionSyntax es)
        {
            return ExpressionStatement(es);
        }
    }
}