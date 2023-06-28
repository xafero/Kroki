using System;
using System.Collections.Generic;
using System.Linq;
using Kroki.Core.API;
using Kroki.Core.Model;
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
    }
}