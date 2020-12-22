using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AoC2020.Common;

namespace AoC2020.Day22
{
    internal static class Solver
    {
        public static void Task1(string deck) // Answer: 34005
        {
            var hands = deck.Split("\r\nP");
            var temp1 = hands[0].Split("\r\n");
            var player1 = temp1.RangeSubset(1, temp1.Length - 2).Select(int.Parse).ToList();
            var temp2 = hands[1].Split("\r\n");
            var player2 = temp2.RangeSubset(1, temp2.Length - 1).Select(int.Parse).ToList();

            while (player1.Count > 0 && player2.Count > 0)
            {
                if (player1.First() > player2.First()) FinalizeRound(player1, player2);
                else FinalizeRound(player2, player1); // Equal cards handled by FinalizeRound
            }

            var winner = player1.Count > player2.Count ? "Player 1" : "Player 2";
            var winnerHand = player1.Count > player2.Count ? player1 : player2;
            Console.WriteLine($"The winner is: {winner}"!);

            var sum = 0;
            for (var i = 0; i < winnerHand.Count; i++)
            {
                var j = winnerHand.Count - (i + 1);
                checked
                {
                    sum += (i + 1) * winnerHand[j];
                }
            }

            Console.WriteLine($"Task 1: {sum}");
        }

        private static void FinalizeRound(List<int> winner, List<int> loser)
        {
            if (winner.First() == loser.First())
            {
                throw new NotImplementedException("Equal cards played by both players is not implemented.");
            }

            winner.Add(winner.First());
            winner.Add(loser.First());
            winner.RemoveAt(0);
            loser.RemoveAt(0);
        }
    }
}