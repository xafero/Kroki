using System;
using System.Text;
using CommandLine;

namespace Kroki.Bulk
{
    internal static class Errors
    {
        internal static void ShowError(ParserResult<Options> parsed)
        {
            var lines = new StringBuilder();
            foreach (var error in parsed.Errors)
            {
                var line = error.ToString();
                switch (error)
                {
                    case UnknownOptionError ue:
                        line = $"Unknown option: {ue.Token}";
                        break;
                    case MissingRequiredOptionError me:
                        line = $"Missing required option: {me.NameInfo.NameText}";
                        break;
                    case VersionRequestedError:
                        continue;
                    case HelpRequestedError:
                        continue;
                }
                lines.AppendLine(line);
            }
            var errText = lines.ToString();
            Console.Error.WriteLine(errText);
        }
    }
}