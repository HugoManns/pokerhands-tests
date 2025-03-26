using System;
using System.Collections.Generic;

namespace CardGame;

public class DeckOfCards
{
    private List<Card> cards;
    private Random rng;

    public DeckOfCards()
    {
        cards = new List<Card>();
        rng = new Random();
        InitializeDeck();
        Shuffle();
    }

    private void InitializeDeck()
    {
        foreach (char suit in "♥♦♣♠")
        {
            foreach (char rank in "23456789TJQKA")
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    private void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }

    public Hand DealHand()
    {
        if (cards.Count < 5)
        {
            throw new InvalidOperationException("Not enough cards in the deck to deal a hand.");
        }

        Hand hand = new Hand(cards.GetRange(0, 5));
        cards.RemoveRange(0, 5);
        return hand;
    }
}
