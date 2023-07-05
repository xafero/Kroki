using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.Model;
using Kroki.Core.Util;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Kroki.Roslyn.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        internal static MethodObj GenerateMethod(this MethodHeadingNode node, Context ctx)
        {
	        var name = node.NameNode.GetText();
	        return GenerateMethod(node, ctx, name);
        }

        internal static MethodObj GenerateMethod(this IMethodLike node, Context ctx, string name)
        {
	        var method = new MethodObj(name);
	        if (node.MethodTypeNode.Type == TokenType.ProcedureKeyword)
		        method.ReturnType = "void";
	        else if (node.MethodTypeNode.Type == TokenType.FunctionKeyword)
		        method.ReturnType = Mapping.ToCSharp(node.ReturnTypeNode);
	        method.IsAbstract = true;
	        var par = GenerateFields(node, ctx);
	        foreach (var pItem in par)
	        {
		        var pObj = new ParamObj(pItem.Name) { Type = pItem.FieldType };
		        method.Params.Add(pObj);
	        }
	        return method;
        }

        internal static IEnumerable<FieldObj> GenerateFields(this FieldSectionNode node, Context ctx)
            => CreateFields(node.FieldListNode.Items, ctx);

        internal static IEnumerable<FieldObj> GenerateFields(this VarSectionNode node, Context ctx)
            => CreateFields(node.VarListNode.Items, ctx);

        internal static IEnumerable<FieldObj> GenerateFields(this ConstSectionNode node, Context ctx)
            => CreateFields(node.ConstListNode.Items, ctx);

        internal static IEnumerable<FieldObj> GenerateFields(this IMethodLike node, Context ctx)
            => CreateFields(node.ParameterListNode.Items.Select(i => i.ItemNode), ctx);

        private static IEnumerable<FieldObj> CreateFields<T>(IEnumerable<T> items, Context ctx)
            where T : IHasTypeAndName
        {
            foreach (var subNode in items)
            {
                var stn = subNode.TypeNode;
                var subType = ParseVarType(stn, ctx);
                foreach (var subName in subNode.NameListNode.Items)
                {
                    var subLabel = subName.ItemNode.Text;
                    var subValue = (subNode as IHasTypeNameAndVal)?.ValueNode;
                    var sharpVal = subValue == null ? null : Extended.ReadEx(subValue, ctx);
                    yield return new FieldObj(subLabel) { FieldType = subType, Value = sharpVal };
                }
            }
        }

        private static string ParseVarType(AstNode? stn, Context ctx)
        {
            if (stn == null) return "object";
            if (stn is EnumeratedTypeNode etn)
            {
                var enm = new EnumObj("_e");
                GenerateEnumerated(etn, enm, ctx);
                var ev = enm.Values;
                enm.Name = string.Join("_", ev.Select(v => ((EnumValObj)v).Name[..1]));
                ctx.Scope!.Members.Add(enm);
                return enm.Name;
            }
            if (stn is BinaryOperationNode bn && bn.OperatorNode.Type == TokenType.DotDot)
            {
                var (k, _) = Values.Parse(bn.RightNode.GetText());
                return k;
            }
            var subType = Mapping.ToCSharp(stn);
            return subType;
        }

        public static ITypedDef GenerateClass(TypeDeclNode node, Context ctx)
        {
	        var name = node.NameNode.Text;
	        switch (node.TypeNode)
	        {
		        case ClassTypeNode ctn:
			        var clazz = new ClassObj(name);
			        GenerateClass(ctn, clazz, ctx);
			        return clazz;
		        case InterfaceTypeNode itn:
			        var inf = new InterfaceObj(name);
			        GenerateInterface(itn, inf, ctx);
			        return inf;
		        case RecordTypeNode rtn:
			        var rec = new ClassObj(name);
			        GenerateRecord(rtn, rec, ctx);
			        return rec;
		        case PackedTypeNode pat:
			        var prr = new StructObj(name);
			        GenerateRecord((RecordTypeNode)pat.TypeNode, prr, ctx);
			        return prr;
		        case EnumeratedTypeNode etn:
			        var enm = new EnumObj(name);
			        GenerateEnumerated(etn, enm, ctx);
			        return enm;
		        case ProcedureTypeNode ptn:
			        var del = new DelegateObj(name);
			        GenerateDelegate(ptn, del, ctx);
			        return del;
		        case ArrayTypeNode atn:
			        var att = new ClassObj(name);
			        GenerateArray(atn, att, ctx);
			        return att;
		        case SetOfNode son:
			        var set = new ClassObj(name);
			        GenerateSetOf(son, set, ctx);
			        return set;
				case ClassOfNode con:
					var @ref = new ClassOfObj(name);
					GenerateClassOf(con, @ref, ctx);
					return @ref;
		        case PointerTypeNode:
			        var prt = new StructObj(name);
			        // TODO Handle pointer?!
			        return prt;
	        }
	        throw new InvalidOperationException($"{node.TypeNode} ?!");
        }

        private static Visibility GetVis(VisibilityNode? visNode)
        {
            var vis = visNode == null ? Visibility.Public : Mapping.ToCSharp(visNode);
            return vis;
        }

        private static void GenerateEnumerated(EnumeratedTypeNode etn, EnumObj clazz, Context ctx)
        {
            foreach (var cln in etn.ItemListNode.Items)
            {
                var item = cln.ItemNode;
                var key = item.NameNode.GetText();
                var val = Extended.ReadEx(item.ValueNode, ctx);
                var ev = new EnumValObj(key) { Value = val };
                clazz.Values.Add(ev);
            }
        }

        private static void GenerateDelegate(ProcedureTypeNode ptn, DelegateObj del, Context ctx)
        {
	        var method = GenerateMethod(ptn, ctx, del.Name);
	        del.Params.AddRange(method.Params);
	        del.ReturnType = method.ReturnType;
	        del.Visibility = method.Visibility;
        }

        private static void GenerateClassOf(ClassOfNode con, ClassOfObj @ref, Context ctx)
        {
	        var type = Extended.ReadEx(con.TypeNode, ctx);
	        @ref.Target = type;
        }

        private static void GenerateArray(ArrayTypeNode atn, ClassObj cla, Context _)
        {
	        var type = atn.TypeNode;
	        var csType = Mapping.ToCSharp(type);
	        cla.Base.Add(Express.NameType("List", csType));
        }

        private static void GenerateSetOf(SetOfNode set, ClassObj cla, Context _)
        {
	        var type = set.TypeNode;
	        var csType = Mapping.ToCSharp(type);
	        cla.Base.Add(Express.NameType("List", csType));
        }

        private static void GenerateRecord(RecordTypeNode rtn, ITypeDef clazz, Context ctx)
        {
            foreach (var cln in rtn.ContentListNode.Items)
            {
                var vis = GetVis(cln.VisibilityNode);
                GenerateRecord(vis, cln, clazz, ctx);
            }
        }

        private static void GenerateClass(ClassTypeNode ctn, ClassObj clazz, Context ctx)
        {
	        foreach (var inherit in ctn.InheritanceListNode.Items)
	        {
		        var parentType = Express.Name(Mapping.ToCSharp(inherit.ItemNode));
		        clazz.Base.Add((TypeSyntax)parentType);
	        }
	        foreach (var cln in ctn.ContentListNode.Items)
	        {
		        var vis = GetVis(cln.VisibilityNode);
		        GenerateClass(vis, cln, clazz, ctx);
	        }
        }

        private static void GenerateInterface(InterfaceTypeNode itn, InterfaceObj ifn, Context ctx)
        {
	        foreach (var cln in itn.MethodAndPropertyListNode.Items)
	        {
		        GenerateMember(Visibility.Public, cln, ifn, ctx);
	        }
        }

        private static void GenerateRecord(Visibility vis, VisibilitySectionNode cln, ITypeDef clazz, Context ctx)
        {
	        foreach (var clnItm in cln.ContentListNode.Items)
	        {
		        switch (clnItm)
		        {
			        case FieldSectionNode fsn:
				        foreach (var field in GenerateFields(fsn, ctx))
				        {
					        field.IsStatic = IsStatic(clazz);
					        field.Visibility = vis;
					        clazz.Members.Add(field);
				        }
				        continue;
		        }
		        throw new InvalidOperationException($"{vis} | {clnItm} ?!");
	        }
        }

        private static bool IsStatic(ITypeDef clazz)
        {
	        return clazz is ClassObj { IsStatic: true };
        }

        private static void GenerateClass(Visibility vis, VisibilitySectionNode cln, ClassObj clazz, Context ctx)
        {
	        foreach (var clnItm in cln.ContentListNode.Items)
	        {
		        GenerateMember(vis, clnItm, clazz, ctx);
	        }
        }

        private static void GenerateMember(Visibility vis, AstNode clnItm, ITypeDef clazz, Context ctx)
        {
	        switch (clnItm)
	        {
		        case FieldSectionNode fsn:
			        foreach (var field in GenerateFields(fsn, ctx))
			        {
				        field.IsStatic = IsStatic(clazz);
				        field.Visibility = vis;
				        clazz.Members.Add(field);
			        }
			        return;
		        case MethodHeadingNode mhn:
			        var method = GenerateMethod(mhn, ctx);
			        method.IsStatic = IsStatic(clazz);
			        method.Visibility = vis;
			        clazz.Members.Add(method);
			        return;
		        case PropertyNode pnn:
			        var prop = GenerateProperty(pnn);
			        prop.IsStatic = IsStatic(clazz);
			        prop.Visibility = vis;
			        clazz.Members.Add(prop);
			        return;
	        }
	        throw new InvalidOperationException($"{vis} | {clnItm} ?!");
        }
    }
}