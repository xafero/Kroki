using System.Linq;
using DGrok.DelphiNodes;
using Kroki.Roslyn.API;
using Kroki.Roslyn.Code;
using Kroki.Roslyn.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Roslyn.Code.Construct;
using static Kroki.Roslyn.Code.Express;
using static Kroki.Roslyn.Code.Prebuilt;
using A = System.Array;

namespace Kroki.Core.Code
{
	internal static class Designer
	{
		public const string Sl = "Create until here";
		public const string Rl = "Now finish the layout";

		public static void CreateBase(NamespaceObj nsp, ObjectNode node)
		{
			var name = node.ObjectNameNode.GetText();
			var (fd, fb) = CreateForm(name);
			nsp.Usings.Add("System.Windows.Forms");
			nsp.Members.Add(fb);
			nsp.Members.Add(fd);
			var dm = fd.Members.OfType<MethodObj>().Last();
			var ds = dm.Statements;
			ds.Add(InvokeS("SuspendLayout").Comment(Sl));
			ds.Add(InvokeS("ResumeLayout", AsBoolValue(false).Arg()).Comment(Rl));
			ds.Add(InvokeS("PerformLayout"));
		}

		public static void CreateChild(NamespaceObj nsp, ObjectNode node)
		{
			var name = node.ObjectNameNode.GetText();
			var type = node.ObjectTypeNode.GetText();
			var design = nsp.Members.OfType<ClassObj>().Last();
			var dm = design.Members.OfType<MethodObj>().Last();
			var cStr = Create(type, A.Empty<ArgumentSyntax>());
			dm.Statements.Insert(1, Assign(name, cStr).AsStat());
			var field = new FieldObj(name)
			{
				FieldType = type, Visibility = Visibility.Private
			};
			design.Members.Add(field);
		}
	}
}