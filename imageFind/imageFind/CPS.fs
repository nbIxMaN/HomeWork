module MapCPS

let rec fact u f =
    if u = 1 then f 1
             else fact (u-1) (fun x -> f(u * x) )

let rec map x fact f =
    match x with
    | [] -> f []
    | head::tail -> fact head (fun o -> map tail fact (fun x -> f(o::x)))

let x = [2 .. 10]
let summ x y = x+y

let z = map x fact (printf "%A")