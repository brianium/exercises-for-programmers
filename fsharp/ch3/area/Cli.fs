module Area.Cli

open Input.Cli

open Area.Core

type UnitInput =
    | Unit of Unit
    | Default of Unit * string

let private makeInput x =
    match x with
    | "feet" -> Unit(Feet)
    | "meters" -> Unit(Meters)
    | _ -> Default(Feet,x)

let private askUnit text =
    prompt text
    |> makeInput

let private askDimensions unit =
    let l = unitLabel unit
    (
        unit, 
        askSignedFloat (sprintf "What is the length of the room in %s?" l),
        askSignedFloat (sprintf "What is the width of the room in %s?" l)
    )

let private getArea () =
    match askUnit "Do you want to give dimensions in 'feet' or 'meters'" with
        | Default(u, x) -> printfn "Did not understand unit '%s'. Defaulting to '%s'" x (unitLabel u)
                           u
        | Unit(u) -> u
        |> askDimensions
        |> Dimensions.Create
        |> Area.Create

let display area converted =
    printfn "The area is"
    printfn "%s" (areaLabel area)
    printfn "%s" (areaLabel converted)

let run () = 
    let area = getArea()
    convert area
    |> display area
