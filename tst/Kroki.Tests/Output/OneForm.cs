using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;
using TD3DXMatrix = TD3DXVector3;
using TD3DXQuaternion = TD3DXVector2;
using TInt64a = Int64;
using TInt64b = Comp;
using TInt64F = TInt64b;
using FInt64 = TInt64F;
using Int64D = TInt64a;

namespace Kroki.Example
{
    public struct PD3DXVector2
    {
    }

    public struct TD3DXVector2
    {
        public float x;
        public float y;
    }

    public struct TD3DXVector3
    {
        public float x;
        public float y;
        public float z;
    }

    public interface IsMatrix
    {
        TD3DXMatrix D3DXMatrix(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33);
        TD3DXMatrix D3DXMatrixAdd(TD3DXMatrix m1, TD3DXMatrix m2, TD3DXMatrix mOut);
        TD3DXMatrix D3DXMatrixSubtract(TD3DXMatrix m1, TD3DXMatrix m2, TD3DXMatrix mOut);
        TD3DXMatrix D3DXMatrixMul(TD3DXMatrix m, float MulBy, TD3DXMatrix mOut);
        bool D3DXMatrixEqual(TD3DXMatrix m1, TD3DXMatrix m2);
        TD3DXQuaternion D3DXQuaternionConjugate(TD3DXQuaternion qOut, TD3DXQuaternion q);
    }

    public struct PID3DXBuffer
    {
    }

    public interface ID3DXBuffer
    {
        IntPtr GetBufferPointer();
        ulong GetBufferSize();
    }

    public class TOneForm : TForm
    {
        public TButton LoginBtn;
        public TEdit NameBox;
        public void LoginBtnClick(object Sender)
        {
            Console.WriteLine("Hello!");
        }

        public static readonly TD3DXVector2 D3DXVector2Zero = new()
        {
            x = 0,
            y = 0
        };
        public static readonly TD3DXVector3 D3DXVector3Zero = new()
        {
            x = 0,
            y = 0,
            z = 0
        };
        public bool D3DXVector2Equal(TD3DXVector2 v1, TD3DXVector2 v2)
        {
            Console.WriteLine((v1), (v2));
            return true;
        }
    }

    public struct PCardinal
    {
    }

    public struct PInt64
    {
    }

    public class AInt64F : List<TInt64F>
    {
    }

    public struct PAInt64F
    {
    }

    public class TPERF_COUNTER_BLOCK
    {
        public uint ByteLength;
    }

    public struct PPERF_COUNTER_BLOCK
    {
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
        public static bool ParseThat(char c)
        {
            string tmp = default;
            tmp = (c);
            Console.WriteLine(tmp);
            return c.In(new[] { "a".."z", " ", "A".."Z" });
        }
    }

    public delegate void TOnLinkClick(object Sender, string Link);
    public enum TStickText
    {
        stTop,
        stNone,
        stBottom
    }

    public interface IDirectXFile
    {
        void Draw();
    }
}
