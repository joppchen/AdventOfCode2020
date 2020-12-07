using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace AoC2020.Day7
{
    internal class Solver
    {
        public static void Task1(string[] rules)
        {
            //var groups = rules.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine(groups[1]);

            foreach (var rule in rules)
            {
                Console.WriteLine(rule);
            }

            var bags = new List<Bag>();
            // For each rule:
            bags.Add(new Bag("light red"));
            bags[0].Contents.Add("bright white", 1);
            bags[0].Contents.Add("muted yellow", 2);
            
            bags.Add(new Bag("dark orange"));
            bags[1].Contents.Add("bright white", 3);
            bags[1].Contents.Add("muted yellow", 4);
            
            bags.Add(new Bag("bright white"));
            bags[2].Contents.Add("shiny gold", 1);
            
            bags.Add(new Bag("muted yellow"));
            bags[3].Contents.Add("shiny gold", 2);
            bags[3].Contents.Add("faded blue", 9);
            
            bags.Add(new Bag("shiny gold"));
            bags[4].Contents.Add("dark olive", 1);
            bags[4].Contents.Add("vibrant plum", 2);

            var myBag = "shiny gold";
            var bagsFound = FindBagInBags(myBag, bags);
            
            
            ICollection<Bag> withoutDuplicates = new HashSet<Bag>(bagsFound);
            bagsFound.RemoveDuplicates<Bag>();
            
            Console.WriteLine(withoutDuplicates.Count);
            Console.WriteLine(bagsFound.Count);
        }

        private static List<Bag> FindBagInBags(string goal, List<Bag> bags)
        {
            var validBags = new List<Bag>();
            
            var bagsFound = bags.Where(item => item.Contents.ContainsKey(goal)).ToList();
            //validBags = bagsFound;
            validBags.AddRange(bagsFound);
            if (bagsFound.Count <= 0) return bagsFound;
            
            foreach (var bag in bagsFound)
            {
                //hits += FindBagInBags(bag.Colour, bags);
                validBags.AddRange(FindBagInBags(bag.Colour, bags));
                
            }
            return validBags;
        }
    }

    internal class Bag
    {
        internal string Colour { get; }

        internal Dictionary<string, int> Contents { get; }

        internal Bag(string colour)
        {
            Colour = colour;
            Contents = new Dictionary<string, int>();
        }
    }

    internal static class Extensions
    {
        public static void Populate<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; ++i)
            {
                arr[i] = value;
            }
        }
        
        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var enumerable = (IEnumerable<T>)list;
            
            ICollection<T> withoutDuplicates = new HashSet<T>(enumerable);

            list.Clear();
            list.AddRange(withoutDuplicates.ToList());
        }
    }
}