namespace CompoundInterest.Domain

type Investment =
    { Principal: float
      InterestRate: float
      CompoundFrequency: int }

type Return =
    { Investment: Investment
      Years: int
      Value: float }

type Goal =
    { Value: float
      Years: int
      InterestRate: float
      CompoundFrequency: int }

module Goal =
    let create (value, years, rate, freq) =
        match rate with
        | x when x < 0.0 || x > 1.0 -> failwith "rate must be between 0.0 and 1.0 (inclusive)"
        | _ when freq < 0 -> failwith "frequency cannot be less than 0"
        | _ when years < 0 -> failwith "years cannot be less than 0"
        | _ -> { InterestRate = rate
                 CompoundFrequency = freq
                 Years = years
                 Value = value }

    let defaultGoal = create (0.0,0,0.0,0)

module Investment =
    open System

    let createReturn investment years value =
        { Investment = investment
          Years = years
          Value = value }

    let create (principal, rate, freq) =
        match rate with
        | x when x < 0.0 || x > 1.0 -> failwith "rate must be between 0.0 and 1.0 (inclusive)"
        | _ when freq < 0 -> failwith "frequency cannot be less than 0"
        | _ ->
            { Principal = principal
              InterestRate = rate
              CompoundFrequency = freq }

    let defaultInvestment = create (0.0,0.0,0)

    let private raiseBy exp b = Math.Pow(b, exp)

    let private divBy b x = x / b

    let private principalFactor (years: int) (investment: Investment) =
        1.0 + investment.InterestRate / float investment.CompoundFrequency
        |> raiseBy (float investment.CompoundFrequency * float years)

    let private nearestCent x =
        x * 100.0
        |> Math.Ceiling
        |> divBy 100.0

    let calculateCompoundInterest (years: int) (investment: Investment) =
        principalFactor years investment
        |> (*) investment.Principal
        |> nearestCent
        |> createReturn investment years

    let calculateInitialInvestment goal =
        let i = create (0.0, goal.InterestRate, goal.CompoundFrequency)
        { i with Principal = goal.Value / principalFactor goal.Years i |> nearestCent }
