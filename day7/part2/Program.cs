using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day7part2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));
            SortedDictionary<char, Node> nodes = new SortedDictionary<char, Node>();

            List<Elf> workers = new List<Elf>();
            for (int i = 0; i < 5; i++)
            {
                workers.Add(new Elf());
            }

            foreach (string line in lines)
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

            int totalTime = 0;
            while (nodes.Any())
            {
                Queue<Elf> freeWorkers = new Queue<Elf>();
                foreach(Elf elf in workers)
                {
                    if (elf.isFree())
                    {
                        freeWorkers.Enqueue(elf);
                    }
                }

                foreach (Node node in nodes.Values)
                {
                    if (!freeWorkers.Any())
                    {
                        break;
                    }

                    if (!node.started && node.canStart())
                    {   
                        freeWorkers.Dequeue().startWork(node, 60 + node.id - 'A' + 1);
                    }
                }

                int minTimeLeft = workers.Min(item => item.timeLeft > 0 ? item.timeLeft : Int32.MaxValue);
                totalTime += minTimeLeft;
                foreach (Elf elf in workers)
                {
                    if (elf.hasWork())
                    {
                        Node work = elf.doWork(minTimeLeft);
                        if (work != null)
                        {
                            nodes.Remove(work.id);
                            elf.clear();
                        }
                    }
                }
            }

            Console.WriteLine("Result: {0}", totalTime);
        }
    }

    class Elf
    {
        private Node workingOn;
        public int timeLeft { get; set; } = 0;

        public void startWork(Node workingOn, int timeLeft)
        {
            workingOn.started = true;
            this.workingOn = workingOn;
            this.timeLeft = timeLeft;
        }

        public Node doWork(int time)
        {
            timeLeft -= time;
            if (timeLeft <= 0)
            {
                workingOn.done = true;
                return workingOn;
            }
            return null;
        }

        public bool hasWork()
        {
            if (timeLeft > 0 && workingOn != null)
            {
                return true;
            }
            return false;
        }

        public void clear()
        {
            workingOn = null;
            timeLeft = 0;
        }

        public bool isFree()
        {
            return workingOn == null;
        }
    }

    class Node
    {
        public char id { get; }
        public bool done { get; set; } = false;
        public bool started { get; set; } = false;

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
            foreach (Node node in dependsOn)
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
