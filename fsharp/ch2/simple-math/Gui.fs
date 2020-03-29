module SimpleMath.Gui

open Elmish

open Avalonia

open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes

open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.Components.Hosts

open SimpleMath.Core
open SimpleMath.Input
open SimpleMath.Calc

type State =
    { FirstNumber: ParseState
      SecondNumber: ParseState }

type State with
    member this.IsValid () =
        match [this.FirstNumber; this.SecondNumber] with
        | [Success(x); Success(y)] -> x > 0.0 || y > 0.0
        | _ -> false

let init =
    { FirstNumber = Success(0.00)
      SecondNumber = Success(0.00) }

type Msg = Change of ParseState * ParseState

let update (msg: Msg) (state: State): State =
    match msg with
    | Change(left, right) ->
        { state with
              FirstNumber = left
              SecondNumber = right }

let feedback (number: ParseState) =
    match number with
    | Failure -> "Value is not a number"
    | Negative -> "Number cannot be negative"
    | _ -> ""

let text (number: ParseState) =
    match number with
    | Success(d) -> d.ToString("0.##")
    | _ -> "0" 

let d (f: float): string =
    f.ToString("0.##")

let renderOp op: Types.IView =
    match op with
    | Operation(symbol, left, right, result) -> TextBlock.create [
        TextBlock.text (sprintf "%s %s %s = %s" (d left) symbol (d right) (d result))
        TextBlock.margin (0.0,5.0)
    ] |> generalize

let renderOps (state: State): List<Types.IView> =
    match [state.FirstNumber; state.SecondNumber] with
    | [Success(x); Success(y)] -> all (x, y) |> List.map renderOp
    | _ -> []

let view (state: State) (dispatch) =
    StackPanel.create
        [ StackPanel.children
            [ WrapPanel.create
                [ WrapPanel.width 350.0
                  WrapPanel.horizontalAlignment Layout.HorizontalAlignment.Center
                  WrapPanel.children
                      [ TextBlock.create
                          [ TextBlock.dock Dock.Left
                            TextBlock.width 170.0
                            TextBlock.margin (0.0, 5.0, 10.0, 0.0)
                            TextBlock.text "First number:" ]

                        TextBlock.create
                            [ TextBlock.dock Dock.Right
                              TextBlock.width 170.0
                              TextBlock.margin (0.0, 5.0, 0.0, 0.0)
                              TextBlock.text "Second number:" ]

                        TextBox.create
                            [ TextBox.dock Dock.Left
                              TextBox.fontSize 22.0
                              TextBox.width 170.0
                              TextBox.margin (0.0, 5.0, 10.0, 0.0)
                              TextBox.text (text state.FirstNumber) // <- This needed to be here to prevent an infinite rerender
                              TextBox.onTextChanged (fun text ->
                                  (parse text, state.SecondNumber)
                                  |> Change
                                  |> dispatch) ]

                        TextBox.create
                            [ TextBox.dock Dock.Right
                              TextBox.fontSize 22.0
                              TextBox.width 170.0
                              TextBox.margin (0.0, 5.0, 0.0, 0.0)
                              TextBox.text (text state.SecondNumber)
                              TextBox.onTextChanged (fun text ->
                                  (state.FirstNumber, parse text)
                                  |> Change
                                  |> dispatch) ]

                        TextBlock.create
                            [ TextBlock.dock Dock.Left
                              TextBlock.width 170.0
                              TextBlock.foreground "Red"
                              TextBlock.margin (0.0, 5.0, 10.0, 0.0)
                              TextBlock.text (feedback state.FirstNumber) ]

                        TextBlock.create
                            [ TextBlock.dock Dock.Right
                              TextBlock.foreground "Red"
                              TextBlock.width 170.0
                              TextBlock.margin (0.0, 5.0, 0.0, 0.0)
                              TextBlock.text (feedback state.SecondNumber) ] ] ]
              
              if state.IsValid () then
                StackPanel.create [
                  StackPanel.width 350.0
                  StackPanel.horizontalAlignment Layout.HorizontalAlignment.Center
                  StackPanel.children (renderOps state) ] ] ]

// Application Boilerplate
type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Simple Math 9000"
        base.Width <- 360.0
        base.Height <- 150.0

        Program.mkSimple (fun () -> init) update view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"
        this.Styles.Load "resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

let run args = AppBuilder.Configure<App>().UsePlatformDetect().UseSkia().StartWithClassicDesktopLifetime(args)
