namespace SimpleInterest.Domain

type Investment =
    { Principal: float
      InterestRate: float }

type Return =
    { Investment: Investment
      Years: int
      Value: float }

module Investment =
    open System

    let createReturn investment years value =
        { Investment = investment
          Years = years
          Value = value }

    let private divBy100 x = x / 100.0

    let calculateSimpleInterest years investment =
        1.0 + investment.InterestRate * float years
        |> (*) investment.Principal
        |> (*) 100.0
        |> Math.Ceiling
        |> divBy100
        |> createReturn investment years

    let calculateYearlySimpleInterest years investment =
      seq { for i in 1..years -> i }
      |> Seq.map (fun y -> calculateSimpleInterest y investment)

    let create (principal, rate) =
        match rate with
        | x when x < 0.0 || x > 1.0 -> failwith "rate must be between 0.0 and 1.0 (inclusive)"
        | _ ->
            { Principal = principal
              InterestRate = rate }
