module Huffman

type CodeTree = 
  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
  | Leaf of char: char * weight: int


// code tree

let createCodeTree (chars: char list) : CodeTree = 
    failwith "Not implemented"

// decode

type Bit = int

let rec get_simv tree bits = 
    match bits with
    | head::tail -> match tree with
                    | Fork(left, right, list, weigth) -> if head = 0 then get_simv left tail
                                                                     else get_simv right tail
                    | Leaf(char, weight) -> char,tail
    | [] -> failwith "error"

let rec decode (tree: CodeTree)  (bits: Bit list) : char list = 
    match bits with
    | head::tail -> get_simv tree list
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
