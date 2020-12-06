using System;
using System.IO;

namespace AoC2020.Day2
{
    internal class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 2:");

            var solver = new solver();

            string textFile = "../../../Day2/input.txt";
            //string textFile ="Day2/inputExample.txt";
            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile);
                //solver.task1(lines);
                solver.task2(lines);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }

    class solver
    {
        public void task1(string[] pwords)
        {
            var length = pwords.Length;
            int count = 0;
            int validPasswords = 0;

            string[] words;
            char[] separators = {'-', ' ', ':'};

            int min;
            int max;
            string letter;
            string pword;

            for (int i = 0; i < length; i++)
            {
                words = pwords[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                min = int.Parse(words[0]);
                max = int.Parse(words[1]);
                letter = words[2];
                pword = words[3];

                count = countTextsInString(pword, letter);
                //Console.WriteLine($"Password: {pword}");

                if (count >= min && count <= max)
                {
                    //Console.WriteLine("Valid password!");
                    validPasswords += 1;
                }
                //else Console.WriteLine("Invalid password!");

                count = 0;
            }

            Console.WriteLine($"Valid passwords: {validPasswords}");
        }

        public void task2(string[] pwords)
        {
            var length = pwords.Length;
            int validPasswords = 0;

            string[] words;
            char[] separators = {'-', ' ', ':'};

            int pos1;
            int pos2;
            string letter;
            string pword;

            for (int i = 0; i < length; i++)
            {
                words = pwords[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                pos1 = int.Parse(words[0]) - 1;
                pos2 = int.Parse(words[1]) - 1;
                letter = words[2];
                pword = words[3];

                int counter = 0;

                if (pos2 <= pword.Length)
                {
                    if (letter.Equals(pword[pos1].ToString())) counter += 1;
                    if (letter.Equals(pword[pos2].ToString())) counter += 1;

                    if (counter == 1) validPasswords += 1;
                }
            }

            Console.WriteLine($"Valid passwords: {validPasswords}");
        }

        private int countTextsInString(string str, string text, int start = 0)
        {
            int index = str.IndexOf(text, start);

            if (index < 0) return 0;
            return 1 + countTextsInString(str, text, index + 1);
        }
    }
}