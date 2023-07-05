namespace Kroki.Core.Model
{
	public record Translated(string File, string Content, TranslateError? Error);
}