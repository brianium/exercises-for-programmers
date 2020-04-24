// Learn more about F# at http://fsharp.org

open System
open PasswordValidator.Domain

[<EntryPoint>]
let main argv =
    ReadLine.ReadPassword "What is your secret? "
    |> Users.lookup
    |> function
       | Some x -> printfn "Welcome %s!" x
       | None -> printfn "I don't know you"
    0 // return an integer exit code
