[<AutoOpen>]
module Madlib.Core

type PartOfSpeech =
    | Noun
    | Verb
    | Adjective
    | Adverb

type OrderedPartOfSpeech =
    { PartOfSpeech: PartOfSpeech
      Order: int }

type Selection = string

type Answer =
    { PartOfSpeech: OrderedPartOfSpeech
      Selection: Selection }

type PartOfSpeechFactory = Option<Answer> -> OrderedPartOfSpeech

type Madlib = List<PartOfSpeechFactory>

type Renderer = List<Selection> -> List<Answer> -> string

let MakeAnswer: OrderedPartOfSpeech -> Selection -> Answer =
    fun pos selection ->
        { PartOfSpeech = pos
          Selection = selection }

let Pos pos order =
    fun _ ->
        { PartOfSpeech = pos
          Order = order }
