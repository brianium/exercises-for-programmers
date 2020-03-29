module SimpleMath.Cli

open SimpleMath.Calc;
open SimpleMath.Core
open SimpleMath.Input

let private read text =
    printf "%s " text
    stdin.ReadLine()
    |> (fun s -> s.Trim())

let rec private ask text result =
    match result with
    | Success(d) -> d
    | Failure -> read (sprintf "Value must be a 32 bit integer.\n%s" text) |> parse |> ask text
    | Negative -> read (sprintf "Value must not be negative.\n%s" text) |> parse |> ask text
    | Empty -> read text |> parse |> ask text

let private prompt question1 question2 =
    (ask question1 Empty, ask question2 Empty)

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

