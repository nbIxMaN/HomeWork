open World
open Disease

type IVillagePeople = 
    inherit IWPeople
    inherit IDPeople

type VillagePeople(name:string) =
    inherit WPeople(name)
    let mutable infection = None
    let mutable parasite = None
    let mutable hideparasite = None
    interface IVillagePeople with
        member x.BeingACarrier() = hideparasite <- parasite
                                   parasite <- None
        member x.Recedive() = parasite <- hideparasite
                              hideparasite <- None
                              match parasite with
                              | Some(illness) -> if illness.Strange*10 > x.Health then (x:>IWCreatures).Die()
                                                                                       (x:>IWPeople).ChangeHealth 0
                              | None -> ()
        member x.CatchInfection (illness:Infection) = infection <- Some(illness)
                                                      if illness.Strange*10 > x.Health then (x:>IWCreatures).Die()
                                                                                            (x:>IWPeople).ChangeHealth 0
        member x.CatchParasite (illness:Parasite) = parasite <- Some(illness)
                                                    if illness.Strange*10 > x.Health then (x:>IWCreatures).Die()
                                                                                          (x:>IWPeople).ChangeHealth 0
        member x.HealthLow y = (x:>IWPeople).ChangeHealth -y
        member x.ThrowOffAnIllness() = match parasite with
                                       | Some(illness) -> if illness.Stability > 5 then (illness:>Disease).Fall()
                                                                                   else parasite <- None
                                       | None -> ()
                                       match infection with
                                       | Some(illness) -> if illness.Stability > 5 then (illness:>Disease).Fall()
                                                                                   else infection <- None
                                       | None -> ()
    member x.Infected = infection <> None || parasite <> None
    member x.TalkWith (men:VillagePeople) = if (men.Health < 100) && (x.Infected) then printfn "%A" "not communicate with patients"
                                                                                       match infection with
                                                                                       | Some(illness) -> (men:>IDPeople).CatchInfection(illness)
                                                                                                          (illness:>Disease).Multiply()
                                                                                       | None -> ()
                                                                                  else printfn "%A" "Hello comrade"
    member x.Hug (men:VillagePeople) = if (men.Health < 100) && ((x.Infected) || (hideparasite <> None)) then printfn "%A" "Do not hug with patients"
                                                                                                              match infection with
                                                                                                              | Some(illness) -> (men:>IDPeople).CatchInfection(illness)
                                                                                                                                 (illness:>Disease).Multiply()
                                                                                                              | None -> ()
                                                                                                              match parasite with
                                                                                                              | Some(illness) -> (men:>IDPeople).CatchParasite(illness)
                                                                                                                                 (illness:>Disease).Multiply()
                                                                                                              | None -> ()
                                                                                                              match hideparasite with
                                                                                                              | Some(illness) -> (men:>IDPeople).CatchParasite(illness)
                                                                                                              | None -> ()
                                                                                                         else printfn "%A" "Сuddle"
                                                                                                                                                 
    override x.GoDoctor(men) = men.Hill(x)
                               (x:>IDPeople).ThrowOffAnIllness()                                  


let bacterias = new WCreature("Lely")
let cat = new WAnimal("cat")
let Bob = new VillagePeople("Bob")
let Semen = new VillagePeople("Semen")
let Lesha = new VillagePeople("Lesha")
let Taylor = new WPeople("Taylor")
let Olivia = new WGoodPeople("Olivia")
let Max = new WDoctor("Max")
let Samantha = new WCop("Samantha")
let Jacob = new WBadPeople("Jacob")
let Justin = new WMurder("Justin")
let Mia = new WBrawlers("Mia")
let Suzi = new Infection("Suzi")
let Paul = new Parasite("Paul")
Suzi.Infect(Bob)
Paul.Infect(Lesha)
Suzi.Mutate()
Paul.Hide(Lesha)
bacterias.Eat()
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
Samantha.GoDoctor(Max)
cat.Play()
Jacob.Offend(Semen)
Jacob.Offend(Justin)
Justin.Kill(Jacob)
Olivia.CheerUp(Semen)
Paul.Lower_immunity(Lesha)
Paul.Reveal(Lesha)
Justin.Kill(Lesha)
Samantha.InvestigateTheMurder(Lesha)
Bob.GoDoctor(Max)
cat.Eat()