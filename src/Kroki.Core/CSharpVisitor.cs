using System;
using System.Collections.Generic;
using DGrok.Framework;
using System.Linq;
using DGrok.DelphiNodes;
using Kroki.Core.Model;

namespace Kroki.Core
{
    public sealed class CSharpVisitor : Visitor
    {
        private readonly List<NamespaceObj> _nspAll;

        public CSharpVisitor(string nspName)
        {
            _nspAll = new List<NamespaceObj> { new(nspName) };
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
                var main = Extensions.CreateMain();
                clazz.Members.Add(main);
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

        public override string ToString()
        {
            var code = string.Join(Environment.NewLine, _nspAll
                .Where(n => n.Members.Count >= 1)
                .Select(n => n.ToString()));
            return code;
        }
    }
}