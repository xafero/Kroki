using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Kroki.Core.API;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public static string? GetQuote(string text)
            => text.StartsWith("'") && text.EndsWith("'") ? text.Trim('\'') : null;
    }
}