using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Model
{
    internal static class Extensions
    {
        public static SyntaxToken[] AsModifier(this Visibility visibility,
            bool isStatic = false, bool isReadOnly = false, bool isAbstract = false)
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

        public static string GetName(this AstNode node)
        {
            switch (node)
            {
                case Token to: return to.Text;
            }
            throw new InvalidOperationException($"{node} ?!");
        }
    }
}