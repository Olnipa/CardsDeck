namespace ArmamentReport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Army army = new Army();
            army.ShowRanks();
        }
    }

    enum MilitaryRanks
    {
        CommonSoldier,
        Sergeant,
        Lieutenant,
        Captain
    }

    enum Armaments
    {
        MachineGun,
        Bazooka,
        Tank,
        Helicopter,
        WarShip
    }

    class Army
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Army()
        {
            _soldiers.Add(new Soldier("Mat", Armaments.Helicopter, MilitaryRanks.Sergeant, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Dew", Armaments.MachineGun, MilitaryRanks.Lieutenant, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Tom", Armaments.Bazooka, MilitaryRanks.Captain, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Tan", Armaments.Tank, MilitaryRanks.CommonSoldier, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Lui", Armaments.WarShip, MilitaryRanks.CommonSoldier, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Oil", Armaments.Helicopter, MilitaryRanks.Captain, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Dru", Armaments.MachineGun, MilitaryRanks.CommonSoldier, UserUtils.GetRandomNumber()));
            _soldiers.Add(new Soldier("Jon", Armaments.MachineGun, MilitaryRanks.Sergeant, UserUtils.GetRandomNumber()));
        }

        public void ShowRanks()
        {
            int index = 1;
            var soldiersRanks = _soldiers.Select(_soldiers => new
            {
                Name = _soldiers.Name,
                Rank = _soldiers.Rank
            });

            foreach (var soldier in soldiersRanks)
            {
                Console.WriteLine($"{index}. Name: {soldier.Name}. Rank: {soldier.Rank}");
                index++;
            }
        }
    }

    class Soldier
    {
        public string Name { get; private set; }
        public Armaments Armament { get; private set; }
        public MilitaryRanks Rank { get; private set; }
        public int ServiceLife { get; private set; }

        public Soldier(string name, Armaments armament, MilitaryRanks rank, int serviceLife)
        {
            Name = name;
            Armament = armament;
            Rank = rank;
            ServiceLife = serviceLife;
        }
    }

    class UserUtils
    {
        public static int GetRandomNumber(int minNumber = 0, int maxNumber = 24)
        {
            Random random = new Random();
            return random.Next(minNumber, maxNumber);
        }
    }
}