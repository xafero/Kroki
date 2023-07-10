using Kroki.Roslyn.API;
using Kroki.Roslyn.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Kroki.Roslyn.Code.Construct;
using static Kroki.Roslyn.Code.Express;
using A = System.Array;

namespace Kroki.Roslyn.Code
{
	public static class Prebuilt
	{
		public static MethodObj CreateMain()
		{
			var main = new MethodObj("Main")
			{
				Visibility = Visibility.Private,
				IsStatic = true,
				IsAbstract = false
			};
			var args = new ParamObj("args") { Type = "string[]" };
			main.Params.Add(args);
			return main;
		}

		public static AttributeObj CreateStaThread()
		{
			var attr = new AttributeObj("STAThread");
			return attr;
		}

		public static ClassObj CreateProgram(bool isSta = true)
		{
			var pro = new ClassObj("Program")
			{
				IsStatic = true,
				Visibility = Visibility.Internal
			};
			var main = CreateMain();
			if (isSta)
			{
				var sta = CreateStaThread();
				main.Attributes.Add(sta);
			}
			pro.Members.Add(main);
			return pro;
		}

		public static (ClassObj design, ClassObj behind) CreateForm(string name = "Form1")
		{
			var design = new ClassObj(name) { Visibility = Visibility.None, IsPartial = true };
			var compFld = new FieldObj("components")
			{
				FieldType = "System.ComponentModel.IContainer",
				Visibility = Visibility.Private, Value = NullValue()
			};
			design.Members.Add(compFld);
			var disMet = new MethodObj("Dispose")
			{
				Visibility = Visibility.Protected, IsOverride = true
			};
			disMet.Params.Add(new ParamObj("disposing") { Type = "bool" });
			var ic = Binary(BinaryMode.And, Name("disposing"),
				Paren(Binary(BinaryMode.EqualNot, Name("components"), NullValue())));
			var then = new[]
			{
				Invoke("components", "Dispose", A.Empty<ArgumentSyntax>())
			};
			var ds = disMet.Statements;
			ds.Add(If(ic, then));
			ds.Add(Invoke("base", "Dispose", new[] { "disposing" }).AsStat());
			design.Members.Add(disMet);
			var dd = new MethodObj("InitializeComponent") { Visibility = Visibility.Private };
			var dde = Assign(Access(Name("this"), Name("components")),
				Create("System.ComponentModel.Container", A.Empty<ArgumentSyntax>()));
			dd.Statements.Add(dde.AsStat());
			design.Members.Add(dd);
			var behind = new ClassObj(name) { IsPartial = true };
			behind.Base.Add((TypeSyntax)Name("Form"));
			var fcm = new MethodObj("Form1") { IsConstructor = true };
			var fcmI = Invoke(Name("InitializeComponent"), A.Empty<ArgumentSyntax>());
			fcm.Statements.Add(fcmI.AsStat());
			behind.Members.Add(fcm);
			return (design, behind);
		}
	}
}