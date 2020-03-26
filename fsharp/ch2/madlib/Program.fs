// Learn more about F# at http://fsharp.org

open System
open Madlib.Core
open Madlib.Input

/// Radical stories!!!!

let interactiveStoryTemplate answers word1 word2 =
    match answers |> List.map (fun a -> a.PartOfSpeech.PartOfSpeech)  with
    | [Noun; Noun] -> sprintf "I am a great %s!! I seek the ancient %s!" word1 word2
    | [Noun; Verb] -> sprintf "I am a mighty %s!! I will %s evil where I find it!" word1 word2
    | _ -> sprintf "I am just a %s trying to be %s." word1 word2

let story selections answers =
    match selections with
    | [a;b;c;d] -> sprintf "Do you %s your %s %s %s? That's hilarious!" a b c d
    | [a;b;c;d;e;f] -> sprintf "Do you %s your %s %s %s? I might %s %s! That's hilarious!" a b c d e f
    | [a;b] -> interactiveStoryTemplate answers a b
    | _ -> failwith "Invalid input"

let hilariousStory = [
        Pos Noun 3
        Pos Verb 1
        Pos Adjective 2
        Pos Adverb 4
    ]

let hilariousStoryPlus = hilariousStory @ [
    Pos Verb 5
    Pos Noun 6
]

let interactiveBranch selection = 
    match selection with
    | "wizard" -> { PartOfSpeech = Noun; Order = 2 }
    | "warrior" -> { PartOfSpeech = Verb; Order = 2 }
    | _ -> { PartOfSpeech = Adjective; Order = 2 }

let interactiveStory = [
    Pos Noun 1
    fun answer ->
        match answer with
        | None -> failwith "No previous answer for some reason"
        | Some {Selection = s} -> interactiveBranch s
]

/// Radical program execution!

let render renderer answers =
    answers
    |> List.map (fun {Selection=s} -> s)
    |> renderer
    |> fun r -> r answers

let Exec madlib renderer =
    madlib
    |> List.fold ask []
    |> List.sortBy (fun { PartOfSpeech=p } -> p.Order)
    |> render renderer
    |> printfn "%s"

[<EntryPoint>]
let main argv =
    if argv.Length > 0
    then match argv.[0] with
         | "1" -> Exec hilariousStory story
         | "2" -> Exec hilariousStoryPlus story
         | "3" -> Exec interactiveStory story
         | _ -> Exec hilariousStory story
    else Exec hilariousStory story
    0 // return an integer exit code
