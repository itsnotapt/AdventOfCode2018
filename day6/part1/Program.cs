using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day6part1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));

            Area area = new Area();

            foreach(string line in lines)
            {
                area.addPoint(stringToPoint(line));   
            }

            int largestArea = area.largestArea();
            Console.WriteLine("Result: {0}", largestArea);

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

        public int largestArea()
        {
            int maxWidth = points.Max(item => item.x);
            int maxHeight = points.Max(item => item.y);

            Dictionary<Point, int> area = new Dictionary<Point, int>();
            HashSet<Point> infinite = new HashSet<Point>();

            for (int x = 0; x <= maxWidth; x++)
            {
                for (int y = 0; y <= maxHeight; y++)
                {
                    Point p1 = new Point(x, y);

                    Point closestPoint = null;
                    int closestDist = Int32.MaxValue;

                    foreach (Point p2 in points)
                    {
                        int dist = p1.distance(p2);

                        if (dist == closestDist)
                        {
                            closestPoint = null;
                        }
                        else if (dist <= closestDist)
                        {
                            closestDist = dist;
                            closestPoint = p2;
                        }
                    }

                    if (closestPoint != null)
                    {
                        if (!area.ContainsKey(closestPoint))
                        {
                            area[closestPoint] = 0;
                        }
                        area[closestPoint] += 1;

                        if (x == 0 || x == maxWidth || y == 0 || y == maxHeight)
                        {
                            infinite.Add(closestPoint);
                        }
                    }
                }
            }

            int maxArea = 0;
            foreach(Point point in points)
            {
                if (area.ContainsKey(point) && !infinite.Contains(point) && area[point] > maxArea)
                {
                    maxArea = area[point];
                }
            }

            return maxArea;
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
