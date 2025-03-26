using System.Collections.Generic;

namespace CardGame;

public class Hand
{
    public List<Card> Cards { get; }

    public Hand(List<Card> cards)
    {
        if (cards.Count != 5)
        {
            throw new ArgumentException("A hand must contain exactly 5 cards.");
        }

        Cards = cards;
    }

    public override string ToString()
    {
        return string.Join(", ", Cards);
    }
}
