using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    internal static class exObjects
    {
        public class Rectangle
        {
            private int length;
            private int width;
            public void setlength(int l)
            {
                length = l;
            }

            public int getlength()
            {
                return length;
            }

            public void setwidth(int w)
            {
                width = w;
            }

            public int getwidth()
            {
                return width;
            }

            public void draw()
            {
                int i = default;
                int j = default;
                for (i = 1; i <= length; i++)
                {
                    for (j = 1; j <= width; j++)
                    {
                        Console.Write(" * ");
                    }

                    Console.WriteLine();
                }
            }
        }

        public static Rectangle r1;
        public static Rectangle pr1;
        private static void Main(string[] args)
        {
            r1.setlength(3);
            r1.setwidth(7);
            Console.WriteLine("Draw a rectangle:", r1.getlength(), " by ", r1.getwidth());
            r1.draw();
            Compat.Renew("pr1");
            pr1.setlength(5);
            pr1.setwidth(4);
            Console.WriteLine("Draw a rectangle:", pr1.getlength(), " by ", pr1.getwidth());
            pr1.draw();
            Compat.Dispose("pr1");
        }
    }
}
