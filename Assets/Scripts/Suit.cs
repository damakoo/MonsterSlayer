using System;

public enum Suit
{
    Hearts = 0,
    Diamonds = 1,
    Clubs = 2,
    Spades = 3,
}
public static class SuitExtensions
{
    public static string GetColor(this Suit suit)
    {
        switch (suit)
        {
            case Suit.Hearts:
            case Suit.Diamonds:
                return "Red";
            case Suit.Clubs:
            case Suit.Spades:
                return "Black";
            default:
                throw new ArgumentOutOfRangeException(nameof(suit), suit, null);
        }
    }
}