using System.IO;
using System.Text;
using System.Reflection;
using Xunit;
using static Kroki.Core.Util.Helpers;

namespace Kroki.Tests
{
    public class VisitorTest
    {
        [Fact(Skip = "Too many parser errors!")]
        public void ConvertCenterMain()
        {
            ParseAndCheck("CenterMainWnd");
        }

        [Fact]
        public void ConvertExObjects()
        {
            ParseAndCheck("ExObjects");
        }

        [Fact]
        public void ConvertFavManager()
        {
            ParseAndCheck("FavManager");
        }

        [Fact]
        public void ConvertUnitFunc()
        {
            ParseAndCheck("UnitFunctions");
        }

        [Fact]
        public void ConvertSmallForm()
        {
            ParseAndCheck("SmForm");
        }

        [Fact]
        public void ConvertSyntax()
        {
            ParseAndCheck("Syntax");
        }

        private static void ParseAndCheck(string name, string ext = "pas")
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var inDir = Path.Combine(dir, "Input");
            var outDir = Path.Combine(dir, "Output");
            var tmpDir = Path.Combine(dir, "Tmp");
            tmpDir = Directory.CreateDirectory(tmpDir).FullName;

            var inFile = Path.Combine(inDir, $"{name}.{ext}");
            var outFile = Path.Combine(outDir, $"{name}.cs");
            var tmpFile = Path.Combine(tmpDir, $"{name}.cs");

            var inSrc = ReadSource(inFile);
            var (_, translated) = Translate(inFile, inSrc, includeDate: false);
            File.WriteAllText(tmpFile, translated);

            var expected = File.ReadAllText(outFile, Encoding.UTF8);
            Assert.Equal(Clean(expected), Clean(translated));
        }

        private static string Clean(string text) => text.Replace("\r\n", "\n");
    }
}