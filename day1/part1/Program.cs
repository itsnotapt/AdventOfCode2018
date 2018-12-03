using System;
using System.IO;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            foreach (string line in File.ReadLines(@"1.txt"))
            {
                int x = Int32.Parse(line);
                count += x;
            }
            Console.WriteLine("Result: {0}", count);
        }
    }
}
