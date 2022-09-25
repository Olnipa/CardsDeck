namespace CarService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    class Detail
    {
        public string Name { get; private set; }
        public int Price { get; private set; }
        public int ReplacementCost { get; private set; }

        public Detail(string name, int price, int replacementCost)
        {
            Name = name;
            Price = price;
            ReplacementCost = replacementCost;
        }
    }

    class DetailStack
    {
        public Detail Detail { get; private set; }
        public int Quantity { get; private set; }

        public DetailStack(Detail detail, int quantity)
        {
            Detail = detail;
            Quantity = quantity;
        }
    }

    class AutoService
    {
        private List<DetailStack> details = new List<DetailStack>();
        private int _money;
    }
}