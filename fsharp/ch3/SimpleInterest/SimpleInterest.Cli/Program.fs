// Learn more about F# at http://fsharp.org

open System
open SimpleInterest.Domain

let rec askFloat prompt asPercent =
    printf "%s " prompt
    let input = stdin.ReadLine() |> (fun x -> x.Trim())
    try
        match Double.TryParse(input) with
        | true, x when asPercent && x >= 0.0 -> x / 100.0
        | true, x when not asPercent && x >= 0.0 -> x
        | _ -> failwith "Error parsing float"
    with
    | _ ->  printfn "%s is not a positive numeric value" input
            askFloat prompt asPercent

let rec askInt prompt =
    printf "%s " prompt
    let input = stdin.ReadLine() |> (fun x -> x.Trim())
    try
        match Int32.TryParse(input) with
        | true, x -> x
        | _ -> failwith "Error parsing int"
    with
    | _ -> printfn "%s is not a numeric value" input
           askInt prompt

let displayReturn (roi: Return) =
    printfn "After %i year(s) at %0.2f%%, the investment will be worth $%0.2f" roi.Years (roi.Investment.InterestRate * 100.0) roi.Value

let display (roi: seq<Return>) =
    Seq.iter displayReturn roi

[<EntryPoint>]
let main argv =
    (askFloat "Enter the principal:" false, askFloat "Enter the rate of interest:" true)
    |> Investment.create
    |> Investment.calculateYearlySimpleInterest (askInt "Enter the number of years:")
    |> display
    0 // return an integer exit code
