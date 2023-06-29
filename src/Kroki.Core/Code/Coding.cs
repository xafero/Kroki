using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.Util;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Kroki.Roslyn.Model;

namespace Kroki.Core.Code
{
    internal static class Coding
    {
        internal static PropertyObj GenerateProperty(this PropertyNode node)
        {
            var name = node.NameNode.GetText();
            var method = new PropertyObj(name)
            {
                PropType = Mapping.ToCSharp(node.TypeNode)
            };
            // TODO Implement get and set
            return method;
        }

        internal static MethodObj GenerateMethod(this MethodHeadingNode node)
        {
            var name = node.NameNode.GetText();
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

        internal static IEnumerable<FieldObj> GenerateFields(this FieldSectionNode node)
            => CreateFields(node.FieldListNode.Items);

        internal static IEnumerable<FieldObj> GenerateFields(this VarSectionNode node)
            => CreateFields(node.VarListNode.Items);

        internal static IEnumerable<FieldObj> GenerateFields(this ConstSectionNode node)
            => CreateFields(node.ConstListNode.Items);

        internal static IEnumerable<FieldObj> GenerateFields(this MethodHeadingNode node)
            => CreateFields(node.ParameterListNode.Items.Select(i => i.ItemNode));

        private static IEnumerable<FieldObj> CreateFields<T>(IEnumerable<T> items)
            where T : IHasTypeAndName
        {
            var ctx = new Context();
            foreach (var subNode in items)
            {
                var stn = subNode.TypeNode;
                var subType = stn == null ? "object" : Mapping.ToCSharp(stn);
                foreach (var subName in subNode.NameListNode.Items)
                {
                    var subLabel = subName.ItemNode.Text;
                    var subValue = (subNode as IHasTypeNameAndVal)?.ValueNode;
                    var sharpVal = subValue == null ? null : Extended.ReadEx(subValue, ctx);
                    yield return new FieldObj(subLabel) { FieldType = subType, Value = sharpVal };
                }
            }
        }

        public static ITypedDef GenerateClass(TypeDeclNode node)
        {
            var name = node.NameNode.Text;
            switch (node.TypeNode)
            {
                case ClassTypeNode ctn:
                    var clazz = new ClassObj(name);
                    GenerateClass(ctn, clazz);
                    return clazz;
                case RecordTypeNode rtn:
                    var rec = new ClassObj(name);
                    GenerateRecord(rtn, rec);
                    return rec;
                case EnumeratedTypeNode etn:
                    var enm = new EnumObj(name);
                    GenerateEnumerated(etn, enm);
                    return enm;
            }
            throw new InvalidOperationException($"{node.TypeNode} ?!");
        }

        private static Visibility GetVis(VisibilityNode? visNode)
        {
            var vis = visNode == null ? Visibility.Public : Mapping.ToCSharp(visNode);
            return vis;
        }

        private static void GenerateEnumerated(EnumeratedTypeNode etn, EnumObj clazz)
        {
            foreach (var cln in etn.ItemListNode.Items)
            {
                var item = cln.ItemNode;
                var key = item.NameNode.GetText();
                var val = Extended.ReadEx(item.ValueNode, new Context());
                var ev = new EnumValObj(key) { Value = val };
                clazz.Values.Add(ev);
            }
        }

        private static void GenerateRecord(RecordTypeNode rtn, ClassObj clazz)
        {
            foreach (var cln in rtn.ContentListNode.Items)
            {
                var vis = GetVis(cln.VisibilityNode);
                GenerateRecord(vis, cln, clazz);
            }
        }

        private static void GenerateClass(ClassTypeNode ctn, ClassObj clazz)
        {
            foreach (var cln in ctn.ContentListNode.Items)
            {
                var vis = GetVis(cln.VisibilityNode);
                GenerateClass(vis, cln, clazz);
            }
        }

        private static void GenerateRecord(Visibility vis, VisibilitySectionNode cln, ClassObj clazz)
        {
            foreach (var clnItm in cln.ContentListNode.Items)
            {
                // TODO
            }
        }

        private static void GenerateClass(Visibility vis, VisibilitySectionNode cln, ClassObj clazz)
        {
            foreach (var clnItm in cln.ContentListNode.Items)
            {
                switch (clnItm)
                {
                    case FieldSectionNode fsn:
                        foreach (var field in GenerateFields(fsn))
                        {
                            field.IsStatic = clazz.IsStatic;
                            field.Visibility = vis;
                            clazz.Members.Add(field);
                        }
                        continue;
                    case MethodHeadingNode mhn:
                        var method = GenerateMethod(mhn);
                        method.IsStatic = clazz.IsStatic;
                        method.Visibility = vis;
                        clazz.Members.Add(method);
                        continue;
                    case PropertyNode pnn:
                        var prop = GenerateProperty(pnn);
                        prop.IsStatic = clazz.IsStatic;
                        prop.Visibility = vis;
                        clazz.Members.Add(prop);
                        continue;
                }
                throw new InvalidOperationException($"{vis} | {clnItm} ?!");
            }
        }
    }
}