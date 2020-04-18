// Learn more about F# at http://fsharp.org

open System
open Currency.Domain
open Currency.Cli.Http

type Direction =
    | To
    | From

let read () =
    stdin.ReadLine() |> (fun s -> s.Trim())

let rec askCountry direction =
    match direction with
    | From -> "Name of country to convert currency from (i.e USA, Canada, Germany)?"
    | To -> "Name of country to convert currency to (i.e Hong Kong, Sweden, France)?"
    |> printf "%s "
    let country = read()
    match Country.symbolFor country with
    | Some iso -> iso
    | None -> Country.closest country |> printfn "Could not find country \"%s\". Did you mean \"%s\"?" country
              askCountry direction

let rec askAmount sym =
    Symbol.shortSymbol sym |> printf "Amount (%s): "
    try
        match read () |> Double.TryParse with
        | true, v -> v
        | _ -> failwith "invalid number"
    with
    | _ -> printfn "Amount must be a valid number"
           askAmount sym

let display (from: Value) (target: Value) =
    printfn "%s at an exchange rate of %0.2f is %s " (from.ToString()) (target.Currency.Rate * 100.00) (target.ToString())

[<EntryPoint>]
let main argv =
    let (f,t) = (askCountry From, askCountry To)
    let value = Value.create (askAmount f,f,1.0)
    match f ->> t with
    | Some c -> Value.convert c value |> display value
                0
    | None -> printfn "Oh boyd! There was an error fetching rates... This is awkward, but can you try again in a bit?"
              1
