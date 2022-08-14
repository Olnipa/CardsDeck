namespace Cards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerDeck player1Deck = new PlayerDeck();
            List<char> cardsSuit = new List<char>() { '♠', '♥', '♦', '♣' };
            List<string> cardsValue = new List<string>() { "2", "3", "4", "Jack" };
            TableDeck cardsDeck = new TableDeck(cardsSuit, cardsValue);
            bool isWorking = true;

            while (isWorking)
            {
                const string TakeCard = "1";
                const string ShowDeckCards = "2";
                const string ShaffleDeckCards = "3";
                const string ShowHandCards = "4";
                const string Exit = "0";

                Console.Write($"\nHello! Enter appropriate number:\n{TakeCard} - Take one card\n{ShowDeckCards} - Show cards from deck" +
                    $"\n{ShaffleDeckCards} - Shuffle cards in deck\n{ShowHandCards} - Show cards from hand\n{Exit} - Exit\n\nEntered number: ");
                string choosenMenu = Console.ReadLine();
                
                switch (choosenMenu)
                {
                    case TakeCard:
                        player1Deck.TakeCard(cardsDeck.GiveCard());
                        break;
                    case ShowDeckCards:
                        cardsDeck.ShowCards();
                        break;
                    case ShaffleDeckCards:
                        cardsDeck.Shuffle();
                        break;
                    case ShowHandCards:
                        player1Deck.ShowCards();
                        break;
                    case Exit:
                        isWorking = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    class TableDeck : DefaultDeck
    {
        public TableDeck(List<char> cardSuits, List<string> cardValues)
        {
            for (int i = 0; i < cardSuits.Count; i++)
            {
                for (int j = 0; j < cardValues.Count; j++)
                {
                    Cards.Add(new Card(cardSuits[i], cardValues[j]));
                }
            }
        }

        public Card GiveCard()
        {
            Card givenCard = null;

            if (Cards.Count > 0)
            {
                int givenCardIndex = 0;

                givenCard = Cards[givenCardIndex];
                Cards.RemoveAt(givenCardIndex);
            }
            else
            {
                Console.WriteLine("\nNo cards in deck!!!");
            }

            return givenCard;
        }

        public void Shuffle()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                int minRandomIndex = 0;
                Random random = new Random();
                int randomIndex = random.Next(minRandomIndex, Cards.Count);
                Card tempMemory = Cards[randomIndex];
                Cards[randomIndex] = Cards[i];
                Cards[i] = tempMemory;
            }
        }
    }

    class Card
    {
        public char Suit { get; private set; }
        public string Value { get; private set; }

        public Card(char suit, string value)
        {
            Suit = suit;
            Value = value;
        }
    }

    class PlayerDeck : DefaultDeck
    {
        public void TakeCard(Card takenCard)
        {
            if (takenCard != null)
            {
                Cards.Add(takenCard);
            }
        }
    }

    class DefaultDeck
    {
        protected List<Card> Cards = new List<Card>();

        public void ShowCards()
        {
            if (Cards.Count > 0)
            {
                Console.WriteLine();

                for (int i = 0; i < Cards.Count; i++)
                {
                    Console.WriteLine(Cards[i].Suit + Cards[i].Value);
                }
            }
            else
            {
                Console.WriteLine("\nNo cards here!");
            }
        }
    }
}