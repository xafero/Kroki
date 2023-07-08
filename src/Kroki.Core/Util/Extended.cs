using System;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.Code;
using Kroki.Core.Model;
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
                case RecordFieldConstantNode rc:
	                return ReadEx(rc, ctx);
                case SetLiteralNode ln:
                    return ReadEx(ln, ctx);
                case ListNode<DelimitedItemNode<AstNode>> lna:
                    return ReadEx(lna, ctx);
                case ConstantListNode cln:
	                return ReadEx(cln, ctx);
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
            var left = Patch(pn.LeftNode, ctx);
            var prm = pn.ParameterListNode;
            var args = prm.Items.Select(p => ReadEx(p, ctx)!.Arg()).ToArray();
            var item = Invoke(left, args);
            return item;
        }

        private static ExpressionSyntax ReadEx(RecordFieldConstantNode fc, Context ctx)
        {
	        var name = ReadEx(fc.NameNode, ctx)!;
	        var value = ReadEx(fc.ValueNode, ctx)!;
	        var item = Assign(name, value);
	        return item;
        }

        internal static ExpressionSyntax Patch(AstNode pn, Context ctx)
        {
            var left = ReadEx(pn, ctx)!;
            if (pn.GetText() is var leftName && Mapping.Replace(leftName) is { } p)
            {
                left = Access(Name(p.owner), Name(p.method));
            }
            return left;
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

        private static ExpressionSyntax ReadEx(ConstantListNode cln, Context ctx)
        {
	        var values = cln.ItemListNode.Items
		        .Select(i => ReadEx(i, ctx)!).ToArray();
	        return ImplicitCreate(values);
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