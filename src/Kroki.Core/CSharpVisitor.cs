using System;
using System.Collections.Generic;
using DGrok.Framework;
using System.Linq;
using DGrok.DelphiNodes;
using Kroki.Core.API;
using Kroki.Core.Code;
using Kroki.Core.Model;
using Kroki.Core.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Core.Code.Coding;

namespace Kroki.Core
{
    public sealed class CSharpVisitor : Visitor
    {
        private readonly List<NamespaceObj> _nspAll;

        public CSharpVisitor(string nspName)
        {
            _nspAll = new List<NamespaceObj> { new(nspName) };
            RootNsp.Usings.Add("Kroki.Runtime");
        }

        private NamespaceObj RootNsp => _nspAll[0];

        public override string ToString()
        {
            var code = string.Join(Environment.NewLine, _nspAll
                .Where(n => n.Members.Count >= 1)
                .Select(n => n.ToString()));
            return code;
        }

        private IEnumerable<T> FindByName<T>(string name) where T : class, IHasName
            => _nspAll.SelectMany(n => FindByName<T>(n, name));

        private static IEnumerable<T> FindByName<T>(IHasMembers list, string name)
            where T : class, IHasName
        {
            foreach (var item in list.Members)
            {
                if (item is T hn && hn.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    yield return hn;
                if (item is not IHasMembers hm)
                    continue;
                foreach (var it in FindByName<T>(hm, name))
                    yield return it;
            }
        }

        public override void VisitProgramNode(ProgramNode node)
        {
            var name = node.NameNode.Text;
            var clazz = new ClassObj(name) { IsStatic = true, Visibility = Visibility.Internal };
            RootNsp.Members.Add(clazz);

            base.VisitProgramNode(node);
        }

        public override void VisitUnitNode(UnitNode node)
        {
            var name = node.UnitNameNode.GetName();
            var nameSp = new NamespaceObj(name);
            _nspAll.Add(nameSp);

            base.VisitUnitNode(node);
        }

        public override void VisitMethodHeadingNode(MethodHeadingNode node)
        {
            var method = GenerateMethod(node);

            if (node.ParentNode.ParentNode is UnitSectionNode)
            {
                var clazz = GetUnitClass();
                method.IsStatic = clazz.IsStatic;
                clazz.Members.Add(method);
            }

            base.VisitMethodHeadingNode(node);
        }

        public override void VisitMethodImplementationNode(MethodImplementationNode node)
        {
            var method = GenerateMethod(node.MethodHeadingNode);

            ClassObj clazz;
            var nameParts = Extensions.SplitName(method.Name);
            if (nameParts is { } np && FindByName<ClassObj>(np.owner)
                    .FirstOrDefault() is { } fc)
            {
                clazz = fc;
                method.Name = np.name;
            }
            else
            {
                clazz = GetUnitClass();
            }

            method.IsStatic = clazz.IsStatic;
            method.IsAbstract = false;

            var ctx = new Context(method.Name);
            foreach (var stat in Read(node.FancyBlockNode, ctx))
                method.Statements.Add(stat);

            var existing = FindByName<MethodObj>(clazz, method.Name).ToArray();
            if (existing.Length == 1)
            {
                var oldOne = existing[0];
                method.Visibility = oldOne.Visibility;
                ReplaceIn(clazz.Members, oldOne, method);
            }
            else
                clazz.Members.Add(method);

            base.VisitMethodImplementationNode(node);
        }

        private static void ReplaceIn<T>(IList<T> list, T former, T replaced)
            where T : CompileObj<MemberDeclarationSyntax>
        {
            var existIdx = list.IndexOf(former);
            list.RemoveAt(existIdx);
            list.Insert(existIdx, replaced);
        }

        private ClassObj GetUnitClass()
        {
            var unit = _nspAll.Skip(1).FirstOrDefault() ?? _nspAll.Last();
            var name = unit.Name;
            var clazz = FindByName<ClassObj>(RootNsp, name).FirstOrDefault();
            if (clazz == null)
                RootNsp.Members.Add(clazz = new ClassObj(name)
                {
                    IsStatic = true, Visibility = Visibility.Public
                });
            return clazz;
        }

        private static MethodObj GenerateMethod(MethodHeadingNode node)
        {
            var name = node.NameNode.GetName();
            var method = new MethodObj(name);
            if (node.MethodTypeNode.Type == TokenType.ProcedureKeyword)
                method.ReturnType = "void";
            else if (node.MethodTypeNode.Type == TokenType.FunctionKeyword)
                method.ReturnType = Mapping.ToCSharp(node.ReturnTypeNode);
            method.IsAbstract = true;
            var par = GenerateFields(node);
            foreach (var pItem in par)
            {
                var pObj = new ParamObj(pItem.Name) { Type = pItem.FieldType };
                method.Params.Add(pObj);
            }
            return method;
        }

        private static IEnumerable<FieldObj> GenerateFields(FieldSectionNode node)
            => CreateFields(node.FieldListNode.Items);

        private static IEnumerable<FieldObj> GenerateFields(VarSectionNode node)
            => CreateFields(node.VarListNode.Items);

        private static IEnumerable<FieldObj> GenerateFields(ConstSectionNode node)
            => CreateFields(node.ConstListNode.Items);

        private static IEnumerable<FieldObj> GenerateFields(MethodHeadingNode node)
            => CreateFields(node.ParameterListNode.Items.Select(i => i.ItemNode));

        private static IEnumerable<FieldObj> CreateFields<T>(IEnumerable<T> items)
            where T : IHasTypeAndName
        {
            foreach (var subNode in items)
            {
                var stn = subNode.TypeNode;
                var subType = stn == null ? "object" : Mapping.ToCSharp(stn);
                foreach (var subName in subNode.NameListNode.Items)
                {
                    var subLabel = subName.ItemNode.Text;
                    var subValue = (subNode as IHasTypeNameAndVal)?.ValueNode;
                    var sharpVal = subValue == null ? null : Mapping.ParseValue(subValue);
                    yield return new FieldObj(subLabel) { FieldType = subType, Value = sharpVal };
                }
            }
        }

        private static IEnumerable<StatementSyntax> Read(AstNode? node, Context ctx)
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
                case Token to:
                    switch (to.Type)
                    {
                        case TokenType.Identifier:
                            return Read(to, ctx);
                        case TokenType.BeginKeyword:
                        case TokenType.EndKeyword:
                            return Array.Empty<StatementSyntax>();
                    }
                    break;
            }
            throw new InvalidOperationException($"{ctx} --> {node} ({node?.ToCode()})");
        }

        private static IEnumerable<StatementSyntax> Read(BinaryOperationNode bo, Context _)
        {
            var ex = ReadEx(bo)!;
            var stat = ex.AsStat();
            yield return stat;
        }

        private static IEnumerable<StatementSyntax> Read(UnaryOperationNode bo, Context _)
        {
            var ex = ReadEx(bo)!;
            var stat = ex.AsStat();
            yield return stat;
        }

        private static IEnumerable<StatementSyntax> Read2(BinaryOperationNode bo, Context ctx)
        {
            var left = bo.LeftNode.GetName();
            var right = Mapping.PatchConstant(bo.RightNode.GetName());
            var opType = bo.OperatorNode.Type;
            switch (opType)
            {
                case TokenType.PlusSign:
                    // TODO
                    break;
                case TokenType.Dot:
                    yield return Invoke(left, right, Array.Empty<ArgumentSyntax>());
                    break;
                case TokenType.ColonEquals:
                    yield return left == ctx.MethodName || left == "Result"
                        ? Return(right)
                        : Assign(left, right);
                    break;
                default:
                    throw new InvalidOperationException($"{ctx} --> {opType} ({bo.ToCode()})");
            }
        }

        private static IEnumerable<StatementSyntax> Read(ParameterizedNode bo, Context _)
        {
            var left = bo.LeftNode.GetName();
            var owner = string.Empty;
            var method = left;
            if (Extensions.SplitName(method) is { } sp)
            {
                owner = sp.owner;
                method = sp.name;
            }
            var prm = bo.ParameterListNode;
            var args = prm.Items.SelectMany(ReadArg).ToArray();
            yield return Invoke(owner, method, args);
        }

        private static IEnumerable<StatementSyntax> Read(Token token, Context _)
        {
            var owner = string.Empty;
            var method = token.GetName();
            var args = Array.Empty<ArgumentSyntax>();
            yield return Invoke(owner, method, args);
        }

        private static IEnumerable<StatementSyntax> Read(ConstSectionNode dvs, Context _)
        {
            foreach (var raw in GenerateFields(dvs))
            {
                var field = new ConstObj(raw);
                var ft = field.FieldType;
                var fv = field.Value!;
                yield return Assign(ft, field.Name, fv, isConst: true);
            }
        }

        private static IEnumerable<StatementSyntax> Read(VarSectionNode dvs, Context _)
        {
            foreach (var field in GenerateFields(dvs))
            {
                var ft = field.FieldType;
                yield return Assign(ft, field.Name, Mapping.GetDefault(ft));
            }
        }

        private static IEnumerable<StatementSyntax> Read(ForStatementNode fs, Context ctx)
        {
            var loop = fs.LoopVariableNode.GetName();
            var start = fs.StartingValueNode.GetName();
            var end = fs.EndingValueNode.GetName();
            var down = fs.DirectionNode.Type == TokenType.DownToKeyword;
            var s = Read(fs.StatementNode, ctx).ToList();
            yield return For(loop, start, end, down, s);
        }

        private static IEnumerable<StatementSyntax> Read(IfStatementNode ins, Context ctx)
        {
            var cond = ReadEx(ins.ConditionNode)!;
            var then = Read(ins.ThenStatementNode, ctx);
            var en = ins.ElseStatementNode;
            var @else = en == null ? null : Read(en, ctx);
            yield return If(cond, then, @else);
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

        private static IEnumerable<StatementSyntax> Read(TryFinallyNode tf, Context ctx)
        {
            var @try = Read(tf.TryStatementListNode, ctx);
            var @finally = Read(tf.FinallyStatementListNode, ctx);
            yield return Try(@try, @finally);
        }

        private static IEnumerable<StatementSyntax> Read(WithStatementNode ws, Context ctx)
        {
            var expr = ReadEx(ws.ExpressionListNode)!;
            var stat = Read(ws.StatementNode, ctx);
            yield return With(expr, stat);
        }

        private static IEnumerable<StatementSyntax> Read(CaseStatementNode ws, Context ctx)
        {
            var expr = ReadEx(ws.ExpressionNode)!;
            var el = (v: ExprDefault(), c: Read(ws.ElseStatementListNode, ctx));
            var sel = ws.SelectorListNode.Items.Select(s =>
                (v: ReadEx(s.ValueListNode)!, c: Read(s.StatementNode, ctx)));
            var all = sel.Concat(new[] { el }).ToArray();
            yield return Switch(expr, all);
        }

        private static ExpressionSyntax? ReadEx(AstNode? node)
        {
            if (node == null)
                return null;
            switch (node)
            {
                case Token { Type: TokenType.Identifier } to:
                    var txt = ExprStr(to.Text);
                    return txt;
                case Token { Type: TokenType.StringLiteral } sl:
                    var sxt = ExprStr(sl.Text);
                    return sxt;
                case Token { Type: TokenType.Number } nl:
                    var nxt = ExprNum(nl.Text);
                    return nxt;
                case Token { Type: TokenType.NilKeyword }:
                    var xxt = ExprNull();
                    return xxt;
                case DelimitedItemNode<AstNode> da:
                    return ReadEx(da.ItemNode);
                case ListNode<DelimitedItemNode<AstNode>> lna:
                    var lnaItems = lna.Items.Select(ReadEx).ToArray();
                    return Array2(lnaItems);
                case SetLiteralNode ln:
                    var lnItems = ln.ItemListNode.Items.Select(ReadEx).ToArray();
                    return Array2(lnItems);
                case BinaryOperationNode bo:
                    var bm = Mapping.ToBinary(bo.OperatorNode);
                    var left = ReadEx(bo.LeftNode)!;
                    var right = ReadEx(bo.RightNode)!;
                    return Binary2(bm, left, right);
                case UnaryOperationNode bo:
                    var um = Mapping.ToUnary(bo.OperatorNode);
                    var uLeft = ReadEx(bo.OperandNode)!;
                    return Unary2(um, uLeft);
                case ParenthesizedExpressionNode po:
                    var core = ReadEx(po.ExpressionNode)!;
                    return Paren(core);

                case ParameterizedNode pt:
                    // TODO Handle
                    return ReadEx(pt.LeftNode);
                case PointerDereferenceNode pn:
                    // TODO Handle pointer
                    return ReadEx(pn.OperandNode);
            }
            throw new InvalidOperationException($"{node} ({node.ToCode()})");
        }

        public override void VisitInitSectionNode(InitSectionNode node)
        {
            if (node.ParentNode is ProgramNode)
            {
                var clazz = RootNsp.Members.OfType<ClassObj>().First();
                var method = Extensions.CreateMain();
                var block = node.InitializationStatementListNode;
                foreach (var stat in Read(block, new Context(method.Name)))
                    method.Statements.Add(stat);
                clazz.Members.Add(method);
            }

            base.VisitInitSectionNode(node);
        }

        public override void VisitVarSectionNode(VarSectionNode node)
        {
            if (node.ParentNode.ParentNode is ProgramNode)
            {
                var clazz = RootNsp.Members.OfType<ClassObj>().First();
                foreach (var field in GenerateFields(node))
                {
                    field.IsStatic = clazz.IsStatic;
                    clazz.Members.Add(field);
                }
            }

            base.VisitVarSectionNode(node);
        }

        public override void VisitConstSectionNode(ConstSectionNode node)
        {
            if (node.ParentNode.ParentNode is ProgramNode)
            {
                var clazz = RootNsp.Members.OfType<ClassObj>().First();
                foreach (var field in GenerateFields(node))
                {
                    var co = new ConstObj(field);
                    clazz.Members.Add(co);
                }
            }

            base.VisitConstSectionNode(node);
        }

        public override void VisitTypeDeclNode(TypeDeclNode node)
        {
            var clazz = GenerateClass(node);

            var inPro = node.ParentNode.ParentNode.ParentNode.ParentNode is ProgramNode;
            if (inPro)
            {
                var pro = RootNsp.Members.OfType<ClassObj>().First();
                pro.Members.Add(clazz);
            }
            else
            {
                RootNsp.Members.Add(clazz);
            }

            base.VisitTypeDeclNode(node);
        }

        private static ClassObj GenerateClass(TypeDeclNode node)
        {
            var tName = node.NameNode.Text;
            var tClazz = new ClassObj(tName);
            if (node.TypeNode is ClassTypeNode ctn)
            {
                foreach (var cln in ctn.ContentListNode.Items)
                {
                    var visNode = cln.VisibilityNode;
                    var vis = visNode == null ? Visibility.Public : Mapping.ToCSharp(visNode);
                    foreach (var clnItm in cln.ContentListNode.Items)
                    {
                        if (clnItm is FieldSectionNode fsn)
                        {
                            foreach (var field in GenerateFields(fsn))
                            {
                                field.IsStatic = tClazz.IsStatic;
                                field.Visibility = vis;
                                tClazz.Members.Add(field);
                            }
                            continue;
                        }
                        if (clnItm is MethodHeadingNode mhn)
                        {
                            var method = GenerateMethod(mhn);
                            method.IsStatic = tClazz.IsStatic;
                            method.Visibility = vis;
                            tClazz.Members.Add(method);
                        }
                    }
                }
            }
            return tClazz;
        }

        private static IEnumerable<ArgumentSyntax> ReadArg(AstNode? node)
        {
            object? debug = null;
            switch (node)
            {
                case BinaryOperationNode bo:
                    return Read(bo, new Context()).Arg();
                case UnaryOperationNode bo:
                    return Read(bo, new Context()).Arg();
                case ParameterizedNode pn:
                    return Read(pn, new Context()).Arg();
                case DelimitedItemNode<AstNode> dia:
                    return ReadArg(dia.ItemNode);
                case Token to:
                    debug = to.Type;
                    switch (to.Type)
                    {
                        case TokenType.Identifier:
                        case TokenType.StringLiteral:
                            return new[] { Arg(to.Text) };
                        case TokenType.Number:
                            return new[] { ArgN(to.Text) };
                        case TokenType.NilKeyword:
                            return new[] { ArgN("null") };
                    }
                    break;
            }
            throw new InvalidOperationException($"{node} ({node?.GetType()}) [{debug}]");
        }
    }
}