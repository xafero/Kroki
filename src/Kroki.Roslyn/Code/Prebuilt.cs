using Kroki.Core.API;
using Kroki.Core.Model;

namespace Kroki.Core.Code
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