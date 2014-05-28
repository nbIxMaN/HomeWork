// Aleksandr Pykhtin
// 2014

namespace UnitTest

open Intersect
open NUnit.Framework


[<TestFixture>]
module ``Test intersect function`` =
    [<Test>]
    let PointPoint () =
        Assert.AreEqual(intersect (Intersect(Point(0.0, 0.0), Point(0.0, 0.0))), (Point(0.0, 0.0)))

    [<Test>]
    let PointPoint2 () = 
        Assert.AreEqual(intersect (Intersect(Point(0.0, 0.0002), Point(0.0, 0.0))), (NoPoint))

    [<Test>]
    let PointLine () =
        Assert.AreEqual(intersect (Intersect(Point(2.0, 1.0), Line(0.5, 0.0))), (Point(2.0, 1.0)))

    [<Test>]
    let PointLine2 () =
        Assert.AreEqual(intersect (Intersect(Point(0.0, 0.0), Line(1.0, 0.001))), (NoPoint))

    [<Test>]
    let SegmentSegment () =
        Assert.AreEqual(intersect (Intersect((LineSegment ((-1.0, 3.0), (-1.0, -7.0))), (LineSegment ((-1.0, 1.0), (-1.0, 5.0))))), (LineSegment((-1.0, 1.0), (-1.0, 3.0))))

    [<Test>]
    let LineVerticalLine () =
        Assert.AreEqual(intersect (Intersect(Line(1.0, 1.0), VerticalLine 0.0)), (Point(0.0, 1.0)))

    [<Test>]
    let VLineVLine () =
        Assert.AreEqual(intersect (Intersect(VerticalLine 1.1, VerticalLine 1.0)), (NoPoint))

    [<Test>]
    let PointSegment () =
        Assert.AreEqual(intersect (Intersect(Point(-1.0, 1.0), LineSegment((9.0, 78.0), (-1.0, 1.0)) )), (Point(-1.0, 1.0)))

    [<Test>]
    let PointSegment2 () =
        Assert.AreEqual(intersect (Intersect(Point(0.0, 0.0123), LineSegment((0.0, 0.0), (0.0, 1.0)) )), (Point(0.0, 0.0123)))

    [<Test>]
    let PointSegment3 () =
        Assert.AreEqual(intersect (Intersect(Point(0.000009, 0.000007), LineSegment((-1.1, 1.0), (7.0, 7.0)) )), (NoPoint))

    [<Test>]
    let PointPoint3 () =
        Assert.AreEqual(intersect (Intersect((Intersect ((Intersect((VerticalLine 2.0), (Line(-3.0, 10.0)))), (Intersect(LineSegment((5.0, 1.0), (1.0, 5.0)), LineSegment((0.0, 6.0), (3.0, 3.0)))))), Intersect((Point(2.0, 4.0)), Line(1.0, 2.0)))), (Point(2.0, 4.0)))

    [<Test>]
    let LineLine () =
        Assert.AreEqual(intersect (Intersect(Line(3.0, 0.0), Line(3.0, 0.0))), (Line(3.0, 0.0)))

    [<Test>]
    let Linesegment () =
        Assert.AreEqual(intersect (Intersect(Line(9.0, 5.0), LineSegment((0.0, 5.0),(2.0, 23.0)))), (LineSegment((0.0, 5.0),(2.0, 23.0))))

    [<Test>]
    let VerticalLineSegment () =
        Assert.AreEqual(intersect (Intersect(LineSegment((-2.0, 0.0),(1.0, 1.0)), VerticalLine(1.0))), (Point(1.0,1.0)))

    [<Test>]
    let SegmentSegment2 () =
        Assert.AreEqual(intersect (Intersect(LineSegment((2.0, 0.0),(3.0,3.0)), LineSegment((1.0,1.0),(5.0,5.0)))), (Point(3.0, 3.0)))

    [<Test>]
    let VerticalLinePoint () =
        Assert.AreEqual(intersect (Intersect(VerticalLine(50.0), Point((50.00002, 20.99999)))), (NoPoint))
