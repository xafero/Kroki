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
using static Kroki.Core.Code.Names;

namespace Kroki.Core.Util
{
    internal static class Extensions
    {
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
    }
}