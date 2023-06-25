using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
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

        public static StatementSyntax For(string loop, string start, string end, IEnumerable<StatementSyntax> s)
        {
            VariableDeclarationSyntax? declaration = null;
            var ini = SeparatedList<ExpressionSyntax>(new[]
            {
                AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, IdentifierName(loop),
                    LiteralExpression(SyntaxKind.NumericLiteralExpression, ParseToken(start)))
            });
            var cond = BinaryExpression(SyntaxKind.LessThanExpression, IdentifierName(loop), ParseExpression(end));
            var inc = SeparatedList<ExpressionSyntax>(new[]
            {
                PostfixUnaryExpression(SyntaxKind.PostIncrementExpression, IdentifierName(loop))
            });
            var statement = Block(s);
            return ForStatement(declaration, ini, cond, inc, statement);
        }
    }
}