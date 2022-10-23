namespace DeterminationOfOverdue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PackOfBraisedBeef packOfBraisedBeef = new PackOfBraisedBeef();
            packOfBraisedBeef.ShowExpiredBraisedBeef();
            Console.WriteLine();
            packOfBraisedBeef.ShowAllBraisedBeef();
        }
    }

    class PackOfBraisedBeef
    {
        private List<BraisedBeef> _braisedBeefCanneds = new List<BraisedBeef>();

        public PackOfBraisedBeef()
        {
            _braisedBeefCanneds.Add(new BraisedBeef("Mikoyan", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("FromUncleVanya", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("Talosto", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("Fornosovo", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("BeefBeef", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("Samson", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("Suvorovskaya", UserUtils.GetRandomNumber()));
            _braisedBeefCanneds.Add(new BraisedBeef("GenerationP", UserUtils.GetRandomNumber()));
        }

        public void ShowAllBraisedBeef()
        {
            Console.WriteLine("List of all braised beef:");

            foreach (var braisedBeef in _braisedBeefCanneds)
            {
                braisedBeef.ShowInfo();
            }
        }

        public void ShowExpiredBraisedBeef()
        {
            int todaysYear = 2022;
            var expiredBraisedBeef = _braisedBeefCanneds.Where(_beefList => _beefList.ProductionYear + _beefList.ShelfLife < todaysYear);

            if (expiredBraisedBeef.Count() > 0)
            {
                Console.WriteLine("List of expired braised beef:");

                foreach (var braisedBeef in expiredBraisedBeef)
                {
                    braisedBeef.ShowInfo();
                }
            }
            else
            {
                Console.WriteLine("There are no expired Braised Beef.");
            }
        }
    }

    class BraisedBeef
    {
        public string Name { get; private set; }
        public int ProductionYear { get; private set; }
        public int ShelfLife { get; private set; }

        public BraisedBeef(string name, int yearOfProduction, int shelfLife = 10)
        {
            Name = name;
            ProductionYear = yearOfProduction;
            ShelfLife = shelfLife;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Name}. Year of production: {ProductionYear}. Shelf life: {ShelfLife}.");
        }
    }

    class UserUtils
    {
        public static int GetRandomNumber(int minNumber = 2009, int maxNumber = 2020)
        {
            Random random = new Random();
            return random.Next(minNumber, maxNumber);
        }
    }
}