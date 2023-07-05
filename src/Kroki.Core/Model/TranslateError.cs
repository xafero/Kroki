using System;
using Microsoft.CodeAnalysis.Text;

namespace Kroki.Core.Model
{
	public sealed class TranslateError : Exception
	{
		public TranslateError(string path, SourceText code, Exception cause)
			: base("Translation failed!", cause)
		{
			Path = path;
			Code = code;
		}

		public string Path { get; }
		public SourceText Code { get; }
	}
}