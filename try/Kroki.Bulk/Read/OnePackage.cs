using System.Collections.Generic;

namespace Kroki.Bulk.Read
{
	internal record OnePackage(string Root, ISet<string> Files);
}