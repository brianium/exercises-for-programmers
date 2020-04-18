namespace Currency.Domain

open System
open F23.StringSimilarity

type Symbol =
    | EUR
    | CAD
    | HKD
    | ISK
    | PHP
    | DKK
    | HUF
    | CZK
    | AUD
    | RON
    | SEK
    | IDR
    | INR
    | BRL
    | RUB
    | HRK
    | JPY
    | THB
    | CHF
    | SGD
    | PLN
    | BGN
    | TRY
    | CNY
    | NOK
    | NZD
    | ZAR
    | USD
    | MXN
    | ILS
    | GBP
    | KRW
    | MYR

module Symbol =
    let shortSymbol symbol =
        match symbol with
        | ISK
        | SEK
        | NOK -> "kr"
        | CAD
        | USD
        | AUD
        | HKD
        | SGD
        | NZD
        | MXN -> sprintf "%c" '\u0024'
        | PHP -> sprintf "%c" '\u20B1'
        | DKK -> "kr."
        | HUF -> "Ft"
        | CZK -> sprintf "K%c" '\u010D'
        | RON -> "lei"
        | EUR -> sprintf "%c" '\u20AC'
        | IDR -> "Rp"
        | INR -> sprintf "%c" '\u20B9'
        | BRL -> USD.ToString() |> sprintf "R%s"
        | RUB -> sprintf "%c" '\u20BD'
        | HRK -> "kn"
        | JPY
        | CNY -> sprintf "%c" '\u00A5'
        | THB -> sprintf "%c" '\u0E3F'
        | PLN -> sprintf "z%c" '\u0142'
        | BGN -> sprintf "%c%c" '\u043B' '\u0432'
        | TRY -> sprintf "%c" '\u20BA'
        | ZAR -> "R"
        | ILS -> sprintf "%c" '\u20AA'
        | GBP -> sprintf "%c" '\u00A3'
        | KRW -> sprintf "%c" '\u20A9'
        | MYR -> "RM"
        | CHF -> "franc"


// render on the right NOK, BGN, PLN, CHF, HRK, RUB, SEK, RON, CZK, HUF, DKK, ISK
module Country =
    let private damerau = new Damerau()

    let countries =
        dict
            [ "Canada", CAD
              "Hong Kong", HKD
              "Iceland", ISK
              "Philippines", PHP
              "Denmark", DKK
              "Hungary", HUF
              "Czech Republic", CZK
              "Australia", AUD
              "Romania", RON
              "Sweden", SEK
              "Indonesia", IDR
              "India", INR
              "Brazil", BRL
              "Russia", RUB
              "Croatia", HRK
              "Japan", JPY
              "Thailand", THB
              "Switzerland", CHF
              "Singapore", SGD
              "Poland", PLN
              "Bulgaria", BGN
              "Turkey", TRY
              "China", CNY
              "Norway", NOK
              "New Zealand", NZD
              "South Africa", ZAR
              "United States of America", USD
              "USA", USD
              "U.S.A", USD
              "Mexico", MXN
              "Israel", ILS
              "United Kingdom", GBP
              "South Korea", KRW
              "Malaysia", MYR
              "Austria", EUR
              "Belgium", EUR
              "Cyprus", EUR
              "Estonia", EUR
              "Finland", EUR
              "France", EUR
              "Germany", EUR
              "Greece", EUR
              "Ireland", EUR
              "Italy", EUR
              "Latvia", EUR
              "Lithuania", EUR
              "Luxembourg", EUR
              "Malta", EUR
              "Netherlands", EUR
              "Portugal", EUR
              "Slovakia", EUR
              "Slovenia", EUR
              "Spain", EUR ]

    let symbolFor country =
        try
            Some(countries.Item country)
        with _ -> None

    let private similarity a b = damerau.Distance(a, b)

    let closest country =
        countries.Keys
        |> Seq.sortBy (fun k -> similarity country k)
        |> Seq.head

type Currency =
    { Symbol: Symbol
      Rate: float }

module Currency =
    let create (symbol, rate) =
        { Symbol = symbol
          Rate = rate }

type Value =
    { Amount: float
      Currency: Currency }
    override x.ToString() =
        match x.Currency.Symbol with
        | ISK
        | DKK
        | HUF
        | CZK
        | RON
        | SEK
        | RUB
        | HRK
        | CHF
        | PLN
        | BGN
        | NOK -> sprintf "%0.2f %s" x.Amount (Symbol.shortSymbol x.Currency.Symbol) // symbol on the right
        | _ -> sprintf "%s%0.2f" (Symbol.shortSymbol x.Currency.Symbol) x.Amount // symbol on the left

module Value =
    let create (amount, symbol, rate) =
        { Amount = amount
          Currency = Currency.create (symbol, rate) }

    let convert { Rate = rateTo; Symbol = sym } { Amount = a; Currency = c } = 
        create (Math.Ceiling((a * c.Rate * rateTo) * 100.0) / 100.0, sym, rateTo)
