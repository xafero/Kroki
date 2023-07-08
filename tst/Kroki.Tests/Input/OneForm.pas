unit OneForm;

{$mode ObjFPC}{$H+}

interface

uses
  Classes, SysUtils, Forms, Controls, Graphics, Dialogs, StdCtrls;

const
    Processor_IDX_Str = '238';
    Processor_IDX = 238;
    CPUUsageIDX = 6;
    d3dx8dll = 'D3DX8ab.dll';
    D3DX_PI    : Single  = 3.141592654;
    D3DX_1BYPI : Single  = 0.318309886;
    D3DX_DEFAULT : Cardinal = $FFFFFFFF;

type
  PD3DXVector2 = ^TD3DXVector2;
  TD3DXVector2 = packed record
    x, y : Single;
  end;
  TD3DXVector3 = packed record
    x, y, z : Single;
  end;

  TD3DXMatrix = TD3DXVector3;
  TD3DXQuaternion =        TD3DXVector2;
  IsMatrix = interface
    function D3DXMatrix(const m00, m01, m02, m03,
                          m10, m11, m12, m13,
                          m20, m21, m22, m23,
                          m30, m31, m32, m33 : Single) : TD3DXMatrix;
    function D3DXMatrixAdd(const m1,m2 : TD3DXMatrix; out mOut : TD3DXMatrix) : TD3DXMatrix;
    function D3DXMatrixSubtract(const m1, m2 : TD3DXMatrix; out mOut : TD3DXMatrix) : TD3DXMatrix;
    function D3DXMatrixMul(const m : TD3DXMatrix; MulBy : Single; out mOut : TD3DXMatrix) : TD3DXMatrix;
    function D3DXMatrixEqual(const m1, m2 : TD3DXMatrix) : Boolean;
    function D3DXQuaternionConjugate(out qOut : TD3DXQuaternion; const q : TD3DXQuaternion) : TD3DXQuaternion;
  end;

  PID3DXBuffer = ^ID3DXBuffer;
  ID3DXBuffer = interface(IUnknown)
    function GetBufferPointer : Pointer; stdcall;
    function GetBufferSize : LongWord; stdcall;
  end;

  { TOneForm }

  TOneForm = class(TForm)
    LoginBtn: TButton;
    NameBox: TEdit;
    procedure LoginBtnClick(Sender: TObject);
  private

  public
    const D3DXVector2Zero : TD3DXVector2 = (x : 0; y : 0);
    const D3DXVector3Zero : TD3DXVector3 = (x : 0; y : 0; z : 0);
    function D3DXVector2Equal(const v1, v2 : TD3DXVector2) : Boolean;
  end;

  PCardinal = ^Cardinal;

  TInt64a = Int64;
  TInt64b = Comp;
  PInt64 = ^TInt64a;

  TInt64F = TInt64b;
  FInt64 = TInt64F;
  Int64D = TInt64a;

  AInt64F = array[0..$FFFF] of TInt64F;
  PAInt64F = ^AInt64F;

  TPERF_COUNTER_BLOCK = record
    ByteLength : DWORD;
  end;
  PPERF_COUNTER_BLOCK = ^TPERF_COUNTER_BLOCK;

  TDrawFlag = (dfDontDraw, dfWordWrap, dfStretchImages, dfAlignMiddle, dfAlignBottom);
  TDrawFlags = set of TDrawFlag;

  TPartType = (ptNone, ptText, ptColor, ptStyle, ptBitmap);

  TFormBorderStyle = (bsNone, bsSingle, bsSizeable, bsDialog, bsToolWindow, bsSizeToolWin);

  TBorderStyle = bsNone..bsSingle;
  TBorderSizes = 23..42;
  TBorderSigns = 'A'..'z';

  TOnLinkClick = procedure(Sender: TObject; Link: String) of object;

  TStickText = (stTop, stNone, stBottom);

  IDirectXFile = interface
      procedure Draw;
  end;

var
  OneForm2: TOneForm;
  DirectXFileCreate : function (out lplpDirectXFile: IDirectXFile) : HResult; stdcall;

implementation

{$R *.lfm}

{ TOneForm }

procedure TOneForm.LoginBtnClick(Sender: TObject);
begin
  WriteLn('Hello!');
end;

function TOneForm.D3DXVector2Equal(const v1, v2: TD3DXVector2): Boolean;
begin
  WriteLn(string(v1), string(v2));
  D3DXVector2Equal := True;
end;

function ParseThat(c: char) : Boolean;
var
  tmp: string;
begin
  tmp := string(c);
  WriteLn( tmp );
  Result := c in ['a'..'z', ' ', 'A'..'Z'];
end;

end.

