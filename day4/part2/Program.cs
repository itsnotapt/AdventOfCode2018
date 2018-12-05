using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace day4part2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> events = new List<string>(File.ReadAllLines(@"input.txt"));
            events.Sort(StringComparer.InvariantCulture);

            Regex dateRegex = new Regex(@"^\[(.*)\] (.*)", RegexOptions.Compiled);
            Regex guardIdRegex = new Regex(@"^\[.*\] Guard #(\d{1,4}).*", RegexOptions.Compiled);

            DateTime startTime = DateTime.Now;
            int id = -1;

            Dictionary<int, Guard> guards = new Dictionary<int, Guard>();

            foreach (string e in events)
            {
                DateTime eventTime;
                {
                    Match m = dateRegex.Match(e);
                    eventTime = DateTime.ParseExact(m.Groups[1].Value, "yyyy-MM-dd HH:mm", null);
                }

                if (e.Contains("begins shift"))
                {
                    Match m = guardIdRegex.Match(e);
                    id = Int32.Parse(m.Groups[1].Value);
                }
                else if (e.Contains("falls asleep"))
                {
                    startTime = eventTime;
                }
                else
                {
                    if (!guards.ContainsKey(id))
                    {
                        guards.Add(id, new Guard(id));
                    }
                    guards[id].addEvent(startTime, eventTime);
                }
            }

            int maxTimesAsleep = 0;
            int minute = 0;
            int guardId = 0;
            for (int i = 0; i < 60; i++)
            {
                foreach(Guard guard in guards.Values)
                {
                    int timeAsleep = guard.timesAsleepAtMinute(i);
                    if (timeAsleep > maxTimesAsleep)
                    {
                        minute = i;
                        maxTimesAsleep = timeAsleep;
                        guardId = guard.id;
                    }
                }
            }

            Console.WriteLine("Result: {0}x{1} = {2}", guardId, minute, (guardId * minute));
        }
    }

    class Guard
    {
        private List<Event> events = new List<Event>();
        public int id { get; }

        public Guard(int id)
        {
            this.id = id;
        }

        public void addEvent(DateTime start, DateTime end)
        {
            events.Add(new Event(start, end));
        }

        public int totalMinutes()
        {
            return events.Sum(item => item.minutes());
        }

        public int minuteMostAsleep()
        {
            Dictionary<int, int> minutes = new Dictionary<int, int>();
            foreach (Event e in events)
            {
                for (DateTime date = e.start; date <= e.end.AddMinutes(-1); date = date.AddMinutes(1))
                {
                    int minute = date.Minute;
                    if (!minutes.ContainsKey(minute))
                    {
                        minutes.Add(minute, 0);
                    }
                    minutes[minute] += 1;
                }
            }
            return minutes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        public int timesAsleepAtMinute(int minute)
        {
            int timesAsleep = 0;
            foreach (Event e in events)
            {
                if (minute >= e.start.Minute && minute < e.end.Minute)
                {
                    timesAsleep++;
                }
            }
            return timesAsleep;
        }
    }

    class Event
    {
        public DateTime start { get; }
        public DateTime end { get; }

        public Event(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }

        public int minutes()
        {
            return (int)Math.Round(end.Subtract(start).TotalMinutes);
        }
    }
}
