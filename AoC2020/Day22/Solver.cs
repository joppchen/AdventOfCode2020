using System;
using System.Collections.Generic;
using System.Linq;
using AoC2020.Common;

namespace AoC2020.Day22
{
    internal enum Players
    {
        Player1 = 1,
        Player2 = 2
    }

    internal static class Solver
    {
        public static void Task1(string deck) // Answer: 34005
        {
            var (player1, player2) = DealCards(deck);

            while (player1.Count > 0 && player2.Count > 0)
            {
                if (player1.First() > player2.First()) FinalizeRound(player1, player2);
                else FinalizeRound(player2, player1);
            }

            Console.WriteLine($"Task 1: {Sum(WinnerHand(player1, player2))}");
            Console.WriteLine($"The winner is: {WinnerOfGame(player1, player2)}"!);
        }

        public static void Task2(string deck) // Answer: 32731
        {
            var (player1, player2) = DealCards(deck);

            var (winner, winnerHand) = PlayRecursiveCombat(player1, player2);

            Console.WriteLine($"Task 2 result: {Sum(winnerHand)}");
            Console.WriteLine($"The winner is: {winner}"!);
            Console.WriteLine($"Player 1's deck: {string.Join(' ', player1)}");
            Console.WriteLine($"Player 2's deck: {string.Join(' ', player2)}");
        }

        private static int Sum(List<int> winnerHand)
        {
            var sum = 0;
            for (var i = 1; i <= winnerHand.Count; i++)
            {
                sum += i * winnerHand[^i];
            }

            return sum;
        }

        private static (Players winner, List<int> winnerHand) PlayRecursiveCombat(List<int> player1, List<int> player2)
        {
            var allPlayer1Decks = new List<List<int>>();
            var allPlayer2Decks = new List<List<int>>();

            while (player1.Count > 0 && player2.Count > 0)
            {
                if (Player1WinsImmediately(player1, player2, allPlayer1Decks, allPlayer2Decks))
                    return (winner: Players.Player1, winnerHand: player1);

                allPlayer1Decks.Add(player1.GetRange(0, player1.Count));
                allPlayer2Decks.Add(player2.GetRange(0, player2.Count));

                UpdatePlayerCards(player1, player2);
            }

            return (WinnerOfGame(player1, player2), WinnerHand(player1, player2));
        }

        private static void UpdatePlayerCards(List<int> player1, List<int> player2)
        {
            if (WinnerOfRecursiveCombat(player1, player2) == Players.Player1) FinalizeRound(player1, player2);
            else FinalizeRound(player2, player1);
        }

        private static List<int> WinnerHand(List<int> player1, List<int> player2)
        {
            return WinnerOfGame(player1, player2) == Players.Player1 ? player1 : player2;
        }

        private static Players WinnerOfGame(List<int> player1, List<int> player2)
        {
            if (player1.Count > 0 && player2.Count > 0)
                throw new Exception("There is no winner if both players have cards left!");
            return player1.Count > player2.Count ? Players.Player1 : Players.Player2;
        }

        private static Players WinnerOfRecursiveCombat(List<int> player1, List<int> player2)
        {
            var p1Card = player1.First();
            var p2Card = player2.First();
            if (p1Card == p2Card) throw new NotImplementedException("Equal playing cards are not implemented.");

            Players winner;
            if (player1.Count - 1 >= p1Card && player2.Count - 1 >= p2Card) // Decide winner of round by Recursive Combat
            {
                (winner, _) = PlayRecursiveCombat(player1.GetRange(1, p1Card),
                    player2.GetRange(1, p2Card));
            }
            else winner = p1Card > p2Card ? Players.Player1 : Players.Player2; // Play normal Combat

            return winner;
        }

        /// <summary>
        /// Before either player deals a card, if there was a previous round in this game that had exactly
        /// the same cards in the same order in the same players' decks, the game instantly ends in a win for player 1.
        /// </summary>
        private static bool Player1WinsImmediately(IEnumerable<int> player1, IEnumerable<int> player2,
            IReadOnlyCollection<List<int>> player1Hands, IEnumerable<List<int>> player2Hands)
        {
            if (player1Hands.Count == 0) return false;
            return player1Hands.Any(player1.SequenceEqual) || player2Hands.Any(player2.SequenceEqual);
        }

        private static (List<int>, List<int>) DealCards(string deck)
        {
            var hands = deck.Split("\r\nP");
            var temp1 = hands[0].Split("\r\n");
            var player1 = temp1.RangeSubset(1, temp1.Length - 2).Select(int.Parse).ToList();
            var temp2 = hands[1].Split("\r\n");
            var player2 = temp2.RangeSubset(1, temp2.Length - 1).Select(int.Parse).ToList();
            return (player1, player2);
        }

        private static void FinalizeRound(IList<int> winner, IList<int> loser)
        {
            if (winner.First() == loser.First())
                throw new NotImplementedException("Equal cards played by both players is not implemented.");

            winner.Add(winner.First());
            winner.Add(loser.First());
            winner.RemoveAt(0);
            loser.RemoveAt(0);
        }
    }
}