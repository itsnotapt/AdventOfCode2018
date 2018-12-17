using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace day9part2
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
            var points = Int32.Parse(matches.Groups[2].Value) * 100;

            var scores = new long[numPlayers];
            var numbers = new LinkedList<int>(new int[] { 0 });
            var ringList = new RingLinkedList(numbers.First);

            for (int currentNum = 1; currentNum < points; currentNum++)
            {
                if (currentNum % 23 == 0)
                {
                    scores[currentNum % numPlayers] += currentNum;
                    ringList.RotateCounterClockwise(7);
                    scores[currentNum % numPlayers] += ringList.Current.Value;
                    ringList.RemoveCurrent();
                }
                else
                {
                    ringList.RotateClockwise(1);
                    ringList.Add(currentNum);
                }
            }

            Console.WriteLine("Result: {0}", scores.Max());
        }
    }

    class RingLinkedList
    {
        public LinkedListNode<int> Current { get; private set; }

        public RingLinkedList(LinkedListNode<int> current)
        {
            Current = current;
        }

        public void RemoveCurrent()
        {
            var nextNode = Current.Next;
            Current.List.Remove(Current);
            Current = nextNode;            
        }

        public void Add(int value)
        {
            Current.List.AddAfter(Current, value);
            Current = Current.Next;
        }

        public void RotateClockwise(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                var nextNode = Current.Next;
                if (nextNode == null)
                {
                    Current = Current.List.First;
                }
                else
                {
                    Current = nextNode;
                }
            }
        }

        public void RotateCounterClockwise(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                var prevNode = Current.Previous;
                if (prevNode == null)
                {
                    Current = Current.List.Last;
                }
                else
                {
                    Current = prevNode;
                }
            }
        }
    }
}
