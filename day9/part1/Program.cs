using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace day9part1
{
    class Program
    {

        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));
            // 411 players; last marble is worth 71058 points
            var rx = new Regex(@"(\d+) players; last marble is worth (\d+) points");
            var matches = rx.Match(lines[0]);

            var numPlayers = Int32.Parse(matches.Groups[1].Value);
            var points = Int32.Parse(matches.Groups[2].Value);

            var scores = new int[numPlayers];
            var currentIdx = 0;
            var numbers = new List<int>(new int[] { 0 });
            
            for (int currentNum = 1; currentNum < points; currentNum++)
            {
                if (currentNum % 23 == 0)
                {
                    scores[currentNum % numPlayers] += currentNum;
                    currentIdx = (currentIdx - 7 + numbers.Count) % numbers.Count;
                    scores[currentNum % numPlayers] += numbers[currentIdx];
                    numbers.RemoveAt(currentIdx);
                }
                else
                {
                    currentIdx = (currentIdx + 2) % numbers.Count;
                    numbers.Insert(currentIdx, currentNum);
                }
            }

            Console.WriteLine("Result: {0}", scores.Max());
        }
    }
}
