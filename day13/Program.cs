using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day13part1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));

            // Pick part 1 or 2
            var part = 2;

            var width = lines[0].Length;
            var height = lines.Count();

            var railway = new Railway(width, height);

            char[] horizontalChars = new char[] { '-', '>', '<', '+' };     // + could be ambiguous e.g. "+\+", but looks like the input avoids this
            char[] verticalChars = new char[] { '|', 'v', '^', '+' };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch(lines[y][x])
                    {
                        case '-':
                            railway.SetTrack(new Track(x, y, Direction.LEFT | Direction.RIGHT));
                            break;
                        case '|':
                            railway.SetTrack(new Track(x, y, Direction.UP | Direction.DOWN));
                            break;
                        case '\\':
                            if (x == width - 1 || y == 0 || (x > 0 && horizontalChars.Contains(lines[y][x - 1])) || (y < height - 1 && verticalChars.Contains(lines[y + 1][x])))
                            {
                                railway.SetTrack(new Track(x, y, Direction.LEFT | Direction.DOWN));
                            }
                            else if (x == 0 || y == height - 1 || (x < width - 1 && horizontalChars.Contains(lines[y][x + 1])) || (y > 0 && verticalChars.Contains(lines[y - 1][x])))
                            {
                                railway.SetTrack(new Track(x, y, Direction.RIGHT | Direction.UP));
                            }
                            else
                            {
                                throw new Exception("Ambiguous character");
                            }
                            break;
                        case '/':
                            if (x == width - 1 || y == height - 1 || (x > 0 && horizontalChars.Contains(lines[y][x - 1])) || (y > 0 && verticalChars.Contains(lines[y - 1][x])))
                            {
                                railway.SetTrack(new Track(x, y, Direction.LEFT | Direction.UP));
                            }
                            else if (x == 0 || y == 0 || (x < width - 1 && horizontalChars.Contains(lines[y][x + 1])) || ( y < height - 1 && verticalChars.Contains(lines[y + 1][x])))
                            {
                                railway.SetTrack(new Track(x, y, Direction.RIGHT | Direction.DOWN));
                            }
                            else
                            {
                                throw new Exception("Ambiguous character");
                            }
                            break;
                        case '+':
                            railway.SetTrack(new Track(x, y, Direction.LEFT | Direction.UP | Direction.DOWN | Direction.RIGHT ));
                            break;
                        case '^':
                            railway.SetTrack(new Track(x, y, Direction.UP | Direction.DOWN));
                            railway.Trains.Add(new Train(x, y, Direction.UP));
                            break;
                        case '<':
                            railway.SetTrack(new Track(x, y, Direction.LEFT | Direction.RIGHT));
                            railway.Trains.Add(new Train(x, y, Direction.LEFT));
                            break;
                        case 'v':
                            railway.SetTrack(new Track(x, y, Direction.UP | Direction.DOWN));
                            railway.Trains.Add(new Train(x, y, Direction.DOWN));
                            break;
                        case '>':
                            railway.SetTrack(new Track(x, y, Direction.LEFT | Direction.RIGHT));
                            railway.Trains.Add(new Train(x, y, Direction.RIGHT));
                            break;
                        default:
                            break;
                    }
                }
            }

            Train train = null;
            if (part == 1)
            {
                while (railway.CollisionTrains.Count() == 0)
                {
                    railway.Step();
                }

                train = railway.CollisionTrains.First();
            }
            else
            {
                while (railway.Trains.Count > 1)
                {
                    railway.Step();
                }

                train = railway.Trains.First();
            }
            Console.WriteLine("Result: {0},{1}", train.X, train.Y);
        }
    }

    [Flags]
    enum Direction
    {
        UP = 0x1,
        DOWN = 0x2,
        LEFT = 0x4,
        RIGHT = 0x8
    }

    class Point
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }

    class Vector
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void RotateLeft()
        {
            var tempX = X;
            X = Y;
            Y = tempX * -1;
        }

        public void RotateRight()
        {
            var tempX = X;
            X = Y * -1;
            Y = tempX;
        }

        public void AddVectorToPoint(Point point)
        {
            point.X += X;
            point.Y += Y;
        }

        public static Vector DirectionToVector(Direction dir)
        {
            switch(dir)
            {
                case Direction.UP:
                    return new Vector(0, -1);
                case Direction.DOWN:
                    return new Vector(0, 1);
                case Direction.LEFT:
                    return new Vector(-1, 0);
                case Direction.RIGHT:
                    return new Vector(1, 0);
            }
            return null;
        }

        public static Vector VectorBetweenPoints(Point from, Point to)
        {
            return new Vector(to.X - from.X, to.Y - from.Y);
        }
    }

    class Track : Point
    {
        public Direction Dir { get; protected set; }

        public Track(int x, int y, Direction dir) : base(x, y)
        {
            Dir = dir;
        }

        public List<Point> Neighbours()
        {
            var neighbours = new List<Point>();

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (Dir.HasFlag(dir))
                {
                    var point = new Point(X, Y);
                    Vector.DirectionToVector(dir).AddVectorToPoint(point);
                    neighbours.Add(point);
                }
            }

            return neighbours;
        }
    }

    class Train : Point
    {
        private Direction NextDirection { get; set; } = Direction.LEFT;
        public Vector ForwardVector { get; private set; }
        public int Id { get; }
        public static int NextId { get; set; } = 0;

        public Train(int x, int y, Direction dir) : base(x, y)
        {
            Id = NextId++;
            ForwardVector = Vector.DirectionToVector(dir);
        }

        public void Step(Railway railway)
        {
            var prevPoint = new Point(X, Y);

            // Move forward
            ForwardVector.AddVectorToPoint(this);

            var track = railway.Tracks[X, Y];

            var neighbours = track.Neighbours();

            // Dont go backwards
            neighbours.Remove(prevPoint);

            // Pick direction
            if (neighbours.Count == 1)
            {
                ForwardVector = Vector.VectorBetweenPoints(this, neighbours[0]);
            }
            else if (neighbours.Count == 3)
            {
                switch(NextDirection)
                {
                    case Direction.LEFT:
                        ForwardVector.RotateLeft();
                        NextDirection = Direction.UP;
                        break;
                    case Direction.UP:
                        NextDirection = Direction.RIGHT;
                        break;
                    case Direction.RIGHT:
                        ForwardVector.RotateRight();
                        NextDirection = Direction.LEFT;
                        break;
                    default:
                        throw new Exception("Abnormal NextDirection");
                }
            }
            else
            {
                throw new Exception("Abnormal number of neighbours");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Train train &&
                   Id == train.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = 1545243542;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }
    }

    class Railway
    {
        public Track[,] Tracks { get; private set; }
        public HashSet<Train> Trains { get; set; } = new HashSet<Train>();
        public HashSet<Train> CollisionTrains { get; private set; } = new HashSet<Train>();

        public Railway(int width, int height)
        {
            Tracks = new Track[width, height];
        }

        public void SetTrack(Track track)
        {
            Tracks[track.X, track.Y] = track;
        }

        public void Step()
        {
            // Clean up collision trains
            foreach (var train in CollisionTrains)
            {
                Trains.Remove(train);
            }

            foreach (var train in Trains.OrderBy(train => train.Y).ThenBy(train => train.X))
            {
                if (CollisionTrains.Contains(train))
                {
                    continue;
                }

                train.Step(this);

                foreach (var collideTrain in Trains)
                {
                    if (train.Equals(collideTrain))
                    {
                        continue;
                    }

                    if (train.X == collideTrain.X && train.Y == collideTrain.Y) {
                        CollisionTrains.Add(train);
                        CollisionTrains.Add(collideTrain);
                    }
                }
            }
        }
    }
}
