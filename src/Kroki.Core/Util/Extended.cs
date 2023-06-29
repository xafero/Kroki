using System;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Roslyn.Code.Express;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Util
{
    internal static class Extended
    {
        public static ExpressionSyntax? ReadEx(AstNode? node, Context ctx)
        {
            if (node == null)
                return null;
            switch (node)
            {
                case Token { Type: TokenType.Identifier } to:
                    return ReadEx(to, ctx);
                case Token { Type: TokenType.StringLiteral } to:
                    return AsTextValue(to.Text);
                case Token { Type: TokenType.Number } to:
                    return AsNumberValue(to.Text);
                case Token { Type: TokenType.NilKeyword }:
                    return NullValue();
                case DelimitedItemNode<AstNode> da:
                    return ReadEx(da.ItemNode, ctx);
                case BinaryOperationNode bo:
                    return ReadEx(bo, ctx);
                case UnaryOperationNode uo:
                    return ReadEx(uo, ctx);
                case ParenthesizedExpressionNode po:
                    return ReadEx(po, ctx);
                case ParameterizedNode pn:
                    return ReadEx(pn, ctx);
                case SetLiteralNode ln:
                    return ReadEx(ln, ctx);
                case ListNode<DelimitedItemNode<AstNode>> lna:
                    return ReadEx(lna, ctx);
                case PointerDereferenceNode pn:
                    return ReadEx(pn.OperandNode, ctx); // TODO Handle pointer
            }
            throw new InvalidOperationException($"{node} ?!");
        }

        private static ExpressionSyntax ReadEx(Token to, Context ctx)
        {
            var txt = to.Text;
            switch (txt)
            {
                case "False": return AsBoolValue(false);
                case "True": return AsBoolValue(true);
            }
            return IdentifierName(txt);
        }

        private static ExpressionSyntax ReadEx(ParameterizedNode pn, Context ctx)
        {
            throw new NotImplementedException(); // TODO
        }

        private static ExpressionSyntax ReadEx(ListNode<DelimitedItemNode<AstNode>> lna, Context ctx)
        {
            var it = lna.Items.Select(i => ReadEx(i, ctx)).ToArray();
            return Array(it);
        }

        private static ExpressionSyntax ReadEx(SetLiteralNode ln, Context ctx)
        {
            var it = ln.ItemListNode.Items.Select(i => ReadEx(i, ctx)).ToArray();
            return Array(it);
        }

        private static ExpressionSyntax ReadEx(ParenthesizedExpressionNode po, Context ctx)
        {
            var core = ReadEx(po.ExpressionNode, ctx)!;
            return Paren(core);
        }

        public static ExpressionSyntax ReadEx(BinaryOperationNode bo, Context ctx)
        {
            var left = ReadEx(bo.LeftNode, ctx)!;
            var right = ReadEx(bo.RightNode, ctx)!;
            var bm = Mapping.ToBinary(bo.OperatorNode);
            return Binary(bm, left, right);
        }

        public static ExpressionSyntax ReadEx(UnaryOperationNode bo, Context ctx)
        {
            var left = ReadEx(bo.OperandNode, ctx)!;
            var bm = Mapping.ToUnary(bo.OperatorNode);
            return Unary(bm, left);
        }
    }
}