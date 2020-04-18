namespace Currency.Cli

module Http =
    open Hopac
    open HttpFs.Client
    open FSharp.Data
    open FSharp.Data.JsonExtensions
    open Currency.Domain

    let private createCurrency symbol response =
        match response?rates.TryGetProperty(symbol.ToString()) with
        | Some r -> Currency.create (symbol, r.AsFloat()) |> Some
        | None -> None

    #if INTERACTIVE
    type Rate = JsonProvider<"../../../Currency.Cli/data/rates.json">
    #else
    type Rate = JsonProvider<"data/rates.json", EmbeddedResource="Currency.Cli, rates.json">
    #endif

    let fetch (from: Symbol) (target: Symbol) =
        Request.createUrl Get "https://api.exchangeratesapi.io/latest"
        |> Request.queryStringItem "symbols" (target.ToString())
        |> Request.queryStringItem "base" (from.ToString())
        |> Request.setHeader (UserAgent "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36")
        |> Request.responseAsString
        |> run
        |> JsonValue.Parse
        |> createCurrency target

    let (->>) (from: Symbol) (target: Symbol) =
        fetch from target
