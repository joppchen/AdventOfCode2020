using System;
using AoC2020.Common;

namespace AoC2020.Day1
{
    internal static class Solver
    {
        public static void Task1(int[] theList) //Answer: 858496
        {
            var (index1, index2) = SharedMethods.TwoUniqueNumbersInArrayThatSumTo(theList, 2020);
            Console.WriteLine(theList[index1]);
            Console.WriteLine(theList[index2]);
            Console.WriteLine(theList[index1] * theList[index2]);
        }

        public static void Task2(int[] theList) //Answer: 263819430
        {
            int listLength = theList.Length;

            int index1 = -1;
            int index2 = -1;
            int index3 = -1;
            bool foundit = false;

            for (int i = 0; i < listLength; i++)
            {
                for (int j = 1; j < listLength; j++)
                {
                    if (i == j) continue;

                    for (int k = 2; k < listLength; k++)
                    {
                        if (j == k) continue;

                        int sum = theList[i] + theList[j] + theList[k];

                        if (sum == 2020)
                        {
                            index1 = i;
                            index2 = j;
                            index3 = k;
                            foundit = true;
                            break;
                        }
                    }
                }

                if (foundit) break;
            }

            var tall1 = theList[index1];
            var tall2 = theList[index2];
            var tall3 = theList[index3];

            Console.WriteLine(tall1);
            Console.WriteLine(tall2);
            Console.WriteLine(tall3);
            Console.WriteLine(tall1 * tall2 * tall3);
        }
    }
}