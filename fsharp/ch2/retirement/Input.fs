module Retirement.Input

open System

type UserInput =
    | Empty
    | Success of int
    | Failure
    | Negative

let private makeParseState (success:bool,result:int) =
    match success with
    | true -> if result < 0 then Negative else Success(result)
    | false -> Failure

let parse (str:string) =
    Int32.TryParse str
    |> makeParseState
