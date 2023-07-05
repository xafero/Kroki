using System;
using System.IO;
using System.Linq;
using Kroki.Bulk.Read;
using Kroki.Core.Model;

namespace Kroki.Bulk
{
	internal static class App
	{
		private static uint statExisting;
		private static uint statCopied;
		private static uint statIgnored;
		private static uint statFailed;
		private static uint statParsed;

		internal static int Run(Options opt)
		{
			if (opt.InputDir is not { } inputDir || !Directory.Exists(inputDir))
				return -2;

			var inputFolder = Path.GetFullPath(inputDir);
			var outputFolder = Path.GetFullPath(opt.OutputDir!);
			EnsureDir(outputFolder);

			Console.WriteLine($"Input folder  = {inputFolder}");
			Console.WriteLine($"Output folder = {outputFolder}");
			Console.WriteLine();

			var sep = Path.DirectorySeparatorChar;
			var files = Directory.GetFiles(inputFolder, "*.*", SearchOption.AllDirectories);
			
			var fileCount = files.Length;
			var i = 0;
			Delphi.Log = txt => Console.WriteLine($" [{(i+1):D4}/{fileCount:D4}] {txt.Replace(outputFolder, ".").Trim()}");

			for (; i < fileCount; i++)
			{
				var file = files[i];
				if (file.Contains($"{sep}.git{sep}"))
				{
					statIgnored++;
					continue;
				}
				if (file.Contains($"{sep}bin{sep}Debug{sep}") || file.Contains($"{sep}bin{sep}x64{sep}") ||
				    file.Contains($"{sep}obj{sep}Debug{sep}") || file.Contains($"{sep}obj{sep}x64{sep}"))
				{
					statIgnored++;
					continue;
				}
				if (file.Contains($"{sep}.vs{sep}"))
				{
					statIgnored++;
					continue;
				}
				if (file.Contains($"{sep}obj{sep}"))
				{
					statIgnored++;
					continue;
				}
				if (file.Contains($"{sep}Debug{sep}"))
				{
					statIgnored++;
					continue;
				}

				var relPath = file[inputFolder.Length..].TrimStart(sep);
				var outFile = Path.Combine(outputFolder, relPath);
				var outDir = Path.GetDirectoryName(outFile);

				var ext = Path.GetExtension(file).ToLowerInvariant();
				var errMsg = $"Unknown extension '{ext}'";
				try
				{
					switch (ext)
					{
						// Existing .NET code
						case ".cs":
						case ".csproj":
						case ".sln":
						case ".xaml":
						case ".resx":
						case ".settings":
						case ".config":
						// It's native C++
						case ".c":
						case ".cpp":
						case ".h":
						case ".vcproj":
						case ".vcxproj":
						// Something
						case ".zip":
						case ".swatch":
						case ".skin":
						case ".dat":
						case ".nsi":
						// Just text files
						case "":
						case ".gitignore":
						case ".bat":
						case ".html":
						case ".htm":
						case ".php":
						case ".chm":
						case ".css":
						case ".js":
						case ".xml":
						case ".xsd":
						case ".txt":
						case ".md":
						case ".inf":
						case ".ini":
						case ".skinscript":
						case ".cfg":
						case ".tex":
						case ".pdf":
						case ".sharpenviro":
						// Wordpress
						case ".mo":
						case ".po":
						// Just audio
						case ".wav":
						// Just graphics
						case ".ico":
						case ".cur":
						case ".ani":
						case ".png":
						case ".bmp":
						case ".svg":
						case ".psd":
						case ".jpg":
						case ".jpeg":
						case ".gif":
						case ".xcf":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;

						// Delphi code
						case ".pas":
							EnsureDir(outDir);
							Delphi.ParsePas(file, outFile);
							statParsed++;
							continue;
						case ".dfm":
							EnsureDir(outDir);
							Delphi.ParseDfm(file, outFile);
							statParsed++;
							continue;
						case ".dpr":
							EnsureDir(outDir);
							Delphi.ParseDpr(file, outFile);
							statParsed++;
							continue;
						case ".dpk":
							EnsureDir(outDir);
							Delphi.ParseDpk(file, outFile);
							statParsed++;
							continue;
						case ".dproj":
							EnsureDir(outDir);
							Delphi.ParseDproj(file, outFile);
							statParsed++;
							continue;
						case ".groupproj":
							EnsureDir(outDir);
							Delphi.ParseGroupPrj(file, outFile);
							statParsed++;
							continue;

						// Ignore these for parsing
						case ".rc":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".res":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".dcr":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".inc":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".bpr":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".bpl":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".bpg":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".bpf":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".def":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".cbproj":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".bdsproj":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;
						case ".manifest":
							EnsureDir(outDir);
							JustCopy(file, outFile);
							continue;

						// Binaries and building
						case ".lock":
						case ".suo":
						case ".lastbuildstate":
						case ".tlog":
						case ".cache":
						case ".baml":
						case ".pdb":
						case ".dll":
						case ".exe":
						case ".lib":
						case ".exp":
						case ".user":
						case ".new":
						case ".save":
						case ".backup":
						case ".filters":
						case ".1":
						case ".2":
							statIgnored++;
							continue;
					}
				}
				catch (Exception ex)
				{
					if (ex is TranslateError err)
					{
						var destDir = Path.GetDirectoryName(outFile)!;
						var origExt = Path.GetExtension(err.Path);
						var origName = Path.GetFileName(err.Target)
							.Replace(origExt, string.Empty) + origExt;
						var destName = Path.Combine(destDir, origName);
						using var writer = File.CreateText(destName);
						err.Code.Write(writer);
					}
					var e = ex.InnerException ?? ex;
					errMsg = e.Message.Split(" at ", 2).FirstOrDefault()?.Trim();
					statFailed++;
				}
				Console.Error.WriteLine($" * {relPath} --> {errMsg}!");
			}

			Console.WriteLine();
			Console.WriteLine($"Ignored files  = {statIgnored}");
			Console.WriteLine($"Existing files = {statExisting}");
			Console.WriteLine($"Copied files   = {statCopied}");
			Console.WriteLine($"Failed files   = {statFailed}");
			Console.WriteLine($"Parsed files   = {statParsed}");
			Console.WriteLine($"All files      = {fileCount}");
			statIgnored = 0;
			statExisting = 0;
			statCopied = 0;
			statFailed = 0;
			statParsed = 0;

			Console.WriteLine();
			Console.WriteLine("Done.");
			return 0;
		}

		private static void EnsureDir(string? folder)
		{
			if (!string.IsNullOrWhiteSpace(folder) && !Directory.Exists(folder))
				Directory.CreateDirectory(folder);
		}

		private static void JustCopy(string inFile, string outFile)
		{
			if (File.Exists(outFile))
			{
				statExisting++;
				return;
			}
			statCopied++;
			File.Copy(inFile, outFile, overwrite: true);
		}
	}
}