using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day7part1
{
    class Program
    {
        public static int Sum { get; set; } = 0;

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

            Console.WriteLine("Result: {0}", Program.Sum);
        }

        static int ParseNode(string[] numbers, int index, Node parent)
        {
            var node = new Node();

            var childCount = Int32.Parse(numbers[index]);
            var metadataCount = Int32.Parse(numbers[index + 1]);

            index += 2;

            var children = new List<int>();
            for (int i = 0; i < childCount; i++)
            {
                index = ParseNode(numbers, index, node);
            }

            var metadata = new List<int>();
            for (int i = 0; i < metadataCount; i++)
            {
                metadata.Add(Int32.Parse(numbers[index + i]));
                Program.Sum += Int32.Parse(numbers[index + i]);
            }

            index += metadataCount;
            return index;
        }
    }

    class Node
    {
        List<Node> Children = new List<Node>();
        List<int> Metadata = new List<int>();
    }
}
