using System;
using System.Text;
using System.IO;

namespace day5part2
{
    class Program
    {
        static void Main(string[] args)
        {
            string polymer = File.ReadAllText(@"input.txt").TrimEnd();

            int shortestLength = polymer.Length;

            for (char c = 'A'; c <= 'Z'; c++)
            {
                StringBuilder sb = new StringBuilder(polymer);
                sb = sb.Replace("" + c, "");            // Remove upper case
                sb = sb.Replace("" + (char)(c | 0x20), "");   // Remove lower case

                int i = 0;
                while (i < sb.Length - 1)
                {
                    // char1 ^ char2 == 0x20 when opposites
                    if (((int)sb[i] ^ (int)sb[i + 1]) == 0x20)
                    {
                        sb.Remove(i, 2);
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (sb.Length < shortestLength)
                {
                    shortestLength = sb.Length;
                }
            }

            Console.WriteLine("Result: {0}", shortestLength);
        }
    }
}
