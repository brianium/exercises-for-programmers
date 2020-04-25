// Learn more about F# at http://fsharp.org

open System
open DrivingAge.Domain
open DrivingAge.Domain.Driving

let read text = 
    printf "%s " text
    stdin.ReadLine() |> (fun s -> s.Trim())

let rec askInt text =
    let intString = read text
    try
        match Int32.TryParse intString with
        | true, x when x > 0 -> x
        | _ -> failwith "Age must be a positive integer greater than 0"
    with
    | e -> printfn "%s" e.Message
           askInt text

[<EntryPoint>]
let main argv =
    askInt "What is your age?"
    |> getLegalCountries
    |> function
       | [] -> printfn "You are not old enough to drive in any of the known countries"
       | (list: List<Country>) -> printfn "You can drive in the following countries:"
                                  List.iter (fun c -> printfn "%s" c.Name) list
    0 // return an integer exit code
