// Learn more about F# at http://fsharp.org
open Counter.Cli
open Counter.Gui

[<EntryPoint>]
let main argv =
    match argv with
    | [| var |] -> match var with
                   | "gui" -> Counter.Gui.exec argv |> ignore
                   | _ -> Counter.Cli.exec "What is the input string?"
    | _ -> Counter.Cli.exec "What is the input string?"
    
    0 // return an integer exit code
