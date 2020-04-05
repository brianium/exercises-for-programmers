namespace Pizza

module Display =
    open Party
    open Order

    let party {People = p; Pizzas = pi} =
        let pizzas = Seq.length pi
        let pLabel = if p = 1 then "person" else "people"
        let piLabel = if pizzas = 1 then "pizza" else "pizzas"
        sprintf "%d %s with %d %s" p pLabel pizzas piLabel

    let ration {SlicesPerPerson = s; LeftOver = l} =
        let pLabel = if s = 1 then "piece" else "pieces"
        let lLabel = if l = 1 then "piece" else "pieces"
        let isAre = if l = 1 then "is" else "are"
        sprintf "Each person gets %d %s of pizza\nThere %s %d leftover %s" s pLabel isAre l lLabel
