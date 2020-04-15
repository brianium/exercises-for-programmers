// Learn more about F# at http://fsharp.org

open System
open System.Threading
open Input.Cli
open Checkout.Domain

/// View

let display order =
    printfn ""
    order.SubTotal |> printfn "Subtotal: $%0.2f" 
    order.TaxRate * order.SubTotal |> printfn "Tax: $%0.2f"
    Order.total order |> printfn "Total: $%0.2f"

let controlKey =
    match int Environment.OSVersion.Platform with
    | 6 -> "Cmd"
    | _ -> "Ctrl"

/// Async Order state

type Msg =
    | Update of Order
    | Checkout

let update (agent: MailboxProcessor<Msg>) order =
    Update order |> agent.Post
    order

let checkout (cts: CancellationTokenSource) (order: Order) = 
    let o = Order.checkout order
    display o
    cts.Cancel()
    o

let createAgent (cts: CancellationTokenSource) (order: Order) =
    MailboxProcessor.Start((fun inbox ->
        let rec messageLoop prevOrder =
            async {
                let! msg = inbox.Receive()

                let newState = match msg with
                               | Update o -> o
                               | Checkout -> checkout cts prevOrder

                return! messageLoop newState
            }
        messageLoop order), cts.Token)

/// Run that bad brother

let rec takeOrder (agent: MailboxProcessor<Msg>) order: Order =
    let num = order.LineItems.Length + 1
    let (price,quantity) = (
        sprintf "Enter the price of item %d:" num |> askUnsignedFloat,
        sprintf "Enter the quantity of item %d:" num |> askUnsignedInt
    )
    LineItem.create price quantity 
    |> Order.addItem order
    |> update agent
    |> takeOrder agent

[<EntryPoint>]
let main argv =
    use cts = new CancellationTokenSource()
    let order = Order.create 0.055
    let agent = createAgent cts order

    Console.CancelKeyPress.Add(
        fun e -> agent.Post Checkout
                 cts.Token.WaitHandle.WaitOne() |> ignore
    )

    printfn "Press %s + C to complete checkout" controlKey
    takeOrder agent order |> ignore

    0 // return an integer exit code
