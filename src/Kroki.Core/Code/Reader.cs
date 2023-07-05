using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Roslyn.Code;
using Kroki.Roslyn.Model;
using Kroki.Roslyn.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Roslyn.Code.Express;
using static Kroki.Core.Util.Extended;
using static Kroki.Roslyn.Code.Construct;
using Arr = System.Array;

namespace Kroki.Core.Code
{
    internal static class Reader
    {
        public static IEnumerable<StatementSyntax> Read(AstNode? node, Context ctx)
        {
            if (node == null)
                return Arr.Empty<StatementSyntax>();
            switch (node)
            {
                case FancyBlockNode fn:
                    return fn.DeclListNode.Items.SelectMany(i => Read(i, ctx))
                        .Concat(Read(fn.BlockNode, ctx));
                case BlockNode bn:
                    return bn.ChildNodes.SelectMany(i => Read(i, ctx));
                case ListNode<DelimitedItemNode<AstNode>> lna:
                    return lna.Items.SelectMany(i => Read(i.ItemNode, ctx));
                case BinaryOperationNode bo:
                    return Read(bo, ctx);
                case UnaryOperationNode bo:
                    return Read(bo, ctx);
                case VarSectionNode vsn:
                    return Read(vsn, ctx);
                case ConstSectionNode vsn:
                    return Read(vsn, ctx);
                case ForStatementNode fs:
                    return Read(fs, ctx);
                case ForInStatementNode fs:
	                return Read(fs, ctx);
                case IfStatementNode ins:
                    return Read(ins, ctx);
                case TryFinallyNode tf:
                    return Read(tf, ctx);
                case TryExceptNode tf:
	                return Read(tf, ctx);
                case WithStatementNode ws:
                    return Read(ws, ctx);
                case CaseStatementNode cs:
                    return Read(cs, ctx);
                case RepeatStatementNode rs:
                    return Read(rs, ctx);
                case RaiseStatementNode rss:
	                return Read(rss, ctx);
                case WhileStatementNode ws:
                    return Read(ws, ctx);
                case ParameterizedNode pn:
                    return Read(pn, ctx);
                case Token { Type: TokenType.Identifier } to:
                    return Read(to, ctx);
                case Token { Type: TokenType.BeginKeyword }:
                case Token { Type: TokenType.EndKeyword }:
                    return Arr.Empty<StatementSyntax>();

                // TODO: Handle inherited?!
                case Token { Type: TokenType.InheritedKeyword }:
	                return Arr.Empty<StatementSyntax>();
                // TODO: Handle asm?!
                case AssemblerStatementNode:
	                return Arr.Empty<StatementSyntax>();
			}
			throw new InvalidOperationException($"{ctx} --> {node} ({node?.ToCode()})");
        }

        private static IEnumerable<StatementSyntax> Read(Token token, Context ctx)
        {
            var method = Patch(token, ctx);
            var args = Arr.Empty<ArgumentSyntax>();
            yield return Invoke(method, args).AsStat();
        }

        private static IEnumerable<StatementSyntax> Read(ParameterizedNode pn, Context ctx)
        {
            var stat = ReadEx(pn, ctx)!;
            yield return stat.AsStat();
        }

        private static IEnumerable<StatementSyntax> Read(WhileStatementNode ws, Context ctx)
        {
            var cond = ReadEx(ws.ConditionNode, ctx)!;
            var then = Read(ws.StatementNode, ctx);
            yield return While(cond, then);
        }

        private static IEnumerable<StatementSyntax> Read(RepeatStatementNode ws, Context ctx)
        {
            var cond = ReadEx(ws.ConditionNode, ctx)!;
            var then = Read(ws.StatementListNode, ctx);
            yield return Repeat(cond, then);
        }

        private static IEnumerable<StatementSyntax> Read(RaiseStatementNode rs, Context ctx)
        {
	        var follow = ReadEx(rs.ExceptionNode, ctx)!;
	        yield return Throw(follow);
        }

        private static IEnumerable<StatementSyntax> Read(CaseStatementNode ws, Context ctx)
        {
            var expr = ReadEx(ws.ExpressionNode, ctx)!;
            var el = (v: DefaultValue(), c: Read(ws.ElseStatementListNode, ctx));
            var sel = ws.SelectorListNode.Items.Select(s =>
                (v: ReadEx(s.ValueListNode, ctx)!, c: Read(s.StatementNode, ctx)));
            var all = sel.Concat(new[] { el }).ToArray();
            yield return Switch(expr, all);
        }

        private static IEnumerable<StatementSyntax> Read(WithStatementNode ws, Context ctx)
        {
            var expr = ReadEx(ws.ExpressionListNode, ctx)!;
            var stat = Read(ws.StatementNode, ctx);
            yield return With(expr, stat);
        }

        private static IEnumerable<StatementSyntax> Read(IfStatementNode ins, Context ctx)
        {
            var cond = ReadEx(ins.ConditionNode, ctx)!;
            var then = Read(ins.ThenStatementNode, ctx);
            var en = ins.ElseStatementNode;
            var @else = en == null ? null : Read(en, ctx);
            yield return If(cond, then, @else);
        }

        private static IEnumerable<StatementSyntax> Read(TryFinallyNode tf, Context ctx)
        {
            var @try = Read(tf.TryStatementListNode, ctx);
            var @finally = Read(tf.FinallyStatementListNode, ctx);
            yield return Try(@try, @finally);
        }

        private static IEnumerable<StatementSyntax> Read(TryExceptNode tf, Context ctx)
        {
	        var @try = Read(tf.TryStatementListNode, ctx);
	        var claus = new List<Catch>();
	        foreach (var item in tf.ExceptionItemListNode.Items)
	        {
		        var it = ReadEx(item.TypeNode, ctx);
		        var nn = ReadEx(item.NameNode, ctx);
		        var sn = Read(item.StatementNode, ctx);
		        var itt = (TypeSyntax?)it;
		        var nnt = nn?.GetFirstToken();
		        claus.Add(new Catch(itt, nnt, sn));
	        }
	        if (tf.ElseStatementListNode is { } @else &&
	            (claus.Count == 0 || @else.Items.Count >= 1))
	        {
		        var @default = Read(@else, ctx);
		        claus.Add(new Catch(null, null, @default));
	        }
	        yield return Try(@try, @catch: claus);
        }

        private static IEnumerable<StatementSyntax> Read(ForStatementNode fs, Context ctx)
        {
            var loop = ReadEx(fs.LoopVariableNode, ctx)!;
            var start = ReadEx(fs.StartingValueNode, ctx)!;
            var end = ReadEx(fs.EndingValueNode, ctx)!;
            var down = fs.DirectionNode.Type == TokenType.DownToKeyword;
            var s = Read(fs.StatementNode, ctx).ToList();
            yield return For(loop, start, end, down, s);
        }

        private static IEnumerable<StatementSyntax> Read(ForInStatementNode fs, Context ctx)
        {
	        var loop = ReadEx(fs.LoopVariableNode, ctx)!;
	        var end = ReadEx(fs.ExpressionNode, ctx)!;
	        var s = Read(fs.StatementNode, ctx).ToList();
	        yield return ForEach(loop, end, s);
        }

        private static IEnumerable<StatementSyntax> Read(UnaryOperationNode bo, Context ctx)
        {
            var ex = ReadEx(bo, ctx)!;
            var stat = ex.AsStat();
            yield return stat;
        }

        private static IEnumerable<StatementSyntax> Read(BinaryOperationNode bo, Context ctx)
        {
            if (bo.OperatorNode.Type == TokenType.ColonEquals
                && ctx.MethodName is { } methodName
                && bo.LeftNode.GetText() is var leftName)
            {
                var left = ReadEx(bo.LeftNode, ctx)!;
                var right = ReadEx(bo.RightNode, ctx)!;
                var s = leftName == methodName || leftName == "Result"
                    ? Return(right)
                    : Assign(left, right).AsStat();
                yield return s;
                yield break;
            }
            var ex = ReadEx(bo, ctx);
            var stat = Invoke(ex, Arr.Empty<ArgumentSyntax>()).AsStat();
            yield return stat;
        }

        private static IEnumerable<StatementSyntax> Read(ConstSectionNode dvs, Context ctx)
        {
            foreach (var raw in dvs.GenerateFields(ctx))
            {
                var field = new ConstObj(raw);
                var ft = field.FieldType;
                var fv = field.Value!;
                yield return Assign(ft, field.Name, fv, isConst: true);
            }
        }

        private static IEnumerable<StatementSyntax> Read(VarSectionNode dvs, Context ctx)
        {
            foreach (var field in dvs.GenerateFields(ctx))
            {
                var ft = field.FieldType;
                var vt = field.Value ?? Mapping.GetDefault(ft);
                yield return Assign(ft, field.Name, vt);
            }
        }
    }
}