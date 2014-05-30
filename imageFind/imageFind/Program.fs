// Aleksandr Pykhtin
// 2014

module imageFind
open System

let url_list = ["http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/"
                "http://vk.com/durov"]

let rec uniqImageList urlList =

    let imageFind url = 
        let index (url:string) (x:int) = url.IndexOf ("<img src=", x)
        let rec imageFound (url:string) x = 
            let s = index url x
            if s = -1 then []
                    else s::imageFound url (s+1)
        imageFound url 0

    let copy position (url:string) = 
        let start = url.IndexOf('"', position) + 1
        let stop = url.IndexOf('"', start + 1) - 1
        if stop < 0 then url.[start..url.Length - 1]
                    else url.[start..stop]

    let imageList url = Seq.distinct (Seq.map (fun x -> copy x url) (imageFind url))

    Seq.toList (Seq.concat (Seq.filter (fun y -> Seq.length y > 5) (Seq.map (fun x -> imageList x) urlList)))

let getImageList list f=
    let flag = ref false
    let rec wait() = 
        if not !flag then System.Threading.Thread.Sleep(100)
                          wait()

    let x = MapCPS.map list WebR.getUrl (fun x -> f (uniqImageList x)
                                                  flag:= true)
    wait()

getImageList url_list (printfn "%A")