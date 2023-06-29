unit project1;

interface

uses
  cthreads,
  Classes, formy;

type
  TDateRec = record
    Year: Integer;
    Month: (Jan, Feb, Mar, Apr, May, Jun,
            Jul, Aug, Sep, Oct, Nov, Dec);
    Day: 1..31;
  end;

var
  Record1: TDateRec;
  age:integer;
  a:boolean;
  _Answer: string;

implementation

procedure DoDiv;
var
  x, y: integer;
  z: real;
begin
  x := 5; // x = 5
  y := 3; // y = 3
  z := x / y; // Z = 1.666 (value of x divided by y)
  WriteLn(z);
end;

procedure DoIntDiv;
var
  x, y, z: integer;
begin
  x := 5; // x = 5
  y := 3; // y = 3
  z := x div y; // Z = 1 (value of x divided by y)
  Write(z);
end;

procedure DoMod;
var
  x, y, z: integer;
begin
  x := 5; // x = 5
  y := 3; // y = 3
  z := x mod y; // Z = 2 (remainder of x divided by y)
  WriteLn(z);
end;

procedure DoCombined;
var
  w, x, y, z: integer;
begin
  w := 10; // w = 10
  x := 5; // x = 5
  y := 3; // y = 3
  z := (w - x) * y; // Y = 15 (w subtracted by x and then multiplied by y)
  WriteLn(z);
end;

procedure DoOr;
var
  x, y, z: boolean;
begin
  x := true;
  y := false;
  z := y or x; // z = true
  WriteLn(z);
end;

procedure DoAnd;
var
  x, y, z: boolean;
begin
  x := true;
  y := false;
  z := y and x; // z = false
  write(z);
end;

procedure DoNot;
var
  x, y: boolean;
begin
  x := true;
  y := not x; // y = false
  write(y);
end;

procedure DoXor;
var
  x, y, z: boolean;
begin
  x := true;
  y := false;
  z := y xor x; // z = true
    write(z);
end;

procedure DoDiff;
var
  x, y: Integer;
  isDifferent: boolean;
begin
  x := 5;
  y := 10;
  isDifferent := (x <> y); // isDifferent = true
  write(isDifferent);
end;

procedure DoAllEqual1;
var
  x, y, z: boolean;
  areAllEqual: boolean;
begin
  x := false;
  y := false;
  z := true;
  areAllEqual := ((x = y) and (x = z) and (y = z)); // areAllEqual = false
  write(areAllEqual);
end;

procedure DoAllEqual2;
var
  x, y, z: boolean;
  areAllEqual: boolean;
begin
  x := false;
  y := false;
  z := true;
  areAllEqual := ((x = y) and ((x = (z and y)) = z)); // areAllEqual = true
  WriteLn(areAllEqual);
end;

procedure DoPlus;
var
 x, y, z: integer;

begin
 x := 5; // Value of x is 5
 y := 3; // Value of y is 3
 z := x + y; // Z now has the value 8 (value of x plus y)
 WriteLn(z);
end;

procedure DoMinus;
var
  x, y, z: integer;

begin
  x := 5; // x = 5
  y := 3; // y = 3
  z := x - y; // Z = 2 (value of x minus y)
  Write(z);
end;

procedure DoMul;
var
  x, y, z: integer;
begin
  x := 5; // x = 5
  y := 3; // y = 3
  z := x * y; // Z = 15 (value of x times y)
  write(z);
end;

procedure takethis();
var I: integer;
begin
for I := 1 to 10 do
  writeln(I);
end;

procedure DoSomething;
begin
end;

procedure DoSomethingElse;
begin
end;

procedure DoACompletelyDifferentThing;
begin
end;

procedure DoCase(X,a: integer);
begin
  if not X in [1,2] then
     DoACompletelyDifferentThing
  else
  case X of
    1: DoSomething;
    2: DoSomethingElse;
  end;

  case a of
  0: Writeln(0);
  else
    Writeln('else');
    Writeln(a);
    end;

case a of
  0: Writeln(0);
  else begin
    Writeln('else');
    Writeln(a);
  end;
end;

case a of
  0: Writeln(0);
else
  Writeln('else');
  Writeln(a);
end;
end;

procedure DoChar();
var
  a, b: char;
begin
  a := 'H';  // a = "H"
  b := 'i';  // b = "i"
  WriteLn(a);  // Display "H"
  WriteLn(a, b);  // Display "Hi"
  WriteLn(a, b, '!');  // Display "Hi!"
end;

procedure DoValues();
var
  a: Integer;
  b: Real = 2.3;   // Real is normally the same like Single
  c: Single = 4.1334;
  d: Double = 5.45;
  e: Extended = 92922233;
  f: Boolean;
begin
  f := True;
  a := 1;
  f := False;
  writeln(a,b,c,d,e,f);
end;

function DoText : string;
var
  text1: string;
  text:string;
  character1: char;
  firstWord,secondWord: string;

begin
    character1 := 'H';
     text1 := 'ello World!';
      WriteLn(character1, text1);

  text1 := 'I''m John Doe.';

  text := 'Hello World!';
   WriteLn(text[1]); // Only print "H".

   firstWord := 'Hello';
 secondWord := 'World';
 WriteLn(firstWord + ' ' + secondWord + '!');
 DoText := firstWord + ' ' + secondWord + '!';
end;

procedure EnterChar;
var
  number: Integer; // Declare number as an integer
  _Input: string;
  _Char1: char;
begin
  WriteLn('Enter a char!');
  ReadLn(_Input);
  _Char1 := _Input[1];
  number := Ord(_Char1);
  WriteLn('ASCII is: ', number);
end;

procedure EnterName;
var
 _Input, _Linkage: string;
 begin
   WriteLn('Hello, what is your name?');
   ReadLn(_Input);
   _Linkage := 'Hello, ' + _Input;
   WriteLn(_Linkage);
 end;

procedure DoLoops;
var
 i:integer;
begin
 for i := 3 to 12 do
 begin
   writeln(i);
 end;
  for i := 22 downto 3 do
 begin
   WriteLn(i);
 end;
  i:=31;
 while i>=9 do
 begin
   writeln(i);
   i:=i-1;
 end;
 i:=32;
  repeat
   writeln(i);
 until i<13;
end;

procedure DoDisplay;
var
 var1:integer;
begin
 var1:= 12;
 WriteLn (var1);
 ReadLn;
end;

function DoRetrieve:LongInt;
const
  const1 = 12;
var
  var1:integer;
begin
  ReadLn (var1);
  Result := var1 + const1;
end;

begin
  writeln('Hello World');

    with Record1 do
    begin
      Year := 1922;
      Month := Nov;
      Day := 26;
    end;

  WriteLn ('How old are you?');
  ReadLn (age);
  Write ('Allowed to play: ');
  WriteLn ((23 < age) and (age > 52)); // Display "Allowed to play: True" or "Allowed to play: False"

  if a = False then
     WriteLn('a is false')
   else WriteLn('a is true');

     WriteLn('Do you want to order a pizza?');
   ReadLn(_Answer);
   if _Answer = 'Yes' then
     WriteLn('You decided for yes!')
   else WriteLn('Don''t want to have a pizza?');
end.

