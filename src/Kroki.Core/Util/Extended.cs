using System;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.Util
{
    internal static class Extended
    {
        public static ExpressionSyntax? ReadEx(AstNode? node)
        {




            throw new InvalidOperationException($"{node} ?!");
        }

        public static ExpressionSyntax? ReadEx(BinaryOperationNode bo)
        {
            throw new NotImplementedException();
        }

        public static ExpressionSyntax? ReadEx(UnaryOperationNode bo)
        {
            throw new NotImplementedException();
        }
    }
}