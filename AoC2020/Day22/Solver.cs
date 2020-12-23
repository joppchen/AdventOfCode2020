using System;
using System.Collections.Generic;
using System.Linq;
using AoC2020.Common;

namespace AoC2020.Day22
{
    internal enum Players
    {
        Undefined = 0,
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

        public static void Task2(string deck) // Answer: 32731
        {
            var (player1, player2) = DealCards(deck);
            var game = 0;

            var (winner, winnerHand) = PlayRecursiveCombat(player1, player2, ref game);
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

            Console.WriteLine();
            Console.WriteLine("== Post-game results ==");
            Console.WriteLine($"Player 1's deck: {string.Join(' ', player1)}");
            Console.WriteLine($"Player 2's deck: {string.Join(' ', player2)}");
            Console.WriteLine($"Task 2: {sum}");
        }

        private static (Players, List<int>) PlayRecursiveCombat(List<int> player1, List<int> player2, ref int game)
        {
            const bool debug = false;
            game++;
            if (debug) Console.WriteLine($"=== Game {game} ===");

            var player1Hands = new List<List<int>>();
            var player2Hands = new List<List<int>>();
            var round = 0;
            var winnerOfGame = Players.Undefined;

            while (player1.Count > 0 && player2.Count > 0)
            {
                round++;
                if (debug)
                {
                    Console.WriteLine($"Round {round} (Game {game})");
                    Console.WriteLine($"Player 1's deck: {string.Join(' ', player1)}");
                    Console.WriteLine($"Player 2's deck: {string.Join(' ', player2)}");
                }

                // Before either player deals a card, if there was a previous round in this game that had exactly
                // the same cards in the same order in the same players' decks, the game instantly ends in a win for player 1.
                if (ExactlySameCardsInPreviousRound(player1, player2, player1Hands, player2Hands))
                {
                    winnerOfGame = Players.Player1;
                    if (debug)
                        Console.WriteLine($"Player 1 wins immediately because Exactly Same Cards in Previous Round");
                    break;
                }

                // Dealt hands so far
                player1Hands.Add(player1.GetRange(0, player1.Count));
                player2Hands.Add(player2.GetRange(0, player2.Count));

                // If both players have at least as many cards remaining in their deck as the value of the card they just
                // drew, the winner of the round is determined by playing a new game of Recursive Combat (see below).
                var p1Card = player1.First();
                var p2Card = player2.First();
                if (debug)
                {
                    Console.WriteLine($"Player 1 plays: {p1Card}");
                    Console.WriteLine($"Player 2 plays: {p2Card}");
                }

                if (p1Card == p2Card) throw new NotImplementedException("Equal playing cards are not implemented.");

                Players winnerOfRound;
                if (player1.Count - 1 >= p1Card && player2.Count - 1 >= p2Card)
                {
                    if (debug)
                    {
                        Console.WriteLine("Playing a sub-game to determine the winner...");
                        Console.WriteLine();
                    }

                    // each player creates a new deck by making a copy of the next cards in their deck
                    // (the quantity of cards copied is equal to the number on the card they drew to trigger the sub-game)
                    var player1Copy = player1.GetRange(1, p1Card);
                    var player2Copy = player2.GetRange(1, p2Card);

                    // During this sub-game, the game that triggered it is on hold and completely unaffected; no cards
                    // are removed from players' decks to form the sub-game. (For example, if player 1 drew the 3 card,
                    // their deck in the sub-game would be copies of the next three cards in their deck.)
                    (winnerOfRound, _) = PlayRecursiveCombat(player1Copy, player2Copy, ref game);
                }
                else winnerOfRound = p1Card > p2Card ? Players.Player1 : Players.Player2;

                if (debug)
                {
                    Console.WriteLine($"{winnerOfRound} wins round {round} of game {game}!");
                    Console.WriteLine();
                }

                if (winnerOfRound == Players.Player1) FinalizeRound(player1, player2);
                else FinalizeRound(player2, player1);

                if (player1.Count != 0 && player2.Count != 0) continue;
                winnerOfGame = player1.Count > player2.Count ? Players.Player1 : Players.Player2;
                game--;
                if (debug)
                    if (game > 0)
                        Console.WriteLine($"...anyway, back to game {game}.");
                return (winnerOfGame, winnerOfGame == Players.Player1 ? player1 : player2);
            }

            if (winnerOfGame == Players.Undefined)
                winnerOfGame = player1.Count > player2.Count ? Players.Player1 : Players.Player2;


            if (debug) Console.WriteLine($"The winner of game {game} is {winnerOfGame}!");

            var winnerHand = winnerOfGame == Players.Player1 ? player1 : player2;

            return (winnerOfGame, winnerHand);
        }

        private static bool ExactlySameCardsInPreviousRound(IEnumerable<int> player1, IEnumerable<int> player2,
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