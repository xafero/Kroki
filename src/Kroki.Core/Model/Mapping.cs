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
    }
}