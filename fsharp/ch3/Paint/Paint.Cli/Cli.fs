namespace Paint

module Cli =
    open Input.Cli
    open Paint.Domain

    module RoomType =

        type T =
            | Rectangle
            | Round
            | LShaped

        let create (s:string) =
            let lower = s.ToLower()
            match lower with
            | "round" -> Round
            | "l-shaped" -> LShaped
            | _ -> Rectangle

        let label t =
            match t with
            | Rectangle -> "rectangle"
            | Round -> "round"
            | LShaped -> "l-shaped"

    let promptRoom () =
        "What type of ceiling are you painting? (round|l-shaped|rectangle)"
        |> prompt
        |> RoomType.create

    let promptDimensions r =
        RoomType.label r |> printfn "Using ceiling shape: %s"
        match r with
        | RoomType.Rectangle -> [askUnsignedFloat "What is the length of the ceiling?";askUnsignedFloat "What is the width of the ceiling?"]
        | RoomType.Round -> [askUnsignedFloat "What is the diameter of the ceiling?"]
        | RoomType.LShaped -> [
                askUnsignedFloat "What is the length of the L's first side?"
                askUnsignedFloat "What is the width of the L's first side?"
                askUnsignedFloat "What is the length of the L's second side?"
                askUnsignedFloat "What is the width of the L's second side?"
            ]

    let display room =
        let area = Room.area room
        Room.gallonsRequired area |> printfn "You will need to purchase %d gallons of"
        area.ToString("0.##") |> printfn "paint to cover %s square feet"

    let run () =
        promptRoom ()
        |> promptDimensions
        |> Room.fromList
        |> display
