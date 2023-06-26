using CommandLine;

namespace Kroki.Bulk
{
    public class Options
    {
        [Option('I', "input", Required = true, HelpText = "Input folder")]
        public string? InputDir { get; set; }

        [Option('O', "output", Required = true, HelpText = "Output folder")]
        public string? OutputDir { get; set; }
    }
}