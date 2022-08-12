namespace bossDamage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int minBossDamage = 10;
            int maxBossDamage = 50;
            int bossDamage = random.Next(minBossDamage, maxBossDamage);

            int minCriticalChance = 1;
            int maxCriticalChance = 101;
            int criticalChance = 50;
            int criticalCoefficient = 3;

            if (random.Next(minCriticalChance, maxCriticalChance) > criticalChance)
            {
                bossDamage *= criticalCoefficient;
            }            
        }
    }
}