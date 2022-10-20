using System.Linq;

namespace Amnesty
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Prison prison = new Prison();
            Crimes crimeUnderAmnesty = Crimes.AntiGovernment;
            Console.WriteLine($"Welcome to Arstocka prison!");
            Console.WriteLine("\nList of prisoners:");
            prison.ShowPrisoners();
            prison.AmnestyForPrisoners(crimeUnderAmnesty);
        }
    }

    enum Crimes
    {
        AntiGovernment,
        GrandTheftAuto,
        Murder,
        DrugTrade
    }

    class Prison
    {
        private List<Prisoner> _prisoners = new List<Prisoner>();

        public Prison()
        {
            _prisoners.Add(new Prisoner("Matew", Crimes.Murder));
            _prisoners.Add(new Prisoner("Kevin", Crimes.GrandTheftAuto));
            _prisoners.Add(new Prisoner("Dmitriy", Crimes.AntiGovernment));
            _prisoners.Add(new Prisoner("Katrin", Crimes.DrugTrade));
        }

        public void AmnestyForPrisoners(Crimes crimeUnderAmnesty)
        {
            ExcludeAmnestied(crimeUnderAmnesty);
            Console.WriteLine("\nPrisoners after amnesty:");
            ShowPrisoners();
        }

        public void ShowPrisoners()
        {
            for (int i = 0; i < _prisoners.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_prisoners[i].Name}. \t{_prisoners[i].Crime}");
            }
        }

        public void ExcludeAmnestied(Crimes crimeUnderAmnesty)
        {
            _prisoners = _prisoners.Where(_prisoners => _prisoners.Crime != crimeUnderAmnesty).ToList();
        }
    }

    class Prisoner
    {
        public string Name { get; private set; }
        public Crimes Crime { get; private set; }

        public Prisoner(string name, Crimes crime)
        {
            Name = name;
            Crime = crime;
        }
    }
}