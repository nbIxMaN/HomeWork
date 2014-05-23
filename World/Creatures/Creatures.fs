namespace Creatures

type Creature(name:string) =
    let mutable dead = false
    abstract member Die: unit -> unit
    default x.Die() = dead <- true
    member x.IsDie = dead
    member x.Eat() = not x.IsDie

type Animal(name:string) =
    inherit Creature(name)
    member x.Play() = not x.IsDie

type People(name:string) =
    inherit Creature(name)
    let mutable health = 500
    let mutable mood = 500
    let mutable killing = None
    member x.Name = name
    member x.Health = health
    member x.Mood = mood
    member x.Killing = killing
    override x.Die() = x.Die()
                       health <- 0
    member x.ChangeHealth y = health <- health + y
    member x.ChangeMood y = mood <- mood + y
    member internal x.DieBy(men:People) = 
        killing <- Some(ref men)
        x.Die()
        health <- 0
    member internal x.ForgetKiller() = killing <- None
        
type BadPeople(name:string) =
    inherit People(name)
    let mutable InArrest = false
    member internal x.Arrested() = InArrest <- true
    member internal  x.GetOutOfJail() = InArrest <- false
    member x.IsArrest = InArrest
    member x.Offend(men:People) = men.ChangeMood (men.Mood - 20)
    

type Murder(name) = 
    inherit BadPeople(name)
    let luck = new System.Random(name.GetHashCode())
    member private x.HideTheEvidence(men:People) = men.ForgetKiller()
    member x.Kill (men:People) = 
        if x.IsArrest then printfn "%A" "Arrested"
        else men.DieBy(x)
             men.ChangeHealth 0
             if luck.Next(10) > 8 then 
                x.HideTheEvidence(men)

type Brawlers(name) =
    inherit BadPeople(name)
    member x.Fight(men:People) = 
        x.ChangeHealth (x.Health - 50)
        men.ChangeHealth (men.Health - 70)

type GoodPeople(name:string) =
    inherit People(name)
    member x.CheerUp(men:People) = men.ChangeMood (men.Mood + 20)


type Cop(name:string) =
    inherit GoodPeople(name)
    member x.Arrest(men:BadPeople) = men.Arrested()
    member x.RidOfJail(men:BadPeople) = men.GetOutOfJail()
    member x.InvestigateTheMurder(men:People) = 
        match men.Killing with
        | Some (l) -> 
            match !l with
            | :? BadPeople as y -> x.Arrest y
            | _ -> ()
        | None -> printfn "%A" "Not die"

type Doctor(name:string) =
    inherit GoodPeople(name)
    member x.Cure (men:People) = men.ChangeHealth 100


