type Geom = NoPoint                  // пустое множество
           | Point of float * float   // точка
           | Line  of float * float   // уравнение прямой y = a*x+b
           | VerticalLine of float    // вертикальная прямая проходящая через x
           | LineSegment of (float * float) * (float * float) // отрезок
           | Intersect of Geom * Geom // пересечение двух множеств

type MyGeom = NoPoint'                    // пустое множество
              | Point' of float * float   // точка
              | Line'  of float * float   // уравнение прямой y = a*x+b
              | VerticalLine' of float    // вертикальная прямая проходящая через x
              | LineSegment' of float * float * (float * float) * (float * float) // отрезок
              | VerticalLineSegment' of float * (float * float)  // вертикальный отрезок
              | Intersect' of MyGeom * MyGeom

let equally x y =
    let residue = 0.00002
    abs (x-y) < residue

let moreOrEqually x1 x2 x3 =
    (x1 < x2 || (equally x1 x2)) && ((x2 < x3) || (equally x2 x3))

let lessOrEqually x1 x2 x3 =
    (x1 > x2 || (equally x1 x2)) && ((x2 > x3) || (equally x2 x3))

let getLine x1 x2 y1 y2 =
    let k = (y2-y1)/(x2-x1)
    let b = y1-(k*x1)
    k, b

let intersectWithVerticalLine k1 b1 x =
    let y = k1*x+b1
    x, y

let getIntersectPoint k1 b1 k2 b2 =
    let x = (b2-b1)/(k1-k2)
    let y = k1*x+b1
    x, y

let totalLineSegment x1 y1 x2 y2 x3 y3 x4 y4 =
    ((max x1 x3), (max y1 y3), (min x2 x4), (min y2 y4))

let rec geomMyGeomConverter geom =
    match geom with
    | NoPoint -> NoPoint'
    | Point(x, y) -> Point'(x, y)
    | Line (k, b) -> Line'(k, b)
    | VerticalLine(x) -> VerticalLine'(x)
    | LineSegment((x1, y1), (x2, y2)) -> 
        if equally x1 x2 then VerticalLineSegment'(x1, ((min y1 y2), (max y1 y2)))
        else
            let (k, b) = getLine x1 x2 y1 y2
            LineSegment' (k, b, ((min x1 x2), (min y1 y2)), ((max x1 x2), (max y1 y2)))
    | Intersect(a, b) -> Intersect'((geomMyGeomConverter a), (geomMyGeomConverter b))

let rec myGeomGeomConverter mygeom =
    match mygeom with
    | NoPoint' -> NoPoint
    | Point'(x, y) -> Point(x, y)
    | Line' (k, b) -> Line(k, b)
    | VerticalLine'(x) -> VerticalLine(x)
    | LineSegment'(_, _, a, b) -> LineSegment(a, b)
    | VerticalLineSegment'(x, (y1, y2)) -> LineSegment((x, y1), (x, y2))
    | Intersect'(a, b) -> Intersect((myGeomGeomConverter a), (myGeomGeomConverter b))

let point p1 p2 =
    match p1 with
    | Point'(x1, y1) ->
        match p2 with
        | NoPoint' -> NoPoint'
        | Point'(x2, y2) ->
            if equally x1 x2 && equally y1 y2 then p1
            else NoPoint'
        | Line'(a, b) ->
            if equally y1 (a*x1+b) then p1
            else NoPoint'
        | VerticalLine'(x) ->
            if equally x1 x then p1
            else NoPoint'
        | LineSegment'(k, b, (x2, y2), (x3, y3)) ->
            if equally y1 (k*x1+b) && moreOrEqually x2 x1 x3 && moreOrEqually y2 y1 y3 then p1
            else NoPoint'
        | VerticalLineSegment'(x2, (y2, y3)) ->
            if equally x1 x2 && moreOrEqually y2 y1 y3 then p1
            else NoPoint'
            
        | _ -> failwith "incorrect data"
    | _ -> failwith "incorrect data"

let line p1 p2 =
    match p1 with
    | Line'(k, b) ->
        match p2 with
        | NoPoint' -> NoPoint'
        | Point' (x2, x3) -> point p2 p1
        | Line'(k1, b1) ->
            if equally k k1 && equally b b1 then p1
            else if equally k k1 then NoPoint'
                 else Point' (getIntersectPoint k b k1 b1)
        | VerticalLine'(x) ->
            Point' (intersectWithVerticalLine k b x )
        | LineSegment'(k1, b1, (x2, y2), (x3, y3)) ->
            if equally k k1 && equally b b1 then p2
            else if  equally k k1 then NoPoint'
                else 
                    let (x, y) = getIntersectPoint k b k1 b1
                    if moreOrEqually x2 x x3 && moreOrEqually y2 y y3 then Point'(x, y)
                        else NoPoint'
        | VerticalLineSegment'(x2, (y2, y3)) ->
            let (x, y) = intersectWithVerticalLine k b x2
            if moreOrEqually y2 y y3 then Point'(x, y)
            else NoPoint'
        | _ -> failwith "incorrect data"
    | _ -> failwith "incorrect data"

let verticalLine p1 p2 =
    match p1 with
    | VerticalLine'(x1) ->
        match p2 with
        | NoPoint' -> NoPoint'
        | Point'(_,_) -> point p2 p1
        | Line'(_,_) -> line p2 p1
        | VerticalLine'(x2) ->
            if equally x1 x2 then p1
            else NoPoint'
        | LineSegment'(k1, b1, (x2, y2), (x3, y3)) ->
            let (x, y) = intersectWithVerticalLine k1 b1 x1
            if moreOrEqually y2 y y3 then Point'(x, y)
            else NoPoint'
        | VerticalLineSegment'(x2, (y2, y3)) ->
            if equally x1 x2 then p2
            else NoPoint'
        | _ -> failwith "incorrect data"
    | _ -> failwith "incorrect data"

let lineSegment p1 p2 =
    match p1 with
    | LineSegment'(k, b, (x1, y1), (x2, y2)) ->
        match p2 with
        | NoPoint' -> NoPoint'
        | Point'(_,_) -> point p2 p1
        | Line'(_,_) -> line p2 p1
        | VerticalLine'(_) -> verticalLine p2 p1
        | LineSegment' (k1, b1, (x3, y3), (x4, y4)) ->
            if equally k k1 && equally b b1 then
                let (x, y, x', y') = totalLineSegment x1 y1 x2 y2 x3 y3 x4 y4
                if x' < x then NoPoint'
                else
                    if equally x x' then Point'(x, y)
                    else
                        LineSegment'(k, b, (x, y), (x', y'))
            else
                if equally k k1 then NoPoint'
                else
                    let (x, y) = getIntersectPoint k b k1 b1
                    if moreOrEqually x1 x x2 && moreOrEqually y1 y y2 && moreOrEqually x3 x x4 && moreOrEqually y3 y y4 then Point'(x, y)
                    else NoPoint'
        | VerticalLineSegment'(x3, (y3, y4)) ->
            let (x, y) = intersectWithVerticalLine k b x3
            if moreOrEqually y1 y y2 && moreOrEqually y3 y y4 then Point'(x, y)
            else NoPoint'
        | _ -> failwith "incorrect data"
    | _ -> failwith "incorrect data"

let verticalLineSegment p1 p2 =
    match p1 with
    | VerticalLineSegment'(x1, (y1, y2)) ->
        match p2 with
        | NoPoint' -> NoPoint'
        | Point'(_,_) -> point p2 p1
        | Line'(_,_) -> line p2 p1
        | VerticalLine'(_) -> verticalLine p2 p1
        | LineSegment'(_,_,_,_) -> lineSegment p2 p1
        | VerticalLineSegment' (x2, (y3, y4)) ->
            if equally x1 x2 then
                let (x, y, x', y') = totalLineSegment x1 y1 x1 y2 x2 y3 x2 y4
                if y' < y then NoPoint'
                else
                    if equally y y' then Point'(x, y)
                    else
                        VerticalLineSegment'(x, (y, y'))
            else NoPoint'
        | _ -> failwith "incorrect data"
    | _ -> failwith "incorrect data"

let rec getIntersect p1 p2 =
    match p1 with
    | NoPoint' -> NoPoint'
    | Point'(_,_) -> point p1 p2
    | Line'(_,_) -> line p1 p2
    | VerticalLine'(_) -> verticalLine p1 p2
    | LineSegment'(_,_,_,_) -> lineSegment p1 p2
    | VerticalLineSegment' (x2, (y3, y4)) -> verticalLineSegment p1 p2
    | Intersect'(a, b) -> getIntersect (getIntersect a b) p2
    | _ -> failwith "incorrect data"

let intersect p1 p2 =
    let p1 = geomMyGeomConverter p1
    let p2 = geomMyGeomConverter p2
    let intersect = getIntersect p1 p2
    myGeomGeomConverter intersect