using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day7part1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));            
            SortedDictionary<char, Node> nodes = new SortedDictionary<char, Node>();            

            foreach(string line in lines)
            {
                char c1 = line[5];
                char c2 = line[36];

                if (!nodes.ContainsKey(c1))
                {
                    nodes[c1] = new Node(c1);
                }

                if (!nodes.ContainsKey(c2))
                {
                    nodes[c2] = new Node(c2);
                }

                nodes[c2].addDependsOn(nodes[c1]);
            }

            string path = "";
            while (nodes.Any())
            {
                char doneNode = nodes.First().Value.id;
                foreach(Node node in nodes.Values)
                {
                    if (node.canStart())
                    {
                        path += node.id;
                        node.done = true;
                        doneNode = node.id;
                        break;
                    }
                }
                nodes.Remove(doneNode);
            }

            Console.WriteLine("Result: {0}", path);
        }
    }

    class Node
    {
        public char id { get; }
        public bool done { get; set; } = false;

        private HashSet<Node> dependsOn = new HashSet<Node>();

        public Node(char id)
        {
            this.id = id;
        }

        public void addDependsOn(Node node)
        {
            dependsOn.Add(node);
        }

        public bool canStart()
        {
            foreach(Node node in dependsOn)
            {
                if (!node.done)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
