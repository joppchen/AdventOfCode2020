using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.XPath;

namespace AoC2020.Day7
{
    internal class Solver
    {
        public static void Task1(string[] rules)
        {
            var bags = new List<Bag>();

            foreach (var rule in rules)
            {
                //var colour = rule.Before(" bag");
                var separator = new[] {" bags", " bag", " contain ", ", ", "."};
                var colourAndContents = rule.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                // Record colour of bag
                var colour = colourAndContents[0];
                bags.Add(new Bag(colour));

                // Record contents of bag
                if (colourAndContents[1].Contains("no other")) continue;
                for (var i = 1; i < colourAndContents.Length; i++)
                {
                    // Taking a shortcut here assuming only 1-digit number of contents
                    // TODO: How to handle two-digit number of contents? This whole thing should probably be replaced by regex
                    bags.Last().Contents.Add(colourAndContents[i].Substring(2),
                        int.Parse(colourAndContents[i][0].ToString()));
                }
            }

            const string myBag = "shiny gold";
            var bagsFound = FindBagInBags(myBag, bags);

            bagsFound.RemoveDuplicates();

            Console.WriteLine(bagsFound.Count);
        }

        private static List<Bag> FindBagInBags(string goal, List<Bag> bags)
        {
            var bagsFound = bags.Where(item => item.Contents.ContainsKey(goal)).ToList();

            var validBags = new List<Bag>();
            validBags.AddRange(bagsFound);

            if (bagsFound.Count <= 0) return bagsFound;

            foreach (var bag in bagsFound) validBags.AddRange(FindBagInBags(bag.Colour, bags));

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
        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var enumerable = (IEnumerable<T>) list;

            ICollection<T> withoutDuplicates = new HashSet<T>(enumerable);

            list.Clear();
            list.AddRange(withoutDuplicates.ToList());
        }
    }
}