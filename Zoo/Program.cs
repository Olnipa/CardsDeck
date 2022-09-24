namespace Zoo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries = new List<Aviary>();

        public Zoo(List<Aviary> aviaries)
        {
            _aviaries = aviaries;
        }
    }

    class Aviary
    {

    }

    class Animal
    {

    }
}