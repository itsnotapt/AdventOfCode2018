using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day8part2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));

            var numbers = lines[0].Split(' ');
            var index = 0;

            var root = new Node();

            while (index < numbers.Length)
            {
                index = ParseNode(numbers, index, root);
            }

            Console.WriteLine("Result: {0}", root.Sum());
        }

        static int ParseNode(string[] numbers, int index, Node parent)
        {
            var childCount = Int32.Parse(numbers[index]);
            var metadataCount = Int32.Parse(numbers[index + 1]);

            index += 2;

            for (int i = 0; i < childCount; i++)
            {
                var child = new Node();
                parent.Children.Add(child);
                index = ParseNode(numbers, index, child);
            }

            for (int i = 0; i < metadataCount; i++)
            {
                parent.Metadata.Add(Int32.Parse(numbers[index + i]));
            }

            index += metadataCount;
            return index;
        }
    }

    class Node
    {
        public List<Node> Children { get; set; } = new List<Node>();
        public List<int> Metadata { get; set; } = new List<int>();

        public int Sum()
        {
            var sum = 0;
            if (Children.Count == 0)
            {
                return Metadata.Sum();
            }

            foreach (var index in Metadata)
            {
                var child = Children.ElementAtOrDefault(index - 1);
                if (child != null)
                {
                    sum += child.Sum();
                }
            }
            return sum;
        }
    }
}
