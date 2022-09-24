namespace Aquarium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to amazing aquarium simulator!");
            Aquarium aquarium = new Aquarium();
            Console.Clear();
            aquarium.StartPlay();
        }
    }

    class Aquarium
    {
        private List<Fish> _fishList = new List<Fish>();

        public Aquarium()
        {
            AddFish("Please, enter amount of fish in aquarium:");
        }

        public void StartPlay()
        {
            bool isWorking = true;

            while (isWorking == true && GetAmountOfCorpse() != _fishList.Count)
            {
                const string Exit = "exit";
                const string AppendFish = "add";
                const string DeleteFish = "remove";
                const string Look = "look";

                ShowFish();
                Console.Write($"Press any key to continue...");
                Console.ReadKey(true);

                Console.WriteLine($"\n\nTo add new fish - write \"{AppendFish}\".\nTo remove fish - write \"{DeleteFish}\".\nJust to look - Write \"{Look}\".\nTo leave aquarium - write \"{Exit}\".");
                Console.Write("\nEnter value:");
                string choosenMenu = Console.ReadLine();
                Console.WriteLine();

                switch (choosenMenu.ToLower())
                {
                    case Exit:
                        isWorking = false;
                        break;
                    case AppendFish:
                        AddFish("Please, enter quantity of fish to add:");
                        break;
                    case DeleteFish:
                        RemoveFish();
                        break;
                    case Look:
                        break;
                    default:
                        SpendTime("You did not notice how one fish year has passed. The fish in this aquarium are very charming!");
                        break;
                }

                SpendTime();
                Console.WriteLine($"Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
            }

            if (GetAmountOfCorpse() == _fishList.Count)
            {
                Console.WriteLine("All fish is dead. Game over.");
            }
        }

        private void AddFish(string text)
        {
            int quantity = ReadNumber(text);

            for (int i = 0; i < quantity; i++)
            {
                _fishList.Add(new Fish());
            }

            Console.WriteLine($"{quantity} pcs of fish successfully added.");
        }

        private int ReadNumber(string text = "Please, enter a number:")
        {
            bool isParsed = false;
            int number = 0;

            while (isParsed == false)
            {
                Console.Write(text);
                string value = Console.ReadLine();

                if (int.TryParse(value, out number))
                {
                    isParsed = true;
                }
                else
                {
                    Console.Write("Entered value is not a number. Please, write a number.");
                }
            }

            return number;
        }

        private void ShowFish()
        {
            Console.WriteLine("Alive fish in aquarium:\n");

            for (int i = 0; i < _fishList.Count; i++)
            {
                Console.WriteLine($"Fish ID {_fishList[i].ID}. Health: {_fishList[i].Health}.");
            }

            Console.WriteLine();
        }

        private void SpendTime(string text = "\nBy the way another fish year has passed.")
        {
            Console.WriteLine(text);

            for (int i = 0; i < _fishList.Count; i++)
            {
                _fishList[i].ReduceHealth();

                if (_fishList[i].Health <= 0)
                    Console.WriteLine($"Fish ID {_fishList[i].ID} is dead. Please, remove it from aquarium.");
            }
        }

        private int GetAmountOfCorpse()
        {
            int quantity = 0;

            for (int i = 0; i < _fishList.Count; i++)
            {
                if (_fishList[i].Health <= 0)
                    quantity++;
            }

            return quantity;
        }

        private void RemoveFish()
        {
            int index = ReadNumber("Write fish ID to remove fish:");
            bool iDisFounded = false;

            for (int i = 0; i < _fishList.Count; i++)
            {
                if (_fishList[i].ID == index)
                {
                    iDisFounded = true;
                    Console.WriteLine($"Fish {_fishList[i].ID} was removed");
                    _fishList.RemoveAt(i);
                }
            }

            if (iDisFounded == false)
            {
                Console.WriteLine($"Entered ID was not founded");
            }
        }
    }

    class Fish
    {
        private static int _lastID;
        public int Health { get; private set; }
        public int ID { get; private set; }

        public Fish()
        {
            Health = GenerateNumber();
            ID = GenerateID();
        }

        public void ReduceHealth()
        {
            if (Health > 0)
                Health--;
        }

        private int GenerateNumber(int minNumber = 10, int maxNumber = 31)
        {
            Random random = new Random();
            return random.Next(minNumber, maxNumber);
        }

        private int GenerateID()
        {
            _lastID++;
            return _lastID;
        }
    }
}