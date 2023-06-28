using Kroki.Core.API;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kroki.Core.API;
using Kroki.Core.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kroki.Core.Util
{
    public static class RoExtensions
    {
        public static T[] AsArray<T>(this List<CompileObj<T>> members)
        {
            return members.Select(m => m.Create()).ToArray();
        }

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
    }
}