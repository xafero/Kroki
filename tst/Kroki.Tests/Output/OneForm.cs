using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public class TOneForm : TForm
    {
        public TButton LoginBtn;
        public TEdit NameBox;
        public void LoginBtnClick(object Sender)
        {
            Console.WriteLine("Hello!");
        }
    }

    public enum TDrawFlag
    {
        dfDontDraw,
        dfWordWrap,
        dfStretchImages,
        dfAlignMiddle,
        dfAlignBottom
    }

    public class TDrawFlags : List<TDrawFlag>
    {
    }

    public enum TPartType
    {
        ptNone,
        ptText,
        ptColor,
        ptStyle,
        ptBitmap
    }

    public enum TFormBorderStyle
    {
        bsNone,
        bsSingle,
        bsSizeable,
        bsDialog,
        bsToolWindow,
        bsSizeToolWin
    }

    public static class OneForm
    {
        public static Range TBorderStyle = bsNone..bsSingle;
        public static Range TBorderSizes = 23..42;
        public static Range TBorderSigns = "A".."z";
    }

    public delegate void TOnLinkClick(object Sender, string Link);
    public enum TStickText
    {
        stTop,
        stNone,
        stBottom
    }
}
