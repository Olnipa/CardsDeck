using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Numerics;

namespace Zoo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Zoo zoo = new Zoo();

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

        public Zoo()
        {
            _aviaries = new List<Aviary>()
            {
                new Aviary("Wolf", new List<Animal>() { new Animal("Mark", "auuuu", "male"), new Animal("Jeimy", "auuuu", "female") }),
                new Aviary("Tiger", new List<Animal>() { new Animal("Alex", "rrrr", "male"), new Animal("Jocabet", "rrrr", "female") }),
                new Aviary("Vorons", new List<Animal>() { new Animal("Tom", "aar", "male"), new Animal("Liza", "aar", "female") }),
                new Aviary("Turtle", new List<Animal>() { new Animal("Javier", "\"no sound\"", "male"), new Animal("Lucy", "\"no sound\"", "female") })
            };
        }

        public void StartWork()
        {
            const string ExitFromZoo = "exit";
            bool isGuestActive = true;

            Console.WriteLine("Welcome to the zoo!");

            while (isGuestActive)
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

                Console.Write($"\nPress any key to continue or write {ExitFromZoo} to exit...");
                string choosenMenu = Console.ReadLine();

                if (choosenMenu == ExitFromZoo.ToLower())
                {
                    isGuestActive = false;
                }

                Console.Clear();
            }
        }

        private void ShowAviaries()
        {
            Console.WriteLine("\nList of available aviaries:");

            for (int i = 0; i < _aviaries.Count; i++)
            {
                Console.WriteLine($"Aviary {i + 1}. {_aviaries[i].Name}.");
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