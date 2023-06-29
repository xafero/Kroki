using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Kroki.Roslyn.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kroki.Core.Code
{
    internal static class Naming
    {
        public static IEnumerable<T> FindByName<T>(this IEnumerable<NamespaceObj> all, string name)
            where T : class, IHasName
            => all.SelectMany(n => FindByName<T>(n, name));

        public static IEnumerable<T> FindByName<T>(this IHasMembers list, string name)
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

        public static void ReplaceIn<T>(this IList<T> list, T former, T replaced)
            where T : CompileObj<MemberDeclarationSyntax>
        {
            var existIdx = list.IndexOf(former);
            list.RemoveAt(existIdx);
            list.Insert(existIdx, replaced);
        }

        public static string GetText(this AstNode? node)
        {
            switch (node)
            {
                case Token { Type: TokenType.Identifier } to:
                    return to.Text;
                case BinaryOperationNode bo:
                    return GetText(bo);
                case ParameterizedNode pn:
                    return GetText(pn.LeftNode);
                case ParenthesizedExpressionNode en:
                    return GetText(en.ExpressionNode);
                case PointerDereferenceNode dn:
                    return GetText(dn.OperandNode);
            }
            throw new InvalidOperationException($"{node} ?!");
        }

        private static string GetText(BinaryOperationNode bo)
        {
            var left = bo.LeftNode.GetText();
            var right = bo.RightNode.GetText();
            return Names.CombineName(left, right)!;
        }
    }
}