using Kroki.Roslyn.Model;

namespace Kroki.Core.Code
{
    internal record Context(string? MethodName = null)
    {
        public static Context By(MethodObj method)
        {
            return new Context(method.Name);
        }
    }
}