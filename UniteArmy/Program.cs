using System.Xml.Linq;

namespace UniteArmy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char firstLetterInSurnameToMove = 'B';
            Army army = new Army();
            Console.WriteLine("Squads before moving:");
            army.ShowAllSoldiers();
            army.MoveSoldiersFromSquad1ToSquad2(firstLetterInSurnameToMove);
            Console.WriteLine("_______________________________\nSquads after moving:");
            army.ShowAllSoldiers();
        }
    }

    class Army
    {
        private List<Soldier> _squad1 = new List<Soldier>();
        private List<Soldier> _squad2 = new List<Soldier>();

        public Army()
        {
            _squad1.Add(new Soldier("Tom"));
            _squad1.Add(new Soldier("Bob"));
            _squad1.Add(new Soldier("Tam"));
            _squad1.Add(new Soldier("Big"));
            _squad1.Add(new Soldier("Met"));
            _squad1.Add(new Soldier("Mark"));
            _squad2.Add(new Soldier("Britney"));
            _squad2.Add(new Soldier("Margaret"));
            _squad2.Add(new Soldier("July"));
            _squad2.Add(new Soldier("Scarlet"));
            _squad2.Add(new Soldier("Sindiya"));
            _squad2.Add(new Soldier("Carmen"));
        }

        public void MoveSoldiersFromSquad1ToSquad2(char letter)
        {
            _squad2 = _squad2.Union(_squad1.Where(_squad1 => _squad1.Name.StartsWith(letter))).ToList();
            _squad1 = _squad1.Except(_squad2).ToList();
        }

        public void ShowAllSoldiers()
        {
            Console.WriteLine("\tSquad 1");
            ShowSaquad(_squad1);
            Console.WriteLine("\tSquad 2");
            ShowSaquad(_squad2);
        }

        private void ShowSaquad(List<Soldier> soldiers)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                Console.WriteLine($"{soldiers[i].Name}");
            }
        }
    }

    class Soldier
    {
        public string Name { get; private set; }

        public Soldier(string name)
        {
            Name = name;
        }
    }
}