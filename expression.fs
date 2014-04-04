// to simplify expressions
// Pychtin Alex 2013

type Expr =
    | Const of int
    | Var of string
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr

let rec arithm expr = 
    match expr with
    | Const x -> Const x
    | Var x -> expr
    | Add (a, b) -> match arithm(a), arithm(b) with
                    | Var x, Var y -> expr
                    | Var x, Const y -> if y = 0 then Var x
                                                 else expr
                    | Const x, Var y -> if x = 0 then Var y
                                                 else expr
                    | Const x , Const y -> Const (x + y)
                    | _ -> expr
    | Sub (a, b) -> match arithm(a), arithm(b) with
                    | Var x, Var y -> expr
                    | Var x, Const y -> if y = 0 then Var x
                                                 else expr
                    | Const x, Var y -> if x = 0 then Var y
                                                 else expr
                    | Const x , Const y -> Const (x - y)
                    | _ -> expr
    | Mul (a, b) -> match arithm(a), arithm(b) with
                    | Var x, Var y -> expr
                    | Var x, Const y -> if y = 0 then Var x
                                                 else expr
                    | Const x, Var y -> if x = 0 then Var y
                                                 else expr
                    | Const x , Const y -> Const (x * y)
                    | _ -> expr
    | Div (a, b) -> match arithm(a), arithm(b) with
                    | Var x, Var y -> expr
                    | Var x, Const y -> if y = 0 then Var x
                                                 else expr
                    | Const x, Var y -> if x = 0 then Var y
                                                 else expr
                    | Const x , Const y -> Const (x / y)
                    | _ -> expr

arithm (Sub (Const(2), Const (13)))

let x = (Add (Sub (Const 2, Const  2), Add (Var "x", Const 0)))

let y = (Add (Add (Const 1, Const 2), Add (Var "a", Const 0)))

let z = (Add (Add (Const 1, Const 2), Mul (Const 0, Const 0)))

arithm z
