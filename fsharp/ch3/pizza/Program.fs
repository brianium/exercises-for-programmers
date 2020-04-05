// Learn more about F# at http://fsharp.org

namespace Pizza

module Program =
    open System

    open Input.Cli
    open Pizza.Party
    open Pizza.Order
    open Pizza.Display

    let partyFirst () =
        let party = Party.create (askUnsignedInt "How many people?", askUnsignedInt "How many pizzas do you have?", askUnsignedInt "How many slices per pizza?")
        let ration = Order.ration party
        printfn ""
        printfn "%s" (Display.party party)
        printfn "%s" (Display.ration ration)

    let orderFirst () =
        let order = Order.create (askUnsignedInt "How many people?", askUnsignedInt "How many slices per person?", askUnsignedInt  "How many slices per pizza?")
        let party = Order.party order
        printfn "You will have %s." (Display.party party)

    [<EntryPoint>]
    let main argv =
        match argv with
        | [|"order"|] -> orderFirst ()
        | _ -> partyFirst ()
        0 // return an integer exit code
