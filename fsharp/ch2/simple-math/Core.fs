namespace SimpleMath.Core

open System

type ParseState =
    | Empty
    | Success of float
    | Failure
    | Negative

type Operation = Operation of string * float * float * float
