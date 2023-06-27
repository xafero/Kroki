unit formy;

interface
  uses
  SysUtils, Variants, Classes, Dialogs;

  type
   TForm1 = class
     Edit1: string;
     Button1: TButton;
     Label1: TLabel;
     procedure Button1Click(Sender: TObject);
   private
     { Private declarations }
     FMQServer: string;
     function getMQServer: string;
     procedure setMQserver(const Value: string);
   public
     { Public declarations }
     property MQServer : string read getMQServer write setMQserver;
   end;
 var
   Form1: TForm1;

implementation

 function TForm1.getMQServer: string;
begin
  Result := FMQServer;
end;

 procedure TForm1.setMQserver(const Value: string);
begin
  FMQServer := Value;
end;

 procedure TForm1.Button1Click(Sender: TObject);
 var s: string;
 begin
  s := 'Hello ' + Edit1 + ' Delphi welcomes you!';
  WriteLn(s);
 end;

end.

