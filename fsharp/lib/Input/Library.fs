namespace Input

open System

module Parse =
    type ParseState<'a> =
    | Empty
    | Positive of 'a
    | Failure
    | Negative of 'a
    
    let private makeParseState (zeroVal) (success:bool,result:'a) =
        match success with
        | true when result < zeroVal -> Negative(result)
        | true when result >= zeroVal -> Positive(result)
        | _ -> Failure

    let parse<'a when 'a : comparison> (parseFunc: string -> (bool * 'a)) (zeroVal: 'a) (str:string) : ParseState<'a> =
        match parseFunc str with
        | true, v -> (true,v)
        | _ -> (false, zeroVal)
        |> makeParseState zeroVal

    let parseInt = parse Int32.TryParse 0

    let parseFloat = parse Double.TryParse 0.00

module Cli =

    open Parse

    let prompt text =
        printf "%s " text
        stdin.ReadLine()
        |> (fun s -> s.Trim())

    type Negatives = Allowed=1 | Restricted=0

    let rec private askForNumber parse text allowNegatives result =
        match result with
        | Positive(d) -> d
        | Negative(d) when allowNegatives -> d
        | _ -> match result with
               | Failure -> (sprintf "Value must be a valid number.\n%s" text)
               | Negative _ -> (sprintf "Value must not be negative.\n%s" text)
               | _ -> text
               |> prompt
               |> parse
               |> askForNumber parse text allowNegatives

    let askInt (negatives: Negatives) text = askForNumber parseInt text (negatives = Negatives.Allowed) Parse.Empty

    let askFloat (negatives: Negatives) text = askForNumber parseFloat text (negatives = Negatives.Allowed) Parse.Empty

    let askUnsignedInt = askInt Negatives.Restricted

    let askSignedInt = askInt Negatives.Allowed

    let askUnsignedFloat = askFloat Negatives.Restricted

    let askSignedFloat = askFloat Negatives.Allowed

    
// open NumberInput.Cli
// askSignedInt "What is the number?"
// askSignedFloat "What is the number?"
// askUnsignedInt "What is the number?"
// askUnsignedFloat "What is the number?"
