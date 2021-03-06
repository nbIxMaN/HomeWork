﻿// Aleksandr Pykhtin
// 2014

module World

open Creatures
open Disease

type VillagePeople(name:string) =
    inherit People(name)
    let mutable infection = None
    let mutable parasite = None
    let mutable hideparasite = None
    member private x.GetInfection() = infection
    member private x.GetParasite() = parasite
    member private x.GetHideParasite() = hideparasite
    member internal x.BeingACarrier() = 
        if x.IsDie || parasite = None then false
        else
            hideparasite <- parasite
            parasite <- None
            true
    member internal x.Recedive() = 
        if x.IsDie || hideparasite = None then false
        else
            parasite <- hideparasite
            hideparasite <- None
            match parasite with
                | Some(illness:Parasite) -> 
                    if illness.Strange*10 > x.Health then x.Die()
                | None -> ()
            true
    member x.CatchInfection (illness:Infection) = 
        if x.IsDie then false
        else
            infection <- Some(illness)
            if illness.Strange*10 > x.Health then x.Die()
            true
    member x.CatchParasite (illness:Parasite) = 
        if x.IsDie then false
        else
            parasite <- Some(illness)
            if illness.Strange*10 > x.Health then x.Die()
            if illness.Strange*10 > x.Health/5 then x.BeingACarrier() |> ignore
            true
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
    member private x.Infected() = infection,parasite,hideparasite
    member x.Ill =
        let getName a =
            match a with
            | Some(infection) -> (infection:>Disease).Name
            | None -> "NOT DETECTED"
        let (infection, parasite, hideparasite) = x.Infected()
        "infection " + getName infection + ", " +
        "parasite " + getName parasite + ", " +
        "hide Parasite " + getName hideparasite
        
    member x.TalkWith (men:VillagePeople) = 
        if x.IsDie then false
        else
            if men.Health < 100 && x.Infected() <> (None, None, None)
                then 
                    match x.GetInfection() with
                    | Some(illness) -> (illness:>Disease).Multiply()
                                       men.CatchInfection(illness) |> ignore                                
                    | None -> ()
            if x.Health< 100 && men.Infected() <> (None, None, None)
                then
                    match men.GetInfection() with
                    | Some(illness) -> (illness:>Disease).Multiply()
                                       x.CatchInfection(illness) |> ignore                                
                    | None -> ()
            true

    member x.Hug (men:VillagePeople) = 
        if x.IsDie then false
        else
            if men.Health < 100 && x.Infected() <> (None, None, None) then 
                    match x.GetInfection() with
                    | Some(illness) -> illness.Multiply()
                                       men.CatchInfection(illness) |> ignore                             
                    | None -> ()
                    match x.GetParasite() with
                    | Some(illness) -> illness.Multiply()
                                       men.CatchParasite(illness) |> ignore  
                    | None -> ()
                    match x.GetHideParasite() with
                    | Some(illness) -> men.CatchParasite(illness) |> ignore  
                    | None -> ()
            if x.Health < 100 && men.Infected() <> (None, None, None) then
                    match men.GetInfection() with
                    | Some(illness) -> illness.Multiply()
                                       x.CatchInfection(illness) |> ignore                             
                    | None -> ()
                    match men.GetParasite() with
                    | Some(illness) -> illness.Multiply()
                                       x.CatchParasite(illness) |> ignore  
                    | None -> ()
                    match men.GetHideParasite() with
                    | Some(illness) -> x.CatchParasite(illness) |> ignore  
                    | None -> ()
            true
                                                                                                                                                 
    member x.GoDoctor(men:Doctor) = 
        if x.IsDie || men.IsDie then false
        else x.ThrowOffAnIllness()        
             men.Cure(x)                          

type VillageParasite(name) =
    inherit Parasite(name)
    member this.LowImmunity(men:VillagePeople) =
        men.ChangeHealth -40
    member this.Hide(men:VillagePeople) =
        men.BeingACarrier()
    member this.Reveal(men:VillagePeople) =
        men.Recedive()
    member this.Infect(men:VillagePeople) =
        men.CatchParasite(this)

type VillageInfection(name) =
    inherit Infection(name)
    member this.Infect(men:VillagePeople) =
        men.CatchInfection(this)



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
let Suzi = new VillageInfection("Suzi")
let Paul = new VillageParasite("Paul")
if Suzi.Infect(Bob) then printfn "%A" "Bob Infected"
if Paul.Infect(Lesha) then printfn "%A" "Lesha Infected"
Suzi.Mutate()
if bacterium.Eat() then printfn "%A" "bacterim say: 'Om-nom-nom'"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Mia.Fight(Semen) then printfn "%A" "Semen get damage"
if Bob.TalkWith(Semen) then printfn "%A" "Hello my friend"
printfn "%A" Semen.Ill
if Lesha.Hug(Semen) then printfn "%A" "Lesha hugs Semen"
printfn "%A" Semen.Ill
if Semen.GoDoctor(Max) then printfn "%A" "Semen recovered"
if Mia.Fight(Samantha) then printfn "%A" "Mia fight with Cop Samantha"
if Samantha.Arrest(Mia) then printfn "%A" "Samantha arrested Mia"
if cat.Play() then printfn "%A" "Cat playing"
if Jacob.Offend(Semen) then printfn "%A" "Jacob offend Semen"
if Jacob.Offend(Justin) then printfn "%A" "Jacob offend Justin, wrong way"
if Justin.Kill(Jacob) then printfn "%A" "Justin kill Jacob, revenge"
if Olivia.CheerUp(Semen) then printfn "%A" "Olivia Cheer Up Semen"
if Justin.Kill(Lesha) then printfn "%A" "Justin kill Lesha"
if Samantha.InvestigateTheMurder(Lesha) then printfn "%A" "killer arrested"
if Bob.GoDoctor(Max) then printfn "%A" "Bob healthy"
if cat.Eat() then printfn "%A" "cat say: 'meow-meow'"