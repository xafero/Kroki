using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public class TDateRec
    {
    }

    public static class project1
    {
        public static void DoDiv()
        {
            int x = default;
            int y = default;
            float z = default;
            x = 5;
            y = 3;
            z = x / y;
            Console.WriteLine(z);
        }

        public static void DoIntDiv()
        {
            int x = default;
            int y = default;
            int z = default;
            x = 5;
            y = 3;
            z = x / y;
            Console.Write(z);
        }

        public static void DoMod()
        {
            int x = default;
            int y = default;
            int z = default;
            x = 5;
            y = 3;
            z = x % y;
            Console.WriteLine(z);
        }

        public static void DoCombined()
        {
            int w = default;
            int x = default;
            int y = default;
            int z = default;
            w = 10;
            x = 5;
            y = 3;
            z = (w - x) * y;
            Console.WriteLine(z);
        }

        public static void DoOr()
        {
            bool x = default;
            bool y = default;
            bool z = default;
            x = true;
            y = false;
            z = y || x;
            Console.WriteLine(z);
        }

        public static void DoAnd()
        {
            bool x = default;
            bool y = default;
            bool z = default;
            x = true;
            y = false;
            z = y && x;
            Console.Write(z);
        }

        public static void DoNot()
        {
            bool x = default;
            bool y = default;
            x = true;
            y = !x;
            Console.Write(y);
        }

        public static void DoXor()
        {
            bool x = default;
            bool y = default;
            bool z = default;
            x = true;
            y = false;
            z = y ^ x;
            Console.Write(z);
        }

        public static void DoDiff()
        {
            int x = default;
            int y = default;
            bool isDifferent = default;
            x = 5;
            y = 10;
            isDifferent = (x != y);
            Console.Write(isDifferent);
        }

        public static void DoAllEqual1()
        {
            bool x = default;
            bool y = default;
            bool z = default;
            bool areAllEqual = default;
            x = false;
            y = false;
            z = true;
            areAllEqual = ((x == y) && (x == z) && (y == z));
            Console.Write(areAllEqual);
        }

        public static void DoAllEqual2()
        {
            bool x = default;
            bool y = default;
            bool z = default;
            bool areAllEqual = default;
            x = false;
            y = false;
            z = true;
            areAllEqual = ((x == y) && ((x == (z && y)) == z));
            Console.WriteLine(areAllEqual);
        }

        public static void DoPlus()
        {
            int x = default;
            int y = default;
            int z = default;
            x = 5;
            y = 3;
            z = x + y;
            Console.WriteLine(z);
        }

        public static void DoMinus()
        {
            int x = default;
            int y = default;
            int z = default;
            x = 5;
            y = 3;
            z = x - y;
            Console.Write(z);
        }

        public static void DoMul()
        {
            int x = default;
            int y = default;
            int z = default;
            x = 5;
            y = 3;
            z = x * y;
            Console.Write(z);
        }

        public static void takethis()
        {
            int I = default;
            for (I = 1; I <= 10; I++)
            {
                Console.WriteLine(I);
            }
        }

        public static void DoSomething()
        {
        }

        public static void DoSomethingElse()
        {
        }

        public static void DoACompletelyDifferentThing()
        {
        }

        public static void DoCase(int X, int a)
        {
            if (!X.In(new[] { 1, 2 }))
            {
                DoACompletelyDifferentThing();
            }
            else
            {
                switch (X)
                {
                    case 1:
                        DoSomething();
                        break;
                    case 2:
                        DoSomethingElse();
                        break;
                    default:
                        break;
                }
            }

            switch (a)
            {
                case 0:
                    Console.WriteLine(0);
                    break;
                default:
                    Console.WriteLine("else");
                    Console.WriteLine(a);
                    break;
            }

            switch (a)
            {
                case 0:
                    Console.WriteLine(0);
                    break;
                default:
                    Console.WriteLine("else");
                    Console.WriteLine(a);
                    break;
            }

            switch (a)
            {
                case 0:
                    Console.WriteLine(0);
                    break;
                default:
                    Console.WriteLine("else");
                    Console.WriteLine(a);
                    break;
            }
        }

        public static void DoChar()
        {
            char a = default;
            char b = default;
            a = "H";
            b = "i";
            Console.WriteLine(a);
            Console.WriteLine(a, b);
            Console.WriteLine(a, b, "!");
        }

        public static void DoValues()
        {
            int a = default;
            float b = default;
            float c = default;
            double d = default;
            long e = default;
            bool f = default;
            f = true;
            a = 1;
            f = false;
            Console.WriteLine(a, b, c, d, e, f);
        }

        public static string DoText()
        {
            string text1 = default;
            string text = default;
            char character1 = default;
            string firstWord = default;
            string secondWord = default;
            character1 = "H";
            text1 = "ello World!";
            Console.WriteLine(character1, text1);
            text1 = "I''m John Doe.";
            text = "Hello World!";
            Console.WriteLine(text(1));
            firstWord = "Hello";
            secondWord = "World";
            Console.WriteLine(firstWord + " " + secondWord + "!");
            return firstWord + " " + secondWord + "!";
        }

        public static void EnterChar()
        {
            int number = default;
            string _Input = default;
            char _Char1 = default;
            Console.WriteLine("Enter a char!");
            Console.ReadLine(_Input);
            _Char1 = _Input(1);
            number = Ord(_Char1);
            Console.WriteLine("ASCII is: ", number);
        }

        public static void EnterName()
        {
            string _Input = default;
            string _Linkage = default;
            Console.WriteLine("Hello, what is your name?");
            Console.ReadLine(_Input);
            _Linkage = "Hello, " + _Input;
            Console.WriteLine(_Linkage);
        }

        public static void DoLoops()
        {
            int i = default;
            for (i = 3; i <= 12; i++)
            {
                Console.WriteLine(i);
            }

            for (i = 22; i >= 3; i--)
            {
                Console.WriteLine(i);
            }

            i = 31;
            while (i >= 9)
            {
                Console.WriteLine(i);
                i = i - 1;
            }

            i = 32;
            do
            {
                Console.WriteLine(i);
            }
            while (!(i < 13));
        }

        public static void DoDisplay()
        {
            int var1 = default;
            var1 = 12;
            Console.WriteLine(var1);
            Console.ReadLine();
        }

        public static uint DoRetrieve()
        {
            const int const1 = 12;
            int var1 = default;
            Console.ReadLine(var1);
            return var1 + const1;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            {
                Record1.Year = 1922;
                Record1.Month = Nov;
                Record1.Day = 26;
            }

            Console.WriteLine("How old are you?");
            Console.ReadLine(age);
            Console.Write("Allowed to play: ");
            Console.WriteLine((23 < age) && (age > 52));
            if (a == false)
            {
                Console.WriteLine("a is false");
            }
            else
            {
                Console.WriteLine("a is true");
            }

            Console.WriteLine("Do you want to order a pizza?");
            Console.ReadLine(_Answer);
            if (_Answer == "Yes")
            {
                Console.WriteLine("You decided for yes!");
            }
            else
            {
                Console.WriteLine("Don''t want to have a pizza?");
            }
        }
    }
}
