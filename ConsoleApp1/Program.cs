namespace Cards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserHand player = new UserHand();
            List<char> cardSuit = new List<char>() { '♠', '♥', '♦', '♣' };
            List<string> cardValue = new List<string>() { "2", "3", "4", "Jack" };
            List<Card> cards = new List<Card>();
            bool isWorking = true;
            Card removedCard;

            for (int i = 0; i < cardSuit.Count; i++)
            {
                for (int j = 0; j < cardValue.Count; j++)
                {
                    cards.Add(new Card(cardSuit[i], cardValue[j]));
                }
            }    
            
            Deck cardsDeck = new Deck(cards);

            while (isWorking)
            {
                Console.Write("\nHello! Enter appropriate number:\n1 - Take one card\n2 - Show cards from deck\n3 - Shuffle cards in deck\n4 - Show cards from hand\n0 - Exit\n\nEntered value: ");
                string choosenMenu = Console.ReadLine();
                
                switch (choosenMenu)
                {
                    case "1":
                        player.TakeCard(cardsDeck.RemoveCard());
                        break;
                    case "2":
                        cardsDeck.ShowCards();
                        break;
                    case "3":
                        cardsDeck.Shuffle();
                        break;
                    case "4":
                        player.ShowCards();
                        break;
                    case "0":
                        isWorking = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    class Deck
    {
        private List<Card> _cardsDeck = new List<Card>();

        public Deck(List<Card> cardsDeck)
        {
            _cardsDeck = cardsDeck;
        }

        public Card RemoveCard()
        {
            Card removedCard = null;

            if (_cardsDeck.Count > 0)
            {
                removedCard = _cardsDeck[0];
                _cardsDeck.RemoveAt(0);
            }
            else
            {
                Console.WriteLine("\nNo cards in deck!!!");
            }

            return removedCard;
        }

        public void Shuffle()
        {
            for (int i = 0; i < _cardsDeck.Count; i++)
            {
                Random random = new Random();
                int randomCard = random.Next(0, _cardsDeck.Count);
                Card tempMemory = _cardsDeck[randomCard];
                _cardsDeck[randomCard] = _cardsDeck[i];
                _cardsDeck[i] = tempMemory;
            }
        }

        public void ShowCards()
        {
             if (_cardsDeck.Count > 0)
            {
                Console.WriteLine();

                for (int i = 0; i < _cardsDeck.Count; i++)
                {
                    Console.WriteLine(_cardsDeck[i].Suit + _cardsDeck[i].Value);
                }
            }
            else
            {
                Console.WriteLine("\nNo cards in deck!!!");
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

    class UserHand
    {
        private List<Card> _takenCards = new List<Card>();

        public void TakeCard(Card takenCard)
        {
            if (takenCard != null)
            {
                _takenCards.Add(takenCard);
            }
        }

        public void ShowCards()
        {
            if (_takenCards.Count > 0)
            {
                Console.WriteLine();

                for (int i = 0; i < _takenCards.Count; i++)
                {
                    Console.WriteLine(_takenCards[i].Suit + _takenCards[i].Value);
                }
            }
            else
            {
                Console.WriteLine("\nNo cards in hand!!!");
            }
        }
    }
}