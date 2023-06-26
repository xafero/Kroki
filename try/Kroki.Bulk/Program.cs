using CommandLine;

namespace Kroki.Bulk
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            var parser = Parser.Default;
            var parsed = parser.ParseArguments<Options>(args);
            if (parsed.Value is { } opt)
            {
                return App.Run(opt);
            }
            Errors.ShowError(parsed);
            return -1;
        }
    }
}