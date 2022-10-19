namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int sum = 0;
            int count = 10;
            int temp = 0;

            for (int i = 0; i < count; i++)
            {
                temp += i;
                sum += 1 + temp * 6;
                Console.WriteLine(sum);
            }
        }
    }
}