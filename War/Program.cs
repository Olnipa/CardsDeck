using System;
using static System.Net.Mime.MediaTypeNames;

namespace War
{
    internal class Program
    {
        static void Main(string[] args)
        {
            War warRedBlue = new War(new Country("Redlandia"), new Country("Blueland"));
            
            Console.WriteLine("No one knows why these armies want to fight. But it is too late to stop them.");
            Console.WriteLine("Press any key to start a fight.");
            Console.ReadKey(true);
            Console.WriteLine($"\nArmy of {warRedBlue.Country1.Name}");
            warRedBlue.Country1.ShowSoldiers();
            Console.WriteLine($"\nArmy of {warRedBlue.Country2.Name}");
            warRedBlue.Country2.ShowSoldiers();
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey(true);

            while (warRedBlue.Country1.GetQuantityOfAliveSoldiers() > 0 && warRedBlue.Country2.GetQuantityOfAliveSoldiers() > 0)
            {
                Country firstArmy = warRedBlue.DetermineInitiative(out Country secondArmy);

                Soldier firstSoldier = firstArmy.GetAliveSoldier();
                Soldier secondSoldier = secondArmy.GetAliveSoldier();

                warRedBlue.StartFight(firstSoldier, secondSoldier);
                firstSoldier.ShowFinalInfo(secondSoldier);
            }

            warRedBlue.Country1.ShowFinalInfo(warRedBlue.Country2);
        }
    }

    static class UserUtils
    {
        public static int GetRandomNumber(int minChance = 0, int maxChance = 101)
        {
            Random random = new Random();
            return random.Next(minChance, maxChance);
        }
    }

    class War
    {
        public Country Country1 { get; private set; }
        public Country Country2 { get; private set; }

        public War(Country country1, Country country2)
        {
            Country1 = country1;
            Country2 = country2;
        }

        public Country DetermineInitiative(out Country secondArmy)
        {
            const int Country1Number = 1;
            const int Country2Number = Country1Number + 1;
            Country firstArmy;

            if (UserUtils.GetRandomNumber(Country1Number, Country2Number + 1) == Country2Number)
            {
                firstArmy = Country2;
                secondArmy = Country1;
            }
            else
            {
                firstArmy = Country1;
                secondArmy = Country2;
            }

            return firstArmy;
        }

        public void StartFight(Soldier firstSoldier, Soldier secondSoldier)
        {
            int roundNumber = 1;

            while (secondSoldier.CurrentHealth > 0 && firstSoldier.CurrentHealth > 0)
            {
                Console.WriteLine($"Round {roundNumber}");
                secondSoldier.Attack(firstSoldier);

                if (secondSoldier.CurrentHealth > 0)
                    firstSoldier.Attack(secondSoldier);

                roundNumber++;
            }
        }
    }

    class Soldier
    {
        public int CurrentHealth { get; protected set; }
        public int Strength { get; protected set; }
        public int Defense { get; protected set; }
        public int Agility { get; protected set; }
        public int Kills { get; protected set; }
        public string PersonalNumber { get; private set; }
        public int Health { get; private set; }

        public Soldier(string personalNumber, int minHealth = 100, int maxHealth = 125, int minStrength = 14, int maxStrength = 18, int minDefense = 3, int maxDefense = 4, int agility = 10)
        {
            PersonalNumber = personalNumber;
            Health = GenerateCharacteristic(minHealth, maxHealth);
            CurrentHealth = Health;
            Strength = GenerateCharacteristic(minStrength, maxStrength);
            Defense = GenerateCharacteristic(minDefense, maxDefense);
            Agility = agility;
        }

        public void ShowFinalInfo(Soldier enemy)
        {
            if (enemy.CurrentHealth <= 0)
            {
                Console.WriteLine($"\n{PersonalNumber} Win with {CurrentHealth}/{Health} health.");
                Console.WriteLine($"{enemy.PersonalNumber} is dead.\n");
            }
            else
            {
                Console.WriteLine($"\n{enemy.PersonalNumber} Win with {enemy.CurrentHealth}/{enemy.Health} health.");
                Console.WriteLine($"{PersonalNumber} is dead.\n");
            }
        }

        public virtual void Attack(Soldier enemy)
        {
            if (enemy.Agility < UserUtils.GetRandomNumber())
            {
                int damage = GetDamage();
                int realDamage = enemy.TakeDamage(damage);

                if (enemy.CurrentHealth <= 0)
                    Kills++;

                Console.WriteLine($"Soldier {PersonalNumber} hit soldier {enemy.PersonalNumber} for {realDamage} damage. Health: {enemy.CurrentHealth}/{enemy.Health}");
            }
            else
            {
                Console.WriteLine($"{PersonalNumber} missed. Health of {enemy.PersonalNumber}: {enemy.CurrentHealth}/{enemy.Health}");
            }
        }

        public virtual int TakeDamage(int damage)
        {
            damage -= Defense;

            if (damage <= 0)
                damage = 1;

            if (damage >= CurrentHealth)
                CurrentHealth = 0;
            else
                CurrentHealth -= damage;

            return damage;
        }

        public virtual int GetDamage()
        {
            int damage = Strength;
            return damage;
        }

        protected int GenerateCharacteristic(int minValue = 13, int maxValue = 17)
        {
            int minChance = 0;
            int maxChance = 101;
            int luckyChance = 90;
            int unluckyChance = 10;
            Random random = new Random();
            int randomChance = random.Next(minChance, maxChance);

            if (randomChance > luckyChance)
            {
                float minLuckyCoefficient = 1.36f;
                float maxLuckyCoefficient = 1.17f;
                minValue = Convert.ToInt32(minValue * minLuckyCoefficient);
                maxValue = Convert.ToInt32(maxValue * maxLuckyCoefficient);
            }
            else if (randomChance < unluckyChance)
            {
                float unluckCoefficient = 1.36f;
                minValue = Convert.ToInt32(minValue / unluckCoefficient);
                maxValue = Convert.ToInt32(maxValue / unluckCoefficient);
            }

            return random.Next(minValue, maxValue + 1);
        }
    }

    class Heavy : Soldier
    {
        private int _blockDamageChance;

        public Heavy(string personalNumber, int blockDamageChance = 7) : base(personalNumber, minHealth: 120, 150, minStrength: 14, 15, minDefense: 4, 5, agility: 1)
        {
            _blockDamageChance = blockDamageChance;
        }

        public override int TakeDamage(int damage)
        {
            damage -= Defense;
            if (damage <= 0)
                damage = 1;

            if (UserUtils.GetRandomNumber() <= _blockDamageChance)
            {
                int blockDamageCoefficient = 2;
                damage /= blockDamageCoefficient;
                Console.WriteLine($"Soldier {PersonalNumber} succussfully blocked 50% of damage.");
            }

            if (damage >= CurrentHealth)
                CurrentHealth = 0;
            else
                CurrentHealth -= damage;

            return damage;
        }
    }

    class Light : Soldier
    {
        private int _criticalChance;

        public Light(string personalNumber, int minCriticalChance = 10, int maxCriticalChance = 15) : base(personalNumber, minHealth: 70, 100, minStrength: 10, 14, minDefense: 1, 3, agility: 41)
        {
            _criticalChance = GenerateCharacteristic(minCriticalChance, maxCriticalChance);
        }

        public override int GetDamage()
        {
            int damage = base.GetDamage();

            if (UserUtils.GetRandomNumber() <= _criticalChance)
            {
                int damageCoefficient = 3;
                damage *= damageCoefficient;
                Console.WriteLine($"Soldier {PersonalNumber} made critical hit.");
            }

            return damage;
        }
    }

    //class Country22
    //{
    //    public string Name { get; private set; }
    //    public Country Platoon { get; private set; }

    //    public Country22(string name, int soldiersCount = 60)
    //    {
    //        Name = name;
    //        Platoon = new Country(Name, soldiersCount + 1);
    //    }

    //    public void ShowFinalInfo(Country22 enemy)
    //    {
    //        if (Platoon.GetQuantityOfAliveSoldiers() <= 0)
    //        {
    //            Console.WriteLine($"\n====== {enemy.Name} Win with {enemy.Platoon.GetQuantityOfAliveSoldiers()}/{enemy.Platoon.GetSoldiersCount()} alive soldiers. Best killer: {enemy.Platoon.GetBestKiller().PersonalNumber} with {enemy.Platoon.GetBestKiller().Kills} kills. His type - {enemy.Platoon.GetBestKiller().GetType()} ======");
    //        }
    //        else
    //        {
    //            Console.WriteLine($"\n====== {Name} Win with {Platoon.GetQuantityOfAliveSoldiers()}/{Platoon.GetSoldiersCount()} alive soldiers. Best killer: {Platoon.GetBestKiller().PersonalNumber} with {Platoon.GetBestKiller().Kills} kills. His type - {Platoon.GetBestKiller().GetType()} ======");
    //        }
    //    }
    //}

    class Country
    {
        private List<Soldier> _soldiers;
        public string Name { get; private set; }

        public Country(string countryName, int soldiersCount = 60)
        {
            Name = countryName;
            _soldiers = CreateSoldiers(countryName, soldiersCount);
        }

        public void ShowFinalInfo(Country enemy)
        {
            if (GetQuantityOfAliveSoldiers() <= 0)
            {
                Console.WriteLine($"\n====== {enemy.Name} Win with {enemy.GetQuantityOfAliveSoldiers()}/{enemy.GetSoldiersCount()} alive soldiers. Best killer: {enemy.GetBestKiller().PersonalNumber} with {enemy.GetBestKiller().Kills} kills. His type - {enemy.GetBestKiller().GetType()} ======");
            }
            else
            {
                Console.WriteLine($"\n====== {Name} Win with {GetQuantityOfAliveSoldiers()}/{GetSoldiersCount()} alive soldiers. Best killer: {GetBestKiller().PersonalNumber} with {GetBestKiller().Kills} kills. His type - {GetBestKiller().GetType()} ======");
            }
        }

        public int GetSoldiersCount()
        {
            return _soldiers.Count;
        }

        public Soldier GetAliveSoldier()
        {
            int firstArmySoldierIndex = GetAliveSoldierIndex();
            return _soldiers[firstArmySoldierIndex];
        }

        public Soldier GetBestKiller()
        {
            Soldier bestKiller = _soldiers[0];

            for (int i = 0; i < _soldiers.Count; i++)
            {
                if (_soldiers[i].Kills > bestKiller.Kills)
                {
                    bestKiller = _soldiers[i];
                }
            }

            return bestKiller;
        }

        public void ShowSoldiers()
        {
            Console.WriteLine("Soldier\t\tStr\tHealth\t\tDef\tAgil\tType");
            for (int i = 0; i < _soldiers.Count; i++)
            {
                Console.WriteLine($"{_soldiers[i].PersonalNumber}\t{_soldiers[i].Strength}\t{_soldiers[i].CurrentHealth}/{_soldiers[i].Health}\t\t{_soldiers[i].Defense}\t{_soldiers[i].Agility}\t{_soldiers[i].GetType()}");
            }
        }

        public int GetQuantityOfAliveSoldiers()
        {
            int quantity = 0;

            for (int i = 0; i < _soldiers.Count; i++)
            {
                if (_soldiers[i].CurrentHealth > 0)
                    quantity++;
            }

            return quantity;
        }

        private int GetAliveSoldierIndex()
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                if (_soldiers[i].CurrentHealth > 0)
                {
                    return i;
                }
            }

            Console.WriteLine("Alive soldiers are not founded.");
            return -1;
        }

        private List<Soldier> CreateSoldiers(string CountryName, int soldiersCount)
        {
            Random random = new Random();
            List<Soldier> platoon = new List<Soldier>();

            for (int i = 1; i < soldiersCount; i++)
            {
                int minCountOfSoldiersType = 1;
                int countOfSoldiersType = 3;
                int randomValue = random.Next(minCountOfSoldiersType, countOfSoldiersType + 1);

                if (randomValue == 1)
                {
                    platoon.Add(new Soldier(Convert.ToString(i) + "-" + CountryName));
                }
                else if (randomValue == 2)
                {
                    platoon.Add(new Heavy(Convert.ToString(i) + "-" + CountryName));
                }
                else
                {
                    platoon.Add(new Light(Convert.ToString(i) + "-" + CountryName));
                }
            }

            return platoon;
        }
    }
}