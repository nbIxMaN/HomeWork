module MapCPS

let rec map x fact f =
    match x with
    | [] -> f []
    | head::tail -> fact head (fun o -> map tail fact (fun x -> f(o::x)))
