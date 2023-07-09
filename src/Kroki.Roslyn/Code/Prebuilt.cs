using Kroki.Roslyn.API;
using Kroki.Roslyn.Model;

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
	}
}