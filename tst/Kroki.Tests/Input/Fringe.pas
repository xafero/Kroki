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

end.

