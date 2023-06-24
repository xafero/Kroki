using System;
using System.Collections.Generic;
using System.Text;
using DGrok.Framework;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Core.CSharpGen;
using System.Linq;
using DGrok.DelphiNodes;

namespace Kroki.Core
{
    public static class DelphiParser
    {
        public static string ParsePas(string simpleName, SourceText sourceText, string nspName)
        {
            var cb = ParsePas(simpleName, sourceText);
            var addIt = new StringBuilder();

            var nspAll = new List<NamespaceDeclarationSyntax>();
            var rootMembers = new List<MemberDeclarationSyntax>();

            foreach (var file in cb.ParsedFiles)
            {
                switch (file.Content)
                {
                    case ProgramNode pn:
                        var pName = pn.NameNode.Text;
                        var pClazz = CreateClass(pName);
                        var members = new List<MemberDeclarationSyntax>();
                        foreach (var child in pn.ChildNodes)
                        {
                            switch (child)
                            {
                                case InitSectionNode isn:
                                    var main = CreateMain();
                                    members.Add(main);
                                    continue;
                                case ListNode<AstNode> ln:
                                    continue;
                                case TypeSectionNode ts:
                                    continue;
                                case TypeDeclNode tn:
                                    var tName = tn.NameNode.Text;
                                    var clazz = CreateClass(tName);
                                    members.Add(clazz);
                                    continue;
                            }
                        }
                        rootMembers.Add(pClazz.AddMembers(members.ToArray()));
                        break;
                }
            }

            if (rootMembers.Count >= 1)
            {
                var rootNsp = CreateNamespace($"{nspName}", rootMembers);
                nspAll.Add(rootNsp);
            }
            var code = string.Join(Environment.NewLine, nspAll.Select(WriteToStr));
            return (code + Environment.NewLine + addIt).Trim();
        }

        private static CodeBase ParsePas(string simpleName, SourceText sourceText)
        {
            var options = new CodeBaseOptions();
            var defines = options.CreateCompilerDefines();
            var loader = new FileLoader(true);
            var cb = new CodeBase(defines, loader);
            var sourceCode = sourceText.ToString();
            cb.AddFileExpectingSuccess(simpleName, sourceCode);
            return cb;
        }
    }
}