namespace Checkout.Domain

type LineItem =
    { Price: float
      Quantity: int }

type Order =
    { LineItems: List<LineItem>
      SubTotal: float
      TaxRate: float }

module LineItem =
    let create price quantity =
        { Price = price
          Quantity = quantity }

    let total init { Quantity = q; Price = p } = p * float q + init

module Order =
    let create taxRate =
        { LineItems = []
          SubTotal = 0.0
          TaxRate = taxRate }

    let addItem order lineItem = { order with LineItems = order.LineItems @ [ lineItem ] }

    let total { SubTotal = s; TaxRate = t } =
        s * t + s

    let checkout order =
        { order with SubTotal = List.fold LineItem.total 0.0 order.LineItems }
