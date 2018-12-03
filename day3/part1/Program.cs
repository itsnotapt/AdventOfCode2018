using System;
using System.IO;
using System.Text.RegularExpressions;

namespace day3part1
{
    class Program
    {
        static void Main(string[] args)
        {
            Fabric f = new Fabric();
            foreach (string line in File.ReadLines(@"input.txt"))
            {
                f.addLine(line);
            }
            Console.WriteLine("Result: {0}", f.countSquares());
        }
    }

    class Fabric
    {
        private int[,] area = new int[1000,1000];
        private Regex rx = new Regex(@"^#(\d{1,4}) @ (\d{1,3}),(\d{1,3}): (\d{1,2})x(\d{1,2})", RegexOptions.Compiled);

        public void addLine(string line)
        {
            Match match = rx.Match(line);
            int id = Int32.Parse(match.Groups[1].Value);
            int leftEdge = Int32.Parse(match.Groups[2].Value);
            int topEdge = Int32.Parse(match.Groups[3].Value);
            int width = Int32.Parse(match.Groups[4].Value);
            int height = Int32.Parse(match.Groups[5].Value);

            for (int x = leftEdge; x < (leftEdge + width); x++)
            {
                for (int y = topEdge; y < (topEdge + height); y++)
                {
                    area[x, y]++;
                }
            }
        }

        public int countSquares()
        {
            int count = 0;
            for (int x = 0; x < area.GetLength(0); x++)
            {
                for (int y = 0; y < area.GetLength(1); y++)
                {
                    if (area[x,y] >= 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
