using System;
using System.IO;
using System.Collections;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int twoCharCount = 0;
            int threeCharCount = 0;
            foreach (string line in File.ReadLines(@"input.txt"))
            {
                int[] charCount = new int[26];
                int hasTwoChar = 0;
                int hasThreeChar = 0;
                foreach (char c in line)
                {
                    // convert char a-z to index 0-26
                    int i = c - 0x61;
                    charCount[i]++;
                }

                foreach (int i in charCount)
                {
                    if (i == 2)
                    {
                        hasTwoChar = 1;
                    }
                    else if (i == 3)
                    {
                        hasThreeChar = 1;
                    }
                }
                twoCharCount += hasTwoChar;
                threeCharCount += hasThreeChar;
            }
            Console.WriteLine("Result: {0} x {1} = {2}", twoCharCount, threeCharCount, (twoCharCount*threeCharCount));
        }
    }
}
