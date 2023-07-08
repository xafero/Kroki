using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Roslyn.Util.RoExtensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Roslyn.Code
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
                case double d: return AsNumberValue(d.ToString(CultureInfo.InvariantCulture));
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
        {
	        var txt = text;
	        if (txt.StartsWith("$"))
	        {
		        txt = Convert.ToInt64(txt.TrimStart('$'), 16).ToString();
	        }
	        var token = ParseToken(txt.Trim('-'));
	        var r = LiteralExpression(SyntaxKind.NumericLiteralExpression, token);
	        if (txt.StartsWith("-"))
	        {
		        return PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, r);
	        }
	        return r;
        }

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
                case UnaryMode.Minus:
                    op = SyntaxKind.UnaryMinusExpression;
                    break;
                case UnaryMode.Inherit:
                    // TODO: Ignore "inherit" for now
	                return left;
                case UnaryMode.At:
	                // TODO: Ignore "@" for now
	                return left;
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
                case BinaryMode.LessEq:
                    op = SyntaxKind.LessThanOrEqualExpression;
                    break;
                case BinaryMode.Equal:
                    op = SyntaxKind.EqualsExpression;
                    break;
                case BinaryMode.EqualNot:
                    op = SyntaxKind.NotEqualsExpression;
                    break;
                case BinaryMode.Is:
                    op = SyntaxKind.IsExpression;
                    break;
                case BinaryMode.As:
	                op = SyntaxKind.AsExpression;
	                break;
                case BinaryMode.Shr:
	                op = SyntaxKind.RightShiftExpression;
                    break;
                case BinaryMode.Shl:
	                op = SyntaxKind.LeftShiftExpression;
                    break;

                case BinaryMode.Range:
	                return Range(left, right);
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

        public static AssignmentExpressionSyntax Assign(string name, ExpressionSyntax value)
        {
            return Assign(IdentifierName(name), value);
        }

        public static LocalDeclarationStatementSyntax Assign(string type, string name, object value,
            bool isConst = false)
        {
            var lds = LocalDeclarationStatement(VariableDeclaration(ParseTypeName(type))
                .AddVariables(VariableDeclarator(name).WithInitializer(EqualsValueClause(AsValue(value)))));
            return isConst ? lds.AddModifiers(Visibility.None.AsModifier(isConst: isConst)) : lds;
        }

        public static LocalFunctionStatementSyntax Method(MethodObj m)
        {
	        var rt = ParseTypeName(m.ReturnType);
	        var method = LocalFunctionStatement(rt, m.Name)
		        .AddModifiers(System.Array.Empty<SyntaxToken>())
		        .AddParameterListParameters(m.Params.AsArray());
	        method = method.AddBodyStatements(m.Statements.ToArray());
	        return method;
        }

        public static InvocationExpressionSyntax Invoke(string owner, string method, IEnumerable<object> a)
        {
            var ar = a.Select(t => Argument(AsValue(t)));
            var args = ArgumentList(SeparatedList(ar));
            ExpressionSyntax acc = string.IsNullOrWhiteSpace(owner)
                ? IdentifierName(method)
                : MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(owner), IdentifierName(method));
            return InvocationExpression(acc, args);
        }

        public static ExpressionStatementSyntax Invoke(string owner, string method, IEnumerable<ArgumentSyntax> a)
        {
            var args = ArgumentList(SeparatedList(a));
            return ExpressionStatement(InvocationExpression(MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression, IdentifierName(owner),
                IdentifierName(method)), args));
        }

        public static ExpressionSyntax Name(string name) => IdentifierName(name);

        public static ExpressionSyntax Access(ExpressionSyntax left, ExpressionSyntax right)
        {
            return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, left, ToName(right));
        }

        public static ExpressionSyntax Invoke(ExpressionSyntax owner, string method, ExpressionSyntax arg)
        {
            return Invoke(owner, IdentifierName(method), new[] { Arg(arg) });
        }

        public static ArgumentSyntax Arg(this ExpressionSyntax syntax) => Argument(syntax);

        public static IEnumerable<ArgumentSyntax> Arg(this IEnumerable<StatementSyntax> stat)
        {
            foreach (var expr in stat.Cast<ExpressionStatementSyntax>())
                yield return Argument(expr.Expression);
        }

        public static ExpressionSyntax Invoke(ExpressionSyntax? owner, SimpleNameSyntax method,
            IEnumerable<ArgumentSyntax> args)
        {
            ExpressionSyntax member = owner == null
                ? method
                : MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, owner, method);
            return InvocationExpression(member, ArgumentList(SeparatedList(args)));
        }

        public static ExpressionSyntax Invoke(ExpressionSyntax member, IEnumerable<ArgumentSyntax> args)
        {
            return InvocationExpression(member, ArgumentList(SeparatedList(args)));
        }

        public static ExpressionSyntax GetSingle(ExpressionSyntax prefix)
        {
            return GetMultiple(prefix).First();
        }

        public static SeparatedSyntaxList<ExpressionSyntax> GetMultiple(ExpressionSyntax prefix)
        {
            var a = (prefix as ImplicitArrayCreationExpressionSyntax)?.Initializer.Expressions;
            return a ?? SeparatedList(new[] { prefix });
        }

        public static ParenthesizedExpressionSyntax Paren(ExpressionSyntax value)
        {
            return ParenthesizedExpression(value);
        }

        public static ImplicitObjectCreationExpressionSyntax ImplicitCreate(IEnumerable<ExpressionSyntax> v)
        {
	        var args = ArgumentList();
	        var expr = SeparatedList(v);
	        var init = InitializerExpression(SyntaxKind.ObjectInitializerExpression, expr);
	        return ImplicitObjectCreationExpression(args, init);
        }

        public static PrefixUnaryExpressionSyntax Not(ExpressionSyntax value)
        {
            const SyntaxKind op = SyntaxKind.LogicalNotExpression;
            return PrefixUnaryExpression(op, value);
        }

        public static TypeOfExpressionSyntax TypeOf(ExpressionSyntax value)
        {
	        var type = (TypeSyntax)value;
	        return TypeOfExpression(type);
        }

        public static RangeExpressionSyntax Range(ExpressionSyntax left, ExpressionSyntax right)
        {
	        return RangeExpression(left, right);
        }

        public static GenericNameSyntax NameType(string name, params string[] subNames)
        {
	        var type = GenericName(name);
	        var nodes = subNames.Select(s => ParseTypeName(s));
	        var args = TypeArgumentList(SeparatedList(nodes));
	        type = type.WithTypeArgumentList(args);
	        return type;
        }
    }
}