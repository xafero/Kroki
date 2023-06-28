using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Kroki.Core.API;
using static Kroki.Core.Util.RoExtensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Code
{
    public static class Express
    {
        public static ExpressionSyntax AsValue(object? obj)
        {
            if (obj == null) return NullValue();
            switch (obj)
            {
                case ExpressionSyntax e: return e;
                case bool b: return AsBoolValue(b);
                case int i: return AsNumberValue(i.ToString());
                case string s: return AsTextValue(s);
            }
            throw new InvalidOperationException($"{obj} ({obj.GetType()})");
        }

        public static ExpressionSyntax DefaultValue()
            => LiteralExpression(SyntaxKind.DefaultLiteralExpression);

        public static ExpressionSyntax NullValue()
            => LiteralExpression(SyntaxKind.NullLiteralExpression);

        public static ExpressionSyntax AsBoolValue(bool b)
            => LiteralExpression(b ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

        public static ExpressionSyntax AsNumberValue(string text)
            => LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(text));

        public static ExpressionSyntax AsTextValue(string s)
        {
            if (GetQuote(s) is { } sl)
                return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(sl));
            return IdentifierName(s);
        }

        public static SimpleNameSyntax ToName(ExpressionSyntax syntax)
        {
            return (SimpleNameSyntax)syntax;
        }

        public static ExpressionSyntax Array(IEnumerable<ExpressionSyntax?> values)
        {
            var v = SeparatedList(values.Select(v => v ?? NullValue()));
            return ImplicitArrayCreationExpression(InitializerExpression(SyntaxKind.ArrayInitializerExpression, v));
        }

        public static ExpressionSyntax Unary(UnaryMode bm, ExpressionSyntax left)
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

        public static ExpressionSyntax Binary(BinaryMode bm, ExpressionSyntax left, ExpressionSyntax right)
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
                    return Invoke(left, nameof(BinaryMode.In), right);
                case BinaryMode.Dot:
                    return Access(left, right);
                case BinaryMode.ColonEq:
                    return Assign(left, right);
                default:
                    throw new InvalidOperationException($"{bm} ?!");
            }
            return BinaryExpression(op, left, right);
        }

        public static AssignmentExpressionSyntax Assign(ExpressionSyntax name, ExpressionSyntax value)
        {
            return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, name, value);
        }

        public static ExpressionSyntax Access(ExpressionSyntax left, ExpressionSyntax right)
        {
            return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, left, ToName(right));
        }

        public static ExpressionSyntax Invoke(ExpressionSyntax owner, string method, ExpressionSyntax arg)
        {
            return Invoke(owner, IdentifierName(method), new[] { Arg(arg) });
        }

        public static ArgumentSyntax Arg(ExpressionSyntax syntax) => Argument(syntax);

        private static ExpressionSyntax Invoke(ExpressionSyntax owner, SimpleNameSyntax method,
            IEnumerable<ArgumentSyntax> args)
        {
            var member = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, owner, method);
            return InvocationExpression(member, ArgumentList(SeparatedList(args)));
        }
    }
}