namespace TaxCalculator.Domain

type Order =
    { Amount: float
      State: string }

type Transaction =
    { Order: Order
      SubTotal: float
      TaxRate: float }

module Order =
    let create (amount, state) =
        { Amount = amount
          State = state }

    let complete tax order =
        match order.State.ToLower() with
        | "wi" | "wisconsin" when tax >= 0.0 && tax <= 100.00 -> { Order = order; SubTotal = order.Amount; TaxRate = tax }
        | _ when tax < 0.0 || tax > 100.0 -> failwith "Tax rate must be between 0.0 and 100.0 inclusive"
        | _ -> { Order = order; SubTotal = order.Amount; TaxRate = 0.0 }

module Transaction =
    open System

    let private divBy100 x = x / 100.0

    let toNearestCent f =
        f * 100.00
        |> Math.Ceiling
        |> divBy100
        |> decimal

    let tax { TaxRate = t; SubTotal = s } =
        t / 100.0
        |> (*) s

    let total transaction =
        tax transaction
        |> (+) transaction.SubTotal
        |> toNearestCent
