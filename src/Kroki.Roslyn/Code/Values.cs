namespace Kroki.Core.Code
{
    public static class Values
    {
        public const string Null = "null";

        public static (string kind, object? val) Parse(string value)
        {
            var vType = "string";
            object? vVal = value;
            if (value == Null)
            {
                vType = "object";
                vVal = null;
            }
            if (bool.TryParse(value, out var bt))
            {
                vType = "bool";
                vVal = bt;
            }
            if (double.TryParse(value, out var dd))
            {
                vType = "double";
                vVal = dd;
            }
            if (int.TryParse(value, out var ii))
            {
                vType = "int";
                vVal = ii;
            }
            return (vType, vVal);
        }
    }
}