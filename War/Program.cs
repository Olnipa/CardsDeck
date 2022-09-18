using System;

namespace War
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
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
                Country firstArmy;
                Country secondArmy;
                const int armyRedIndex = 1;
                const int armyBlueIndex = 2;
                int randomNumber = random.Next(armyRedIndex, armyBlueIndex + 1);

                if (randomNumber == armyRedIndex)
                {
                    firstArmy = armyRed;
                    secondArmy = armyBlue;
                }
                else
                {
                    firstArmy = armyBlue;
                    secondArmy = armyRed;
                }

                int firstCountrySoldierIndex = firstArmy.Platoon.GetAliveSoldierIndex();
                int secondCountrySoldierIndex = secondArmy.Platoon.GetAliveSoldierIndex();
                Soldier firstSoldier = firstArmy.Platoon.Soldiers[firstCountrySoldierIndex];
                Soldier secondSoldier = secondArmy.Platoon.Soldiers[secondCountrySoldierIndex];
                int roundNumber = 1;
                
                while (secondSoldier.CurrentHealth > 0 && firstSoldier.CurrentHealth > 0)
                {
                    Console.WriteLine($"Round {roundNumber}");
                    secondSoldier.TakeDamage(firstSoldier);

                    if (secondSoldier.CurrentHealth > 0)
                        firstSoldier.TakeDamage(secondSoldier);
                    
                    roundNumber++;
                }

                if (secondSoldier.CurrentHealth <= 0)
                {
                    Console.WriteLine($"\n{firstSoldier.PersonalNumber} Win with {firstSoldier.CurrentHealth}/{firstSoldier.Health} health.");
                    Console.WriteLine($"{secondSoldier.PersonalNumber} is dead.\n");
                }
                else
                {
                    Console.WriteLine($"\n{secondSoldier.PersonalNumber} Win with {secondSoldier.CurrentHealth}/{secondSoldier.Health} health.");
                    Console.WriteLine($"{firstSoldier.PersonalNumber} is dead.\n");
                }
            }

            if (armyRed.Platoon.GetQuantityOfAliveSoldiers() <= 0)
            {
                Console.WriteLine($"\n====== {armyBlue.Name} Win with {armyBlue.Platoon.GetQuantityOfAliveSoldiers()}/{armyBlue.Platoon.Soldiers.Count} alive soldiers. Best killer: {armyBlue.Platoon.FindBestKiller().PersonalNumber} with {armyBlue.Platoon.FindBestKiller().Kills} kills. His type - {armyBlue.Platoon.FindBestKiller().GetType()} ======");
            }
            else
            {
                Console.WriteLine($"\n====== {armyRed.Name} Win with {armyRed.Platoon.GetQuantityOfAliveSoldiers()}/{armyRed.Platoon.Soldiers.Count} alive soldiers. Best killer: {armyRed.Platoon.FindBestKiller().PersonalNumber} with {armyRed.Platoon.FindBestKiller().Kills} kills. His type - {armyRed.Platoon.FindBestKiller().GetType()} ======");
            }
        }
    }

    class Soldier
    {
        public string PersonalNumber { get; private set; }
        public int Health { get; private set; }
        public int CurrentHealth { get; protected set; }
        public int Strength { get; protected set; }
        public int Defense { get; protected set; }
        public int Agility { get; protected set; }
        public int Kills { get; protected set; }

        public Soldier(string personalNumber, int minHealth = 90, int maxHealth = 120, int minStrength = 13, int maxStrength = 17, int minDefense = 2, int maxDefense = 4, int agility = 10)
        {
            PersonalNumber = personalNumber;
            Health = GenerateCharacteristic(minHealth, maxHealth);
            CurrentHealth = Health;
            Strength = GenerateCharacteristic(minStrength, maxStrength);
            Defense = GenerateCharacteristic(minDefense, maxDefense);
            Agility = agility;
        }

        public virtual void TakeDamage(Soldier enemy)
        {
            if (CheckEnemyAccuracy())
            {
                int damage = enemy.GenerateDamage();

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

        public virtual int GenerateDamage()
        {
            return Strength;
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

        protected int GetRandomNumber()
        {
            Random random = new Random();
            int minChance = 0;
            int maxChance = 101;
            return random.Next(minChance, maxChance);
        }
    }

    class Heavy : Soldier
    {
        private int _blockDamageChance;

        public Heavy(string personalNumber, int blockDamageChance = 7) : base(personalNumber, minHealth: 120, 150, minStrength: 14, 18, minDefense: 4, 8, agility: 1)
        {
            _blockDamageChance = blockDamageChance;
        }

        public override void TakeDamage(Soldier enemy)
        {
            if (CheckEnemyAccuracy())
            {
                int damage = enemy.GenerateDamage();

                if (GetRandomNumber() <= _blockDamageChance)
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

        public override int GenerateDamage()
        {
            int damage = base.GenerateDamage();

            if (GetRandomNumber() <= _criticalChance)
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

        public Soldier FindBestKiller()
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

        public int GetAliveSoldierIndex()
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

        public int GetRandomNumber(int minChance = 0, int maxChance = 101)
        {
            Random random = new Random();
            return random.Next(minChance, maxChance);
        }
    }
}