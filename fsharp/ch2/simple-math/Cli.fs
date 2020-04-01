module SimpleMath.Cli

open SimpleMath.Calc
open SimpleMath.Core
open Input.Cli

let private prompt question1 question2 =
    (askUnsignedFloat question1, askUnsignedFloat question2)

let private d (f:float): string =
    f.ToString("0.##")

let display (results: List<Operation>) =
    for op in results do
     match op with
     | Operation(symbol, left, right, result) -> printfn "%s %s %s = %s" (d left) symbol (d right) (d result)
     
let run question1 question2 =
    prompt question1 question2
    |> all
    |> display

