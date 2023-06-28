using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.API;
using Kroki.Core.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Util
{
    internal static class Extensions
    {
        public static SyntaxToken[] AsModifier(this Visibility visibility, bool isStatic = false,
            bool isReadOnly = false, bool isAbstract = false, bool isConst = false)
        {
            var tok = new List<SyntaxToken>();
            switch (visibility)
            {
                case Visibility.Public:
                    tok.Add(Token(SyntaxKind.PublicKeyword));
                    break;
                case Visibility.Internal:
                    tok.Add(Token(SyntaxKind.InternalKeyword));
                    break;
                case Visibility.Protected:
                    tok.Add(Token(SyntaxKind.ProtectedKeyword));
                    break;
                case Visibility.Private:
                    tok.Add(Token(SyntaxKind.PrivateKeyword));
                    break;
            }
            if (isStatic)
                tok.Add(Token(SyntaxKind.StaticKeyword));
            if (isReadOnly)
                tok.Add(Token(SyntaxKind.ReadOnlyKeyword));
            if (isAbstract)
                tok.Add(Token(SyntaxKind.AbstractKeyword));
            if (isConst)
                tok.Add(Token(SyntaxKind.ConstKeyword));
            return tok.ToArray();
        }

        public static T[] AsArray<T>(this List<CompileObj<T>> members)
        {
            return members.Select(m => m.Create()).ToArray();
        }

        public static MethodObj CreateMain()
        {
            var main = new MethodObj("Main")
            {
                Visibility = Visibility.Private, IsStatic = true, IsAbstract = false
            };
            var args = new ParamObj("args") { Type = "string[]" };
            main.Params.Add(args);
            return main;
        }

        public const string NameSep = "::";

        public static string GetName(this AstNode node)
        {
            switch (node)
            {
                case Token to:
                    return to.Text;
                case BinaryOperationNode bo:
                    var left = bo.LeftNode.GetName();
                    var opt = bo.OperatorNode.GetName();
                    if (opt == ".") opt = NameSep;
                    var right = bo.RightNode.GetName();
                    return left + opt + right;
                case PointerDereferenceNode pd:
                    // TODO Handle pointer!
                    return pd.OperandNode.GetName();
            }
            throw new InvalidOperationException($"{node} ?!");
        }

        public static string? GetFileName(this UsedUnitNode node)
        {
            var text = node.FileNameNode?.Text;
            text ??= node.Location.FileName;
            text ??= node.EndLocation.FileName;
            if (!string.IsNullOrWhiteSpace(text))
                text = Path.GetFileNameWithoutExtension(text);
            return text;
        }

        public static (string owner, string name)? SplitName(string name)
        {
            var t = name.Split(new[] { NameSep }, 2, StringSplitOptions.None);
            if (t.Length == 2)
                return (t[0], t[1]);
            return null;
        }
    }
}