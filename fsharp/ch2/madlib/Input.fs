[<AutoOpen>]
module Madlib.Input

open Madlib.Core

let private prompt text =
    printf "%s " text
    stdin.ReadLine() |> (fun s -> s.Trim())

let private promptText (pos: OrderedPartOfSpeech) =
    match pos with
    | {PartOfSpeech = Noun} -> "Enter a noun:"
    | {PartOfSpeech = Verb} -> "Enter a verb:"
    | {PartOfSpeech = Adjective} -> "Enter an adjective:"
    | {PartOfSpeech = Adverb} -> "Enter an adverb:"

let ask: List<Answer> -> PartOfSpeechFactory -> List<Answer> =
    fun answers gen ->
        let partOfSpeech = gen (if answers.IsEmpty then None else Some(answers.Head))
        promptText partOfSpeech
        |> prompt
        |> MakeAnswer partOfSpeech 
        |> fun answer -> answer::answers

