using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day12part1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines(@"input.txt"));

            var initialState = ParseInitialState(lines[0]);
            var rules = new List<Rule>();

            for (int i = 2; i < lines.Count; i++)
            {
                rules.Add(new Rule(ParseRule(lines[i])));
            }

            var plantState = new PlantState(initialState, rules);

            var currentCount = 0;
            var prevCount = -1;
            var round = 0;
            var result = 0L;

            while (true)
            {
                plantState.Step();
                var count = plantState.CountPlants();
                // Look for normalisation
                if ((count - currentCount) == (currentCount - prevCount))
                {
                    result = count + (50000000000L - round - 1) * (currentCount - prevCount);
                    break;
                }
                prevCount = currentCount;
                currentCount = count;
                round++;
            }

            Console.WriteLine("Result: {0}", result);
        }

        static List<bool> ParseInitialState(string s)
        {
            s = s.Substring(15);

            var plants = new List<bool>(s.Length);

            foreach (var c in s)
            {
                plants.Add(c == '#');
            }
            return plants;
        }

        static List<bool> ParseRule(string s)
        {
            var rulePattern = new List<bool>(6);

            foreach(var c in s.Substring(0, 5))
            {
                rulePattern.Add(c == '#');
            }
            rulePattern.Add(s[9] == '#');
            return rulePattern;
        }
    }

    class PlantState
    {
        public List<bool> Plants { get; private set; }
        public List<Rule> Rules { get; private set; }
        public int FirstIndex { get; private set; } = 0;

        public PlantState(List<bool> plants, List<Rule> rules)
        {
            Plants = plants;
            Rules = rules;
        }

        public void Step()
        {
            var result = new List<bool>();
            var maxIndex = Plants.Count;
            for (int i = -2; i < Plants.Count + 2; i++)
            {
                var change = false;
                foreach (var rule in Rules)
                {
                    if (rule.Match(Plants, i))
                    {
                        change = rule.Pattern[5] ^ Plants.ElementAtOrDefault(i);
                    }
                }

                if (change && i < 0)
                {
                    FirstIndex += i;
                    result.Add(Plants.ElementAtOrDefault(i) ^ change);
                }
                else if (i >= 0)
                {
                    result.Add(Plants.ElementAtOrDefault(i) ^ change);
                }
            }

            // Trim the end
            while (true)
            {
                if (!result.Last())
                {
                    result.RemoveAt(result.Count - 1);
                }
                else
                {
                    break;
                }
            }
            Plants = result;
        }

        public int CountPlants()
        {
            var count = 0;
            for (int i = 0; i < Plants.Count; i++)
            {
                if (Plants[i])
                    count += (i + FirstIndex);
            }
            return count;
        }
    }

    class Rule
    {
        public List<bool> Pattern { get; private set; }

        public Rule(List<bool> pattern)
        {
            Pattern = pattern;
        }

        public bool Match(List<bool> plants, int index)
        {
            var patternMatch = true;

            for (int j = 0; j < 5; j++)
            {
                // two bits don't match
                if (Pattern.ElementAtOrDefault(j) ^ plants.ElementAtOrDefault(index + j - 2))
                {
                    patternMatch = false;
                    break;
                }
            }
            return patternMatch;
        }
    }
}
