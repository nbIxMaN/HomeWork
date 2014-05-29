// Aleksandr Pykhtin
// 2014

namespace Disease

[<AbstractClass>]
type Disease(name:string) =
    let mutable strange = 3
    let mutable stability = 2
    member internal x.ChangeStrange i = strange <- strange + i
    member internal x.ChangeStability i = stability <- stability + i
    member x.Strange = strange
    member x.Stability = stability
    member x.Multiply () = strange <- strange + 1
    member x.Fall() = strange <- strange - 1
    member x.Name = name

type Infection(name:string) = 
     inherit Disease(name)
     member x.Mutate () = x.ChangeStrange 4
                          x.ChangeStability -1

type Parasite(name:string) =
     inherit Disease(name)
     member x.Evolve() = x.ChangeStrange -1
                         x.ChangeStability 2