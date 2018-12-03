using System;
using System.IO;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // I'm sure this can be better than O(m*n^2)...
            foreach (string line1 in File.ReadLines(@"input.txt"))
            {
                foreach (string line2 in File.ReadLines(@"input.txt"))
                {
                    if (line1.Equals(line2))
                    {
                        continue;
                    }

                    int diff = 0;
                    string s = "";
                    for (int i = 0; i < line1.Length; i++)
                    {
                        if (line1[i] != line2[i])
                        {                            
                            diff++;
                        } else
                        {
                            s += line1[i];
                        }
                    }

                    if (diff == 1)
                    {
                        Console.WriteLine("Result: {0}", s);
                    }
                }
            }            
        }
    }
}
