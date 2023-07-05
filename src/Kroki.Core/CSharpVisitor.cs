using System;
using System.Collections.Generic;
using DGrok.Framework;
using System.Linq;
using DGrok.DelphiNodes;
using Kroki.Core.Code;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Kroki.Roslyn.Model;
using static Kroki.Core.Code.Reader;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        private ClassObj GetUnitClass()
        {
            var unit = _nspAll.Skip(1).FirstOrDefault() ?? _nspAll.Last();
            var name = unit.Name;
            var clazz = RootNsp.FindByName<ClassObj>(name).FirstOrDefault();
            if (clazz == null)
                RootNsp.Members.Add(clazz = new ClassObj(name)
                {
                    IsStatic = true, Visibility = Visibility.Public
                });
            return clazz;
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
            var name = node.UnitNameNode.GetText();
            var nameSp = new NamespaceObj(name);
            _nspAll.Add(nameSp);

            base.VisitUnitNode(node);
        }

        public override void VisitMethodHeadingNode(MethodHeadingNode node)
        {
            var ctx = Context.By(null, RootNsp);
            var method = node.GenerateMethod(ctx);

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
	        if (node.ParentNode.ParentNode.ParentNode is MethodImplementationNode)
	        {
		        base.VisitMethodImplementationNode(node);
                return;
			}

	        var ctx = Context.By(null, RootNsp);
            var method = node.MethodHeadingNode.GenerateMethod(ctx);

            ClassObj clazz;
            var nameParts = Names.SplitName(method.Name);
            if (nameParts is { } np && _nspAll.FindByName<ClassObj>(np.owner)
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

            ctx = Context.By(method, clazz);
            foreach (var stat in Read(node.FancyBlockNode, ctx))
                method.Statements.Add(stat);

            var existing = clazz.FindByName<MethodObj>(method.Name).ToArray();
            if (existing.Length == 1)
            {
                var oldOne = existing[0];
                method.Visibility = oldOne.Visibility;
                clazz.Members.ReplaceIn(oldOne, method);
            }
            else
                clazz.Members.Add(method);

            base.VisitMethodImplementationNode(node);
        }

        public override void VisitInitSectionNode(InitSectionNode node)
        {
            var block = node.InitializationStatementListNode;
            var method = Prebuilt.CreateMain();
            foreach (var stat in Read(block, Context.By(method, RootNsp)))
                method.Statements.Add(stat);

            if (method.Statements.Count >= 1)
            {
                ClassObj? clazz = null;
                if (node.ParentNode is ProgramNode)
                    clazz = RootNsp.Members.OfType<ClassObj>().First();
                else if (node.ParentNode is UnitNode)
                    clazz = GetUnitClass();
                clazz?.Members.Add(method);
            }

            base.VisitInitSectionNode(node);
        }

        public override void VisitVarSectionNode(VarSectionNode node)
        {
            if (node.ParentNode.ParentNode is ProgramNode)
            {
                var clazz = RootNsp.Members.OfType<ClassObj>().First();
                var ctx = Context.By(null, clazz);
                foreach (var field in node.GenerateFields(ctx))
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
                var ctx = Context.By(null, clazz);
                foreach (var field in node.GenerateFields(ctx))
                {
                    var co = new ConstObj(field);
                    clazz.Members.Add(co);
                }
            }

            base.VisitConstSectionNode(node);
        }

        public override void VisitTypeDeclNode(TypeDeclNode node)
        {
	        var ctx = Context.By(null, RootNsp);
	        var raw = Coding.GenerateClass(node, ctx);
	        var clazz = (CompileObj<MemberDeclarationSyntax>)raw;

	        var inPro = node.ParentNode.ParentNode.ParentNode.ParentNode is ProgramNode;
	        if (inPro)
	        {
		        var pro = RootNsp.Members.OfType<ClassObj>().First();
		        pro.Members.Add(clazz);
	        }
	        else
	        {
		        var inMethod = node.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode is MethodImplementationNode;
		        if (!inMethod)
		        {
			        RootNsp.Members.Add(clazz);
		        }
	        }

	        base.VisitTypeDeclNode(node);
        }
    }
}