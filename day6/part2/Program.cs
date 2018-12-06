using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day6part2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));

            Area area = new Area();

            foreach (string line in lines)
            {
                area.addPoint(stringToPoint(line));
            }

            int regionSize = area.regionSize();
            Console.WriteLine("Result: {0}", regionSize);

        }

        static Point stringToPoint(string line)
        {
            string[] parts = line.Split(',');
            int x = Int32.Parse(parts[0].Trim());
            int y = Int32.Parse(parts[1].Trim());
            return new Point(x, y);
        }
    }

    class Area
    {
        private List<Point> points = new List<Point>();

        public void addPoint(Point p)
        {
            points.Add(p);
        }

        public int regionSize()
        {
            int maxWidth = points.Max(item => item.x);
            int maxHeight = points.Max(item => item.y);

            HashSet<Point> region = new HashSet<Point>();

            for (int x = 0; x <= maxWidth; x++)
            {
                for (int y = 0; y <= maxHeight; y++)
                {
                    Point p1 = new Point(x, y);

                    int totalDist = 0;

                    foreach (Point p2 in points)
                    {
                        int dist = p1.distance(p2);
                        totalDist += dist;
                    }

                    if (totalDist < 10000)
                    {
                        region.Add(p1);
                    }
                }
            }

            return region.Count;
        }
    }
    class Point
    {
        public int x { get; }
        public int y { get; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int distance(Point p)
        {
            return Math.Abs(x - p.x) + Math.Abs(y - p.y);
        }
    }
}
