//module Huffman

type CodeTree = 
  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
  | Leaf of char: char * weight: int


// code tree

let uniq_list list =
    let rec list_Contains list element =
        match list with
        | head::tail -> match head with
                        | (a,b) -> if element = a then (a, b+1) :: tail 
                                                  else head :: list_Contains tail element
        | [] -> (element, 1)::[]
    let rec uniq_list' list new_list =
        match list with
        | head::tail -> uniq_list' tail (list_Contains new_list head)
        | [] -> new_list
    uniq_list' list []

let rec ListToLeafList list = 
    match list with
    | head::tail -> match head with
                    | (a,b) -> Leaf (a , b) :: ListToLeafList tail
    | [] -> []

let equalLeaf a b = 
    match a,b with
    |Leaf(a,b), Leaf(c,d) -> if b < d then -1
                             else if b > d then 1
                             else if b = d then 0
                                           else failwith "error"
    |Fork(left1, right1, list1, weigth1), Fork(left2, right2, list2, weigth2) -> if weigth1 < weigth2 then -1
                                                                                 else if weigth1 > weigth2 then 1
                                                                                 else if weigth1 = weigth2 then 0
                                                                                                           else failwith "error"
    |Fork(left1, right1, list1, weigth1), Leaf(list2, weigth2) -> if weigth1 < weigth2 then -1
                                                                  else if weigth1 > weigth2 then 1
                                                                  else if weigth1 = weigth2 then 0
                                                                                            else failwith "error"
    |Leaf(list1, weigth1), Fork(left2, right2, list2, weigth2) -> if weigth1 < weigth2 then -1
                                                                  else if weigth1 > weigth2 then 1
                                                                  else if weigth1 = weigth2 then 0
                                                                                            else failwith "error"
    

let rec TryList list =
    match list with
    | head::tail -> head > 
    | head::[] -> head::[]
    | [] -> []

let rec DoTree list =
    

let rr = ['a';'b';'c';'a';'a';'a';]
let eee= uniq_list rr
let ooo = ListToLeafList eee



//let createCodeTree (chars: char list) : CodeTree = 
    

// decode

type Bit = int

let rec get_simv tree bits = 
    match bits with
    | head::tail -> match tree with
                    | Fork(left, right, list, weigth) -> if head = 0 then get_simv left tail
                                                                     else get_simv right tail
                    | Leaf(char, weight) -> char,bits
    | [] -> match tree with
                    | Fork(left, right, list, weigth) -> failwith("error")
                    | Leaf(char, weight) -> char,bits

let rec decode (tree: CodeTree)  (bits: Bit list) : char list = 
    match bits with
    | head::tail -> match get_simv tree bits with
                    | (a,b) -> [a] @ decode tree b
    | [] -> []

// encode

let rec find (tree: CodeTree) a =
    match tree with
    | Fork(left, right, list, weigth) -> match right with
                                         | Fork(left', right', list', weigth') -> if List.exists (fun x -> x = a) list' then [1] @ (find right a)
                                                                                                                      else [0] @ (find left a)
                                         | Leaf(char', weigth') -> if (char' = a) then [1]
                                                                                  else find left a
    | Leaf(char', weigth') -> if (char' = a) then [0]
                                             else failwith("Error")


let tree = Fork ( Fork (Leaf ('a', 3), Leaf('b', 2), ['a'; 'b'], 5), Fork (Leaf ('c', 2), Leaf('d', 1), ['c'; 'd'], 3) , ['a'; 'b'; 'c'; 'd'], 8)

let x = find tree 'a'

let rec encode (tree: CodeTree)  (text: char list) : Bit list = 
    match text with
    | head::tail -> find tree head @ encode tree tail
    | [] -> []

let p = ['a';'b';'c']
let c = encode tree p
let m = decode tree c