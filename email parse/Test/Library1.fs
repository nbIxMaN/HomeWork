namespace UnitTest

open emailParse
open NUnit.Framework

[<TestFixture>]
module ``Test isValid function`` =
    [<Test>]
    let Email1 () =
        Assert.IsTrue(isValid "ef.w.e.f.w.e.f@wf.w.f.cat")
    [<Test>]
    let Email2 () =
        Assert.IsTrue(isValid "victor.polozov@mail.ru")
    [<Test>]
    let Email3 () =
        Assert.IsTrue(isValid "paints_department@hermitage.museum")
    [<Test>]
    let Email4 () =
        Assert.IsTrue(isValid "my@domain.info")
    [<Test>]
    let Email5 () =
        Assert.IsTrue(isValid "tigr_zelenii.s-serenkim@jivaya.priroda_5.coop")
    [<Test>]
    let Email6 () =
        Assert.IsFalse(isValid "1_.2@google.com")
    [<Test>]
    let Email7 () =
        Assert.IsFalse(isValid "mySQL.@mail.ru")
    [<Test>]
    let Email8 () =
        Assert.IsFalse(isValid "undead'flower@mail.ru")
    [<Test>]
    let Email9 () =
        Assert.IsFalse(isValid "b@d.ru")
    [<Test>]
    let Email10 () =
        Assert.IsFalse(isValid "whereIsMyAt.ru")
    [<Test>]
    let Email11 () =
        Assert.IsFalse(isValid "BIG@BANG.rus")