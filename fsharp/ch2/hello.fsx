let prompt question =
    printf "%s " question
    stdin.ReadLine() |> (fun s -> s.Trim())

let formatMessage input =
    match input with
    | "Brian" -> "Greetings benevolent creator!"
    | "Jennie" -> "Hello Jennie! You are super rad!"
    | _ -> sprintf "Hello, %s, nice to meet you!" input

let outputMessage msg = printfn "%s" msg

let main =
    prompt "What is your name?"
    |> formatMessage
    |> outputMessage

main
