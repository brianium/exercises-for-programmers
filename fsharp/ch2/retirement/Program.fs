open System

open Retirement.Core
open Retirement.Input

let read text =
    printf "%s " text
    stdin.ReadLine()
    |> (fun s -> s.Trim())

let rec ask text result =
    match result with
    | Success(d) -> d
    | Failure -> read (sprintf "Value must be a 32 bit integer.\n%s" text) |> parse |> ask text
    | Negative -> read (sprintf "Value must not be negative.\n%s" text) |> parse |> ask text
    | Empty -> read text |> parse |> ask text

let displayRetirementDate (user: User) currentYear =
    match user.YearsRemaining <= 0 with
    | true -> "right now"
    | _ -> sprintf "in %d" (currentYear + user.YearsRemaining)

let display (user: User) =
    let currentYear = DateTime.Now.Year
    printfn "You have %d year(s) until you can retire." user.YearsRemaining
    printfn "It's %d, so you can retire %s" currentYear (displayRetirementDate user currentYear)

[<EntryPoint>]
let main argv =
    (ask "What is your current age?" Empty, ask "At what age would you like to retire?" Empty)
    |> User.New
    |> display
    |> ignore
    0 // return an integer exit code
