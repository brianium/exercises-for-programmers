module Area.Core

[<Literal>]
let ConversionFactor = 0.09290304

let toMetersSquared x = x * ConversionFactor

let toFeetSquared x = x / ConversionFactor

type Unit =
    | Feet
    | Meters

type Dimensions =
    { Length: float
      Width: float
      Unit: Unit }

type Dimensions with
    static member Create(unit: Unit, length, width) =
        { Length = length
          Width = width
          Unit = unit }

type Area =
    { Value: float
      Unit: Unit }

type Area with
    static member Create { Length = l; Width = w; Unit = u } =
        { Value = l * w; Unit = u }

let convert area =
    match area.Unit with
    | Feet -> { Value = toMetersSquared area.Value; Unit = Meters; }
    | Meters -> { Value = toFeetSquared area.Value; Unit = Feet }

let unitLabel unit =
    match unit with
    | Feet -> "feet"
    | Meters -> "meters"

let areaLabel area =
    sprintf "%s square %s" (area.Value.ToString("0.###")) (unitLabel area.Unit)
