using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Code
{
    internal static class Coding
    {
        public static LocalDeclarationStatementSyntax Assign(string type, string name, string value)
            => LocalDeclarationStatement(VariableDeclaration(ParseTypeName(type))
                .AddVariables(VariableDeclarator(name).WithInitializer(EqualsValueClause(
                    LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(value))))));

        public static ExpressionStatementSyntax Assign(string name, string value)
            => ExpressionStatement(AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression, IdentifierName(name), ToValue(value)));

        public static ReturnStatementSyntax Return(string value)
            => ReturnStatement(ToValue(value));

        private static IdentifierNameSyntax ToValue(string value)
        {
            return IdentifierName(value);
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
    }
}