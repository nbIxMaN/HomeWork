namespace Disease

type Disease =
    abstract member Multiply: unit -> unit
    abstract member Fall: unit -> unit

type IDPeople = 
    abstract member BeingACarrier: unit -> unit 
    abstract member Recedive: unit -> unit
    abstract member CatchInfection: Infection -> unit
    abstract member CatchParasite: Parasite -> unit
    abstract member HealthLow: int -> unit
    abstract member ThrowOffAnIllness: unit -> unit

and Infection(name:string) = 
    let mutable strange = 3
    let mutable stability = 2
    member x.Strange = strange
    member x.Stability = stability
    interface Disease with
        override x.Multiply () = strange <- strange + 1
        override x.Fall() = strange <- strange - 1
    member x.Infect(char:IDPeople) = char.CatchInfection(x)
    member x.Mutate () = strange <- 7
                         stability <- 7

and Parasite(name:string) =
    let mutable strange = 3
    let mutable stability = 2
    member x.Strange = strange
    member x.Stability = stability
    interface Disease with
        override x.Multiply () = strange <- strange + 1
        override x.Fall() = strange <- strange - 1
    member x.Infect(char:IDPeople) = char.CatchParasite(x)
    member x.Lower_immunity (char:IDPeople) = char.HealthLow 5
    member x.Hide (char:IDPeople) = char.BeingACarrier()
    member x.Reveal(char:IDPeople) = char.Recedive()