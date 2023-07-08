unit OneForm;

{$mode ObjFPC}{$H+}

interface

uses
  Classes, SysUtils, Forms, Controls, Graphics, Dialogs, StdCtrls;

type

  { TOneForm }

  TOneForm = class(TForm)
    LoginBtn: TButton;
    NameBox: TEdit;
    procedure LoginBtnClick(Sender: TObject);
  private

  public

  end;

  TDrawFlag = (dfDontDraw, dfWordWrap, dfStretchImages, dfAlignMiddle, dfAlignBottom);
  TDrawFlags = set of TDrawFlag;

  TPartType = (ptNone, ptText, ptColor, ptStyle, ptBitmap);

  TFormBorderStyle = (bsNone, bsSingle, bsSizeable, bsDialog, bsToolWindow, bsSizeToolWin);

  TBorderStyle = bsNone..bsSingle;
  TBorderSizes = 23..42;
  TBorderSigns = 'A'..'z';

  TOnLinkClick = procedure(Sender: TObject; Link: String) of object;

  TStickText = (stTop, stNone, stBottom);

var
  OneForm2: TOneForm;

implementation

{$R *.lfm}

{ TOneForm }

procedure TOneForm.LoginBtnClick(Sender: TObject);
begin
  WriteLn('Hello!');
end;

end.

