using System.IO;
using DGrok.DelphiNodes;

namespace Kroki.Core.Util
{
    internal static class Extensions
    {
        public static string? GetFileName(this UsedUnitNode node)
        {
            var text = node.FileNameNode?.Text;
            text ??= node.Location.FileName;
            text ??= node.EndLocation.FileName;
            if (!string.IsNullOrWhiteSpace(text))
                text = Path.GetFileNameWithoutExtension(text);
            return text;
        }
    }
}