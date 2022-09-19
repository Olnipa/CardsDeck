using System;

namespace War
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Country armyRed = new Country("Redlandia");
            Country armyBlue = new Country("Blueland");
            
            Console.WriteLine("No one knows why these armies want to fight. But it is too late to stop them.");
            Console.WriteLine("Press any key to start a fight.");
            Console.ReadKey(true);
            Console.WriteLine($"\nArmy of {armyRed.Name}");
            armyRed.Platoon.ShowSoldiers();
            Console.WriteLine($"\nArmy of {armyBlue.Name}");
            armyBlue.Platoon.ShowSoldiers();
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey(true);

            while (armyRed.Platoon.GetQuantityOfAliveSoldiers() > 0 && armyBlue.Platoon.GetQuantityOfAliveSoldiers() > 0)
            {
                Country firstArmy = armyRed.DetermineInitiative(armyBlue, out Country secondArmy);

                Soldier firstSoldier = firstArmy.Platoon.GetAliveSoldier();
                Soldier secondSoldier = secondArmy.Platoon.GetAliveSoldier();

                firstSoldier.StartFight(secondSoldier);
                firstSoldier.ShowFinalInfo(secondSoldier);
            }

            armyRed.ShowFinalInfo(armyBlue);
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

        public void StartFight(Soldier secondSoldier)
        {
            int roundNumber = 1;

            while (secondSoldier.CurrentHealth > 0 && CurrentHealth > 0)
            {
                Console.WriteLine($"Round {roundNumber}");
                secondSoldier.TakeDamage(this);

                if (secondSoldier.CurrentHealth > 0)
                    TakeDamage(secondSoldier);

                roundNumber++;
            }
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

        public virtual void TakeDamage(Soldier enemy)
        {
            if (CheckEnemyAccuracy())
            {
                int damage = enemy.GetDamage();

                if (damage - Defense >= CurrentHealth)
                    CurrentHealth = 0;
                else
                    CurrentHealth -= damage - Defense;

                enemy.IncreaseKills(this);

                Console.WriteLine($"Soldier {enemy.PersonalNumber} hit soldier {PersonalNumber} for {damage} damage. Health: {CurrentHealth}/{Health}");
            }
            else
            {
                Console.WriteLine($"{enemy.PersonalNumber} missed. Health of {PersonalNumber}: {CurrentHealth}/{Health}");
            }
        }

        public virtual int GetDamage()
        {
            int damage = Strength;
            return damage;
        }

        public void IncreaseKills(Soldier enemy)
        {
            if (enemy.CheckAlive() == false)
                Kills++;
        }

        protected bool CheckAlive()
        {
            return CurrentHealth > 0;
        }

        protected bool CheckEnemyAccuracy()
        {
            Random random = new Random();
            int minAccuracy = 0;
            int maxAccuracy = 101;
            return Agility < random.Next(minAccuracy, maxAccuracy);
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

        public Heavy(string personalNumber, int blockDamageChance = 7) : base(personalNumber, minHealth: 120, 150, minStrength: 14, 16, minDefense: 4, 5, agility: 1)
        {
            _blockDamageChance = blockDamageChance;
        }

        public override void TakeDamage(Soldier enemy)
        {
            if (CheckEnemyAccuracy())
            {
                int damage = enemy.GetDamage();

                if (UserUtils.GetRandomNumber() <= _blockDamageChance)
                {
                    int blockDamageCoefficient = 2;
                    damage /= blockDamageCoefficient;
                    Console.WriteLine($"Soldier {PersonalNumber} succussfully blocked half of damage and got {damage} damage. Health: {CurrentHealth}/{Health}");
                }

                if (damage - Defense >= CurrentHealth)
                    CurrentHealth = 0;
                else
                    CurrentHealth -= damage - Defense;

                Console.WriteLine($"Soldier {enemy.PersonalNumber} hit soldier {PersonalNumber} for {damage} damage. Health: {CurrentHealth}/{Health}");
            }

            Console.WriteLine($"{enemy.PersonalNumber} missed. Health of {PersonalNumber}: {CurrentHealth}/{Health}");
            enemy.IncreaseKills(this);
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

    class Platoon
    {
        public List<Soldier> Soldiers { get; private set; }

        public Platoon(string countryName, int soldiersCount = 15)
        {
            Soldiers = GetSoldiers(countryName, soldiersCount);
        }

        public Soldier GetAliveSoldier()
        {
            int firstArmySoldierIndex = GetAliveSoldierIndex();
            return Soldiers[firstArmySoldierIndex];
        }

        public Soldier GetBestKiller()
        {
            Soldier bestKiller = Soldiers[0];

            for (int i = 0; i < Soldiers.Count; i++)
            {
                if (Soldiers[i].Kills > bestKiller.Kills)
                {
                    bestKiller = Soldiers[i];
                }
            }

            return bestKiller;
        }

        public void ShowSoldiers()
        {
            Console.WriteLine("Soldier\t\tStr\tHealth\t\tDef\tAgil\tType");
            for (int i = 0; i < Soldiers.Count; i++)
            {
                Console.WriteLine($"{Soldiers[i].PersonalNumber}\t{Soldiers[i].Strength}\t{Soldiers[i].CurrentHealth}/{Soldiers[i].Health}\t\t{Soldiers[i].Defense}\t{Soldiers[i].Agility}\t{Soldiers[i].GetType()}");
            }
        }

        public int GetQuantityOfAliveSoldiers()
        {
            int quantity = 0;

            for (int i = 0; i < Soldiers.Count; i++)
            {
                if (Soldiers[i].CurrentHealth > 0)
                    quantity++;
            }

            return quantity;
        }

        private int GetAliveSoldierIndex()
        {
            for (int i = 0; i < Soldiers.Count; i++)
            {
                if (Soldiers[i].CurrentHealth > 0)
                {
                    return i;
                }
            }

            Console.WriteLine("Alive soldiers are not founded.");
            return -1;
        }

        private List<Soldier> GetSoldiers(string CountryName, int soldiersCount)
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

    class Country
    {
        public string Name { get; private set; }
        public Platoon Platoon { get; private set; }

        public Country(string name, int soldiersCount = 60)
        {
            Name = name;
            Platoon = new Platoon(Name, soldiersCount + 1);
        }

        public Country DetermineInitiative(Country country2, out Country secondArmy)
        {
            const int Country1Number = 1;
            const int Country2Number = Country1Number + 1;
            Country firstArmy;

            if (UserUtils.GetRandomNumber(Country1Number, Country2Number + 1) == Country2Number)
            {
                firstArmy = country2;
                secondArmy = this;
            }
            else
            {
                firstArmy = this;
                secondArmy = country2;
            }

            return firstArmy;
        }

        public void ShowFinalInfo(Country enemy)
        {
            if (Platoon.GetQuantityOfAliveSoldiers() <= 0)
            {
                Console.WriteLine($"\n====== {enemy.Name} Win with {enemy.Platoon.GetQuantityOfAliveSoldiers()}/{enemy.Platoon.Soldiers.Count} alive soldiers. Best killer: {enemy.Platoon.GetBestKiller().PersonalNumber} with {enemy.Platoon.GetBestKiller().Kills} kills. His type - {enemy.Platoon.GetBestKiller().GetType()} ======");
            }
            else
            {
                Console.WriteLine($"\n====== {Name} Win with {Platoon.GetQuantityOfAliveSoldiers()}/{Platoon.Soldiers.Count} alive soldiers. Best killer: {Platoon.GetBestKiller().PersonalNumber} with {Platoon.GetBestKiller().Kills} kills. His type - {Platoon.GetBestKiller().GetType()} ======");
            }
        }
    }
}