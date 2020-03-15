[<AutoOpen>]
module Counter.Cli

let prompt question =
    printf "%s " question
    stdin.ReadLine() |> (fun s -> s.Trim())

let rec run question input =
    if input <> "" then printfn "%s has %d characters" input input.Length
    else (sprintf "Input cannot be empty. %s" question) |> prompt |> run question

let exec question = prompt question |> (run question)
