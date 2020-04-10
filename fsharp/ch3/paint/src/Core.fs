namespace Paint

module Rectangle =
    type Dimensions =
        { Length: float
          Width: float }

    let dimensions l w =
        { Length = l
          Width = w }

    let area { Length = l; Width = w } = l * w

module Circle =
    open System

    type Radius = Radius of float

    let dimensions radius = Radius radius

    let dimensionsFromDiameter diameter =
        diameter / 2.0
        |> dimensions

    let area (Radius r) = Math.PI * r ** 2.0

module Compound =
    type Dimensions = List<Rectangle.Dimensions>

    let private fromTuple (l, w) = Rectangle.dimensions l w

    let dimensions (dims: List<float * float>) = List.map fromTuple dims

    let area dims =
        List.map Rectangle.area dims
        |> List.fold (+) 0.0

type Room =
    | Rectangle of Rectangle.Dimensions
    | Round of Circle.Radius
    | LShaped of Compound.Dimensions

module Room =
    [<Literal>]
    let FeetPerGallon = 350.0

    let fromList list =
        match list with
        | [l;w] -> Rectangle.dimensions l w |> Rectangle |> Some
        | [d] -> Circle.dimensionsFromDiameter d |> Round |> Some
        | [l1;w1;l2;w2] -> Compound.dimensions [(l1,w1);(l2,w2)] |> LShaped |> Some
        | _ -> None

    let area room =
        match room with
        | Some(Rectangle d) -> Rectangle.area d
        | Some(Round r) -> Circle.area r
        | Some(LShaped dims) -> Compound.area dims
        | None -> 0.0

    let gallonsRequired area =
        area / FeetPerGallon |> ceil |> int
