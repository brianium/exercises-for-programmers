namespace Area.Gui

module Window =
    open System
    open Elmish
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Input
    open Avalonia.Layout
    open Avalonia.FuncUI.Elmish
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Components
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.DSL
    open Input.Parse
    open Area.Core

    type State = Dimensions

    type Msg =
        | ChangeUnit of Unit
        | SetLength of float
        | SetWidth of float

    let init () = Dimensions.Create (Feet,0.0,0.0)

    let update (msg: Msg) (state: State) = 
        match msg with
        | ChangeUnit u -> { state with Unit =  u }
        | SetLength l -> { state with Length = l }
        | SetWidth w -> { state with Width = w }

    let dataItems = [Feet;Meters]

    let unitTemplate unit =
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text (unitLabel unit)
                ]
            ]
        ]

    let dispatchChangeUnit (args: SelectionChangedEventArgs) =
        seq { for i in args.AddedItems -> i }
        |> Seq.head
        :?> Unit
        |> ChangeUnit
    
    let unitSelect (state: State) dispatch =
        let index = match state.Unit with
                    | Feet -> 0
                    | Meters -> 1

        StackPanel.create [
            StackPanel.spacing 8.0
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "Select unit"
                ]
                ComboBox.create [
                    ComboBox.dataItems dataItems
                    ComboBox.selectedIndex index
                    ComboBox.onSelectionChanged (dispatchChangeUnit >> dispatch)
                    ComboBox.itemTemplate (DataTemplateView<Unit>.create unitTemplate)
                ]
            ]
        ]

    let input label props =
        StackPanel.create [
            StackPanel.spacing 8.0
            StackPanel.children [
                TextBlock.create [                    
                    TextBlock.text label
                ]
                TextBox.create props
            ]
        ]

    let areaSummary state =
        let area = Area.Create state
        let converted = convert area
        StackPanel.create [
            StackPanel.spacing 15.0
            StackPanel.children [
                TextBlock.create [                    
                    TextBlock.text "The area is:"
                ]
                TextBlock.create [                    
                    TextBlock.text (areaLabel area)
                ]
                TextBlock.create [                    
                    TextBlock.text (areaLabel converted)
                ]
            ]
        ]

    let private toFloat x =
        match parseFloat x with
        | Positive x | Negative x -> x
        | _ -> 0.0

    let view (state: State) dispatch =
        StackPanel.create [
            StackPanel.margin 10.0
            StackPanel.spacing 20.0
            StackPanel.verticalAlignment VerticalAlignment.Top
            StackPanel.children [
                unitSelect state dispatch
                UniformGrid.create [
                    UniformGrid.columns 2
                    UniformGrid.children [
                        input (sprintf "What is the length of the room in %s?" (unitLabel state.Unit)) [
                            TextBox.onTextChanged (toFloat >> SetLength >> dispatch)
                            TextBox.text (state.Length.ToString "0.###")
                        ]
                        input (sprintf "What is the width of the room in %s?" (unitLabel state.Unit)) [
                            TextBox.onTextChanged (toFloat >> SetWidth >> dispatch)
                            TextBox.text (state.Width.ToString "0.###")
                        ]
                    ]
                ]
                areaSummary state
            ]
        ]
    
    type GuiWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Area Tron"
            base.Width <- 500.0
            base.Height <- 220.0

            Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
