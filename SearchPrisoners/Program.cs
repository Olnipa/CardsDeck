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
            bool SearchNotFinished = true;
            List<Prisoner> filteredPrisoners = new List<Prisoner>();
            filteredPrisoners.AddRange(_prisoners);
            Console.WriteLine("Welcome to Prisoner Data Base\n");

            while (SearchNotFinished)
            {
                const string Exit = "0";
                const string Weight = "1";
                const string Height = "2";
                const string Nation = "3";
                const string ResetFilters = "4";
                string choosenMenu;

                if (filteredPrisoners.Count == 0)
                    filteredPrisoners.AddRange(_prisoners);

                Console.Write($"Choose the filter:\n\"{Weight}\" - by Weight\n\"{Height}\" - by Height\n" +
                    $"\"{Nation}\" - by Nation\n\"{ResetFilters}\" - Reset filters\n\"{Exit}\" - Exit\n" +
                    $"\nChoosen menu:");
                choosenMenu = Console.ReadLine();

                switch (choosenMenu.ToLower())
                {
                    case Weight:
                        filteredPrisoners = FilterByWeight(filteredPrisoners);
                        break;
                    case Height:
                        filteredPrisoners = FilterByHeight(filteredPrisoners);
                        break;
                    case Nation:
                        filteredPrisoners = FilterByNationalities(filteredPrisoners);
                        break;
                    case ResetFilters:
                        filteredPrisoners.Clear();
                        break;
                    case Exit:
                        SearchNotFinished = false;
                        break;
                    default:
                        break;
                }

                if (filteredPrisoners.Count > 0 && SearchNotFinished == true)
                {
                    Console.WriteLine("List of filtered prisoners:");
                    ShowPrisoners(filteredPrisoners);
                    Console.WriteLine();
                }
                else if (SearchNotFinished == false)
                {
                    Console.WriteLine("Goodbye!");
                }
                else
                {
                    Console.Write("No prisonerers, matching your filter. Press any key to continue and reset filters...");
                    Console.ReadKey(true);
                    Console.Clear();
                    filteredPrisoners.Clear();
                }
            }
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
            filteredPrisoners = filteredPrisoners.Where(_prisoners => _prisoners.Nationality == (Nationality)nationalityIndex && _prisoners.IsArrested == false).ToList();
            return filteredPrisoners;
        }

        private List<Prisoner> FilterByWeight(List<Prisoner> filteredPrisoners)
        {
            int minWeight = UserUtils.ReadNumber("Enter min prisoner weight:");
            int maxWeight = UserUtils.ReadNumber("Enter max prisoner weight:");
            filteredPrisoners = filteredPrisoners.Where(_prisoners => _prisoners.Weight >= minWeight && _prisoners.Weight <= maxWeight && _prisoners.IsArrested == false).ToList();
            return filteredPrisoners;
        }

        private List<Prisoner> FilterByHeight(List<Prisoner> filteredPrisoners)
        {
            int minHeight = UserUtils.ReadNumber("Enter min prisoner height:");
            int maxHeight = UserUtils.ReadNumber("Enter max prisoner height:");
            filteredPrisoners = filteredPrisoners.Where(_prisoners => _prisoners.Height >= minHeight && _prisoners.Height <= maxHeight && _prisoners.IsArrested == false).ToList();
            return filteredPrisoners;
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