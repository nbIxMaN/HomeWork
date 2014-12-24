module ShoppingTests.Tests 
 
open NUnit.Framework
open FsUnit
open Shop
open NSubstitute
 
type EmptyShop() = 
    interface IShop with
        member x.CanSell (good : Good) = false
        member x.Sell (goods:Good list) = []
 
type AnytimeFullShop() = 
    interface IShop with
        member x.CanSell (good : Good) = true
        member x.Sell (goods:Good list) = goods
 
type TestCalendar (day : System.DayOfWeek) =
    interface Customer.ICalendar  with
        member x.DayOfWeek = day
 
let calendar day = new TestCalendar(day) :> Customer.ICalendar
let calendarFriday() = calendar System.DayOfWeek.Friday 
   
[<Test>]
let test1 () = 
    let customer = new Customer.Customer(calendarFriday())
    customer.IsDrunk |> should be False
 
[<Test>]
let test2 () = 
//    let cal = calendar System.DayOfWeek.Friday 
    let customer = new Customer.Customer(calendarFriday())
    let emptyShop = new EmptyShop()
 
    customer.GoShopping emptyShop
    customer.GetDrunk ()
    customer.IsDrunk |> should be False
 
[<Test>]
let test3 () = 
    let customer = new Customer.Customer(calendarFriday())
    let allInclusiveShop = new AnytimeFullShop()
 
    customer.GoShopping allInclusiveShop
    customer.GetDrunk ()
    customer.IsDrunk |> should be True
 
[<Test>]
let ``test not drunk on Thursday`` () = 
    let customer = new Customer.Customer(calendar System.DayOfWeek.Thursday)
    let allInclusiveShop = new AnytimeFullShop()
 
    customer.GoShopping allInclusiveShop
    customer.GetDrunk ()
    customer.IsDrunk |> should be False
 
let shouldDrunk (day : System.DayOfWeek) = 
    let customer = new Customer.Customer(calendar day)
    let allInclusiveShop = new AnytimeFullShop()
 
    customer.GoShopping allInclusiveShop
    customer.GetDrunk ()
    if day = System.DayOfWeek.Friday then customer.IsDrunk
    else not customer.IsDrunk
 
[<Test>]
let testQuick() = 
    FsCheck.Check.Quick shouldDrunk

[<Test>]
let nSubst() = 
    let customer = new Customer.Customer(calendarFriday())
    let shop = NSubstitute.Substitute.For<Shop.IShop>()
    customer.GoShopping shop
    NSubstitute.Received.InOrder(fun() -> 
                                    ignore (shop.CanSell(NSubstitute.Arg.Any<Good>()))
                                    ignore (shop.Sell(NSubstitute.Arg.Any<list<Good>>())))

[<Test>]
let nSubst2() =
    let day = System.DayOfWeek.Friday
    let customer = new Customer.Customer(calendar day)
    let shop = Substitute.For<IShop>()
    let list = new System.Collections.Generic.List<Good>()
    let isContains = ref true

    ignore (shop
                .CanSell(Arg.Any<Good>()).Returns(false)
                .AndDoes(fun x -> list.Add(x.[0] :?> Good)))

    ignore (shop
                .Sell(Arg.Any<List<Good>>()).Returns([])
                .AndDoes(fun x ->
                                let check l = List.fold (fun b x -> b && not (list.Contains(x))) true l
                                isContains := !isContains && check (x.[0] :?> list<Good>)))

    customer.GoShopping shop
    Assert.AreEqual(!isContains, true)
                                





