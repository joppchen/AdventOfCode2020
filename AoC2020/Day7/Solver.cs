using System;
using System.Collections.Generic;
using System.Linq;
using AoC2020.Common;

namespace AoC2020.Day7
{
    internal class Solver
    {
        public static void Task1(string[] rules) // Answer: 131
        {
            var bags = BagsFromRules(rules);

            const string myBag = "shiny gold";
            var bagsFound = FindBagInBags(myBag, bags);

            bagsFound.RemoveDuplicates();

            Console.WriteLine(bagsFound.Count);
        }

        public static void Task2(string[] rules) // Answer: 11261
        {
            var bags = BagsFromRules(rules);

            const string myBag = "shiny gold";

            var bagsFound = FindBagsInBag(myBag, bags); // Includes myBag, exclude it from the count

            Console.WriteLine(bagsFound.Count - 1);
        }

        private static List<Bag> BagsFromRules(string[] rules)
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

            return bags;
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

        private static List<Bag> FindBagsInBag(string myBag, List<Bag> bags)
        {
            var bagsFound = bags.Where(item => item.Colour.Equals(myBag)).ToList();

            var contentsFound = bagsFound[0].Contents;

            if (contentsFound.Count <= 0) return bagsFound;

            foreach (var (key, value) in contentsFound)
            {
                for (var i = 0; i < value; i++)
                {
                    bagsFound.AddRange(FindBagsInBag(key, bags));
                }
            }

            return bagsFound;
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
}