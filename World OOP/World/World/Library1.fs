namespace World

type IWCreatures = 
    abstract member Die: unit -> unit

type IWBadPeople =
    abstract member Arrested: unit -> unit
    abstract member GetOutOfJail: unit -> unit

type IWMurder =
    inherit IWBadPeople
    abstract member HideTheEvidence : IWPeople -> unit

and IWPeople =
    abstract member ChangeHealth: int -> unit 
    abstract member ChangeMood: int -> unit
    abstract member DieBy: IWMurder -> unit
    abstract member ForgetKiller: unit -> unit


type IWDoctor = 
    abstract member Hill : IWPeople -> unit

type WCreature(name:string) =
    let mutable die = false
    interface IWCreatures with
        override x.Die() = die <- true
    member x.IsDie = die
    member x.Eat() = if x.IsDie then printfn "%s" "I die"
                                else printfn "%s" "Om-nom-nom"

type WAnimal(name:string) =
    inherit WCreature(name)
    member x.Play() = if x.IsDie then printfn "%s" "I die"
                                 else printfn "%A" "playing"

type WPeople(name:string) =
    inherit WCreature(name)
    let mutable health = 500
    let mutable mood = 500
    let mutable killing = None
    member x.Name = name
    member x.Health = health
    member x.Mood = mood
    member x.Killing = killing
    interface IWPeople with
        override x.ChangeHealth y = health <- y
        override x.ChangeMood y = mood <- mood + y
        override x.DieBy(men: IWMurder) = killing <- Some(ref men)
                                          (x:>IWCreatures).Die()
                                          health <- 0
        override x.ForgetKiller() = killing <- None
    abstract member GoDoctor: IWDoctor -> unit
    override x.GoDoctor(men: IWDoctor) = men.Hill(x)
        
type WBadPeople(name:string) =
    inherit WPeople(name)
    let mutable InArrest = false
    interface IWBadPeople with
        override x.Arrested() = InArrest <- true
        override x.GetOutOfJail() = InArrest <- false
    member x.IsArrest = InArrest
    member x.Offend(men:WPeople) = (men:>IWPeople).ChangeMood (men.Mood - 20)
    

type WMurder(name) = 
    inherit WBadPeople(name)
    let luck = new System.Random(name.GetHashCode())
    interface IWMurder with
        override x.HideTheEvidence(men:IWPeople) = men.ForgetKiller()
    member x.Kill (men:IWPeople) = if x.IsArrest then printfn "%A" "Arrested"
                                                 else men.DieBy(x)
                                                      (x:>IWPeople).ChangeHealth 0
                                                      if luck.Next(10) > 8 then (x:>IWMurder).HideTheEvidence(men)

type WBrawlers(name) =
    inherit WBadPeople(name)
    member x.Fight(men:WPeople) = (x:>IWPeople).ChangeHealth (x.Health - 50)
                                  (men:>IWPeople).ChangeHealth (men.Health - 70)

type WGoodPeople(name:string) =
    inherit WPeople(name)
    member x.CheerUp(men:WPeople) = (men:>IWPeople).ChangeMood (men.Mood + 20)


type WCop(name:string) =
    inherit WGoodPeople(name)
    member x.Arrest(men:IWBadPeople) = men.Arrested()
    member x.RidOfJail(men:IWBadPeople) = men.GetOutOfJail()
    member x.InvestigateTheMurder(men:WPeople) = match men.Killing with
                                                 | Some (murder) -> x.Arrest(!murder)
                                                 | None -> printfn "%A" "Not die"

type WDoctor(name:string) =
    inherit WGoodPeople(name)
    interface IWDoctor with
        override x.Hill(men : IWPeople) = men.ChangeHealth 500


