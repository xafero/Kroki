using System;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.API;

namespace Kroki.Core.Code
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
                        case TokenType.StringKeyword:
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
                case "Boolean":
                case "boolean":
                case "WordBool":
                    return "bool";
                case "Integer":
                case "integer":
                    return "int";
                case "LongWord":
                case "LongInt":
                    return "uint";
                case "Currency":
                    return "decimal";
                case "Char":
                    return "char";
                case "Real":
                case "real":
                case "Single":
                    return "float";
                case "Double":
                    return "double";
                case "Extended":
                    return "long";
                case "TDateTime":
                    return "DateTime";
                case "HResult":
                    return "IntPtr";
                case "WideString":
                case "String":
                case "string":
                    return "string";
                case "TObject":
                    return "object";
                case "OleVariant":
                    return "DataTable";
                case "TFDQuery":
                    return "ClientDataSet";
                case "Exception":
                case "exception":
                    return "Exception";
                case "TStringList":
                case "List<String>":
                    return "List<String>";
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
                case "protected":
                    return Visibility.Protected;
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
                switch (method.ToLower())
                {
                    case "read":
                        owner = "Console";
                        method = "Read";
                        break;
                    case "readln":
                        owner = "Console";
                        method = "ReadLine";
                        break;
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

        public static string PatchConstant(string text)
        {
            switch (text)
            {
                case "False": return "false";
            }
            if (text.StartsWith("'") && text.EndsWith("'"))
                return @$"""{text.Trim('\'')}""";
            return text;
        }
    }
}