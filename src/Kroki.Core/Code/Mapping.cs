using System;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using Kroki.Core.Util;
using Kroki.Roslyn.API;
using static System.StringSplitOptions;

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
                case "NativeInt":
                case "LongInt":
                case "Int64":
                    return "long";
                case "LongWord":
                case "NativeUInt":
                case "UInt64":
                    return "ulong";
                case "ShortInt":
                    return "sbyte";
                case "DWORD":
	                return "uint";
                case "Word":
                    return "ushort";
                case "Pointer":
	                return "IntPtr";
                case "Byte":
                    return "byte";
                case "SmallInt":
                    return "short";
                case "qword":
	                return "ushort";
                case "Boolean":
                case "boolean":
                case "ByteBool":
                case "LongBool":
                case "WordBool":
                    return "bool";
                case "Cardinal":
                case "FixedUInt":
                    return "uint";
                case "FixedInt":
                case "Integer":
                case "integer":
                    return "int";
                case "Currency":
                case "Extended":
                case "Comp":
                    return "decimal";
                case "Char":
                    return "char";
                case "Real":
                case "real":
                case "Single":
                    return "float";
                case "Double":
                    return "double";
                case "TDateTime":
                    return "DateTime";
                case "HResult":
                    return "IntPtr";
                case "UnicodeString":
                case "AnsiChar":
                case "WideString":
                case "WideChar":
                case "UCS2Char":
                case "UCS4Char":
                case "UCS4char":
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
            switch (visTxt.ToLowerInvariant())
            {
                case "private":
                    return Visibility.Private;
                case "protected":
                    return Visibility.Protected;
                case "published":
	                return Visibility.Internal;
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
                case TokenType.LessOrEqual:
                    return BinaryMode.LessEq;
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
                case TokenType.IsKeyword:
                    return BinaryMode.Is;
                case TokenType.AsKeyword:
	                return BinaryMode.As;
                case TokenType.ShrKeyword:
	                return BinaryMode.Shr;
                case TokenType.ShlKeyword:
	                return BinaryMode.Shl;
                case TokenType.DotDot:
	                return BinaryMode.Range;
			}
			throw new InvalidOperationException(opToken.ToString());
        }

        public static UnaryMode ToUnary(Token opToken)
        {
            switch (opToken.Type)
            {
                case TokenType.NotKeyword:
                    return UnaryMode.Not;
                case TokenType.MinusSign:
                    return UnaryMode.Minus;
                case TokenType.InheritedKeyword:
	                return UnaryMode.Inherit;
                case TokenType.AtSign:
	                return UnaryMode.At;
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
            var cast = text.Split(new[] { Extended.CastPrefix }, 2, None);
            if (cast.Length == 2)
            {
	            owner = "Convert";
	            method = $"To{Naming.ToTitle(cast.Last())}";
            }
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