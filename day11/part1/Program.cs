using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day11part1
{
    class Program
    {
        static void Main(string[] args)
        {
            var serial = 5235;
            var grid = new int[300, 300];

            // initialise powerlevels
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    var p = new Point(x, y);
                    grid[x - 1, y - 1] = p.PowerLevel(serial);
                }
            }

            // best 3x3
            var bestPower = 0;
            Point bestPoint = null;
            for (int x = 2; x <= 299; x++)
            {
                for (int y = 2; y <= 299; y++)
                {
                    var power = grid[x - 2, y - 2] + grid[x - 2, y - 1] + grid[x - 2, y] + grid[x - 1, y - 2] + grid[x - 1, y - 1] + grid[x - 1, y] + grid[x, y - 2] + grid[x, y - 1] + grid[x, y];
                    if (power > bestPower)
                    {
                        bestPower = power;
                        bestPoint = new Point(x - 1, y - 1);
                    }
                }
            }

            Console.WriteLine("Result: {0}, {1}", bestPoint.X, bestPoint.Y);
        }
    }

    class Point
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int PowerLevel(int serial)
        {
            var rackId = X + 10;
            var powerLevel = rackId * Y;
            powerLevel += serial;
            powerLevel *= rackId;
            powerLevel = (powerLevel / 100) % 10;
            powerLevel -= 5;
            return powerLevel;
        }
    }
}
