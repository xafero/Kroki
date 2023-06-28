using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.Model;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Core.Code.Express;
using static Kroki.Core.Util.Extended;
using static Kroki.Roslyn.Code.Construct;

namespace Kroki.Core.Code
{
    internal static class Reader
    {
        public static IEnumerable<StatementSyntax> Read(AstNode? node, Context ctx)
        {
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
                case IfStatementNode ins:
                    return Read(ins, ctx);
                case TryFinallyNode tf:
                    return Read(tf, ctx);
                case WithStatementNode ws:
                    return Read(ws, ctx);
                case CaseStatementNode cs:
                    return Read(cs, ctx);
                case RepeatStatementNode rs:
                    return Read(rs, ctx);
                case WhileStatementNode ws:
                    return Read(ws, ctx);
                case ParameterizedNode pn:
                    return Read(pn, ctx);
                case Token { Type: TokenType.Identifier } to:
                    return Read(to, ctx);
                case Token { Type: TokenType.BeginKeyword }:
                case Token { Type: TokenType.EndKeyword }:
                    return System.Array.Empty<StatementSyntax>();
            }
            throw new InvalidOperationException($"{ctx} --> {node} ({node?.ToCode()})");
        }

        private static IEnumerable<StatementSyntax> Read(Token token, Context _)
        {
            var method = ReadEx(token)!;
            var args = System.Array.Empty<ArgumentSyntax>();
            yield return Invoke(method, args).AsStat();
        }

        private static IEnumerable<StatementSyntax> Read(ParameterizedNode pn, Context ctx)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<StatementSyntax> Read(WhileStatementNode ws, Context ctx)
        {
            var cond = ReadEx(ws.ConditionNode)!;
            var then = Read(ws.StatementNode, ctx);
            yield return While(cond, then);
        }

        private static IEnumerable<StatementSyntax> Read(RepeatStatementNode ws, Context ctx)
        {
            var cond = ReadEx(ws.ConditionNode)!;
            var then = Read(ws.StatementListNode, ctx);
            yield return Repeat(cond, then);
        }

        private static IEnumerable<StatementSyntax> Read(CaseStatementNode ws, Context ctx)
        {
            var expr = ReadEx(ws.ExpressionNode)!;
            var el = (v: DefaultValue(), c: Read(ws.ElseStatementListNode, ctx));
            var sel = ws.SelectorListNode.Items.Select(s =>
                (v: ReadEx(s.ValueListNode)!, c: Read(s.StatementNode, ctx)));
            var all = sel.Concat(new[] { el }).ToArray();
            yield return Switch(expr, all);
        }

        private static IEnumerable<StatementSyntax> Read(WithStatementNode ws, Context ctx)
        {
            var expr = ReadEx(ws.ExpressionListNode)!;
            var stat = Read(ws.StatementNode, ctx);
            yield return With(expr, stat);
        }

        private static IEnumerable<StatementSyntax> Read(IfStatementNode ins, Context ctx)
        {
            var cond = ReadEx(ins.ConditionNode)!;
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

        private static IEnumerable<StatementSyntax> Read(ForStatementNode fs, Context ctx)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<StatementSyntax> Read(UnaryOperationNode bo, Context ctx)
        {
            var ex = ReadEx(bo)!;
            var stat = ex.AsStat();
            yield return stat;
        }

        private static IEnumerable<StatementSyntax> Read(BinaryOperationNode bo, Context _)
        {
            var ex = ReadEx(bo)!;
            var stat = ex.AsStat();
            yield return stat;
        }

        private static IEnumerable<StatementSyntax> Read(ConstSectionNode dvs, Context _)
        {
            foreach (var raw in dvs.GenerateFields())
            {
                var field = new ConstObj(raw);
                var ft = field.FieldType;
                var fv = field.Value!;
                yield return Assign(ft, field.Name, fv, isConst: true);
            }
        }

        private static IEnumerable<StatementSyntax> Read(VarSectionNode dvs, Context _)
        {
            foreach (var field in dvs.GenerateFields())
            {
                var ft = field.FieldType;
                yield return Assign(ft, field.Name, Mapping.GetDefault(ft));
            }
        }
    }
}