// Aleksandr Pykhtin
// 2014

module imageFind
open System

let url_list = ["http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/";
                "http://www.goodfon.ru/catalog/landscapes/"
                "http://vk.com/durov"]

let rec uniqImageList urlList =

    let getImageList html =
        let copy position (html:string)  = 
            let start = html.IndexOf('"', position) + 1
            let stop = html.IndexOf('"', start + 1) - 1
            if stop < 0 then html.[start..html.Length - 1]
                        else html.[start..stop]

        let imageFind html = 
            let index (html:string) (x:int) = html.IndexOf ("<img src=", x)
            let rec imageFound (html:string) x = 
                let s = index html x
                if s = -1 then []
                          else copy s html::imageFound html (s+1)
            imageFound html 0
        imageFind html

        

    let getUniqImageList = getImageList >> Seq.distinct

    urlList
    |> Seq.map getUniqImageList
    |> Seq.filter (fun y -> Seq.length y > 5)
    |> Seq.concat
    |> Seq.toList

let getImageList list f=
    let flag = ref false

    let x = MapCPS.map list WebR.getUrl (fun x -> f (uniqImageList x)
                                                  flag:= true)

    while not !flag do
        System.Threading.Thread.Sleep(100)

getImageList url_list (printfn "%A")