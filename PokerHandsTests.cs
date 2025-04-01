using Xunit;

namespace CardGame.Test
{

    //NOTE: Inte uppdaterad med nya fixade pokerhands
    // Gamla versionen




    public class CardTests
    {
        [Fact]
        public void Card_Creation_ShouldSetSuitAndRank()
        {
            // Arrange
            char suit = '♥';
            char rank = 'A';

            // Act
            Card card = new Card(suit, rank);

            // Assert
            Assert.Equal(suit, card.Suit);
            Assert.Equal(rank, card.Rank);
        }

        [Fact]
        public void Card_ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            Card card = new Card('♦', 'K');

            // Act
            string cardString = card.ToString();

            // Assert
            Assert.Equal("K♦", cardString);
        }
    }

    public class HandTests
    {
        [Fact]
        public void Hand_ShouldCreateWithFiveCards()
        {
            // Arrange
            List<Card> cards = new List<Card>
            {
                new Card('♥', 'A'),
                new Card('♦', 'K'),
                new Card('♣', 'Q'),
                new Card('♠', 'J'),
                new Card('♥', 'T')
            };

            // Act
            Hand hand = new Hand(cards);

            // Assert
            Assert.NotNull(hand);
            Assert.Equal(5, hand.Cards.Count);
        }

        [Fact]
        public void Hand_CreationWithWrongNumberOfCards_ShouldThrowException()
        {
            // Arrange
            List<Card> cards = new List<Card>
            {
                new Card('♥', 'A'),
                new Card('♦', 'K')
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Hand(cards));
        }

        [Fact]
        public void Hand_ToString_ShouldReturnFormattedString()
        {
            // Arrange
            List<Card> cards = new List<Card>
            {
                new Card('♥', 'A'),
                new Card('♦', 'K'),
                new Card('♣', 'Q'),
                new Card('♠', 'J'),
                new Card('♥', 'T')
            };
            Hand hand = new Hand(cards);

            // Act
            string handString = hand.ToString();

            // Assert
            Assert.Contains("A♥", handString);
            Assert.Contains("K♦", handString);
            Assert.Contains("Q♣", handString);
            Assert.Contains("J♠", handString);
            Assert.Contains("T♥", handString);
        }
    }

    public class DeckOfCardsTests
    {
        [Fact]
        public void DeckOfCards_Initialization_ShouldCreate52Cards()
        {
            // Arrange & Act
            DeckOfCards deck = new DeckOfCards();

            
            var deckType = typeof(DeckOfCards);
            var cardsField = deckType.GetField("cards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cards = (List<Card>)cardsField?.GetValue(deck);

            Assert.NotNull(cards);
            Assert.Equal(52, cards.Count);
        }

        [Fact]
        public void DealHand_ShouldRemoveCardsFromDeck()
        {
            // Arrange
            DeckOfCards deck = new DeckOfCards();

            // Act
            Hand hand = deck.DealHand();

            // Assert
            var deckType = typeof(DeckOfCards);
            var cardsField = deckType.GetField("cards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cards = (List<Card>)cardsField?.GetValue(deck);

            Assert.Equal(47, cards.Count);
        }

        [Fact]
        public void DealHand_MultipleCalls_ShouldNotRepeatCards()
        {
            // Arrange
            DeckOfCards deck = new DeckOfCards();

            // Act
            Hand hand1 = deck.DealHand();
            Hand hand2 = deck.DealHand();

            // Assert
            // Check that no cards are repeated between hands
            var allCards = hand1.Cards.Concat(hand2.Cards);
            var uniqueCards = allCards.Distinct().ToList();
            Assert.Equal(allCards.Count(), uniqueCards.Count);
        }

        [Fact]
        public void DealHand_WhenNotEnoughCards_ShouldThrowException()
        {
            // Arrange
            DeckOfCards deck = new DeckOfCards();

            // Act & Assert
            // Deal out most of the deck
            for (int i = 0; i < 10; i++)
            {
                deck.DealHand();
            }

            Assert.Throws<InvalidOperationException>(() =>
            {
                // Try to deal more hands than possible
                for (int i = 0; i < 10; i++)
                {
                    deck.DealHand();
                }
            });
        }
    }

    public class CompareHandsTests
    {
        [Fact]
        public void CheckHands_ShouldNotReturnNull()
        {
            // Arrange
            DeckOfCards deck = new DeckOfCards();
            Hand hand1 = deck.DealHand();
            Hand hand2 = deck.DealHand();

            // Act
            var result = CompareHands.CheckHands(hand1, hand2);

            // Assert
            Assert.NotNull(result.winningHand);
            Assert.False(string.IsNullOrEmpty(result.handType));
        }

        [Fact]
        public void CompareHighestCard_ShouldReturnCorrectHand()
        {
            // Arrange
            List<Card> higherHand = new List<Card>
            {
                new Card('♥', 'A'),
                new Card('♦', 'K'),
                new Card('♣', 'Q'),
                new Card('♠', 'J'),
                new Card('♥', 'T')
            };

            List<Card> lowerHand = new List<Card>
            {
                new Card('♥', '9'),
                new Card('♦', '8'),
                new Card('♣', '7'),
                new Card('♠', '6'),
                new Card('♥', '5')
            };

            Hand hand1 = new Hand(higherHand);
            Hand hand2 = new Hand(lowerHand);

            // Act
            Hand winner = CompareHands.CompareHighestCard(hand1, hand2);

            // Assert
            Assert.Equal(hand1, winner);
        }
    }

    public class PokerHandTests
    {
        // Test methods for individual hand identification

        [Fact]
        public void TestIsPair_ValidPair_ReturnsHand()
        {
            // Arrange
            var pairHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsPair(pairHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pairHand, result);
        }

        [Fact]
        public void TestIsPair_NoPair_ReturnsNull()
        {
            // Arrange
            var noPairHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '6'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsPair(noPairHand);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestIsTwoPair_ValidTwoPair_ReturnsHand()
        {
            // Arrange
            var twoPairHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♦', '4'),
            new Card('♣', '4'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsTwoPair(twoPairHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(twoPairHand, result);
        }

        [Fact]
        public void TestIsTwoPair_NoTwoPair_ReturnsNull()
        {
            // Arrange
            var noPairHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '6'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsTwoPair(noPairHand);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestIsThreeOfAKind_ValidThreeOfAKind_ReturnsHand()
        {
            // Arrange
            var threeOfAKindHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♦', '5'),
            new Card('♣', '4'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsThreeOfAKind(threeOfAKindHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(threeOfAKindHand, result);
        }

        [Fact]
        public void TestIsStraight_ValidStraight_ReturnsHand()
        {
            // Arrange
            var straightHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '6'),
            new Card('♦', '7'),
            new Card('♣', '8'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsStraight(straightHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(straightHand, result);
        }

        [Fact]
        public void TestIsFlush_ValidFlush_ReturnsHand()
        {
            // Arrange
            var flushHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♣', '6'),
            new Card('♣', '7'),
            new Card('♣', '8'),
            new Card('♣', '9')
        });

            // Act
            var result = CompareHands.IsFlush(flushHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(flushHand, result);
        }

        [Fact]
        public void TestIsFullHouse_ValidFullHouse_ReturnsHand()
        {
            // Arrange
            var fullHouseHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♦', '5'),
            new Card('♣', '9'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsFullHouse(fullHouseHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fullHouseHand, result);
        }

        [Fact]
        public void TestIsFourOfAKind_ValidFourOfAKind_ReturnsHand()
        {
            // Arrange
            var fourOfAKindHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♦', '5'),
            new Card('♥', '5'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.IsFourOfAKind(fourOfAKindHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fourOfAKindHand, result);
        }

        [Fact]
        public void TestIsStraightFlush_ValidStraightFlush_ReturnsHand()
        {
            // Arrange
            var straightFlushHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♣', '6'),
            new Card('♣', '7'),
            new Card('♣', '8'),
            new Card('♣', '9')
        });

            // Act
            var result = CompareHands.IsStraightFlush(straightFlushHand);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(straightFlushHand, result);
        }

        // Tests for CheckHands method

        [Fact]
        public void TestCheckHands_PairVsTwoPair_TwoPairWins()
        {
            // Arrange
            var pairHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            var twoPairHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♦', '4'),
            new Card('♣', '4'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.CheckHands(pairHand, twoPairHand);

            // Assert
            Assert.Equal(twoPairHand, result.winningHand);
            Assert.Equal("Two Pair", result.handType);
        }

        [Fact]
        public void TestCheckHands_RoyalFlushVsFourOfAKind_RoyalFlushWins()
        {
            var royalFlushHand = new Hand(new List<Card>
        {
            new Card('♣', 'T'),
            new Card('♣', 'J'),
            new Card('♣', 'Q'),
            new Card('♣', 'K'),
            new Card('♣', 'A')
        });

            var fourOfAKindHand = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♦', '5'),
            new Card('♥', '5'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.CheckHands(royalFlushHand, fourOfAKindHand);

            // Assert
            Assert.Equal(royalFlushHand, result.winningHand);
            Assert.Equal("Royal Flush", result.handType);
        }

        [Fact]
        public void TestCheckHands_SameHandType_HighCardDeterminesWinner()
        {
            // Arrange
            var hand1 = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            var hand2 = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', 'T')
        });

            // Act
            var result = CompareHands.CheckHands(hand1, hand2);

            // Assert
            Assert.Equal(hand2, result.winningHand);
            Assert.Equal("Pair", result.handType);
        }

        [Fact]
        public void TestCheckHands_IdenticalHands_ReturnsNull()
        {
            // Arrange
            var hand1 = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            var hand2 = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.CheckHands(hand1, hand2);

            // Assert
            Assert.NotNull(result.winningHand);
        }
    
    
        [Fact]
        public void TestCheckHandsTwoBiggerPar()
        {
            // Arrange
            var hand1 = new Hand(new List<Card>
        {
            new Card('♣', '5'),
            new Card('♠', '5'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            var hand2 = new Hand(new List<Card>
        {
            new Card('♣', '6'),
            new Card('♠', '6'),
            new Card('♠', '4'),
            new Card('♣', '7'),
            new Card('♠', '9')
        });

            // Act
            var result = CompareHands.CheckHands(hand1, hand2);

            // Assert
            Assert.NotNull(result.winningHand);
        }
    }
}
