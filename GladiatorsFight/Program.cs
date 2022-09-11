namespace GladiatorsFight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArenaOfGladiators arena = new ArenaOfGladiators();

            Console.WriteLine("_____________ Welcom to the combat arena _____________\n\n" +
                "You need to choose 2 fighters from below list:\n");
            ArenaOfGladiators.ShowFighters();
            Console.WriteLine();

            arena.BeginCombat();

            Console.WriteLine("\nPress any key to try more, or write \"n\" to exit.");
            string choosenMenu = Console.ReadLine();

            switch (choosenMenu)
            {
                case "n":
                    break;
                default:
                    Console.Clear();
                    Main(args);
                    break;
            }
        }
    }

    class ArenaOfGladiators
    {
        private static List<Fighter> _fighters;

        public ArenaOfGladiators()
        {
            _fighters = new List <Fighter>() {
                new Elf("Elf", health: 150, damageMin: 10, 17, agility: 17, accuracy: 8),
                new Orc("Orc", health: 300, damageMin: 17, 25, agility: 13, accuracy: 2),
                new Vampire("Vampire", health: 150, damageMin: 10, 17, agility: 16, accuracy: 7),
                new Draconian("Draconian", health: 200, damageMin: 11, 19, agility: 14, accuracy: 6, firePoints: 4),
                new Paladin("Paladin", health: 210, damageMin: 13, 20, agility: 13, accuracy: 6, divineScore: 5)};
        }

        static public int ReadIndex(string text)
        {
            bool indexIsOutOfRange = true;
            int errorCode = -1;
            int value = errorCode;

            while (indexIsOutOfRange)
            {
                Console.Write(text);

                if (int.TryParse(Console.ReadLine(), out value))
                {
                    value -= 1;

                    if (value >= 0 && value < _fighters.Count)
                    {
                        indexIsOutOfRange = false;
                        break;
                    }
                    else
                    {
                        value = errorCode;
                        Console.WriteLine("\nError. Index is out of Range.");
                    }
                }
                else
                {
                    Console.WriteLine("\nError. Entered value must include only numbers.");
                }
            }

            return value;
        }

        static public void ShowFighters()
        {
            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_fighters[i].Name}. Health: {_fighters[i].Health}. " +
                    $"Damage: {_fighters[i].DamageMin}-{_fighters[i].DamageMax}. Agility: {_fighters[i].Agility}. " +
                    $"Accuracy: {_fighters[i].Accuracy}.");
            }
        }

        public void BeginCombat()
        {
            int roundNumber = 1;
            int fighterIndex = ReadIndex("Enter the index of the fighter for the red corner:");
            Fighter redFighter = TransformateFighterClass(_fighters[fighterIndex]);
            fighterIndex = ReadIndex("Enter the index of the fighter for the blue corner:");
            Fighter blueFighter = TransformateFighterClass(_fighters[fighterIndex]);
            blueFighter.MakeFighterColorBlue();

            while (redFighter.CurrentHealth > 0 & blueFighter.CurrentHealth > 0)
            {
                redFighter.MakeFighterMove(blueFighter, roundNumber);
                blueFighter.MakeFighterMove(redFighter, roundNumber);

                roundNumber++;
                Console.WriteLine();
            }

            if (redFighter.CurrentHealth <= 0 & blueFighter.CurrentHealth <= 0)
                Console.WriteLine("________________________ Draw ________________________");
            else if (redFighter.CurrentHealth <= 0)
                Console.WriteLine("________________________ " + blueFighter.Name + " Win ________________________");
            else if (blueFighter.CurrentHealth <= 0)
                Console.WriteLine("________________________ " + redFighter.Name + " Win ________________________");
            Console.WriteLine();
        }

        private Fighter TransformateFighterClass(Fighter choosenfighter)
        {
            switch (choosenfighter)
            {
                case Elf fighter:
                    Elf fighterElf = new Elf(fighter);
                    return fighterElf;
                case Orc fighter:
                    Orc fighterOrc = new Orc(fighter);
                    return fighterOrc;
                case Vampire fighter:
                    Vampire fighterVampire = new Vampire(fighter);
                    return fighterVampire;
                case Draconian fighter:
                    Draconian fighterDraconian = new Draconian(fighter);
                    return fighterDraconian;
                case Paladin fighter:
                    Paladin fighterPaladin = new Paladin(fighter);
                    return fighterPaladin;
                default:
                    Console.WriteLine("Error. Can not transform class of fighter.");
                    return choosenfighter;
            }
        }
    }

    class Fighter
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int CurrentHealth { get; protected set; }
        public int DamageMin { get; protected set; }
        public int DamageMax { get; protected set; }
        public int Agility { get; protected set; }
        public int Accuracy { get; protected set; }
        public string Color { get; protected set; }

        public Fighter(string name, int health, int damageMin, int damageMax, int agility, int accuracy, string color = "Red")
        {
            Name = name;
            Health = CurrentHealth = health;
            DamageMin = damageMin;
            DamageMax = damageMax;
            Agility = agility;
            Accuracy = accuracy;
            Color = color;
        }

        public Fighter(Fighter fighter, string color = "Red")
        {
            Name = fighter.Name;
            Health = CurrentHealth = fighter.Health;
            DamageMin = fighter.DamageMin;
            DamageMax = fighter.DamageMax;
            Agility = fighter.Agility;
            Accuracy = fighter.Accuracy;
            Color = color;
        }

        public virtual void MakeFighterMove(Fighter victim, int roundNumber)
        {
            int damage = CalculateDamage(victim, roundNumber, out int chanceHit);
            victim.TakeDamage(damage);
            ShowSystemMessage(victim, chanceHit, damage);
        }

        public void MakeFighterColorBlue()
        {
            Color = "blue";
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        protected virtual int CalculateDamage(Fighter enemy, int roundNumber, out int chanceHit)
        {
            int luckyChanceHit = 20;
            int failChanceHit = 1;
            int luckyCoefficient = 2;
            int minDamage = 1;
            int failDamage = 0;

            chanceHit = GenerateChanceHit(roundNumber);
            int damage = minDamage;

            if (chanceHit == luckyChanceHit)
            {
                damage = GenerateDamage() * luckyCoefficient;
            }
            else if (chanceHit == failChanceHit)
            {
                damage = failDamage;
            }
            else if (chanceHit + Accuracy >= enemy.Agility)
            {
                damage = GenerateDamage();
            }

            return damage;
        }

        protected virtual int GenerateChanceHit(int roundNumber)
        {
            Random random = new Random();
            int minHitChance = 1;
            int maxHitChance = 21;
            int chanceHit = random.Next(minHitChance, maxHitChance);
            return chanceHit;
        }

        protected virtual int GenerateDamage()
        {
            Random random = new Random();
            int damage = random.Next(DamageMin, DamageMax + 1);
            return damage;
        }

        protected void ShowSystemMessage(Fighter enemy, int chanceHit, int damage)
        {
            Console.WriteLine($"=== {Name}-{Color} === Chance hit: {chanceHit} + {Accuracy} = {chanceHit + Accuracy}. {enemy.Name} agility: {enemy.Agility}. Damage: {damage}.");
            Console.WriteLine($"{enemy.Name}-{enemy.Color} health: {enemy.CurrentHealth} / {enemy.Health}");
        }
    }

    class Elf : Fighter
    {
        
        public Elf(string name, int health, int damageMin, int damageMax, int agility, int accuracy, string color = "red") : base(name, health, damageMin, damageMax, agility, accuracy, color)
        {
        }

        public Elf(Elf fighter, string color = "red") : base (fighter, color)
        {
        }

        protected override int GenerateChanceHit(int roundNumber)
        {
            Random random = new Random();
            int minHitChance = 1;
            int maxHitChance = 21;
            int luckyRound = 4;
            int luckyMinHitChance = 17;
            if (roundNumber % luckyRound == 0)
                minHitChance = luckyMinHitChance;
            int chanceHit = random.Next(minHitChance, maxHitChance);
            return chanceHit;
        }
    }

    class Orc : Fighter
    {
        public Orc(string name, int health, int damageMin, int damageMax, int agility, int accuracy, string color = "red") : base(name, health, damageMin, damageMax, agility, accuracy)
        {
        }

        public Orc(Orc fighter, string color = "red") : base(fighter, color)
        {
        }
    }

    class Vampire : Fighter
    {
        private float vampireHealingCoefficient = 0.6f;

        public Vampire(string name, int health, int damageMin, int damageMax, int agility, int accuracy, string color = "Red") : base(name, health, damageMin, damageMax, agility, accuracy, color)
        {
        }

        public Vampire(Vampire fighter, string color = "red") : base(fighter, color)
        {
        }

        public override void MakeFighterMove(Fighter victim, int roundNumber)
        {
            int damage = CalculateDamage(victim, roundNumber, out int chanceHit);
            victim.TakeDamage(damage);
            DrinkBlood(damage);
            ShowSystemMessage(victim, chanceHit, damage);
        }

        private void DrinkBlood(int damage)
        {
            CurrentHealth += Convert.ToInt32(damage * vampireHealingCoefficient);
        }
    }

    class Draconian : Fighter
    {
        private int _firePoints;
        private int _currentFirePoints;

        public Draconian(string name, int health, int damageMin, int damageMax, int agility, int accuracy, int firePoints, string color = "Red") : base(name, health, damageMin, damageMax, agility, accuracy, color)
        {
            _firePoints = _currentFirePoints = firePoints;
        }

        public Draconian(Draconian fighter, string color = "red") : base(fighter, color)
        {
            _firePoints = _currentFirePoints = fighter._firePoints;
        }

        protected override int CalculateDamage(Fighter enemy, int roundNumber, out int chanceHit)
        {
            int damage = base.CalculateDamage(enemy, roundNumber, out chanceHit);

            if (_currentFirePoints >= _firePoints)
            {
                int fireDamageCoefficient = 3;
                _currentFirePoints -= _firePoints;
                damage *= fireDamageCoefficient;
            }

            _currentFirePoints++;
            return damage;
        }
    }

    class Paladin : Fighter
    {
        private int _divineScore;
        private int _currentDivineScore;
        private bool _prayerIsUsed;
        private int _prayerRounds;
        private int _prayerRoundsLeft;

        public Paladin(string name, int health, int damageMin, int damageMax, int agility, int accuracy, int divineScore, string color = "Red") : base(name, health, damageMin, damageMax, agility, accuracy, color)
        {
            _divineScore = divineScore;
            _currentDivineScore = Convert.ToInt32(_divineScore / 2);
            _prayerIsUsed = false;
            _prayerRounds = _prayerRoundsLeft = 2;
        }

        public Paladin(Paladin fighter, string color = "red") : base(fighter, color)
        {
            _divineScore = fighter._divineScore;
            _currentDivineScore = fighter._currentDivineScore;
            _prayerIsUsed = fighter._prayerIsUsed;
            _prayerRounds = fighter._prayerRounds;
            _prayerRoundsLeft = fighter._prayerRoundsLeft;
        }

        public override void MakeFighterMove(Fighter victim, int roundNumber)
        {
            base.MakeFighterMove(victim, roundNumber);
            Pray();
            _prayerRoundsLeft--;
            _currentDivineScore++;
        }

        private void Pray()
        {
            float prayerCoefficient = 0.1f;
            int prayerCharacteristicsIncrease = 2;

            if (_currentDivineScore >= _divineScore && _prayerIsUsed == false)
            {
                _currentDivineScore -= _divineScore;
                Accuracy += prayerCharacteristicsIncrease;
                DamageMin += prayerCharacteristicsIncrease;
                DamageMax += prayerCharacteristicsIncrease;
                Agility += prayerCharacteristicsIncrease;
                CurrentHealth += Convert.ToInt32(Health * prayerCoefficient);

                _prayerIsUsed = true;
                _prayerRoundsLeft = _prayerRounds;
            }

            if (_prayerRoundsLeft == 0)
            {
                _prayerIsUsed = false;
                Accuracy -= prayerCharacteristicsIncrease;
                DamageMin -= prayerCharacteristicsIncrease;
                DamageMax -= prayerCharacteristicsIncrease;
                Agility -= prayerCharacteristicsIncrease;
            }
        }
    }
}