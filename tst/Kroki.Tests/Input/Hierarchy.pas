unit hierarch;

{$H+}

interface

uses
  Classes, SysUtils;

const
  WM_USER = 24;
  UM_CREATEDETAILS = WM_USER + $100;

type
  TDebugDialog = class(TObject)
  private
    FDetailsVisible: Boolean;
  end;

  TDebugDialogClass = class of TDebugDialog;

  TGeometry = class
  public
    function MyName: String; virtual; abstract;
  end;
  TGeometryClass = class of TGeometry;

  TGeometryMaker = class
  private
    FCurrentGeometryClass: TGeometryClass;
  public
    function GetNextGeometry: TGeometry;
    property CurrentGeometryClass: TGeometryClass read FCurrentGeometryClass write FCurrentGeometryClass;
  end;

  TRectangle = class(TGeometry)
  public
    function MyName: String; override;
  end;

  TCircle = class(TGeometry)
  public
    function MyName: String; override;
  end;

var
  DebugDialogClass: TDebugDialogClass = TDebugDialog;

implementation

function TGeometryMaker.GetNextGeometry: TGeometry;
begin
  Result := CurrentGeometryClass.Create;
end;

function TRectangle.MyName: String;
begin
  Result := 'Some rectangle!';
end;

function TCircle.MyName: String;
begin
  Result := 'I am the one and only circle.';
end;

procedure Button1Click(Sender: TObject; Lines: TStringList);
var
  Geo: TGeometry;
begin
  with TGeometryMaker.Create do
  try
    CurrentGeometryClass := TRectangle;
    Geo := GetNextGeometry;
    try
      Lines.Add(Geo.MyName);
    finally
      Geo.Free;
    end;

    CurrentGeometryClass := TCircle;
    Geo := GetNextGeometry;
    try
      Lines.Add(Geo.MyName);
    finally
      Geo.Free;
    end;
  finally
    Free;
  end;
end;

end.

