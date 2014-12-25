using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEXIC
{
    enum RotationType
    {
        LeftLeftRotation,
        LeftRigthRotation,
        TopLeftLeftRotation,
        TopLeftRigthRotation,
        TopRigthLeftRotation,
        TopRigthRigthRotation,
        RigthLeftRotation,
        RigthRigthRotation,
        DownLeftLeftRotation,
        DownLeftRigthRotation,
        DownRigthLeftRotation,
        DownRigthRigthRotation,
        NoRotation
    }
    struct Rotation
    {
        public Pair dot;
        public RotationType rotation;
        public long score;
        public Rotation(Pair x, RotationType y, long z)
        {
            dot = x;
            rotation = y;
            score = z;
        }
    }
    struct Pair
    {
        public int first;
        public int second;
        public Pair(int x, int y)
        {
            first = x;
            second = y;
        }
    }

    class Map
    {
        Random r = new Random(4);
        public int[][] map;
        private int s;
        private int n;
        public Map()
        {
            map = new int[17][];
            for (int i = 0; i < 17; ++i)
            {
                map[i] = new int[10];
            }
            foreach (Pair number in GetEnumerator())
            {
                map[number.first][number.second] = r.Next(7)+1;
            }
        }
        public IEnumerable<Pair> GetEnumerator()
        {
            s = 0;
            n = 1;
            while (s < 17)
            {
                while (n < 10)
                {
                    yield return new Pair(s, n);
                    n += 2;
                }
                ++s;
                if (s % 2 == 0)
                {
                    n = 1;
                }
                else
                {
                    n = 0;
                }
            }
        }
        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        static void RigthRotation<T>(ref T left, ref T x, ref T rigth)
        {
            Swap<T>(ref left, ref x);
            Swap<T>(ref rigth, ref x);
        }
        static void LeftRotation<T>(ref T left, ref T x, ref T rigth)
        {
            Swap<T>(ref rigth, ref x);
            Swap<T>(ref left, ref x);
        } 
        // проверить методы от сюда
        private void LeftLeftRotation(int x, int y)
        {
            if ((x > 1) && (y > 0) && (x < 15) && (y < 10))
            {
                LeftRotation<int>(ref map[x + 1][y - 1], ref map[x][y], ref map[x - 1][y - 1]);
            }
        }
        private void LeftRigthRotation(int x, int y)
        {
            if ((x > 1) && (y > 0) && (x < 15) && (y < 10))
            {
                RigthRotation<int>(ref map[x + 1][y - 1], ref map[x][y], ref map[x - 1][y - 1]);

            }
        }
        private void TopLeftLeftRotation(int x, int y)
        {
            if ((x > 1) && (y > 0) && (x < 17) && (y < 10))
            {
                LeftRotation<int>(ref map[x - 2][y], ref map[x - 1][y - 1], ref map[x][y]);
            }
        }
        private void TopLeftRigthRotation(int x, int y)
        {
            if ((x > 1) && (y > 0) && (x < 17) && (y < 10))
            {
                RigthRotation<int>(ref map[x - 2][y], ref map[x - 1][y - 1], ref map[x][y]);
            }
        }
        private void TopRigthLeftRotation(int x, int y)
        {
            if ((x > 1) && (y >= 0) && (x < 17) && (y < 9))
            {
                LeftRotation<int>(ref map[x][y], ref map[x - 1][y + 1], ref map[x - 2][y]);
            }
        }
        private void TopRigthRigthRotation(int x, int y)
        {
            if ((x > 1) && (y >= 0) && (x < 17) && (y < 9))
            {
                RigthRotation<int>(ref map[x][y], ref map[x - 1][y + 1], ref map[x - 2][y]);
            }
        }
        private void RigthLeftRotation(int x, int y)
        {
            if ((x > 0) && (y >= 0) && (x < 16) && (y < 9))
            {
                LeftRotation<int>(ref map[x - 1][y + 1], ref map[x][y], ref map[x + 1][y + 1]);
            }
        }
        private void RigthRigthRotation(int x, int y)
        {
            if ((x > 0) && (y >= 0) && (x < 16) && (y < 9))
            {
                RigthRotation<int>(ref map[x - 1][y + 1], ref map[x][y], ref map[x + 1][y + 1]);
            }
        }
        private void DownLeftLeftRotation(int x, int y)
        {
            if ((x >= 0) && (y > 0) && (x < 15) && (y < 10))
            {
                LeftRotation<int>(ref map[x][y], ref map[x + 1][y - 1], ref map[x + 2][y]);
            }
        }
        private void DownLeftRigthRotation(int x, int y)
        {
            if ((x >= 0) && (y > 0) && (x < 15) && (y < 10))
            {
                RigthRotation<int>(ref map[x][y], ref map[x + 1][y - 1], ref map[x + 2][y]);
            }
        }
        private void DownRigthLeftRotation(int x, int y)
        {
            if ((x >= 0) && (y >= 0) && (x < 15) && (y < 9))
            {
                LeftRotation<int>(ref map[x + 2][y], ref map[x + 1][y + 1], ref map[x][y]);
            }
        }
        private void DownRigthRigthRotation(int x, int y)
        {
            if ((x >= 0) && (y >= 0) && (x < 15) && (y < 9))
            {
                RigthRotation<int>(ref map[x + 2][y], ref map[x + 1][y + 1], ref map[x][y]);
            }
        } // до сюда
        public void Rotation(RotationType rotation, Pair dot)
        {
            switch (rotation)
            {
                case RotationType.DownLeftLeftRotation:
                    this.DownLeftLeftRotation(dot.first, dot.second);
                    break;
                case RotationType.DownLeftRigthRotation:
                    this.DownLeftRigthRotation(dot.first, dot.second);
                    break;
                case RotationType.DownRigthLeftRotation:
                    this.DownRigthLeftRotation(dot.first, dot.second);
                    break;
                case RotationType.DownRigthRigthRotation:
                    this.DownRigthRigthRotation(dot.first, dot.second);
                    break;
                case RotationType.LeftLeftRotation:
                    this.LeftLeftRotation(dot.first, dot.second);
                    break;
                case RotationType.LeftRigthRotation:
                    this.LeftRigthRotation(dot.first, dot.second);
                    break;
                case RotationType.RigthLeftRotation:
                    this.RigthLeftRotation(dot.first, dot.second);
                    break;
                case RotationType.RigthRigthRotation:
                    this.RigthRigthRotation(dot.first, dot.second);
                    break;
                case RotationType.TopLeftLeftRotation:
                    this.TopLeftLeftRotation(dot.first, dot.second);
                    break;
                case RotationType.TopLeftRigthRotation:
                    this.TopLeftRigthRotation(dot.first, dot.second);
                    break;
                case RotationType.TopRigthLeftRotation:
                    this.TopRigthLeftRotation(dot.first, dot.second);
                    break;
                case RotationType.TopRigthRigthRotation:
                    this.TopRigthRigthRotation(dot.first, dot.second);
                    break;
                default:
                    break;
            }
        }
        public void Сancel(RotationType rotation, Pair dot)
        {
            switch (rotation)
            {
                case RotationType.DownLeftLeftRotation:
                    this.Rotation(RotationType.DownLeftRigthRotation, dot);
                    break;
                case RotationType.DownLeftRigthRotation:
                    this.Rotation(RotationType.DownLeftLeftRotation, dot);
                    break;
                case RotationType.DownRigthLeftRotation:
                    this.Rotation(RotationType.DownRigthRigthRotation, dot);
                    break;
                case RotationType.DownRigthRigthRotation:
                    this.Rotation(RotationType.DownRigthLeftRotation, dot);
                    break;
                case RotationType.LeftLeftRotation:
                    this.Rotation(RotationType.LeftRigthRotation, dot);
                    break;
                case RotationType.LeftRigthRotation:
                    this.Rotation(RotationType.LeftLeftRotation, dot);
                    break;
                case RotationType.RigthLeftRotation:
                    this.Rotation(RotationType.RigthRigthRotation, dot);
                    break;
                case RotationType.RigthRigthRotation:
                    this.Rotation(RotationType.RigthLeftRotation, dot);
                    break;
                case RotationType.TopLeftLeftRotation:
                    this.Rotation(RotationType.TopLeftRigthRotation, dot);
                    break;
                case RotationType.TopLeftRigthRotation:
                    this.Rotation(RotationType.TopLeftLeftRotation, dot);
                    break;
                case RotationType.TopRigthLeftRotation:
                    this.Rotation(RotationType.TopRigthRigthRotation, dot);
                    break;
                case RotationType.TopRigthRigthRotation:
                    this.Rotation(RotationType.TopRigthLeftRotation, dot);
                    break;
                default:
                    break;
            }
        }
        public void Drop()
        {
            foreach (Pair number in GetEnumerator())
            {
                if (map[number.first][number.second] == 0)
                {
                    int s = number.first;
                    while (s >= 2)
                    {
                        map[s][number.second] = map[s -= 2][number.second];
                    }
                    map[s][number.second] = r.Next(7) + 1;
                }
            }
        }
        public void WriteMap()
        {
            System.Console.WriteLine();
            System.Console.WriteLine();
            //s = 0;
            //while (s < 17)
            //{
            //    if (s % 2 == 0)
            //    {
            //        n = 1;
            //        System.Console.Write(" ");
            //    }
            //    else
            //    {
            //        n = 0;
            //    }
            //    while (n < 10)
            //    {
            //        System.Console.Write(map[s][n]);
            //        System.Console.Write(" ");
            //        n += 2;
            //    }
            //    ++s;
            //    System.Console.WriteLine();
            //}
            foreach (Pair number in GetEnumerator())
            {
                if (((n == 0) || (n == 1)) && (s > 0))
                {
                    System.Console.WriteLine();
                }
                if (n == 1)
                {
                    System.Console.Write(" ");
                }
                System.Console.Write(map[number.first][number.second]);
                System.Console.Write(" ");
            }
        }
    }

    class Player
    {
        Map map;
        Queue<Pair> queue = new Queue<Pair>();
        HashSet<Pair> set = new HashSet<Pair>();
        int score = 0;
        public Player(Map m)
        {
            map = m;
        }
        private bool Correct (Pair x, Pair y)
        {
            return (y.first >= 0) && (x.first >= 0) &&
                   (y.first < 17) && (x.first < 17) &&
                   (y.second >= 0) && (x.second >= 0) &&
                   (y.second < 10) && (x.second < 10) &&
                   (map.map[x.first][x.second] == map.map[y.first][y.second]);
        }

        private bool Add (Pair x, Pair y, Pair z)
        {
            if (Correct(x, y) && Correct(x, z))
            {
                if (!set.Contains(x))
                {
                    set.Add(x);
                }
                if (!set.Contains(y))
                {
                    queue.Enqueue(y);
                    set.Add(y);
                }
                if (!set.Contains(z))
                {
                    queue.Enqueue(z);
                    set.Add(z);
                }
                return true;
            }
            return false;
        }
        private void SetToZero()
        {
                foreach (Pair number in set)
                {
                    map.map[number.first][number.second] = 0;
                }
                set.Clear();
        }
        public int Delete(Pair dot)
        {
            set.Clear();
            int score = 0;
            score += Find(dot);
            SetToZero();
            map.Drop();
            score += Delete();
            return score;
        }
        public void PrepareMap()
        {
            set.Clear();
            Delete();
        }
        private int Delete()
        {
            int score = 0;
            do
            {
                SetToZero();
                map.Drop();
                foreach (Pair dot in map.GetEnumerator())
                {
                    score += Find(dot);
                }
            }
            while (set.Count != 0);
            return score;
        }
        public int Find(Pair dot)
        {
            queue.Clear();
            int score = 0;
            bool added;
            queue.Enqueue(dot);
            while (!(queue.Count == 0))
            {
                added = false;
                Pair thisElem = queue.Dequeue();
                Pair topLeftElem = new Pair(thisElem.first - 1, thisElem.second - 1);
                Pair topRigthElem = new Pair(thisElem.first - 1, thisElem.second + 1);
                Pair topElem = new Pair(thisElem.first - 2, thisElem.second);
                Pair downElem = new Pair(thisElem.first + 2, thisElem.second);
                Pair downLeftElem = new Pair(thisElem.first + 1, thisElem.second - 1);
                Pair downRigthElem = new Pair(thisElem.first + 1, thisElem.second + 1);
                if (Add(thisElem, topLeftElem, topElem) && !added)
                {
                    score += 1;
                    added = true;
                }
                if (Add(thisElem, topElem, topRigthElem) && !added)
                {
                    score += 1;
                    added = true;
                }
                if (Add(thisElem, topRigthElem, downRigthElem) && !added)
                {
                    score += 1;
                    added = true;
                }
                if (Add(thisElem, downRigthElem, downElem) && !added)
                {
                    score += 1;
                    added = true;
                }
                if (Add(thisElem, downElem, downLeftElem) && !added)
                {
                    score += 1;
                    added = true;
                }
                if (Add(thisElem, downLeftElem, topLeftElem) && !added)
                {
                    score += 1;
                    added = true;
                }
            }
            return score;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map();
            Player p = new Player(map);
            Rotation rotation = new Rotation();
            long totalScore = 0;
            long score = 0;
            p.PrepareMap();
//            System.Console.WriteLine();
//            System.Console.WriteLine(p.Find(new Pair(1, 2)));
            do
            {
                rotation = new Rotation();
                score = 0;
                foreach (Pair dot in map.GetEnumerator())
                {
                    foreach (RotationType rot in Enum.GetValues(typeof(RotationType)))
                    {
                        map.Rotation(rot, dot);
                        score = p.Find(dot);
                        if (score > rotation.score)
                        {
                            rotation = new Rotation(dot, rot, score);
                        }
                        map.Сancel(rot, dot);
                    }
                }
                map.Rotation(rotation.rotation, rotation.dot);
                totalScore += p.Delete(rotation.dot);
            }
            while (rotation.score != 0);
            System.Console.WriteLine(totalScore);
        }
    }
}
