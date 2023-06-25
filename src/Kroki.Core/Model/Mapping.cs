using System;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace Kroki.Core.Model
{
    internal static class Mapping
    {
        public static string ToCSharp(AstNode node)
        {
            switch (node)
            {
                case PointerTypeNode pt:
                    // TODO Handle pointer!
                    return ToCSharp(pt.TypeNode);

                case Token to:
                    var typeName = to.Text;
                    switch (to.Type)
                    {
                        case TokenType.Identifier:
                            return ConvertType(typeName);
                    }
                    break;
            }
            return node.ToString();
        }

        private static string ConvertType(string typeName)
        {
            switch (typeName)
            {
                case "integer":
                    return "int";
            }
            return typeName;
        }

        public static Visibility ToCSharp(VisibilityNode visNode)
        {
            var visTxt = visNode.VisibilityKeywordNode.Text;
            switch (visTxt)
            {
                case "private":
                    return Visibility.Private;
                case "public":
                    return Visibility.Public;
            }
            throw new InvalidOperationException(visTxt);
        }

        public static string GetDefault(string type)
        {
            return "default";
        }

        public static void Replace(ref string owner, ref string method)
        {
            if (string.IsNullOrWhiteSpace(owner))
                switch (method)
                {
                    case "write":
                        owner = "Console";
                        method = "Write";
                        break;
                    case "writeln":
                        owner = "Console";
                        method = "WriteLine";
                        break;
                    case "new":
                        owner = "Compat";
                        method = "Renew";
                        break;
                    case "dispose":
                        owner = "Compat";
                        method = "Dispose";
                        break;
                }
        }
    }
}