using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public class TDebugDialog : object
    {
        private bool FDetailsVisible;
    }

    public class TGeometry
    {
        public abstract string MyName();
    }

    public class TGeometryMaker
    {
        private TGeometryClass FCurrentGeometryClass;
        public TGeometry GetNextGeometry()
        {
            return CurrentGeometryClass.Create;
        }

        public TGeometryClass CurrentGeometryClass { get; set; }
    }

    public class TRectangle : TGeometry
    {
        public string MyName()
        {
            return "Some rectangle!";
        }
    }

    public class TCircle : TGeometry
    {
        public string MyName()
        {
            return "I am the one and only circle.";
        }
    }

    public static class hierarch
    {
        public static void Button1Click(object Sender, List<String> Lines)
        {
            TGeometry Geo = default;
            {
                try
                {
                    CurrentGeometryClass = TRectangle;
                    Geo = GetNextGeometry;
                    try
                    {
                        Lines.Add(Geo.MyName);
                    }
                    finally
                    {
                        Geo.Free();
                    }

                    CurrentGeometryClass = TCircle;
                    Geo = GetNextGeometry;
                    try
                    {
                        Lines.Add(Geo.MyName);
                    }
                    finally
                    {
                        Geo.Free();
                    }
                }
                finally
                {
                    Free();
                }
            }
        }
    }
}
