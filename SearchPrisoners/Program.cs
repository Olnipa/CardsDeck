using System.Linq;

namespace SearchPrisoners
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrisonersData prisonersData = new PrisonersData();
            prisonersData.StartWork();
        }
    }

    enum Nationality
    {
        Russian,
        Belarus,
        Ukrainian
    }

    class PrisonersData
    {
        private List<Prisoner> _prisoners = new List<Prisoner>();

        public PrisonersData()
        {
            _prisoners.AddRange(new List<Prisoner>() 
            { 
                new Prisoner("Bill", false, 170, 90, Nationality.Russian),
                new Prisoner("Jose", false, 150, 110, Nationality.Ukrainian),
                new Prisoner("Diego", false, 190, 85, Nationality.Belarus),
                new Prisoner("Iosif", true, 180, 90, Nationality.Russian)
            });
        }

        public void StartWork()
        {
            bool searchNotFinished = true;
            Console.WriteLine("Welcome to Prisoner Data Base\n");
            List<Prisoner> filteredPrisoners;

            while (searchNotFinished)
            {
                const string StartFilters = "1";
                const string Exit = "0";
                Console.Write($"Choose the menu:\n\"{StartFilters}\" - Start filter\n\"{Exit}\" - Exit\n" +
                    $"\nChoosen menu:");
                string choosenMenu = Console.ReadLine();

                switch (choosenMenu)
                {
                    case StartFilters:
                        StartFilter();
                        break;
                    case Exit:
                        searchNotFinished = false;
                        break;
                    default:
                        break;
                }

                if (searchNotFinished == false)
                {
                    Console.WriteLine("Goodbye!");
                }
            }
        }

        private void StartFilter()
        {
            List<Prisoner> filteredPrisoners = _prisoners.Where(_prisoners => _prisoners.IsArrested == false).ToList();
            filteredPrisoners = FilterByHeight(FilterByWeight(FilterByNationalities(filteredPrisoners))).ToList();

            if (filteredPrisoners.Count > 0)
            {
                Console.WriteLine("\nList of filtered prisoners:");
                ShowPrisoners(filteredPrisoners);
                Console.Write("\nPress any key to start new filter...");
            }
            else
            {
                Console.Write("\nNo prisonerers, matching your filter. Press any key to start new filter...");
            }

            Console.ReadKey(true);
            Console.Clear();
        }

        private void ShowNationalities()
        {
            int nationalitiesCount = Enum.GetValues(typeof(Nationality)).Cast<int>().Max() + 1;
            
            for (int i = 0; i < nationalitiesCount; i++)
            {
                Console.WriteLine($"{i + 1}. {(Nationality) i}");
            }
        }

        private void ShowPrisoners(List<Prisoner> prisoners)
        {
            for (int i = 0; i < prisoners.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\t{prisoners[i].Name}.\tHeight - {prisoners[i].Height}. Weight - {prisoners[i].Weight}. Nationality - {prisoners[i].Nationality}.");
            }
        }

        private List<Prisoner> FilterByNationalities(List<Prisoner> filteredPrisoners)
        {
            Console.WriteLine("\nList of Nationalities:");
            ShowNationalities();
            int nationalityIndex = UserUtils.ReadNumber("\nEnter a nationality index:") - 1;
            Console.WriteLine();
            return filteredPrisoners.Where(_prisoners => _prisoners.Nationality == (Nationality)nationalityIndex).ToList();
        }

        private List<Prisoner> FilterByWeight(List<Prisoner> filteredPrisoners)
        {
            int minWeight = UserUtils.ReadNumber("Enter min prisoner weight:");
            int maxWeight = UserUtils.ReadNumber("Enter max prisoner weight:");
            return filteredPrisoners.Where(_prisoners => _prisoners.Weight >= minWeight && _prisoners.Weight <= maxWeight).ToList();
        }

        private List<Prisoner> FilterByHeight(List<Prisoner> filteredPrisoners)
        {
            int minHeight = UserUtils.ReadNumber("Enter min prisoner height:");
            int maxHeight = UserUtils.ReadNumber("Enter max prisoner height:");
            return filteredPrisoners.Where(_prisoners => _prisoners.Height >= minHeight && _prisoners.Height <= maxHeight).ToList();
        }
    }

    class Prisoner
    {
        public string Name { get; private set; }
        public bool IsArrested { get; private set; }
        public int Height { get; private set; }
        public int Weight { get; private set; }
        public Nationality Nationality { get; private set; }

        public Prisoner(string name, bool isArrested, int height, int weight, Nationality nationality)
        {
            Name = name;
            IsArrested = isArrested;
            Height = height;
            Weight = weight;
            Nationality = nationality;
        }
    }

    class UserUtils
    {
        public static int ReadNumber(string text = "Enter a number:")
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
                    Console.Write("Entered value is not a number. Please, write a number.\n");
                }
            }

            return number;
        }
    }
}