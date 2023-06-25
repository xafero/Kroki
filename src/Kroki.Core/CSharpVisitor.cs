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

        public override string ToString()
        {
            var code = string.Join(Environment.NewLine, _nspAll
                .Where(n => n.Members.Count >= 1)
                .Select(n => n.ToString()));
            return code;
        }
    }
}