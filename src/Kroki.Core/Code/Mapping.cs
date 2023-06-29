using System;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Roslyn.API;

namespace Kroki.Core.Code
{
    internal static class Mapping
    {
        public static string ParseValue(AstNode node)
        {
            switch (node)
            {
                case Token { Type: TokenType.Number } to:
                    return to.Text;
            }
            throw new InvalidOperationException($"{node} ({node.GetType()})");
        }

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

        public static BinaryMode ToBinary(Token opToken)
        {
            switch (opToken.Type)
            {
                case TokenType.Dot:
                    return BinaryMode.Dot;
                case TokenType.ColonEquals:
                    return BinaryMode.ColonEq;
                case TokenType.GreaterThan:
                    return BinaryMode.Greater;
                case TokenType.GreaterOrEqual:
                    return BinaryMode.GreaterEq;
                case TokenType.LessThan:
                    return BinaryMode.Less;
                case TokenType.PlusSign:
                    return BinaryMode.Plus;
                case TokenType.MinusSign:
                    return BinaryMode.Minus;
                case TokenType.EqualSign:
                    return BinaryMode.Equal;
                case TokenType.NotEqual:
                    return BinaryMode.EqualNot;
                case TokenType.AndKeyword:
                    return BinaryMode.And;
                case TokenType.OrKeyword:
                    return BinaryMode.Or;
                case TokenType.DivideBySign:
                    return BinaryMode.Divide;
                case TokenType.DivKeyword:
                    return BinaryMode.IntDiv;
                case TokenType.ModKeyword:
                    return BinaryMode.Mod;
                case TokenType.TimesSign:
                    return BinaryMode.Multiply;
                case TokenType.XorKeyword:
                    return BinaryMode.Xor;
                case TokenType.InKeyword:
                    return BinaryMode.In;
            }
            throw new InvalidOperationException(opToken.ToString());
        }

        public static UnaryMode ToUnary(Token opToken)
        {
            switch (opToken.Type)
            {
                case TokenType.NotKeyword:
                    return UnaryMode.Not;
            }
            throw new InvalidOperationException(opToken.ToString());
        }

        public static string GetDefault(string type)
        {
            return "default";
        }

        public static (string owner, string method)? Replace(string text)
        {
            string? owner = null;
            string? method = null;
            switch (text.ToLower())
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
            if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(method))
                return null;
            return (owner!, method!);
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