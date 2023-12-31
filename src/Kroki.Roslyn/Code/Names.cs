using System;

namespace Kroki.Roslyn.Code
{
    public static class Names
    {
        public const string NameSep = "::";

        public static (string owner, string name)? SplitName(string name)
        {
            var t = name.Split(new[] { NameSep }, 2, StringSplitOptions.None);
            if (t.Length == 2)
                return (t[0], t[1]);
            return null;
        }

        public static string? CombineName(string owner, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return string.IsNullOrWhiteSpace(owner) ? name : owner + NameSep + name;
        }

        public static string CleanName(string name)
        {
	        switch (name)
	        {
		        case "new": return "@new";
                case "lock": return "@lock";
	        }
	        return name;
        }
    }
}