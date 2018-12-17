using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day10part1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));

            // position=<-50948,  20587> velocity=< 5, -2>
            var rx = new Regex(@"position=<\s?([-\d]+),\s+([-\d]+)> velocity=<\s?([-\d]+),\s+([-\d]+)>");

            var points = new List<Vector>();
            foreach (var line in lines)
            {
                var matches = rx.Match(line);

                var posX = Int32.Parse(matches.Groups[1].Value);
                var posY = Int32.Parse(matches.Groups[2].Value);

                var velX = Int32.Parse(matches.Groups[3].Value);
                var velY = Int32.Parse(matches.Groups[4].Value);

                points.Add(new Vector(posX, posY, velX, velY));
            }

            var minDistX = Int32.MaxValue;
            var minDistY = Int32.MaxValue;

            var c = 0;
            while (true)
            {
                foreach (var point in points)
                {
                    point.Step();
                }

                var distX = points.Max(item => item.PosX) - points.Min(item => item.PosX);
                var distY = points.Max(item => item.PosY) - points.Min(item => item.PosY);

                if (distX >= minDistX && distY >= minDistY)
                {
                    foreach (var point in points)
                    {
                        point.UndoStep();
                    }
                    break;
                }

                minDistX = Math.Min(distX, minDistX);
                minDistY = Math.Min(distY, minDistY);
                c++;
            }

            var billBoard = new char[minDistX + 1, minDistY + 1];
            for (int y = 0; y <= minDistY; y++)
            {
                for (int x = 0; x <= minDistX; x++)
                {
                    billBoard[x, y] = '.';
                }
            }

            var minX = points.Min(item => item.PosX);
            var minY = points.Min(item => item.PosY);

            foreach (var point in points)
            {
                billBoard[point.PosX - minX, point.PosY - minY] = '#';
            }

            for (int y = 0; y <= minDistY; y++)
            {
                for (int x = 0; x <= minDistX; x++)
                {
                    Console.Write(billBoard[x, y]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Time Taken: {0}", c);
        }
    }

    class Vector
    {
        public int PosX { get; private set; } = 0;
        public int PosY { get; private set; } = 0;

        public int VelX { get; private set; } = 0;
        public int VelY { get; private set; } = 0;

        public Vector(int posX, int posY, int velX, int velY)
        {
            PosX = posX;
            PosY = posY;
            VelX = velX;
            VelY = velY;
        }

        public void Step()
        {
            PosX += VelX;
            PosY += VelY;
        }

        public void UndoStep()
        {
            PosX -= VelX;
            PosY -= VelY;
        }
    }
}
