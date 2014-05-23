open Creatures
open Disease

type VillagePeople(name:string) =
    inherit People(name)
    let mutable infection = None
    let mutable parasite = None
    let mutable hideparasite = None
    member private x.BeingACarrier() = 
        hideparasite <- parasite
        parasite <- None
    member private x.Recedive() = 
        parasite <- hideparasite
        hideparasite <- None
        match parasite with
            | Some(illness:Parasite) -> 
                if illness.Strange*10 > x.Health then x.Die()
            | None -> ()
    member x.CatchInfection (illness:Infection) = 
        infection <- Some(illness)
        if illness.Strange*10 > x.Health then x.Die()
    member x.CatchParasite (illness:Parasite) = 
        parasite <- Some(illness)
        if illness.Strange*10 > x.Health then x.Die()
        if illness.Strange*10 > x.Health/5 then x.BeingACarrier()
    member private x.HealthLow y = x.ChangeHealth -y
    member private x.ThrowOffAnIllness() = 
        match parasite with
        | Some(illness) -> 
            if illness.Stability > 5 then (illness:>Disease).Fall()
            else parasite <- None
        | None -> ()
        match infection with
        | Some(illness) -> 
            if illness.Stability > 5 then (illness:>Disease).Fall()
            else infection <- None
        | None -> ()
    member x.Infected = infection <> None || parasite <> None
    member x.TalkWith (men:VillagePeople) = 
        if (men.Health < 100) && (x.Infected) 
            then printfn "%A" "not communicate with patients"
                 match infection with
                 | Some(illness) -> men.CatchInfection(illness)
                                    (illness:>Disease).Multiply()
                 | None -> ()
            else printfn "%A" "Hello comrade"
    member x.Hug (men:VillagePeople) = 
        if (men.Health < 100) && ((x.Infected) || (hideparasite <> None)) 
            then printfn "%A" "Do not hug with patients"
                 match infection with
                 | Some(illness) -> men.CatchInfection(illness)
                                    illness.Multiply()
                 | None -> ()
                 match parasite with
                 | Some(illness) -> men.CatchParasite(illness)
                                    illness.Multiply()
                 | None -> ()
                 match hideparasite with
                 | Some(illness) -> men.CatchParasite(illness)
                 | None -> ()
            else printfn "%A" "Сuddle"
                                                                                                                                                 
    member x.GoDoctor(men:Doctor) = men.Cure(x)
                                    x.ThrowOffAnIllness()                                  


let bacterium = new Creature("Lely")
let cat = new Animal("cat")
let Bob = new VillagePeople("Bob")
let Semen = new VillagePeople("Semen")
let Lesha = new VillagePeople("Lesha")
let Taylor = new People("Taylor")
let Olivia = new GoodPeople("Olivia")
let Max = new Doctor("Max")
let Samantha = new Cop("Samantha")
let Jacob = new BadPeople("Jacob")
let Justin = new Murder("Justin")
let Mia = new Brawlers("Mia")
let Suzi = new Infection("Suzi")
let Paul = new Parasite("Paul")
Bob.CatchInfection Suzi
Lesha.CatchParasite Paul
Suzi.Mutate()
bacterium.Eat()
Mia.Fight(Semen)
Mia.Fight(Semen)
Mia.Fight(Semen)
Mia.Fight(Semen)
Mia.Fight(Semen)
Mia.Fight(Semen)
Bob.TalkWith(Semen)
Lesha.Hug(Semen)
Semen.GoDoctor(Max)
Mia.Fight(Samantha)
Samantha.Arrest(Mia)
cat.Play()
Jacob.Offend(Semen)
Jacob.Offend(Justin)
Justin.Kill(Jacob)
Olivia.CheerUp(Semen)
Justin.Kill(Lesha)
Samantha.InvestigateTheMurder(Lesha)
Bob.GoDoctor(Max)
cat.Eat()