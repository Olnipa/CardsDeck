namespace bossDamage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int minBossDamage = 10;
            int maxBossDamage = 50;
            int bossDamage;

            int minRandomNumber = 1;
            int maxRandomNumber = 101;
            int criticalChancePercent = 60;
            int criticalCoefficient = 3;

            if (random.Next(minRandomNumber, maxRandomNumber) <= criticalChancePercent)
            {
                bossDamage = random.Next(minBossDamage, maxBossDamage) * criticalCoefficient;
            }
        }
    }
}