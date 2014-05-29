// Aleksandr Pykhtin
// 2014

module imageFind
open System

let url_list = ["http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/"
                "http://vk.com/durov"]

let rec ImageList f =

    let imageFind f = 
        let index (f:string) (x:int) = f.IndexOf ("<img src=", x)
        let rec imageFound (f:string) x = 
            let s = index f x
            if s = -1 then []
                    else s::imageFound f (s+1)
        imageFound f 0

    let copy position (f:string) = 
        let start = f.IndexOf('"', position) + 1
        let stop = f.IndexOf('"', start + 1) - 1
        if stop < 0 then f.[start..(string f).Length - 1]
                    else f.[start..stop]

    let rec imageList l f= 
            match l with
            |head::tail -> copy head f::imageList tail f
            |[] ->  []

    let checkNumber f =   
        if (imageFind f).Length > 5 then imageList (imageFind f) f
                                    else []
    match f with
    | head::tail -> checkNumber head :: ImageList tail
    |[] -> []


let getImageList list f=
    let flag = ref false
    let rec wait() = 
        if not !flag then System.Threading.Thread.Sleep(100)
                          wait()

    let x = MapCPS.map list WebR.getUrl (fun x -> f (Seq.distinct (ImageList x))
                                                  flag:= true)
    wait()

getImageList url_list (printfn "%A")