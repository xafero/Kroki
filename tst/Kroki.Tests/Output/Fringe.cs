using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public delegate int TStartFunc(int owner);
    public delegate int TStartFuncEx(int owner, bool pSkinInterface);
    public delegate void TStartAction(int owner, float hello);
    public delegate bool TWTSRegisterSessionNotification(int Wnd, uint dwFlags);
    public class characterChoice1 : List<string>
    {
    }

    public class characterChoice2 : List<string>
    {
    }

    public enum TSpeed
    {
        spVerySlow,
        spSlow,
        spAverage,
        spFast,
        spVeryFast
    }

    public class TPossibleSpeeds : List<TSpeed>
    {
    }

    public struct ptrToInt2
    {
    }

    public class signedQword
    {
        public ushort value;
        public int signum;
    }

    public struct signedQwordPacked
    {
        public ushort value;
        public int signum;
    }

    public class TA : object
    {
        private int GetA()
        {
            return -1;
        }

        private void SetA(int AValue)
        {
            Console.WriteLine(AValue);
        }

        public void Create()
        {
            Console.WriteLine("Class constructor TA");
        }

        public void Destroy()
        {
            Console.WriteLine("Class destructor TA");
        }

        public int A { get; set; }
    }

    public interface IMyDelegate
    {
        void DoThis(int value);
    }

    public class TMyClass : IMyDelegate
    {
        public void DoThis(int value)
        {
            string Str = default;
            Console.WriteLine("Success!!! Type <enter> to continue ", value);
            Console.ReadLine(Str);
        }
    }

    public static class fringe
    {
        public static bool IsFilterConnected()
        {
            return true;
        }

        public static void Comp(bool FGrabsSamples)
        {
            if (FGrabsSamples)
            {
                if (!IsFilterConnected())
                {
                    throw Exception.Create("Sample grabber not connected");
                }
            }
        }

        public static void DoFor(List<String> Items, long width, long height)
        {
            string menuItem = default;
            int I = default;
            foreach (var menuItem in Items)
            {
                Console.WriteLine(" (" + menuItem + ")");
            }

            for (I = 0; I <= width * height - 1; I++)
            {
                Console.WriteLine(I);
            }
        }

        public static bool CtrlDown()
        {
            int State = 453;
            Console.WriteLine(State);
            return ((State && 128) != 0);
        }

        public static void DrawPart()
        {
            float part = 4.223;
            Console.Write(part);
        }

        public static int DoTry(List<String> FLog, string logmsg)
        {
            int res = default;
            try
            {
                if (FLog != null)
                {
                    FLog.Add(logmsg);
                }
            }
            catch
            {
                FLog = null;
            }

            try
            {
                res = 42;
            }
            catch (Exception E)
            {
                res = -1;
            }

            return res;
        }

        public static void DoAssmbl()
        {
        }

        public delegate hResult TSHGetImageList(int iImageList, TGUID riid);
        public static string ImageListExtraLarge()
        {
            THandle hInstShell32 = default;
            TSHGetImageList SHGetImageList = default;
            const int SHIL_LARGE = 0;
            const int SHIL_SMALL = 1;
            const int SHIL_EXTRALARGE = 2;
            const string IID_IImageList = "{46EB5926-582E-4017-9FDF-E8998DAA0950}";
            hInstShell32 = LoadLibrary("Shell32.dll");
            if (hInstShell32 != 0)
            {
                try
                {
                    Console.WriteLine(hInstShell32, SHIL_LARGE, SHIL_EXTRALARGE, SHIL_SMALL, SHGetImageList(0, IID_IImageList));
                }
                finally
                {
                    FreeLibrary(hInstShell32);
                }
            }

            return "?";
        }

        public delegate bool TWTSRegisterSessionNotification(int Wnd, uint dwFlags);
        public static bool RegisterSessionNotification(int Wnd, uint dwFlags)
        {
            THandle hWTSapi32dll = default;
            TWTSRegisterSessionNotification WTSRegisterSessionNotification = default;
            hWTSapi32dll = LoadLibrary("Wtsapi32.dll");
            if (hWTSapi32dll > 0)
            {
                try
                {
                    return WTSRegisterSessionNotification(Wnd, dwFlags);
                }
                finally
                {
                    if (hWTSapi32dll > 0)
                    {
                        FreeLibrary(hWTSAPI32DLL);
                    }
                }
            }

            return false;
        }
    }
}
