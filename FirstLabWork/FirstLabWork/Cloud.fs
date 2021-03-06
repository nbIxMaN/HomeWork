﻿module Creature
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
         let power = wind.power
         let current = daylight.Current
         let isShining = luminary.IsShining
         if isShining then
            if current = Morning then
                if power = 0 then
                    new Creature(Kitten, Stork)
                else 
                    if (power > 0) && (power < 10) then
                        new Creature(Puppy, Stork)
                    else
                        if power = 10 then
                            new Creature(Kitten, Daemon)
                        else raise <| new System.NotImplementedException()
            else
                if current = Day then
                    if power = 0 then
                        new Creature(Balloon, Stork)
                    else 
                        if (power > 0) && (power < 10) then
                            new Creature(Puppy, Stork)
                        else
                            if power = 10 then
                                new Creature(Hedgehog, Daemon)
                            else raise <| new System.NotImplementedException()
                else
                    if current = Evening then
                        if power = 0 then
                            new Creature(Kitten, Stork)
                        else 
                            if (power > 0) && (power < 5) then
                                new Creature(Bat, Daemon)
                            else
                                if (power > 4) && (power < 10) then
                                    new Creature(Bearcub, Stork)
                                else
                                    if power = 10 then
                                        new Creature(Kitten, Daemon)
                                    else raise <| new System.NotImplementedException()
                    else
                        if current = Night then
                            if power = 0 then
                                new Creature(Bat, Daemon)
                            else 
                                if (power > 0) && (power < 5) then
                                    new Creature(Bat, Daemon)
                                else
                                    if (power > 4) && (power < 10) then
                                        new Creature(Piglet, Daemon)
                                    else
                                        if power = 10 then
                                            new Creature(Hedgehog, Daemon)
                                        else raise <| new System.NotImplementedException()
                        else
                            raise <| new System.NotImplementedException()
         else
            if current = Morning then
                if (power > -1) && (power < 5) then
                    new Creature(Piglet, Stork)
                else 
                    if (power > 4) && (power < 10) then
                        new Creature(Bearcub, Daemon)
                    else
                        if power = 10 then
                            new Creature(Kitten, Daemon)
                        else raise <| new System.NotImplementedException()
            else
                if current = Day then
                    if (power > -1) && (power < 5) then
                        new Creature(Balloon, Stork)
                    else 
                        if (power > 4) && (power < 10) then
                            new Creature(Piglet, Daemon)
                        else
                            if power = 10 then
                                new Creature(Hedgehog, Daemon)
                            else raise <| new System.NotImplementedException()
                else
                    if current = Evening then
                        if (power > -1) && (power < 5) then
                            new Creature(Bearcub, Stork)
                        else 
                            if (power > 4) && (power < 11) then
                                new Creature(Hedgehog, Daemon)
                            else raise <| new System.NotImplementedException()
                    else
                        if current = Night then
                            if power = 0 then
                                new Creature(Balloon, Stork)
                            else 
                                if (power > 0) && (power < 5) then
                                    new Creature(Bat, Daemon)
                                else
                                    if (power > 4) && (power < 10) then
                                        new Creature(Puppy, Daemon)
                                    else
                                        if power = 10 then
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