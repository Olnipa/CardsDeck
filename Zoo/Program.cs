using System.Diagnostics.Metrics;
using System.Numerics;

namespace Zoo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Aviary> aviaries = new List<Aviary>() 
            {
                new Aviary("Wolf", new List<Animal>() { new Animal("Mark", "auuuu", "male"), new Animal("Jeimy", "auuuu", "female") }),
                new Aviary("Tiger", new List<Animal>() { new Animal("Alex", "rrrr", "male"), new Animal("Jocabet", "rrrr", "female") }),
                new Aviary("Vorons", new List<Animal>() { new Animal("Tom", "aar", "male"), new Animal("Liza", "aar", "female") }),
                new Aviary("Turtle", new List<Animal>() { new Animal("Javier", "\"no sound\"", "male"), new Animal("Lucy", "\"no sound\"", "female") })
            };
            Zoo zoo = new Zoo(aviaries);

            zoo.StartWork();
        }
    }

    class UserUtils
    {
        public static int ReadNumber(string text = "Please, enter a number:")
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
    }

    class Zoo
    {
        private List<Aviary> _aviaries = new List<Aviary>();

        public Zoo(List<Aviary> aviaries)
        {
            _aviaries = aviaries;
        }

        public void ShowAviaries()
        {
            Console.WriteLine("\nList of available aviaries:");

            for (int i = 0; i < _aviaries.Count; i++)
            {
                Console.WriteLine($"Aviary {i + 1}. {_aviaries[i].Name}.");
            }
        }

        public void StartWork()
        {
            bool guestIsActive = true;
            Console.WriteLine("Welcome to the zoo!");

            while (guestIsActive)
            {
                ShowAviaries();
                int choosenAviaryIndex = UserUtils.ReadNumber("\nWrite index of aviary whihch you want to see:") - 1;

                if (choosenAviaryIndex >= 0 && choosenAviaryIndex < _aviaries.Count)
                {
                    Console.WriteLine($"\nYou arrived to aviary of {_aviaries[choosenAviaryIndex].Name}." +
                        $"\nHere lives {_aviaries[choosenAviaryIndex].GetCount()} {_aviaries[choosenAviaryIndex].Name}.");
                    _aviaries[choosenAviaryIndex].ShowAnimalsInfo();
                }
                else
                {
                    Console.WriteLine("Index is out of range.");
                }

                const string Exit = "exit";
                Console.Write($"\nPress any key to continue or write {Exit} to exit...");
                string ChoosenMenu = Console.ReadLine();

                if (ChoosenMenu == Exit.ToLower())
                {
                    guestIsActive = false;
                }

                Console.Clear();
            }
        }
    }

    class Aviary
    {
        private List<Animal> _animals = new List<Animal>();
        public string Name { get; private set; }

        public Aviary(string name, List<Animal> animals)
        {
            Name = name;
            _animals = animals;
        }

        public int GetCount()
        {
            return _animals.Count;
        }

        public void ShowAnimalsInfo()
        {
            Console.WriteLine("\nInformation of animals who live here:");

            for (int i = 0; i < _animals.Count; i++)
            {
                _animals[i].ShowInfo();
            }
        }
    }

    class Animal
    {
        public string Name { get; private set; }
        public string Sound { get; private set; }
        public string Gender { get; private set; }

        public Animal(string name, string sound, string gender)
        {
            Name = name;
            Sound = sound;
            Gender = gender;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Name - {Name}. The sound of this animal - \"{Sound}\". Gender {Gender}");
        }
    }
}