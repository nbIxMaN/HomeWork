module Creature
type CreatureType =
    |  Puppy
    |  Kitten
    |  Hedgehog
    |  Bearcub
    |  Piglet
    |  Bat
    |  Balloon

type DaylightType =
    | Morning
    | Day
    | Evening
    | Night

type CourierType =
    | Daemon
    | Stork

type IDaylight =
    abstract member Current : DaylightType

type ILuminary = 
    abstract member IsShining : bool
type IWind = 
    abstract member power : int

type Creature(baby : CreatureType, courier : CourierType) =
    member x.GiveCourierType() = courier
    member x.GiveBabyType() = baby

type ICourier = 
    abstract member GiveBaby : Creature -> unit

type IMagic =
    abstract member CallDaemon : unit -> ICourier
    abstract member CallStork : unit -> ICourier


type Cloud(daylight : IDaylight, luminary : ILuminary, wind : IWind, magic : IMagic) =
    member private x.InternalCreate() =
         if luminary.IsShining then
            if daylight.Current = Morning then
                if wind.power = 0 then
                    new Creature(Kitten, Stork)
                else 
                    if (wind.power > 0) && (wind.power < 10) then
                        new Creature(Puppy, Stork)
                    else
                        if wind.power = 10 then
                            new Creature(Kitten, Daemon)
                        else raise <| new System.NotImplementedException()
            else
                if daylight.Current = Day then
                    if wind.power = 0 then
                        new Creature(Balloon, Stork)
                    else 
                        if (wind.power > 0) && (wind.power < 10) then
                            new Creature(Puppy, Stork)
                        else
                            if wind.power = 10 then
                                new Creature(Hedgehog, Daemon)
                            else raise <| new System.NotImplementedException()
                else
                    if daylight.Current = Evening then
                        if wind.power = 0 then
                            new Creature(Kitten, Stork)
                        else 
                            if (wind.power > 0) && (wind.power < 5) then
                                new Creature(Bat, Daemon)
                            else
                                if (wind.power > 4) && (wind.power < 10) then
                                    new Creature(Bearcub, Stork)
                                else
                                    if wind.power = 10 then
                                        new Creature(Kitten, Daemon)
                                    else raise <| new System.NotImplementedException()
                    else
                        if daylight.Current = Night then
                            if wind.power = 0 then
                                new Creature(Bat, Daemon)
                            else 
                                if (wind.power > 0) && (wind.power < 5) then
                                    new Creature(Bat, Daemon)
                                else
                                    if (wind.power > 4) && (wind.power < 10) then
                                        new Creature(Piglet, Daemon)
                                    else
                                        if wind.power = 10 then
                                            new Creature(Hedgehog, Daemon)
                                        else raise <| new System.NotImplementedException()
                        else
                            raise <| new System.NotImplementedException()
         else
            if daylight.Current = Morning then
                if (wind.power > -1) && (wind.power < 5) then
                    new Creature(Piglet, Stork)
                else 
                    if (wind.power > 4) && (wind.power < 10) then
                        new Creature(Bearcub, Daemon)
                    else
                        if wind.power = 10 then
                            new Creature(Kitten, Daemon)
                        else raise <| new System.NotImplementedException()
            else
                if daylight.Current = Day then
                    if (wind.power > -1) && (wind.power < 5) then
                        new Creature(Balloon, Stork)
                    else 
                        if (wind.power > 4) && (wind.power < 10) then
                            new Creature(Piglet, Daemon)
                        else
                            if wind.power = 10 then
                                new Creature(Hedgehog, Daemon)
                            else raise <| new System.NotImplementedException()
                else
                    if daylight.Current = Evening then
                        if (wind.power > -1) && (wind.power < 5) then
                            new Creature(Bearcub, Stork)
                        else 
                            if (wind.power > 4) && (wind.power < 11) then
                                new Creature(Hedgehog, Daemon)
                            else raise <| new System.NotImplementedException()
                    else
                        if daylight.Current = Night then
                            if wind.power = 0 then
                                new Creature(Balloon, Stork)
                            else 
                                if (wind.power > 0) && (wind.power < 5) then
                                    new Creature(Bat, Daemon)
                                else
                                    if (wind.power > 4) && (wind.power < 10) then
                                        new Creature(Puppy, Daemon)
                                    else
                                        if wind.power = 10 then
                                            new Creature(Hedgehog, Daemon)
                                        else raise <| new System.NotImplementedException()
                        else
                            raise <| new System.NotImplementedException()
 
    member x.Create() =
      let creature = x.InternalCreate()
      if creature.GiveCourierType() = Stork then
          magic.CallStork().GiveBaby(creature) 
      else 
          magic.CallDaemon().GiveBaby(creature)
      creature