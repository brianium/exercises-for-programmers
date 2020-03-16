[<AutoOpen>]
module Counter.Gui

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.Components.Hosts
open Avalonia
open Avalonia.Controls.ApplicationLifetimes

type State = {
    Input: string
}

let init = {
    Input = ""
}

type Msg =
| Change of string

let update (msg: Msg) (state: State) : State =
    match msg with
    | Change content -> { state with Input = content }

let view (state: State) (dispatch) =
    StackPanel.create [
        StackPanel.children [
            TextBlock.create [
                TextBlock.dock Dock.Top
                TextBlock.fontSize 22.0
                TextBlock.text (sprintf "Characters: %d" state.Input.Length)
            ]
            TextBox.create [
                TextBox.dock Dock.Top
                TextBox.fontSize 22.0
                TextBox.onTextChanged (Change >> dispatch)
            ]
        ]
    ]

// Application Boilerplate
type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Counter 5000"
        base.Height <- 60.0
        base.Width <- 400.0

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

let exec args = 
    AppBuilder
        .Configure<App>()
        .UsePlatformDetect()
        .UseSkia()
        .StartWithClassicDesktopLifetime(args)
