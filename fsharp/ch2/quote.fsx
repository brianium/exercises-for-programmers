let prompt question =
    printf "%s " question
    stdin.ReadLine() |> (fun s -> s.Trim())

[<Struct>]
type Quote =
    { Author: string
      Text: string }

let output (quotes: List<Quote>) = quotes |> List.iter (fun q -> printfn "%s says, \"%s\"" q.Author q.Text)

let cli () =
    { Text = prompt "What is the quote?"
      Author = prompt "Who said it?" }
    |> List.singleton
    |> output

let multipleQuotes (quotes: List<Quote>) = output quotes

let main isCli =
    if isCli then cli ()
    else
        multipleQuotes
            [ { Author = "Ashton Kutcher"
                Text = "Dude! Where's my car?" }
              { Author = "Obi-Wan Kenobi"
                Text = "These aren't the droids you're looking for." } ]

main true
