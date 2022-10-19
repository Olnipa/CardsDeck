using System.Linq;

namespace Amnesty
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Prison prison = new Prison();
            Console.WriteLine($"Welcome to Arstocka prison! Today prisoners convicted of a \"{prison._CrimeUnderAmnesty}\" crime will be amnestied");
            Console.WriteLine("\nPrisoners before amnesty:");
            prison.ShowPrisoners();
            prison.ExcludeAmnestied();
            Console.WriteLine("\nPrisoners after amnesty:");
            prison.ShowPrisoners();
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
        public Crimes _CrimeUnderAmnesty { get; private set; }

        public Prison()
        {
            _CrimeUnderAmnesty = Crimes.AntiGovernment;
            _prisoners.Add(new Prisoner("Matew", Crimes.Murder));
            _prisoners.Add(new Prisoner("Kevin", Crimes.GrandTheftAuto));
            _prisoners.Add(new Prisoner("Dmitriy", Crimes.AntiGovernment));
            _prisoners.Add(new Prisoner("Katrin", Crimes.DrugTrade));
        }

        public void ShowPrisoners()
        {
            for (int i = 0; i < _prisoners.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_prisoners[i].Name}. \t{_prisoners[i].Crime}");
            }
        }

        public void ExcludeAmnestied()
        {
            _prisoners = _prisoners.Where(_prisoners => _prisoners.Crime != _CrimeUnderAmnesty).ToList();
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