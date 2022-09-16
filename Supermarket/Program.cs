namespace Supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int clientsCount = 7;
            Supermarket supermarket = new Supermarket(clientsCount);
            supermarket.StartWork();
        }
    }

    class Supermarket
    {
        private int _money;
        private Queue<Client> _clients;
        private List<ProductStack> _warehouse;

        public Supermarket(int clientsQuantity, int money = 0)
        {
            int productCountInWarehouse = 100;

            _warehouse = new List<ProductStack>();
            AddProducts(productCountInWarehouse, GeneratePriceList());
            _clients = new Queue<Client>();
            CreateNewClient(clientsQuantity);
            _money = money;
        }

        public void StartWork()
        {
            Console.WriteLine("Welcome to cashier simulator game!\nPress any key to start this amazing game...\n");
            Console.ReadKey(true);

            Console.WriteLine($"Money in the shop - {_money} USD\n");

            while (_clients.Count > 0)
            {
                Console.WriteLine($"There are {_clients.Count} clients in queue.");
                Console.WriteLine("Press any key to serve next client.\n");
                Console.ReadKey(true);
                Console.WriteLine("__________________________________________");
                ServeNextClient();
            }

            Console.WriteLine("Shop is closed.\nBalance of products in the shop:");
            ShowAllProducts();
            Console.WriteLine();
            Console.WriteLine($"Money in the shop - {_money} USD\n");
        }

        private List<Product> GeneratePriceList()
        {
            List<Product> priceList = new List<Product>() {
                new Product("Carrot", 5), new Product("Potato", 3), new Product("Tomato", 12),
                new Product("Cucumber", 5), new Product("Eggs", 15), new Product("Meat", 20)};
            return priceList;
        }

        private void ShowAllProducts()
        {
            for (int i = 0; i < _warehouse.Count; i++)
            {
                Console.WriteLine($"{_warehouse[i].Product.Name} - {_warehouse[i].Quantity} pcs");
            }
        }

        private void AddProducts(int productsQuantity, List<Product> pricelist)
        {
            for (int i = 0; i < pricelist.Count; i++)
            {
                _warehouse.Add(new ProductStack(pricelist[i], productsQuantity));
            }
        }

        private void CreateNewClient(int clientsQuantity)
        {
            Random random = new Random();

            for (int i = 0; i < clientsQuantity; i++)
            {
                int minClientMoney = 30;
                int maxClientMoney = 151;
                int clientMoney = random.Next(minClientMoney, maxClientMoney);
                _clients.Enqueue(new Client(GenerateBusket(), clientMoney));
            }
        }

        private List<ProductStack> GenerateBusket()
        {
            Random random = new Random();

            int minProductToBuy = 1;
            int maxProductToBuy = 10;
            int productCount = random.Next(minProductToBuy, maxProductToBuy);
            List<ProductStack> basket = new List<ProductStack>();

            for (int i = 0; i < productCount; i++)
            {
                Product newProduct = GenerateProduct();
                int productIndex = GetProductIndex(newProduct.Name);
                
                if (productIndex >= 0)
                {
                    int quantity = GetProducts(productIndex);
                    
                    if (quantity > 0)
                    {
                        basket.Add(new ProductStack(newProduct, quantity));
                    }
                }
            }
            
            return basket;
        }

        private int GetProducts(int productIndex)
        {
            if (productIndex >= 0 & productIndex < _warehouse.Count)
            {
                Random random = new Random();
                int minProductQuantity = 1;
                int maxProductQuantity = 5;
                int quantity = random.Next(minProductQuantity, maxProductQuantity);

                if (_warehouse[productIndex].Quantity >= quantity)
                {
                    _warehouse[productIndex].ReduceProductQuantity(quantity);
                    return quantity;
                }
                else if (_warehouse[productIndex].Quantity > 0)
                {
                    quantity = _warehouse[productIndex].Quantity;
                    _warehouse[productIndex].ReduceProductQuantity(quantity);
                    return quantity;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                Console.WriteLine("Product Index is out of range");
                return 0;
            } 
        }

        private int GetProductIndex(string productName)
        {
            for (int i = 0; i < _warehouse.Count; i++)
            {
                if (productName == _warehouse[i].Product.Name)
                    return i;
            }

            Console.WriteLine("Such product does not exist in Price List");
            return -1;
        }

        private Product GenerateProduct()
        {
            Random random = new Random();

            Product product = new Product(_warehouse[random.Next(0, _warehouse.Count)].Product);
            return product;
        }

        private void ServeNextClient()
        {
            Client nextClient = _clients.Dequeue();
            Console.WriteLine("Preliminary receipt:");
            nextClient.ShowBasket();
            nextClient.ShowMoney();
            int totalPrice = nextClient.GetTotalPrice();
            Console.WriteLine("\nTotal to be paid - " + totalPrice);
            Console.WriteLine("__________________________________________");
            Console.WriteLine();

            while (nextClient.CheckSolvency() == false)
            {
                string discardedProductName = nextClient.DiscardRandomProduct();
                Console.Write($"Client does not have enought money. Press any key to throw out something from basket... ");
                Console.ReadKey(true);
                Console.WriteLine($"\nThe cashier throws out one piece of {discardedProductName} from the cart.");
                _warehouse[GetProductIndex(discardedProductName)].IncreaseProductQuantity();
            }

            int moneyReceived = nextClient.PayMoney();
            _money += moneyReceived;

            Console.WriteLine();
            Console.WriteLine("__________________________________________");
            Console.WriteLine("Final receipt:");
            nextClient.ShowBasket();
            Console.Write($"\nTotal paid: {moneyReceived}\n");
            Console.WriteLine("__________________________________________");
            Console.WriteLine();
        }
    }

    class Client
    {
        private List<ProductStack> _basket;
        private int _money;

        public Client(List<ProductStack> basket, int money)
        {
            _basket = basket;
            _money = money;
        }

        public int PayMoney()
        {
            int moneyToPay = GetTotalPrice();
            _money -= moneyToPay;
            return moneyToPay;
        }

        public string DiscardRandomProduct()
        {
            Random random = new Random();
            int itemIndex = random.Next(0, _basket.Count);
            string discardedProductName = _basket[itemIndex].Product.Name;
            _basket[itemIndex].ReduceProductQuantity();

            if (_basket[itemIndex].Quantity == 0)
                _basket.Remove(_basket[itemIndex]);
             
            return discardedProductName;
        }

        public int GetTotalPrice()
        {
            int totalPrice = 0;

            for (int i = 0; i < _basket.Count; i++)
            {
                totalPrice += GetSumOfProducts(i);
            }

            return totalPrice;
        }

        public bool CheckSolvency()
        {
            return _money >= GetTotalPrice();
        }

        public void ShowBasket()
        {
            if (_basket.Count > 0)
            {
                for (int i = 0; i < _basket.Count; i++)
                {
                    Console.WriteLine($"{_basket[i].Product.Name} - {_basket[i].Quantity} pcs");
                }
            }
            else
            {
                Console.WriteLine("No products in client's basket.");
            }
        }

        public void ShowMoney()
        {
            Console.WriteLine($"Client money - {_money} USD");
        }

        private int GetSumOfProducts(int itemIndex)
        {
            return _basket[itemIndex].Product.Price * _basket[itemIndex].Quantity;
        }
    }

    class ProductStack
    {
        public Product Product { get; private set; }

        public int Quantity { get; private set; }

        public ProductStack(Product product, int quaintity)
        {
            Product = product;
            Quantity = quaintity;
        }

        public void ReduceProductQuantity(int quantity = 1)
        {
            if (quantity > 0)
                Quantity -= quantity;
        }

        public void IncreaseProductQuantity()
        {
            Quantity++;
        }
    }

    class Product
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public Product(Product product)
        {
            Name = product.Name;
            Price = product.Price;
        }
    }
}