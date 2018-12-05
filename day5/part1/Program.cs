using System;
using System.Text;
using System.IO;

namespace day5part1
{
    class Program
    {
        static void Main(string[] args)
        {
            string polymer = File.ReadAllText(@"input.txt").TrimEnd();
            StringBuilder sb = new StringBuilder(polymer);

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

            Console.WriteLine("Result: {0}", sb.Length);
        }
    }
}
