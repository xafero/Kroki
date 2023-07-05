unit fringe;

{$H+}

interface

uses
  Classes, SysUtils;

type
  TStartFunc   = function(owner: integer): integer;
  TStartFuncEx = function(owner: integer; pSkinInterface : Boolean): integer;
  TStartAction = procedure(owner: integer; hello: Single);
  TWTSRegisterSessionNotification = function(Wnd: integer; dwFlags: DWORD): Boolean; stdcall;
  characterChoice1 = array[integer] of UCS4char;
  characterChoice2 = array of UCS4char;
  TSpeed = (spVerySlow,spSlow,spAverage,spFast,spVeryFast);
  TPossibleSpeeds = set of TSpeed;
  ptrToInt2 = ^Integer;
  signedQword = record
 	value: qword;
 	signum: -1..1;
  end;
  signedQwordPacked = packed record
	value: qword;
	signum: -1..1;
  end;
  TA = Class(TObject)
  Private
    Function GetA : Integer;
    Procedure SetA(AValue : integer);
  public
    Class Constructor create;
    Class Destructor destroy;
    Property A : Integer Read GetA Write SetA;
  end;
  IMyDelegate = interface
    procedure DoThis (value: integer);
  end;
  TMyClass = class (IMyDelegate)
    procedure DoThis (value: integer);
  end;

implementation

function IsFilterConnected(): Boolean;
begin
  Result := True;
end;

procedure Comp(FGrabsSamples: Boolean);
begin
  if FGrabsSamples then
  begin
    if not IsFilterConnected() then
    begin
      raise Exception.Create('Sample grabber not connected');
    end;
  end;
end;

{Class} Function TA.GetA : Integer;
begin
  Result:=-1;
end;

{Class} Procedure TA.SetA(AValue : integer);
begin
  WriteLn(AValue);
end;

Class Constructor TA.Create;
begin
  Inherited;
  Writeln('Class constructor TA');
end;

Class Destructor TA.Destroy;
begin
  Writeln('Class destructor TA');
end;

procedure DoFor(Items : TStringList; width: LongInt; height: LongInt);
var
  menuItem : String;
  I : integer;
begin
  for menuItem in Items do
    begin
      WriteLn(' (' + menuItem + ')');
    end;
  for I := 0 to width * height - 1 do
    begin
      WriteLn(I);
    end;
end;

function CtrlDown: Boolean;
  var
    State: integer = 453;
  begin
    WriteLn(State);
    Result := ((State and 128) <> 0);
  end;

procedure DrawPart;
  var
    part: Single = 4.223;
  begin
    Write(part);
  end;

function DoTry(FLog : TStringList; logmsg : string): integer;
var
  res : integer;
begin
  try
    if FLog <> nil then FLog.Add(logmsg);
  except
    FLog := nil;
  end;
  try
    res := 42;
  except
    on E:Exception do
    begin
      res := -1;
    end;
  end;
  Result := res;
end;

procedure TMyClass.DoThis(value: integer);
var
  Str: string;
begin
  WriteLn('Success!!! Type <enter> to continue ', value);
  ReadLn(Str);
end;

procedure DoAssmbl();
begin
  asm
      CMP       EDX,EAX
      CMOVG     EAX,EDX
      CMP       ECX,EAX
      CMOVG     EAX,ECX
    mov eax, FS:[4]
    sub eax, 3
  end;
end;

function ImageListExtraLarge: String;
type
  TSHGetImageList = function (iImageList: integer; const riid: TGUID): hResult; stdcall;
var
  hInstShell32: THandle;
  SHGetImageList: TSHGetImageList;
const
  SHIL_LARGE= 0; //32X32
  SHIL_SMALL= 1; //16X16
  SHIL_EXTRALARGE= 2;
  IID_IImageList: TGUID= '{46EB5926-582E-4017-9FDF-E8998DAA0950}';
begin
  hInstShell32:= LoadLibrary('Shell32.dll');
  if hInstShell32<> 0 then
  try
    WriteLn(hInstShell32, SHIL_LARGE, SHIL_EXTRALARGE, SHIL_SMALL, SHGetImageList(0,IID_IImageList));
  finally
    FreeLibrary(hInstShell32);
  end;
  Result:= '?';
end;

function RegisterSessionNotification(Wnd: Integer; dwFlags: Cardinal) : Boolean;
type
  TWTSRegisterSessionNotification = function(Wnd: Integer; dwFlags: DWORD): Boolean; stdcall;
var
  hWTSapi32dll : THandle;
  WTSRegisterSessionNotification : TWTSRegisterSessionNotification;
begin
  hWTSapi32dll := LoadLibrary('Wtsapi32.dll');
  if hWTSapi32dll > 0 then
  begin
    try
      Result:= WTSRegisterSessionNotification(Wnd, dwFlags);
    finally
      if hWTSapi32dll > 0 then
        FreeLibrary(hWTSAPI32DLL);
    end;
  end;
  Result := False;
end;

end.

