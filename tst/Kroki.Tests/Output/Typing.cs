using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public enum TSound
    {
        Click,
        Clack,
        Clock
    }

    public enum Suit
    {
        Club,
        Diamond,
        Heart,
        Spade
    }

    public enum TMyColor
    {
        mcRed,
        mcBlue,
        mcGreen,
        mcYellow,
        mcOrange
    }

    public enum Answer
    {
        ansYes,
        ansNo,
        ansMaybe
    }

    public enum Size
    {
        Small = 5,
        Medium = 10,
        Large = Small + Medium
    }

    public enum SomeEnum
    {
        e1 = 1,
        e2 = 2,
        e3 = 3
    }

    public static class typing
    {
        public static void DBGridEnter(object Sender)
        {
            TSound Thing = default;
            Thing = Click;
            Console.WriteLine(Thing);
        }

        public static void DoNative()
        {
            long a = 1;
            ulong b = 2;
            long c = 3;
            ulong d = 4;
            sbyte e = 119;
            short f = 31767;
            int g = 204040347;
            int h = 147483647;
            long i = 372036854775807;
            byte j = 215;
            ushort k = 61535;
            uint l = 94967295;
            uint m = 94961195;
            ulong n = 467440737551615;
            string c3 = "?";
            string c4 = "?";
            string c5 = "?";
            string c6 = UCS4Char("?");
            char c2 = "?";
            string c7 = "?";
            bool b2 = false;
            bool b3 = true;
            bool b4 = false;
            bool b5 = true;
            EnumeratedTypeNode e1 = Heart;
            TSound e2 = TSound.Clack;
            Suit e3 = Suit.Spade;
            TMyColor e4 = TMyColor.mcGreen;
            Answer e5 = Answer.ansMaybe;
            Size e6 = Size.Medium;
            SomeEnum e7 = SomeEnum.e2;
            float f1 = 1.7038;
            double f2 = 1.79308;
            float f3 = 2.23308;
            decimal f4 = 1.184932;
            decimal f5 = 3372036854775807;
            decimal f6 = -92233720.5807;
            Console.WriteLine(a, b, c, d, e, f, g, h, i, j, k, l, m, n);
            Console.WriteLine(c2, c3, c4, c5, c6, c7);
            Console.WriteLine(b2, b3, b4, b5);
            Console.WriteLine(e1, e2, e3, e4, e5, e6, e7);
            Console.WriteLine(f1, f2, f3, f4, f5, f6);
        }
    }
}
