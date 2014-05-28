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
    member private x.Kill() = (x:>Creature).Die
    override x.Die() = x.Kill() |> ignore
                       health <- 0
    member x.ChangeHealth y = if x.IsDie then false
                              else health <- health + y
                                   if x.Health < 50 then health <- 50
                                   true
    member x.ChangeMood y = if x.IsDie then false
                            else mood <- mood + y
                                 true
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
    member x.Offend(men:People) = 
        if x.IsDie || men.IsDie then false
        else men.ChangeMood (men.Mood - 20)

type Murder(name) = 
    inherit BadPeople(name)
    let luck = new System.Random(name.GetHashCode())
    member private x.HideTheEvidence(men:People) = men.ForgetKiller()
    member x.Kill (men:People) = 
        if x.IsArrest || men.IsDie || x.IsDie then false
        else men.DieBy(x)
             if luck.Next(10) > 8 then 
                x.HideTheEvidence(men)
             true

type Brawlers(name) =
    inherit BadPeople(name)
    member x.Fight(men:People) = 
        if x.IsDie then false
        else x.ChangeHealth -50 |> ignore
             men.ChangeHealth -70

type GoodPeople(name:string) =
    inherit People(name)
    member x.CheerUp(men:People) = 
        if x.IsDie || men.IsDie then false
        else men.ChangeMood (men.Mood + 20)


type Cop(name:string) =
    inherit GoodPeople(name)
    member x.Arrest(men:BadPeople) = 
        if x.IsDie || men.IsDie then false
        else men.Arrested()
             true
    member x.RidOfJail(men:BadPeople) = 
        if x.IsDie || men.IsDie then false
        else men.GetOutOfJail()
             true
    member x.InvestigateTheMurder(men:People) = 
        if x.IsDie then false
        else 
            match men.Killing with
            | Some (l) -> 
                match !l with
                | :? BadPeople as y -> x.Arrest y
                | _ -> false
            | None -> false

type Doctor(name:string) =
    inherit GoodPeople(name)
    member x.Cure (men:People) = 
        if x.IsDie || men.IsDie then false
        else men.ChangeHealth 100