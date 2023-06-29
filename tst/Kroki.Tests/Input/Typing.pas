unit typing;

interface

uses
  Classes, SysUtils;

type
  TSound = (Click, Clack, Clock);
  Suit = (Club, Diamond, Heart, Spade);
  TMyColor = (mcRed, mcBlue, mcGreen, mcYellow, mcOrange);
  Answer = (ansYes, ansNo, ansMaybe);
  Size = (Small = 5, Medium = 10, Large = Small + Medium);
  SomeEnum = (e1 = 1, e2 = 2, e3 = 3);

implementation

procedure DBGridEnter(Sender: TObject);
 var
   Thing: TSound;
 begin
   Thing := Click;
   WriteLn(Thing);
 end;

procedure DoNative();
var
  a : NativeInt = 1; // Signed 64-bit; Int64
  b : NativeUInt = 2; // Unsigned 64-bit; UInt64
  c : LongInt = 3; // Signed 64-bit; Int64
  d : LongWord = 4; // Unsigned 64-bit; UInt64
  e : ShortInt = 119; // Signed 8-bit; Int8
  f : SmallInt = 31767; // Signed 16-bit; Int16
  g : FixedInt = 204040347; // Signed 32-bit; Int32
  h : Integer = 147483647; // Signed 32-bit; Int32
  i : Int64 = 372036854775807; // Signed 64-bit
  j : Byte = 215; // Unsigned 8-bit; UInt8
  k : Word = 61535; // Unsigned 16-bit; UInt16
  l : FixedUInt = 94967295; // Unsigned 32-bit; UInt32
  m : Cardinal = 94961195; // Unsigned 32-bit; UInt32
  n : UInt64 = 467440737551615; // Unsigned 64-bit
  c3 : AnsiChar = '?'; // byte-sized (8-bit) characters
  c4 : WideChar = '?'; // word-sized (16-bit) characters
  c5 : UCS2Char = '?'; // alias for WideChar
  c6 : UCS4Char = UCS4Char('?'); // 4–byte Unicode characters
  c2 : Char = '?';
  c7 : UnicodeString = '?';
  b2 : Boolean = False; // one byte of memory
  b3 : ByteBool = True; // one byte
  b4 : WordBool = False; // 2 bytes (one word)
  b5 : LongBool = True; // 4 bytes (2 words)
  e1: (Club, Diamond, Heart, Spade) = Heart; // enumerated types
  e2 : TSound = TSound.Clack;
  e3 : Suit = Suit.Spade;
  e4 : TMyColor = TMyColor.mcGreen;
  e5 : Answer = Answer.ansMaybe;
  e6 : Size = Size.Medium;
  e7 : SomeEnum = SomeEnum.e2;
  f1 : Single = 1.7038; // 4 bytes
  f2 : Double = 1.79308; // 8 bytes
  f3 : Real = 2.23308; // 8 bytes
  f4 : Extended = 1.184932; // 16 bytes
  f5 : Comp = 3372036854775807; // 8 bytes
  f6 : Currency = -92233720.5807; // 8 bytes
begin
  WriteLn(a, b, c, d, e, f, g, h, i, j, k, l, m, n);
  WriteLn(c2,c3,c4,c5,c6,c7);
  WriteLn(b2,b3,b4,b5);
  WriteLn(e1,e2,e3,e4,e5,e6,e7);
  WriteLn(f1,f2,f3,f4,f5,f6);
end;

end.

