module Huffman

type CodeTree = 
  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
  | Leaf of char: char * weight: int


// code tree

let UniqList list =
    let rec list_Contains list element =
        match list with
        | head::tail -> match head with
                        | (a,b) -> if element = a then (a, b+1) :: tail 
                                                  else head :: list_Contains tail element
        | [] -> (element, 1)::[]
    let rec UniqList' list new_list =
        match list with
        | head::tail -> UniqList' tail (list_Contains new_list head)
        | [] -> new_list
    UniqList' list []
    
let GetValue list =
    match list with
    |Leaf(char, _) -> [char]
    |Fork(_,_,char,_) -> char

let GetWeigth list =
    match list with
    |Leaf(_, weigth) -> weigth
    |Fork(_,_,_,weigth) -> weigth

let rec PasteSort element list = 
    match list with
    | head::tail -> if GetWeigth element <= GetWeigth head then element :: list
                                                             else head :: PasteSort element tail
    | [] -> element::[]

let rec ListToLeafList list = 
    match list with
    | head::tail -> match head with
                    | (a,b) -> PasteSort (Leaf (a , b)) (ListToLeafList tail)
    | [] -> []

let Collect a b = Fork(a, b, GetValue a @ GetValue b, GetWeigth a + GetWeigth b)

let rec TreeBilder list = 
    match list with
    | a::b::tail -> TreeBilder (PasteSort (Collect a b) tail)
    | head::[] -> head
    | [] -> failwith "incorrect data"  

let createCodeTree (chars: char list) : CodeTree = 
    TreeBilder (ListToLeafList (UniqList chars))
    // UniqList chars |> ListToLeafList |> tree_builder

// decode

type Bit = int

let rec GetChar tree bits = 
    match bits with
    | head::tail -> match tree with
                    | Fork(left, right, list, weigth) -> if head = 0 then GetChar left tail
                                                                     else GetChar right tail
                    | Leaf(char, weight) -> char,bits
    | [] -> match tree with
                    | Fork(left, right, list, weigth) -> failwith("error")
                    | Leaf(char, weight) -> char,bits

let rec decode (tree: CodeTree)  (bits: Bit list) : char list = 
    match bits with
    | head::tail -> match GetChar tree bits with
                    | (a,b) -> [a] @ decode tree b
    | [] -> []

// encode

let rec find (tree: CodeTree) a =
    match tree with
    | Fork(left, right, list, weigth) -> if List.exists (fun x -> x = a) (GetValue(left)) then [0] @ (find left a)
                                                                                           else [1] @ (find right a)
    | Leaf(char', weigth') -> if (char' = a) then []
                                             else failwith("Error")

let rec encode (tree: CodeTree)  (text: char list) : Bit list = 
    match text with
    | head::tail -> find tree head @ encode tree tail
    | [] -> []

//let p = ['a';'b';'c';'c';'d'; 'e';'r'; 'a']
//let z =ListToLeafList (UniqList p)
//let tree = createCodeTree(p)
//let c = encode tree p
//let m = decode tree c