using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            bool resultFound = false;
            HashSet<int> freqSet = new HashSet<int>();
            while (!resultFound)
            {
                foreach (string line in File.ReadLines(@"1.txt"))
                {
                    int x = Int32.Parse(line);
                    count += x;
                    if (freqSet.Contains(count))
                    {                        
                        resultFound = true;
                        break;
                    }
                    else
                    {
                        freqSet.Add(count);
                    }
                }
            }
            Console.WriteLine("Result: {0}", count);
        }
    }
}
