using System.Numerics;

namespace TopPlayersOnServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.ShowTopByLevel();
            Console.WriteLine();
            game.ShowTopByStrength();
            Console.WriteLine();
            game.ShowAllPlayers();
        }
    }

    class Game
    {
        private List<Player> _players = new List<Player>();

        public Game()
        {
            int minLevel = 0;
            int maxLevel = 61;
            int minStrengh = 10;
            int maxStrenght = 51;

            _players.Add(new Player("Gump", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Bubba", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Den", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Jenny", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Forest", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Bengamin", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Robert", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Andrew", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("David", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
            _players.Add(new Player("Martin", UserUtils.GetRandomNumber(minLevel, maxLevel), UserUtils.GetRandomNumber(minStrengh, maxStrenght)));
        }

        public void ShowAllPlayers()
        {
            WritePlayers(_players, $"List of all registered players:");
        }

        public void ShowTopByLevel()
        {
            int countInTop = 3;
            var topPlayers = _players.OrderByDescending(_players => _players.Level).Take(countInTop).ToList();
            WritePlayers(topPlayers, $"Top {countInTop} players by level:");
        }

        public void ShowTopByStrength()
        {
            int countInTop = 3;
            var topPlayers = _players.OrderByDescending(_players => _players.Strengh).Take(countInTop).ToList();
            WritePlayers(topPlayers, $"Top {countInTop} players by strength:");
        }

        private void WritePlayers(List<Player> players, string text)
        {
            Console.WriteLine(text);

            for (int i = 0; i < players.Count; i++)
            {
                Console.Write($"#{i + 1}: ");
                players[i].ShowInfo();
            }
        }
    }

    class Player
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int Strengh { get; private set; }

        public Player(string name, int level, int strengh)
        {
            Name = name;
            Level = level;
            Strengh = strengh;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Name}. Level: {Level}. Strength: {Strengh}.");
        }
    }

    class UserUtils
    {
        public static int GetRandomNumber(int minNumber = 16, int maxNumber = 110)
        {
            Random random = new Random();
            return random.Next(minNumber, maxNumber);
        }
    }
}