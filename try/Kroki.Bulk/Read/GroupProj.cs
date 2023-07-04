using System.Collections.Generic;

namespace Kroki.Bulk.Read
{
	internal record GroupProj(string Root, ISet<string> Links);
}