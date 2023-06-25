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

        public static StatementSyntax Assign(string name, string value)
            => ExpressionStatement(AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression, IdentifierName(name), ToValue(value)));

        public static StatementSyntax Return(string value)
            => ReturnStatement(ToValue(value));

        private static IdentifierNameSyntax ToValue(string value)
        {
            return IdentifierName(value);
        }
    }
}