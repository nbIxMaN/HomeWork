module tests
open Creature
open NSubstitute
open FsCheck
open NUnit.Framework
open FsUnit

type Dayligth(time : DaylightType) =
    interface IDaylight with
        member x.Current = time

type Luminary(isShining : bool) =
    interface ILuminary with
        member x.IsShining = isShining

type Wind (power : int) = 
    interface IWind with
        member x.power = power

type Courier(courierType : CourierType) = 
    let mutable xcourier = Daemon
    interface ICourier with
        member x.GiveBaby baby = ()
    member x.addCoutier(courier : CourierType) = xcourier <- courier

type Magic() =
    let daemon = new Courier(Daemon)
    let stork = new Courier(Stork)
    interface IMagic with
        member x.CallDaemon() = (daemon:>ICourier)
        member x.CallStork() = (stork:>ICourier)
let mockDayLigth = Substitute.For<IDaylight>()
let mockLuminary = Substitute.For<ILuminary>()
let mockWind = Substitute.For<IWind>()
let mockMagic = Substitute.For<IMagic>()
let mockCourier = Substitute.For<ICourier>()

let rightCreature(time : DaylightType, lum : bool, windPower : int) =
    if (lum && (windPower = 0) && (time = Day)) ||
        (not lum && (windPower > -1) && (windPower < 5) && (time = Day)) ||
        (not lum && (windPower = 0) && (time = Night)) then
            new Creature(Balloon, Stork)
    else if (lum && (windPower = 10) && ((time = Day) || (time = Night))) ||
        (not lum && (windPower > 4) && (time = Evening)) ||
        (not lum && (windPower = 10) && ((time = Night) || (time = Day))) then
            new Creature(Hedgehog, Daemon)
    else if (lum && (windPower = 0) && ((time = Morning) || (time = Evening))) then
        new Creature(Kitten, Stork)
    else if (lum && (windPower = 10) && ((time = Morning) || (time = Evening))) ||
    (not lum && (windPower = 10) && (time = Morning)) then
        new Creature(Kitten, Daemon)
    else if (lum && (windPower > 0) && (windPower < 5) && (time = Evening)) ||
        (lum && (windPower > -1) && (windPower < 5) && (time = Night)) ||
        (not lum && (windPower > 0) && (windPower < 5) && (time = Night)) then
            new Creature(Bat, Daemon)
    else if (lum && (windPower > 4) && (windPower < 10) && (time = Evening)) ||
        (not lum && (windPower > -1) && (windPower < 5) && (time = Evening)) then
            new Creature(Bearcub, Stork)
    else if (not lum && (windPower > 4) && (windPower < 10) && (time = Morning)) then
        new Creature(Bearcub, Daemon)
    else if (lum && (windPower > 4) && (windPower < 10) && (time = Night)) ||
        (not lum && (windPower > 4) && (windPower < 10) && (time = Day)) then
            new Creature(Piglet, Daemon)
    else if (not lum && (windPower > -1) && (windPower < 5) && (time = Morning)) then
        new Creature(Piglet, Stork)
    else if (not lum && (windPower > 4) && (windPower < 10) && (time = Night)) then
        new Creature(Puppy, Daemon)
    else if (lum && (windPower > 0) && (windPower < 10) && ((time = Morning) || (time = Day))) then
        new Creature(Puppy, Stork)
    else raise <| new System.NotImplementedException()

let courierTest(time : DaylightType, lum : bool, windPower : int) = 
    let normalizeWind = abs(windPower) / 11
    let dayligth = new Dayligth(time)
    let luminary = new Luminary(lum)
    let wind = new Wind(normalizeWind)
    mockMagic.CallStork().Returns mockCourier |> ignore
    mockMagic.CallDaemon().Returns mockCourier |> ignore
    let cloud = Cloud(dayligth, luminary, wind, mockMagic)
    let baby = cloud.Create()
    let rightBaby = rightCreature(time, lum, normalizeWind)
    if rightBaby.GiveCourierType() = Stork then 
        mockMagic.Received().CallStork() |> ignore
    else
        mockMagic.Received().CallDaemon() |> ignore
    mockCourier.Received().GiveBaby(Arg.Is<Creature>(fun (x : Creature)-> x = baby)) |> ignore

[<Test>]
let TestCourier() =
    FsCheck.Check.Quick(courierTest)

let creatureTest(time : DaylightType, lum : bool, windPower : int) =
    let normalizeWind = abs(windPower) % 11
    let dayligth = new Dayligth(time)
    let luminary = new Luminary(lum)
    let wind = new Wind(normalizeWind)
    let cloud = Cloud(dayligth, luminary, wind, mockMagic)
    let baby = cloud.Create()
    let rightBaby = rightCreature(time, lum, normalizeWind)
    baby.GiveBabyType() = rightBaby.GiveBabyType()

[<Test>]
let CreatureTest() =
    FsCheck.Check.Quick(creatureTest)
//    creatureTest(Day, false, 21) |> should be True
    


//тесы на каждую строчку
//правильные существа
//правильный курьер
//существо передано