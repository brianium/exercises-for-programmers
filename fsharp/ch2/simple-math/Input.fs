module SimpleMath.Input

open System
open SimpleMath.Core

let private makeParseState (success:bool,result:float) =
    match success with
    | true -> if result < 0.00 then Negative else Success(result)
    | false -> Failure

let parse (str:string) =
    Double.TryParse str
    |> makeParseState


