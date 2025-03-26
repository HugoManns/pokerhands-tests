namespace CardGame;
public class Card
{
    public char Suit { get; }
    public char Rank { get; }

    public Card(char suit, char rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank}{Suit}";
    }
}
