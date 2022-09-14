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
        private int Money;
        private Queue<Client> _clients;
        private List<Product> _priceList;
        private List<ProductStack> _warehouse;

        public Supermarket(int clientsQuantity, int money = 0)
        {
            int productCountInWarehouse = 100;
            _priceList = new List<Product>() {
                new Product("Carrot", 5), new Product("Potato", 3), new Product("Tomato", 12),
                new Product("Cucumber", 5), new Product("Eggs", 15), new Product("Meat", 20)};
            _warehouse = new List<ProductStack>();
            AddProductsToWarehouse(productCountInWarehouse);
            _clients = new Queue<Client>();
            CreateNewClient(clientsQuantity);
            Money = money;
        }

        public void StartWork()
        {
            Console.WriteLine("Welcome to cashier simulator game!\nPress any key to start this amazing game...\n");
            Console.ReadKey(true);

            Console.WriteLine($"Money in the shop - {Money} USD\n");

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
            Console.WriteLine($"Money in the shop - {Money} USD\n");
        }

        private void ShowAllProducts()
        {
            for (int i = 0; i < _warehouse.Count; i++)
            {
                Console.WriteLine($"{_warehouse[i].GetProductName()} - {_warehouse[i].Quaintity} pcs");
            }
        }

        private void AddProductsToWarehouse(int productsQuantity)
        {
            for (int i = 0; i < _priceList.Count; i++)
            {
                _warehouse.Add(new ProductStack(_priceList[i], productsQuantity));
            }
        }

        private void CreateNewClient(int clientsQuantity)
        {
            Random random = new Random();

            for (int i = 0; i < clientsQuantity; i++)
            {
                int minClientMoney = 30;
                int maxClientMoney = 151;
                int money = random.Next(minClientMoney, maxClientMoney);
                _clients.Enqueue(new Client(GenerateBusket(), money));
            }
        }

        private List<ProductStack> GenerateBusket()
        {
            Random random = new Random();

            int minProductToBuy = 1;
            int maxProductToBuy = 10;
            int ProductToBuy = random.Next(minProductToBuy, maxProductToBuy);
            List<ProductStack> basket = new List<ProductStack>();

            for (int i = 0; i < ProductToBuy; i++)
            {
                Product newProduct = GenerateProduct();
                int productIndex = GetProductIndex(newProduct.Name);
                
                if (productIndex >= 0)
                {
                    basket.Add(new ProductStack(newProduct, GetProductsAmountToPutInBasket(productIndex)));
                    
                    if (basket[basket.Count - 1].Quaintity == 0)
                    {
                        basket.RemoveAt(basket.Count - 1);
                    }
                }
            }
            
            return basket;
        }

        private int GetProductsAmountToPutInBasket(int productIndex)
        {
            if (productIndex >= 0 & productIndex < _priceList.Count)
            {
                return _warehouse[productIndex].TakeProductFromWarehouse();
            }
            else
            {
                Console.WriteLine("Product Index is out of range");
                return 0;
            } 
        }

        private int GetProductIndex(string productName)
        {
            for (int i = 0; i < _priceList.Count; i++)
            {
                if (productName == _priceList[i].Name)
                    return i;
            }

            Console.WriteLine("Such product does not exist in Price List");
            return -1;
        }

        private Product GenerateProduct()
        {
            Random random = new Random();

            Product product = new Product(_priceList[random.Next(0, _priceList.Count)]);
            return product;
        }

        private void ServeNextClient()
        {
            Client nextClient = _clients.Dequeue();
            Console.WriteLine("Preliminary receipt:");
            nextClient.ShowClientBasket();
            nextClient.ShowClientMoney();
            int totalPrice = nextClient.GetTotalPrice();
            Console.WriteLine("\nTotal to be paid - " + totalPrice);
            Console.WriteLine("__________________________________________");
            Console.WriteLine();

            while (nextClient.CheckSolvency() == false)
            {
                string discardedProduct = nextClient.DiscardRandomProduct();
                Console.Write($"Client does not have enought money. Press any key to throw out something from basket... ");
                Console.ReadKey(true);
                Console.WriteLine($"The cashier throws out one piece of {discardedProduct} from the cart.");
                _warehouse[GetProductIndex(discardedProduct)].IncreaseProductQuantity();
            }

            int moneyReceived = nextClient.GiveMoney();
            Money += moneyReceived;

            Console.WriteLine();
            Console.WriteLine("__________________________________________");
            Console.WriteLine("Final receipt:");
            nextClient.ShowClientBasket();
            Console.Write($"\nTotal paid: {moneyReceived}\n");
            Console.WriteLine("__________________________________________");
            Console.WriteLine();
        }
    }

    class Client
    {
        private List<ProductStack> _basket;
        private int Money;
        private int MoneyToPay;

        public Client(List<ProductStack> basket, int money)
        {
            _basket = basket;
            Money = money;
        }

        public int GiveMoney()
        {
            int tempMoneyToPay = MoneyToPay;
            Money -= MoneyToPay;
            MoneyToPay = 0;
            return tempMoneyToPay;
        }

        public string DiscardRandomProduct()
        {
            Random random = new Random();
            int itemIndex = random.Next(0, _basket.Count);
            string discardedProduct = _basket[itemIndex].GetProductName();
            _basket[itemIndex].ReduceProductQuantity();

            if (_basket[itemIndex].Quaintity == 0)
                _basket.Remove(_basket[itemIndex]);
             
            return discardedProduct;
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
            MoneyToPay = GetTotalPrice();

            if (Money >= MoneyToPay)
            {
                return true;
            }
            else
            {
                MoneyToPay = 0;
                return false;
            }
        }

        public void ShowClientBasket()
        {
            if (_basket.Count > 0)
            {
                for (int i = 0; i < _basket.Count; i++)
                {
                    Console.WriteLine($"{_basket[i].GetProductName()} - {_basket[i].Quaintity} pcs");
                }
            }
            else
            {
                Console.WriteLine("No products in client's basket.");
            }
        }

        public void ShowClientMoney()
        {
            Console.WriteLine($"Client money - {Money} USD");
        }

        private int GetSumOfProducts(int itemIndex)
        {
            return _basket[itemIndex].GetProductPrice() * _basket[itemIndex].Quaintity;
        }
    }

    class ProductStack
    {
        private Product _product;

        public int Quaintity { get; private set; }

        public ProductStack(Product product, int quaintity)
        {
            _product = product;
            Quaintity = quaintity;
        }

        public int TakeProductFromWarehouse()
        {
            Random random = new Random();
            int minProductQuantity = 1;
            int maxProductQuantity = 5;
            int quantity = random.Next(minProductQuantity, maxProductQuantity);

            if (Quaintity >= quantity)
            {
                Quaintity -= quantity;
                return quantity;
            }
            else if (Quaintity > 0)
            {
                Quaintity = 0;
                return Quaintity;
            }
            else
            {
                return 0;
            }
        }

        public void ReduceProductQuantity()
        {
            Quaintity--;
        }

        public void IncreaseProductQuantity()
        {
            Quaintity++;
        }

        public string GetProductName()
        {
            return _product.Name;
        }

        public int GetProductPrice()
        {
            return _product.Price;
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