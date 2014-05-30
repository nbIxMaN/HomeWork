// Aleksandr Pykhtin
// 2014

module Collection

open System.Collections
open System

type Tree<'key, 'value> =
    | Empty
    | Node of Tree<'key, 'value> * 'key * 'value * int * Tree<'key, 'value>

let getLeftBranch = function
    | Node(left, _, _, _, _) -> left
    | Empty -> failwith "Left branch not found"

let getKey = function
    | Node(_, key, _, _, _) -> key
    | Empty -> failwith "Wrong Node"

let getValue = function
    | Node(_, _, value, _, _) -> value
    | Empty -> failwith "Wrong Node"

let getLength = function
    | Node(_, _, _, length, _) -> length
    | Empty -> 0

let getRightBranch = function
    | Node(_, _, _, _, right) -> right
    | Empty -> failwith "Rigth branch not found"

let rec find t key =
    match t with
    | Node(left', key', _, _, rigth') -> 
        if key < key' then find left' key
        else if key > key' then find rigth' key
        else t
    | Empty -> Empty

let BuildNode leftNode key value rightNode = 
    Node(leftNode, key, value, max (getLength leftNode) (getLength rightNode) + 1, rightNode)

let rec GetLength acc tree = 
    match tree with
    | Node(left, _, _, _, rigth) -> GetLength (GetLength (acc+1) left) rigth
    | Empty -> acc

let rec GetLookLike t key =
        match t with
        | Node(left', key', _, _, right') -> 
           if key < key then 
            match left' with
            | Empty -> t
            | _ -> GetLookLike left' key
           else
            match right' with
            | Empty -> t
            | _ -> GetLookLike right' key
        | Empty -> Empty

let Balance t =

        let SmallLeft t =
            match t with
            | Node(left, key, value, length, right) ->
                match right with
                | Node(left', key', value', length', right')->
                    let a = BuildNode left key value left'
                    BuildNode a key' value' right'
                | Empty -> Empty
            | Empty -> Empty

        let BigLeft t =
            match t with
            | Node(left, key, value, length, right) ->
                match right with
                | Node(left', key', value', length', right')->
                    match left' with
                    | Node(left'', key'', value'', length'', right'')->
                        let a = BuildNode left key value left''
                        let b = BuildNode right'' key' value' right'
                        BuildNode a key'' value'' b
                    | Empty -> Empty
                | Empty -> Empty
            | Empty -> Empty

        let SmallRight t =
            match t with
            | Node(left, key, value, length, right)->
                match left with
                | Node(left', key', value', length', right')->
                    let a = BuildNode right' key value right
                    BuildNode left' key' value' a
                | Empty -> Empty
            | Empty -> Empty

        let BigRight t =
            match t with
            | Node(left, key, value, lengthR, right) ->
                match left with
                | Node(left', key', value', length, right')->
                    match right' with
                    | Node(left'', key'', value'', length'', right'')->
                        let a = BuildNode right'' key value right
                        let b = BuildNode left' key' value' left''
                        BuildNode a key'' value'' b
                    | Empty -> Empty
                | Empty -> Empty
            | Empty -> Empty

        match t with
        | Node(left, key, value, length, right) ->
            if getLength right - getLength left = 2 then
                match right with
                | Node(left', key', value', length', right') ->
                    if getLength left' <= getLength right' then SmallLeft t
                                                           else BigLeft t
                | Empty -> Empty
            else if getLength left - getLength right = 2 then
                match left with
                |Node(left', key', value', length', right') ->
                    if getLength right' <= getLength left' then SmallRight t
                                                           else BigRight t
                | Empty -> Empty
            else t
        | Empty -> Empty

let rec add t key value = 
            match t with
            | Node(left', key', value', length, right') -> 
                if key < key' then 
                    Balance (BuildNode (add left' key value) key' value' right')
                else if key > key' then 
                    Balance (BuildNode left' key' value' (add right' key value))
                else 
                    BuildNode left' key value right'
            | Empty -> Node(Empty, key, value, 0, Empty)

let rec delete t key =
            match t with
            | Node(left', key', value', lengthRightBranch, right') ->
                if key < key' then
                    Balance (BuildNode (delete left' key) key' value' right')
                else if key > key' then
                    Balance (BuildNode left' key' value' (delete right' key))
                else 
                    let a = GetLookLike right' key
                    match a with
                    | Empty -> 
                        let a = GetLookLike left' key
                        match a with
                        | Empty -> Empty
                        | _ -> Balance (BuildNode (delete left' (getKey a)) (getKey a) (getValue a) right')
                    | _ -> Balance (BuildNode left' (getKey a) (getValue a) (delete right' (getKey a)))           
            | Empty -> Empty

let rec GetList tree =
    match tree with
    | Node(left, key, value, _, rigth) -> (GetList left) @ [key,value] @ (GetList rigth)
    | Empty -> []

type Map<'key, 'value when 'key: comparison and 'value: equality> =
    val tree : Tree<'key, 'value>
    private new (tree') = {tree = tree'}
    new (seq') = 
        {tree = 
            if Seq.isEmpty seq' then Empty
                                else Seq.fold (fun tree' (key, value) -> add tree' key value) Empty seq' 
        }

    interface IEnumerable with
        override this.GetEnumerator() = 
            let rec GetList tree =
                match tree with
                | Node(left, key, value, _, rigth) -> (GetList left) @ [key,value] @ (GetList rigth)
                | Empty -> []
            let TrueList = GetList this.tree
            let NodeList = ref TrueList
            let IsInit = ref false
            let AlreadyFinished() = raise (new InvalidOperationException("Already Finished"))
            let NotStarted() = raise (new InvalidOperationException("Not Started"))
            let moveNext() =
                if !IsInit then NodeList := (!NodeList).Tail
                           else IsInit:=true
                not (!NodeList).IsEmpty
            let current() = 
                if !IsInit then
                    if (!NodeList).IsEmpty then AlreadyFinished()
                    else box (!NodeList).Head
                else 
                    NotStarted()
            {new IEnumerator with
                 override this.Current = current()
                 override this.MoveNext() = moveNext()
                 override this.Reset() = IsInit := false
                                         NodeList := TrueList}

    member this.Add(key, value) =
        new Map<_,_>(Balance(add this.tree key value))

    member this.ContainsKey(key) =
        match find this.tree key with
        | Empty -> false
        | _ -> true

    member this.Count = GetLength 0 this.tree

    member this.IsEmpty = 
        match this.tree with
        | Empty -> true
        | _ -> false

    member this.Item (key) =
        match find this.tree key with
        | Empty -> 
            raise (new System.Collections.Generic.KeyNotFoundException("Key Not Found"))
        | Node(_, _, value, _, _) -> value

    member this.Remove (key) =
        new Map<_,_>(Balance(delete this.tree key))

    member this.TryFind (key) =
        match find this.tree key with
        | Empty -> None
        | Node(_,_,_,_,_) -> Some(Node)

    override this.ToString() =
        let rec tostring tree =
            match tree with
            | Node(left, key, value, _, rigth) -> 
                (tostring left) + key.ToString() + " " + value.ToString() + " " + (tostring rigth)
            | Empty -> ""
        tostring this.tree

    override this.Equals(x: obj) =                        
          match x with
          | :? Map<'key, 'value> as y ->
              let xList = GetList (y.tree)
              let thisList = GetList (this.tree) 
              xList.Length = thisList.Length && Seq.forall2(=) thisList xList
          | _ -> false

    override this.GetHashCode() =
        let thisList = GetList (this.tree)
        let rec hashCode list acc = 
            match list with
            | (key, value)::tail -> 
                ((acc + (key.GetHashCode() - value.GetHashCode()) % 14863923) % 91823628 + hashCode tail acc) % 1379879782
            | [] -> acc
        hashCode thisList 0

let x = new Map<_,_> ([(5, "a"); (4, "5"); (8, "dsfsdfjoidsfjsdflkjsdflk")])
printfn "%A" x.Count
let y = x.Add(1,"RERER")
let z = y.Remove 3
printfn "%A" y.Count
printfn "%A" (y.Item(8).ToString())
printfn "%A" (x.ToString())
printfn "%A" (y.ToString())
printfn "%A" (z.ToString())
let n = (y:>IEnumerable).GetEnumerator()
n.MoveNext() |> ignore
n.MoveNext() |> ignore
n.MoveNext() |> ignore
n.MoveNext() |> ignore
n.Reset()
n.MoveNext() |> ignore
if x.Equals z then printfn "%A" "true"
              else printfn "%A" "false"               
printfn "%A" n.Current
printfn "%A" (y.GetHashCode())
printfn "%A" (x.GetHashCode())
printfn "%A" (z.GetHashCode())

for i in y do printfn "%A" i