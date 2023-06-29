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
            NativeInt a = default;
            NativeUInt b = default;
            uint c = default;
            uint d = default;
            ShortInt e = default;
            SmallInt f = default;
            FixedInt g = default;
            int h = default;
            Int64 i = default;
            Byte j = default;
            Word k = default;
            FixedUInt l = default;
            Cardinal m = default;
            UInt64 n = default;
            AnsiChar c3 = default;
            WideChar c4 = default;
            UCS2Char c5 = default;
            UCS4Char c6 = default;
            char c2 = default;
            UnicodeString c7 = default;
            bool b2 = default;
            ByteBool b3 = default;
            bool b4 = default;
            LongBool b5 = default;
            EnumeratedTypeNode e1 = default;
            TSound e2 = default;
            Suit e3 = default;
            TMyColor e4 = default;
            Answer e5 = default;
            Size e6 = default;
            SomeEnum e7 = default;
            float f1 = default;
            double f2 = default;
            float f3 = default;
            long f4 = default;
            Comp f5 = default;
            decimal f6 = default;
            Console.WriteLine(a, b, c, d, e, f, g, h, i, j, k, l, m, n);
            Console.WriteLine(c2, c3, c4, c5, c6, c7);
            Console.WriteLine(b2, b3, b4, b5);
            Console.WriteLine(e1, e2, e3, e4, e5, e6, e7);
            Console.WriteLine(f1, f2, f3, f4, f5, f6);
        }
    }
}
