// Learn more about F# at http://fsharp.org

open System
open TaxCalculator.Domain

let prompt text =
    printf "%s " text
    stdin.ReadLine() |> (fun x -> x.Trim())

let rec promptFloat text =
    let x = prompt text
    try 
        match Double.TryParse x with
        | true, f when f > 0.0 -> f
        | _ -> failwith "Value must be numeric and greater than 0"
    with
        | ex -> printfn "%s" ex.Message
                promptFloat text

let formatCurrency (x:decimal) = x.ToString("C")

let display (t: Transaction) =
    match t.TaxRate with
    | x when x > 0.0 ->  Transaction.toNearestCent t.SubTotal |> formatCurrency |> printfn "The subtotal is %s."
                         Transaction.tax t |> decimal |> formatCurrency |> printfn "The tax is %s."
    | _ -> ()
    Transaction.total t |> formatCurrency |>
     printfn "The total is %s."

[<EntryPoint>]
let main argv =
    (promptFloat "What is the order amount?", prompt "What is the state?")
    |> Order.create
    |> Order.complete 5.5
    |> display
    0 // return an integer exit code
