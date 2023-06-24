program exObjects;
type 
   Rectangle = object  
   private  
      length, width: integer; 
   
   public  
      procedure setlength(l: integer);
      function getlength(): integer;  
      
      procedure setwidth(w: integer);  
      function getwidth(): integer;  
      
      procedure draw;
end;
var
   r1: Rectangle;
   pr1: ^Rectangle;

procedure Rectangle.setlength(l: integer);
begin
   length := l;
end;

procedure Rectangle.setwidth(w: integer);
begin
   width :=w;
end;

function Rectangle.getlength(): integer;  
begin
   getlength := length;
end;

function Rectangle.getwidth(): integer;  
begin
   getwidth := width;
end;

procedure Rectangle.draw;
var 
   i, j: integer;
begin
   for i:= 1 to length do
   begin
     for j:= 1 to width do
        write(' * ');
     writeln;
   end;
end;

begin
   r1.setlength(3);
   r1.setwidth(7);
   
   writeln('Draw a rectangle:', r1.getlength(), ' by ' , r1.getwidth());
   r1.draw;
   new(pr1);
   pr1^.setlength(5);
   pr1^.setwidth(4);
   
   writeln('Draw a rectangle:', pr1^.getlength(), ' by ' ,pr1^.getwidth());
   pr1^.draw;
   dispose(pr1);
end.
