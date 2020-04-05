namespace Pizza

module Party =

    type Pizza =
        { NumSlices: int }

    type Party =
        { People: int
          Pizzas: seq<Pizza> }

    let create (people, pizzas, slicesPerPizza) =
        { People = people
          Pizzas = Seq.init pizzas (fun _ -> { NumSlices = slicesPerPizza }) }

module Order =

    open Party

    type Order =
        { People: int
          SlicesPerPerson: int
          SlicesPerPizza: int }

    let create (people, slicesPerPerson, slicesPerPizza) =
            { People = people
              SlicesPerPerson = slicesPerPerson
              SlicesPerPizza = slicesPerPizza }

    let party { People = p; SlicesPerPerson = sPe; SlicesPerPizza = sPi } =
        let pizzas = (double sPi)
                     |> (/) (double (p * sPe))
                     |> round
                     |> int
        Party.create (p, pizzas ,sPi)

    type Ration =
        { SlicesPerPerson: int
          LeftOver: int }

    type Ration with
        static member Create people totalSlices = 
            { SlicesPerPerson = totalSlices / people; LeftOver = totalSlices % people }

    let ration { People = p; Pizzas = pi } =
            pi
            |> Seq.fold (fun acc { NumSlices = n } -> acc + n) 0
            |> Ration.Create p
