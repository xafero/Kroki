unit unitfunctions;

interface

procedure MyPublicProcedure();

function MyPublicFunction() : string;
function MyPublicBooleanFunction() : Boolean;

implementation

procedure MyPublicProcedure();
begin
end;


function MyPublicFunction() : string;
begin
	Result := 'coucou';
end;

function MyPublicBooleanFunction() : Boolean;
begin
	Result := False;
end;

end.
