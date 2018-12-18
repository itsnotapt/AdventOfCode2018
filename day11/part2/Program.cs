using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day11part2
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
            int bestGridSize = 0;
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    var totalPower = 0;
                    for (int gridSize = 0; gridSize <= Math.Min(300-x, 300-y); gridSize++)
                    {
                        for (int row = y; row <= y + gridSize; row++)
                        {
                            var col = x + gridSize;
                            totalPower += grid[col-1, row-1];
                        }

                        for (int col = x; col <= x + gridSize - 1; col++)
                        {
                            var row = y + gridSize;
                            totalPower += grid[col-1, row-1];
                        }

                        if (totalPower > bestPower)
                        {
                            bestPower = totalPower;
                            bestPoint = new Point(x, y);
                            bestGridSize = gridSize;
                        }
                    }
                }
            }

            Console.WriteLine("Result: {0}, {1}, {2}", bestPoint.X, bestPoint.Y, bestGridSize+1);
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
