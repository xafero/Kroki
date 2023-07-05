using System;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Core.Model
{
	public sealed class TranslateError : Exception
	{
		public TranslateError(string path, SourceText code, Exception cause, string target)
			: base("Translation failed!", cause)
		{
			Path = path;
			Code = code;
			Target = target;
		}

		public string Path { get; }
		public SourceText Code { get; }
		public string Target { get; }
	}
}