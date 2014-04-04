open System

let uniq_list list =
    let rec list_Contains list element =
        match list with
        | head::tail -> if element = head then false
                                          else list_Contains tail element
        | [] -> true
    let rec uniq_list' list new_list =
        match list with
        | head::tail -> if list_Contains tail head then uniq_list' tail (head::new_list)
                                                   else uniq_list' tail new_list
        | [] -> new_list
    uniq_list' list []

let get_uniq_image_list url_list func =
    let img_fnd f =
        let index f x = (string f).IndexOf("<img src=", int x)
        let rec img_fnd' f x = 
            if (index f x = -1) then []
                                else index f x::img_fnd' f (index f x + 1)
        img_fnd' f 0

    let copy position f = 
        let start = (string f).IndexOf('"', position) + 1
        let stop = (string f).IndexOf('"', start + 1) - 1
        if stop < 0 then (string f).[start..(string f).Length - 1]
                    else (string f).[start..stop]

    let rec img_list l f= 
        match l with
        |head::tail -> copy head f::img_list tail f
        |[] ->  []

    let image_found f =
        if (img_fnd f).Length > 5 then img_list (img_fnd f) f
                                  else []

    let rec get_uniq_image_list' list func func' =
            match list with
            | head::tail -> func head (fun x -> get_uniq_image_list' tail func (fun y -> func' (uniq_list (image_found x@y))))
            | [] -> func' []
    get_uniq_image_list' url_list WebR.getUrl func

let url_list = ["http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/"]


get_uniq_image_list url_list (printf "%A")

System.Console.ReadLine()