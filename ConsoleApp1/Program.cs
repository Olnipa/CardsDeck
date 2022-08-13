namespace Cards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            List<char> cardsSuit = new List<char>() { '♠', '♥', '♦', '♣' };
            List<string> cardsValue = new List<string>() { "2", "3", "4", "Jack" };
            NPC cardsDeck = new NPC(cardsSuit, cardsValue);
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
                        player.TakeCard(cardsDeck.RemoveCard());
                        break;
                    case ShowDeckCards:
                        cardsDeck.ShowCards();
                        break;
                    case ShaffleDeckCards:
                        cardsDeck.Shuffle();
                        break;
                    case ShowHandCards:
                        player.ShowCards();
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
    
    class NPC : Deck
    {
        public NPC(List<char> cardsSuit, List<string> cardsValues)
        {
            for (int i = 0; i < cardsSuit.Count; i++)
            {
                for (int j = 0; j < cardsValues.Count; j++)
                {
                    _cards.Add(new Card(cardsSuit[i], cardsValues[j]));
                }
            }
        }

        public Card RemoveCard()
        {
            Card removedCard = null;

            if (_cards.Count > 0)
            {
                removedCard = _cards[0];
                _cards.RemoveAt(0);
            }
            else
            {
                Console.WriteLine("\nNo cards in deck!!!");
            }

            return removedCard;
        }

        public void Shuffle()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, _cards.Count);
                Card tempMemory = _cards[randomIndex];
                _cards[randomIndex] = _cards[i];
                _cards[i] = tempMemory;
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

    class Player : Deck
    {
        public void TakeCard(Card takenCard)
        {
            if (takenCard != null)
            {
                _cards.Add(takenCard);
            }
        }
    }

    class Deck
    {
        protected List<Card> _cards = new List<Card>();

        public void ShowCards()
        {
            if (_cards.Count > 0)
            {
                Console.WriteLine();

                for (int i = 0; i < _cards.Count; i++)
                {
                    Console.WriteLine(_cards[i].Suit + _cards[i].Value);
                }
            }
            else
            {
                Console.WriteLine("\nNo cards here!");
            }
        }
    }
}