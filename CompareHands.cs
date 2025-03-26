using System.Text.RegularExpressions;

namespace CardGame
{
    public class CompareHands
    {

        // SHOULD PROBABLY BE REFACTORED TO USE A POINT SYSTEM TO DETERMINE WINNER INSTEAD OF STRING COMPARISON - TODO - THINK ABOUT THIS

        public static (Hand winningHand, string handType) CheckHands(Hand hand1, Hand hand2)
        {
            var hands = new[] { hand1, hand2 };
            var methods = new Func<Hand, Hand>[] { IsRoyalFlush, IsStraightFlush, IsFourOfAKind, IsFullHouse, IsFlush, IsStraight, IsThreeOfAKind, IsTwoPair, IsPair };
            foreach (var method in methods)
            {
                foreach (var hand in hands)
                {
                    if (method(hand) != null)
                    {
                        var handType = Regex.Replace(
                                method.Method.Name[2..], @"([A-Z])", " $1"
                            )[1..].Replace("Of A", "of a");
                        return (hand, handType);
                    }
                }
            }
            return (CompareHighestCard(hand1, hand2), "High Card"); // No special hand, highest card wins
        }

        // This is a helper method to get the hand type as a string
        /*private static string GetHandType(Func<Hand, Hand> method)
        {
            if (method == IsRoyalStraightFlush)
            {
                return "Royal Straight Flush";
            }
            else if (method == IsRoyalFlush)
            {
                return "Royal Flush";
            }
            else if (method == IsStraightFlush)
            {
                return "Straight Flush";
            }
            else if (method == IsFourOfAKind)
            {
                return "Four of a Kind";
            }
            else if (method == IsFullHouse)
            {
                return "Full House";
            }
            else if (method == IsFlush)
            {
                return "Flush";
            }
            else if (method == IsStraight)
            {
                return "Straight";
            }
            else if (method == IsThreeOfAKind)
            {
                return "Three of a Kind";
            }
            else if (method == IsTwoPair)
            {
                return "Two Pair";
            }
            else if (method == IsPair)
            {
                return "Pair";
            }
            else
            {
                return "Unknown"; // Should never happen
            }
    }*/

        // This is a helper method to compare the highest card in each hand
        public static Hand CompareHighestCard(Hand hand1, Hand hand2)
        {
            var ranks = "23456789TJQKA";
            var sorted1 = hand1.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            var sorted2 = hand2.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            for (var i = 4; i >= 0; i--)
            {
                if (ranks.IndexOf(sorted1[i].Rank) > ranks.IndexOf(sorted2[i].Rank))
                {
                    return hand1;
                }
                if (ranks.IndexOf(sorted1[i].Rank) < ranks.IndexOf(sorted2[i].Rank))
                {
                    return hand2;
                }
            }
            return null!;
        }

        // Method to check if a hand is a pair
        public static Hand IsPair(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 2)
                {
                    return hand;
                }
            }
            return null!;
        }

        // At this moment this method is broken. It returns two pairs if there is one pair in each hand....TODO NEEDS FIX
        // This is kinda working now...
        public static Hand IsTwoPair(Hand hand)
        {
            var pair1 = 0;
            var pair2 = 0;
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 2)
                {
                    if (pair1 == 0)
                    {
                        pair1 = card.Rank;
                    }
                    else if (pair2 == 0 && card.Rank != pair1)
                    {
                        pair2 = card.Rank;
                    }

                    if (pair1 != 0 && pair2 != 0)
                    {
                        return hand;
                    }
                }
            }
            return null!;
        }

        // Method to check if a hand is three of a kind
        public static Hand IsThreeOfAKind(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 3)
                {
                    return hand;
                }
            }
            return null!;
        }

        // Method to check if a hand is a straight
        public static Hand IsStraight(Hand hand)
        {
            var ranks = "23456789TJQKA";
            var sorted = hand.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            for (var i = 0; i < sorted.Count - 1; i++)
            {
                if (ranks.IndexOf(sorted[i + 1].Rank) - ranks.IndexOf(sorted[i].Rank) != 1)
                {
                    return null!;
                }
            }
            return hand;
        }

        // Method to check if a hand is a flush
        public static Hand IsFlush(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Suit == card.Suit) == 5)
                {
                    return hand;
                }
            }
            return null!;
        }

        // Method to check if a hand is a full house
        public static Hand IsFullHouse(Hand hand)
        {
            return IsPair(hand) != null && IsThreeOfAKind(hand) != null ? hand : null!;
        }

        public static Hand IsFourOfAKind(Hand hand)
        {
            foreach (var card in hand.Cards)
            {
                if (hand.Cards.Count(c => c.Rank == card.Rank) == 4)
                {
                    return hand;
                }
            }
            return null!;
        }

        // Method to check if a hand is a straight flush
        public static Hand IsStraightFlush(Hand hand)
        {
            return IsStraight(hand) != null && IsFlush(hand) != null ? hand : null!;
        }

        // Need to fix this method - TODO
        public static Hand IsRoyalFlush(Hand hand)
        {
            if (IsFlush(hand) == null)
            {
                return null!;
            }
            var ranks = "TJQKA";
            var sorted = hand.Cards.OrderBy(c => ranks.IndexOf(c.Rank)).ToList();
            for (var i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].Rank != ranks[i])
                {
                    return null!;
                }
            }
            return hand;
        }

        // Need to fix this method - TODO
        public static Hand IsRoyalStraightFlush(Hand hand)
        {
            return IsRoyalFlush(hand) != null && IsStraightFlush(hand) != null ? hand : null!;
        }
    }
}
