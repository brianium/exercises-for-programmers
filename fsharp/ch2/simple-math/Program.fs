// Learn more about F# at http://fsharp.org

open System
open SimpleMath.Gui
open SimpleMath.Cli

[<EntryPoint>]
let main argv =
    match argv with
    | [| "cli" |] -> SimpleMath.Cli.run "Enter the first number:" "Enter the second number:" |> ignore
    | _ -> SimpleMath.Gui.run argv |> ignore
    0 // return an integer exit code
