using System.Collections.Generic;

namespace Kroki.Bulk.Read
{
	internal record OneProj(string Root, IDictionary<string, string> Meta,
		ISet<string> Sources, ISet<string> Links);
}