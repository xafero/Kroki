using Kroki.Roslyn.API;
using Kroki.Roslyn.Model;

namespace Kroki.Roslyn.Code
{
    public static class Prebuilt
    {
        public static MethodObj CreateMain()
        {
            var main = new MethodObj("Main")
            {
                Visibility = Visibility.Private,
                IsStatic = true,
                IsAbstract = false
            };
            var args = new ParamObj("args") { Type = "string[]" };
            main.Params.Add(args);
            return main;
        }
    }
}