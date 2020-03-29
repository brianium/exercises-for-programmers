module SimpleMath.Calc

open SimpleMath.Core

let add (x:float) y = Operation("+",x,y,x + y)

let sub (x:float) y = Operation("-",x,y,x - y)

let mult (x:float) y = Operation("*",x,y,x * y)

let div (x:float) y = Operation("/",x,y,x / y)

let all (x,y) =
    [add x y; sub x y; mult x y; div x y]
