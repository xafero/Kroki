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

        public override void VisitProgramNode(ProgramNode node)
        {
            var pName = node.NameNode.Text;
            var pClazz = new ClassObj(pName) { IsStatic = true, Visibility = Visibility.Internal };
            RootNsp.Members.Add(pClazz);

            base.VisitProgramNode(node);
        }

        public override void VisitInitSectionNode(InitSectionNode node)
        {
            var clazz = RootNsp.Members.OfType<ClassObj>().FirstOrDefault();
            if (clazz != null)
            {
                var method = Extensions.CreateMain();
                var block = node.InitializationStatementListNode;
                foreach (var stat in Read(block, string.Empty))
                    method.Statements.Add(stat);
                clazz.Members.Add(method);
            }

            base.VisitInitSectionNode(node);
        }

        public override void VisitTypeDeclNode(TypeDeclNode node)
        {
            var livesInProgram = node.ParentNode.ParentNode.ParentNode.ParentNode is ProgramNode;
            var clazz = RootNsp.Members.OfType<ClassObj>().FirstOrDefault();
            if (clazz != null && livesInProgram)
            {
                var tClazz = GenerateClass(node);
                clazz.Members.Add(tClazz);
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
                    var vis = Mapping.ToCSharp(cln.VisibilityNode);
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

        private static IEnumerable<FieldObj> GenerateFields(MethodHeadingNode node)
            => CreateFields(node.ParameterListNode.Items.Select(i => i.ItemNode));

        private static IEnumerable<FieldObj> GenerateFields(FieldSectionNode node)
            => CreateFields(node.FieldListNode.Items);

        private static IEnumerable<FieldObj> GenerateFields(VarSectionNode node)
            => CreateFields(node.VarListNode.Items);

        private static IEnumerable<FieldObj> CreateFields<T>(IEnumerable<T> items)
            where T : IHasTypeAndName
        {
            foreach (var subNode in items)
            {
                var subType = Mapping.ToCSharp(subNode.TypeNode);
                foreach (var subName in subNode.NameListNode.Items)
                {
                    var subLabel = subName.ItemNode.Text;
                    yield return new FieldObj(subLabel) { FieldType = subType };
                }
            }
        }

        public override void VisitVarSectionNode(VarSectionNode node)
        {
            var livesInProgram = node.ParentNode.ParentNode is ProgramNode;
            var clazz = RootNsp.Members.OfType<ClassObj>().FirstOrDefault();
            if (clazz != null && livesInProgram)
            {
                foreach (var field in GenerateFields(node))
                {
                    field.IsStatic = clazz.IsStatic;
                    clazz.Members.Add(field);
                }
            }

            base.VisitVarSectionNode(node);
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

        public override void VisitMethodImplementationNode(MethodImplementationNode node)
        {
            var clazz = RootNsp.Members.OfType<ClassObj>().FirstOrDefault();
            if (node.MethodHeadingNode is { } head)
            {
                var method = GenerateMethod(head);
                var nameParts = Extensions.SplitName(method.Name);
                if (nameParts is { } np && FindByName<ClassObj>(np.owner).FirstOrDefault() is { } fc)
                {
                    clazz = fc;
                    method.Name = np.name;
                }
                var block = node.FancyBlockNode;
                foreach (var stat in Read(block, method.Name))
                    method.Statements.Add(stat);
                if (clazz != null)
                {
                    method.IsStatic = clazz.IsStatic;
                    method.IsAbstract = false;

                    var existing = FindByName<MethodObj>(clazz, method.Name).ToArray();
                    if (existing.Length == 1)
                    {
                        var existSingle = existing[0];
                        var existIdx = clazz.Members.IndexOf(existSingle);
                        clazz.Members.RemoveAt(existIdx);
                        clazz.Members.Insert(existIdx, method);
                    }
                    else
                    {
                        clazz.Members.Add(method);
                    }
                }
            }

            base.VisitMethodImplementationNode(node);
        }

        private static IEnumerable<ArgumentSyntax> ReadArg(AstNode? node)
        {
            object? debug = null;
            switch (node)
            {
                case DelimitedItemNode<AstNode> dia:
                    return ReadArg(dia.ItemNode);
                case ParameterizedNode pn:
                    return Read(pn, string.Empty).Arg();
                case Token to:
                    debug = to.Type;
                    switch (to.Type)
                    {
                        case TokenType.Number:
                            return new[] { ArgN(to.Text) };
                        case TokenType.Identifier:
                        case TokenType.StringLiteral:
                            return new[] { Arg(to.Text) };
                    }
                    break;
            }
            throw new InvalidOperationException($"{node} ({node?.GetType()}) [{debug}]");
        }

        private static IEnumerable<StatementSyntax> Read(AstNode? node, string methodName)
        {
            switch (node)
            {
                case VarSectionNode vsn:
                    return Read(vsn, methodName);
                case ListNode<DelimitedItemNode<AstNode>> lna:
                    return lna.Items.SelectMany(i => Read(i.ItemNode, methodName));
                case BinaryOperationNode bo:
                    return Read(bo, methodName);
                case ForStatementNode fs:
                    return Read(fs, methodName);
                case BlockNode bn:
                    return bn.ChildNodes.SelectMany(i => Read(i, methodName));
                case FancyBlockNode fn:
                    return fn.DeclListNode.Items.SelectMany(i => Read(i, methodName))
                        .Concat(Read(fn.BlockNode, methodName));
                case ParameterizedNode pn:
                    return Read(pn, methodName);
                case Token to:
                    switch (to.Type)
                    {
                        case TokenType.Identifier:
                            return Read(to, methodName);
                        case TokenType.BeginKeyword:
                        case TokenType.EndKeyword:
                            return Array.Empty<StatementSyntax>();
                    }
                    break;
            }
            throw new InvalidOperationException($"{methodName} --> {node}");
        }

        private static IEnumerable<StatementSyntax> Read(VarSectionNode dvs, string _)
        {
            foreach (var field in GenerateFields(dvs))
            {
                var ft = field.FieldType;
                yield return Assign(ft, field.Name, Mapping.GetDefault(ft));
            }
        }

        private static IEnumerable<StatementSyntax> Read(Token token, string _)
        {
            var owner = string.Empty;
            var method = token.GetName();
            var args = Array.Empty<ArgumentSyntax>();
            yield return Invoke(owner, method, args);
        }

        private static IEnumerable<StatementSyntax> Read(ParameterizedNode bo, string _)
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

        private static IEnumerable<StatementSyntax> Read(BinaryOperationNode bo, string methodName)
        {
            var left = bo.LeftNode.GetName();
            var right = bo.RightNode.GetName();
            if (bo.OperatorNode.Type == TokenType.ColonEquals)
                yield return left == methodName
                    ? Return(right)
                    : Assign(left, right);
        }

        private static IEnumerable<StatementSyntax> Read(ForStatementNode fs, string methodName)
        {
            var loop = fs.LoopVariableNode.GetName();
            var start = fs.StartingValueNode.GetName();
            var end = fs.EndingValueNode.GetName();
            var s = Read(fs.StatementNode, methodName).ToList();
            yield return For(loop, start, end, s);
        }

        public override string ToString()
        {
            var code = string.Join(Environment.NewLine, _nspAll
                .Where(n => n.Members.Count >= 1)
                .Select(n => n.ToString()));
            return code;
        }
    }
}