// Learn more about F# at http://fsharp.org

open System

open Area.Cli
open Area.Gui

[<EntryPoint>]
let main argv =
    match argv with
    | [| "cli" |] -> Area.Cli.run () |> ignore
    | _ -> App.Run argv |> ignore
    0 // return an integer exit code
