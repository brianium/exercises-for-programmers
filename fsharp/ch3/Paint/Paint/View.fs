namespace Paint

module View = 
    open Xamarin.Forms
    open Fabulous.XamarinForms
    open Input.Parse
    open Paint.Domain

    let buttons dispatch =
        View.Grid(
            rowdefs = [Auto],
            coldefs = [Stars 33.3; Stars 33.3; Stars 33.3],
            children=[
                View.Button(
                    image = Path "rectangle@2x.png",
                    command = (fun _ -> Rectangle.defaultDimensions |> Rectangle |> UpdateRoom |> dispatch)
                ).Row(0).Column(0)
                View.Button(
                    image = Path "round@2x.png",
                    command = (fun _ -> Circle.defaultDimensions |> Round |> UpdateRoom |> dispatch)
                ).Row(0).Column(1)
                View.Button(
                    image = Path "lshaped@2x.png",
                    command = (fun _ -> Compound.defaultDimensions |> LShaped |> UpdateRoom |> dispatch)
                ).Row(0).Column(2)
            ]
        )

    let private changeHandler handler =
        (fun (args:TextChangedEventArgs) -> parseFloat args.NewTextValue |> handler |> UpdateRoom)

    type LabeledEntryProps = {label: string; value: float; onChange: ParseState<float> -> Room }

    /// This seems kind of meh compared to Avalonia.FuncUI
    /// I suspect the cleaner way to do this is to extend Fabulous.ViewElement
    /// or use something like https://github.com/Zaid-Ajaj/fabulous-simple-elements
    let labeledEntry dispatch (props: LabeledEntryProps) = [
        View.Label(
            margin = Thickness(0., 15.,0.,0.),
            text = props.label,
            textColor = Color.Black,
            fontSize = FontSize 14.
        )
        View.Entry(
            margin = Thickness(0.,0.,0.,15.),
            placeholder = props.label,
            keyboard = Keyboard.Numeric,
            text = props.value.ToString("0.##"),
            textChanged = ((changeHandler props.onChange) >> dispatch)
        )                                                       
    ]

    let rectangle (dims:Domain.Rectangle.Dimensions) (state: State) dispatch =
        View.StackLayout(
            margin = Thickness(0., 15.),
            children = [
                yield View.Label(
                    text = "Rectangular Ceiling",
                    textColor = Color.Black,
                    fontSize = FontSize 24.
                )
                yield! labeledEntry dispatch
                    {label = "Length of room";
                     value = dims.Length;
                     onChange = (fun parseState -> match parseState with
                                                   | Positive x -> Room.Rectangle { Length = x; Width = dims.Width }
                                                   | Failure -> Room.Rectangle { Length = 0.0; Width = dims.Width }
                                                   | _ -> state.Room)} 
                yield! labeledEntry dispatch
                    {label = "Width of room";
                     value = dims.Width;
                     onChange = (fun parseState -> match parseState with
                                                   | Positive x -> Room.Rectangle { Length = dims.Length; Width = x}
                                                   | Failure -> Room.Rectangle { Length = dims.Length; Width = 0.0 }
                                                   | _ -> state.Room)}
            ]
        )

    let round radius state dispatch =
        View.StackLayout(
            margin = Thickness(0., 15.),
            children = [
                yield View.Label(
                    text = "Round Ceiling",
                    textColor = Color.Black,
                    fontSize = FontSize 24.
                )
                yield! labeledEntry dispatch
                    {label = "Diameter of room";
                     value = (Circle.unwrapDiameter radius);
                     onChange = (fun parseState -> match parseState with
                                                   | Positive x -> Circle.dimensionsFromDiameter x |> Room.Round
                                                   | Failure -> radius |> Room.Round
                                                   | _ -> state.Room)}
            ]
        )

    let lshaped (dims: Domain.Compound.Dimensions) state dispatch =
        View.Grid(
            rowdefs = [Auto;Auto],
            coldefs = [Stars 50.0; Stars 50.0],
            margin = Thickness(0., 15.),
            children = [
                View.Label(
                    text = "L-Shaped Ceiling",
                    textColor = Color.Black,
                    fontSize = FontSize 24.
                ).Row(0).Column(0).ColumnSpan(2)
                View.StackLayout(
                    children = [
                        yield! labeledEntry dispatch
                            {label = "Length of L's first side";
                             value = dims.[0].Length
                             onChange = (fun parseState -> match parseState with
                                                           | Positive x -> [{dims.[0] with Length = x};dims.[1]] |> Room.LShaped
                                                           | Failure -> dims |> Room.LShaped
                                                           | _ -> state.Room)}
                        yield! labeledEntry dispatch
                            {label = "Width of L's first side";
                             value = dims.[0].Width
                             onChange = (fun parseState -> match parseState with
                                                           | Positive x -> [{dims.[0] with Width = x};dims.[1]] |> Room.LShaped
                                                           | Failure -> dims |> Room.LShaped
                                                           | _ -> state.Room)}
                    ]
                ).Row(1).Column(0)
                View.StackLayout(
                    children = [
                        yield! labeledEntry dispatch
                            {label = "Length of L's second side";
                             value = dims.[1].Length
                             onChange = (fun parseState -> match parseState with
                                                           | Positive x -> [dims.[0]; {dims.[1] with Length = x}] |> Room.LShaped
                                                           | Failure -> dims |> Room.LShaped
                                                           | _ -> state.Room)}
                        yield! labeledEntry dispatch
                            {label = "Width of L's second side";
                             value = dims.[1].Width
                             onChange = (fun parseState -> match parseState with
                                                           | Positive x -> [dims.[0]; {dims.[1] with Width = x}] |> Room.LShaped
                                                           | Failure -> dims |> Room.LShaped
                                                           | _ -> state.Room)}
                    ]
                ).Row(1).Column(1)
            ]
        )

    let paint (room: Room) =
        let area = room |> Some |> Room.area
        View.StackLayout(
            margin = Thickness(0.,15.,0.,0.),
            children = [
                View.Label(
                    text = sprintf "You will need to purchase %d gallons of paint to cover %s square feet" (Room.gallonsRequired area) (area.ToString("0.##")),
                    isVisible = (area > 0.0)
                )
            ]
        )

    let room (state: State) dispatch =
        match state.Room with
        | Room.Rectangle x -> rectangle x state dispatch
        | Room.Round x -> round x state dispatch
        | Room.LShaped x -> lshaped x state dispatch