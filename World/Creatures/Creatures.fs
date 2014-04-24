namespace Creatures

type IСCreatures = 
    abstract member Die: unit -> unit

type IСBadPeople =
    abstract member Arrested: unit -> unit
    abstract member GetOutOfJail: unit -> unit

type IСMurder =
    inherit IСBadPeople
    abstract member HideTheEvidence : IСPeople -> unit

and IСPeople =
    abstract member ChangeHealth: int -> unit 
    abstract member ChangeMood: int -> unit
    abstract member DieBy: IСMurder -> unit
    abstract member ForgetKiller: unit -> unit


type IСDoctor = 
    abstract member Cure : IСPeople -> unit

type СCreature(name:string) =
    let mutable die = false
    interface IСCreatures with
        override x.Die() = die <- true
    member x.IsDie = die
    member x.Eat() = if x.IsDie then printfn "%s" "I die"
                                else printfn "%s" "Om-nom-nom"

type СAnimal(name:string) =
    inherit СCreature(name)
    member x.Play() = if x.IsDie then printfn "%s" "I die"
                                 else printfn "%A" "playing"

type СPeople(name:string) =
    inherit СCreature(name)
    let mutable health = 500
    let mutable mood = 500
    let mutable killing = None
    member x.Name = name
    member x.Health = health
    member x.Mood = mood
    member x.Killing = killing
    interface IСPeople with
        override x.ChangeHealth y = health <- y
        override x.ChangeMood y = mood <- mood + y
        override x.DieBy(men: IСMurder) = killing <- Some(ref men)
                                          (x:>IСCreatures).Die()
                                          health <- 0
        override x.ForgetKiller() = killing <- None
    abstract member GoDoctor: IСDoctor -> unit
    override x.GoDoctor(men: IСDoctor) = men.Cure(x)
        
type СBadPeople(name:string) =
    inherit СPeople(name)
    let mutable InArrest = false
    interface IСBadPeople with
        override x.Arrested() = InArrest <- true
        override x.GetOutOfJail() = InArrest <- false
    member x.IsArrest = InArrest
    member x.Offend(men:СPeople) = (men:>IСPeople).ChangeMood (men.Mood - 20)
    

type СMurder(name) = 
    inherit СBadPeople(name)
    let luck = new System.Random(name.GetHashCode())
    interface IСMurder with
        override x.HideTheEvidence(men:IСPeople) = men.ForgetKiller()
    member x.Kill (men:IСPeople) = if x.IsArrest then printfn "%A" "Arrested"
                                                 else men.DieBy(x)
                                                      (x:>IСPeople).ChangeHealth 0
                                                      if luck.Next(10) > 8 then (x:>IСMurder).HideTheEvidence(men)

type СBrawlers(name) =
    inherit СBadPeople(name)
    member x.Fight(men:СPeople) = (x:>IСPeople).ChangeHealth (x.Health - 50)
                                  (men:>IСPeople).ChangeHealth (men.Health - 70)

type СGoodPeople(name:string) =
    inherit СPeople(name)
    member x.CheerUp(men:СPeople) = (men:>IСPeople).ChangeMood (men.Mood + 20)


type СCop(name:string) =
    inherit СGoodPeople(name)
    member x.Arrest(men:IСBadPeople) = men.Arrested()
    member x.RidOfJail(men:IСBadPeople) = men.GetOutOfJail()
    member x.InvestigateTheMurder(men:СPeople) = match men.Killing with
                                                 | Some (murder) -> x.Arrest(!murder)
                                                 | None -> printfn "%A" "Not die"

type СDoctor(name:string) =
    inherit СGoodPeople(name)
    interface IСDoctor with
        override x.Cure(men : IСPeople) = men.ChangeHealth 500


