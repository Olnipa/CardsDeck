namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int initialMoney = 1000;
            bool isWorking = true;
            Inventory playerInventory = new Inventory(new List<Item>()
            { (new Item("Old sock", 1, 3, 2)),
              (new Food("Apple", 5, 5, 10)) });
            Inventory nPCInventory = new Inventory(new List<Item>()
            { (new MeleeWeapon("Small axe", 200, 15, false, 100, 3)),
              (new MeleeWeapon("Big sword", 300, 25, true, 200, 2)),
              (new Food("Small potion", 100, 50, 10, 5)) });
            NPC seller = new NPC("John Seller", initialMoney, nPCInventory);
            Player player1 = new Player("Bill Buyer", initialMoney, playerInventory);

            Console.Write($"\nWelcome to the shop \"Shoot & Heal\".");

            while (isWorking)
            {
                const string ShowSellerGoods = "1";
                const string ShowBuyerGoods = "2";
                const string BuyGood = "3";
                const string Exit = "0";

                Console.Write($"Choose What you want to do:\n{ShowSellerGoods} - Show all goods from store" +
                    $"\n{ShowBuyerGoods} - Show your own inventory\n{BuyGood} - Buy something\n{Exit} - Exit\n\nEnter number: ");
                string choosenMenu = Console.ReadLine();
                Console.WriteLine();

                switch (choosenMenu)
                {
                    case ShowSellerGoods:
                        seller.ShowInventory(seller.Name);
                        break;
                    case ShowBuyerGoods:
                        player1.ShowInventory(player1.Name);
                        break;
                    case BuyGood:
                        player1.BuyItem(seller);
                        break;
                    case Exit:
                        isWorking = false;
                        break;
                    default:
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
                Console.WriteLine();
            }
        }
    }

    class Inventory
    {
        private List<Item> _items;

        public Inventory(List<Item> items)
        {
            _items = items;
        }

        public int GetCount()
        {
            return _items.Count;
        }

        public int GetItemWeight(int itemIndex)
        {
            return _items[itemIndex].Weight;
        }

        public int GetItemPrice(int itemIndex)
        {
            return _items[itemIndex].Price;
        }

        public int GetItemQuantity(int itemIndex)
        {
            return _items[itemIndex].Quantity;
        }

        public string GetItemName(int itemIndex)
        {
            return _items[itemIndex].Name;
        }

        public void AddItem(Item newItem)
        {
            _items.Add(newItem);
        }

        public void AddItemQuantity(int itemIndex, int quantityForIncrease)
        {
            _items[itemIndex].IncreaseQuantity(quantityForIncrease);
        }

        public void RemoveItem(int itemIndex)
        {
            _items.RemoveAt(itemIndex);
        }

        public void RemoveItemQuantity(int itemIndex, int quantityForDecrease)
        {
            _items[itemIndex].DecreaseQuantity(quantityForDecrease);

            if (GetItemQuantity(itemIndex) == 0)
            {
                RemoveItem(itemIndex);
            }
        }

        public void ShowItems()
        {
            if (_items.Count > 0)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_items[i].Name}: Price - {_items[i].Price}. Quantity - {_items[i].Quantity} pcs.");
                }
            }
            else
            {
                Console.WriteLine("All goods is sold out. Try to come back tomorrow.");
            }
        }
    }

    class Item
    {
        protected const int DefaultQuantity = 1;
        public string Name { get; private set; }
        public int Price { get; private set; }
        public int Weight { get; private set; }
        public int Quantity { get; protected set; }

        public Item(string name, int price, int weight, int quantity = DefaultQuantity)
        {
            Name = name;
            Price = price;
            Weight = weight;
            Quantity = quantity;
        }

        public void DecreaseQuantity(int quantityForDecrease)
        {
            if (Quantity >= quantityForDecrease)
            {
                Quantity -= quantityForDecrease;
            }
            else
            {
                Console.WriteLine($"Not enough quantity of {Name}");
            }
        }

        public void IncreaseQuantity(int quantityForIncrease)
        {
            Quantity += quantityForIncrease;
        }
    }

    class MeleeWeapon : Item
    {
        public bool UseTwoHands { get; private set; }
        public int Damage { get; private set; }

        public MeleeWeapon(string name, int price, int damage, bool useTwoHands, int weight, int quantity = DefaultQuantity) : base(name, price, weight)
        {
            UseTwoHands = useTwoHands;
            Damage = damage;
            Quantity = quantity;
        }
    }

    class Food : Item
    {
        public int HealthIncreae { get; private set; }

        public Food(string name, int price, int healthIncrease, int weight, int quantity = DefaultQuantity) : base(name, price, weight)
        {
            HealthIncreae = healthIncrease;
            Quantity = quantity;
        }
    }

    class Character
    {
        private Inventory _inventory;
        public string Name { get; protected set; }
        public int Money { get; protected set; }
 
        public Character(string name, Inventory inventory, int money = 500)
        {
            Name = name;
            Money = money;
            _inventory = inventory;
        }

        public void ShowInventory(string characterName)
        {
            Console.WriteLine($"{characterName}'s Inventory:\nMoney - {Money}");
            _inventory.ShowItems();
        }

        public void BuyItem(Character seller)
        {
            if (seller._inventory.GetCount() > 0)
            {
                Console.WriteLine("Items fo sale:");
                seller._inventory.ShowItems();

                int itemIndex = ReadNumber("Write item index for buy:") - 1;

                if (itemIndex < seller._inventory.GetCount() & itemIndex >= 0)
                {
                    int quantityForBuy = ReadNumber("Write quantity for buy:");
                    int totalPrice = quantityForBuy * seller._inventory.GetItemPrice(itemIndex);

                    if (totalPrice <= Money & quantityForBuy <= seller._inventory.GetItemQuantity(itemIndex))
                    {
                        Money -= totalPrice;
                        AddItemQuantity(seller, itemIndex, quantityForBuy);
                        Console.WriteLine($"You spent {totalPrice} units of money and bought {quantityForBuy} pcs of {seller._inventory.GetItemName(itemIndex)}.");
                        seller.Money += totalPrice;
                        seller._inventory.RemoveItemQuantity(itemIndex, quantityForBuy);
                    }
                    else if (totalPrice > Money)
                    {
                        Console.WriteLine("Not enough money.");
                    }
                    else
                    {
                        Console.WriteLine($"Not enough quanntity of {seller._inventory.GetItemName(itemIndex)}.");
                    }
                }
                else
                {
                    Console.WriteLine("Error. Item Index is out of range.");
                }
            }
            else
            {
                Console.WriteLine("All goods is sold out. Try to come back tomorrow.");
            }
        }


        private void AddItemQuantity(Character Seller, int itemIndex, int quantityForIncrease)
        {
            bool itemNotExistInBuyerInventory = true;
            string itemName = Seller._inventory.GetItemName(itemIndex);

            for (int i = 0; i < _inventory.GetCount(); i++)
            {
                if (_inventory.GetItemName(i) == Seller._inventory.GetItemName(itemIndex))
                {
                    _inventory.AddItemQuantity(i, quantityForIncrease);
                    itemNotExistInBuyerInventory = false;
                    break;
                }
            }

            if (itemNotExistInBuyerInventory)
            {
                int itemPrice = Seller._inventory.GetItemPrice(itemIndex);
                int itemWeight = Seller._inventory.GetItemWeight(itemIndex);
                _inventory.AddItem(new Item(itemName, itemPrice, itemWeight, quantityForIncrease));
            }
        }

        private int ReadNumber(string text)
        {
            bool isNotParsed = true;
            int value = 0;

            while (isNotParsed)
            {
                Console.Write(text);
                string itemForBuy = Console.ReadLine();

                if (int.TryParse(itemForBuy, out value))
                {
                    isNotParsed = false;
                }
                else
                {
                    Console.WriteLine("Error. Entered value can include only numbers.");
                }
            }

            return value;
        }
    }

    class Player : Character
    {
        public Player(string name, int money, Inventory inventory) : base(name, inventory, 2000)
        {

        }
    }

    class NPC : Character
    {
        public NPC(string name, int money, Inventory inventory) : base(name, inventory, money)
        {

        }
    }
}