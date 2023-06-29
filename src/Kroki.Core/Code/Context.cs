using Kroki.Roslyn.API;
using Kroki.Roslyn.Model;

namespace Kroki.Core.Code
{
    internal record Context(string? MethodName = null, IHasMembers? Scope = null)
    {
        public static Context By(MethodObj? method, IHasMembers? scope)
        {
            return new Context(method?.Name, scope);
        }
    }
}