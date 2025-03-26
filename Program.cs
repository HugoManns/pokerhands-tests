// Pokerhands Program for unit testing purposes

using System;
using CardGame;

class Program
{
    static void Main(string[] args)
    {

        while (true)
        {
            // Create a new deck of cards
            DeckOfCards deck = new DeckOfCards();
            Console.WriteLine("Deck created and shuffled.");

            // Deal two hands
            Hand hand1 = deck.DealHand();
            Hand hand2 = deck.DealHand();
            Console.WriteLine("Hands dealt.");
            Console.WriteLine("Hand 1: " + hand1);
            Console.WriteLine("Hand 2: " + hand2);

            // Compare hands to determine the winner
            var winner = CompareHands.CheckHands(hand1, hand2);

            if (winner.winningHand == hand1)
            {
                Console.WriteLine("Hand 1 wins!");
            }
            else if (winner.winningHand == hand2)
            {
                Console.WriteLine("Hand 2 wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
            //Console.WriteLine("Winner: " + winner.winningHand);
            Console.WriteLine("Hand Type: " + winner.handType);
            //Console.WriteLine(CompareHands.CheckHands(hand1, hand2));
            //Console.WriteLine(winner == hand1 ? "Hand 1 wins!" : winner == hand2 ? "Hand 2 wins!" : "It's a tie!");


            // Print the result
            /*if (winner == null)
            {
                Console.WriteLine("It's a tie!");
                Console.WriteLine(hand1); Console.WriteLine(hand2);
            }
            else
            {
                Console.WriteLine($"Hand {(winner == hand1 ? "1" : "2")} wins!");

                Console.WriteLine(winner);
            }*/
            Console.WriteLine("Press Enter to play again, or any other key to exit.");
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Thanks for playing!");
                break;
            }
        }
    }
}