namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int initialMoney = 1000;
            bool isWorking = true;
            Inventory playerInventory = new Inventory(new List<Item>());
            Inventory nPCInventory = new Inventory(new List<Item>());
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
        public List<Item> Items { get; private set; }

        public Inventory(List<Item> items)
        {
            Items = items;
        }

        public void ShowItems()
        {
            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Items[i].Name}: Price - {Items[i].Price}. Quantity - {Items[i].Quantity} pcs.");
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

        public void decreaseQuantity(int quantityForDecrease)
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

        public void increaseQuantity(int quantityForIncrease)
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
        public string Name { get; protected set; }
        public int Money { get; protected set; }
        public Inventory Inventory { get; protected set; }

        public Character(string name, Inventory inventory, int money = 500)
        {
            Name = name;
            Money = money;
            Inventory = inventory;
        }

        public void ShowInventory(string characterName)
        {
            Console.WriteLine($"{characterName}'s Inventory:\nMoney - {Money}");
            Inventory.ShowItems();
        }

        public void BuyItem(Character seller)
        {
            if (seller.Inventory.Items.Count > 0)
            {
                Console.WriteLine("Items fo sale:");
                seller.Inventory.ShowItems();

                int itemIndex = ReadNumber("Write item index for buy:") - 1;

                if (itemIndex < seller.Inventory.Items.Count & itemIndex >= 0)
                {
                    int quantityForBuy = ReadNumber("Write quantity for buy:");
                    int totalPrice = quantityForBuy * seller.Inventory.Items[itemIndex].Price;

                    if (totalPrice <= Money & quantityForBuy <= seller.Inventory.Items[itemIndex].Quantity)
                    {
                        Money -= totalPrice;
                        AddItemQuantity(seller, itemIndex, quantityForBuy);
                        Console.WriteLine($"You spent {totalPrice} units of money and bought {quantityForBuy} pcs of {seller.Inventory.Items[itemIndex].Name}.");
                        seller.Money += totalPrice;
                        seller.RemoveItemQuantity(itemIndex, quantityForBuy);
                    }
                    else if (totalPrice > Money)
                    {
                        Console.WriteLine("Not enough money.");
                    }
                    else
                    {
                        Console.WriteLine($"Not enough quanntity of {seller.Inventory.Items[itemIndex].Name}.");
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

        private void RemoveItemQuantity(int itemIndex, int quantityForDecrease)
        {
            Inventory.Items[itemIndex].decreaseQuantity(quantityForDecrease);

            if (Inventory.Items[itemIndex].Quantity == 0)
            {
                Inventory.Items.RemoveAt(itemIndex);
            }
        }

        private void AddItemQuantity(Character Seller, int itemIndex, int quantityForIncrease)
        {
            bool itemNotExistInBuyerInventory = true;
            string itemName = Seller.Inventory.Items[itemIndex].Name;

            for (int i = 0; i < Inventory.Items.Count; i++)
            {
                if (Inventory.Items[i].Name == Seller.Inventory.Items[itemIndex].Name)
                {
                    Inventory.Items[i].increaseQuantity(quantityForIncrease);
                    itemNotExistInBuyerInventory = false;
                    break;
                }
            }

            if (itemNotExistInBuyerInventory)
            {
                int itemPrice = Seller.Inventory.Items[itemIndex].Price;
                int itemWeight = Seller.Inventory.Items[itemIndex].Weight;
                Inventory.Items.Add(new Item(itemName, itemPrice, itemWeight, quantityForIncrease));
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
            Inventory.Items.Add(new Item("Old sock", 1, 3, 2));
            Inventory.Items.Add(new Food("Apple", 5, 5, 10));
        }
    }

    class NPC : Character
    {
        public NPC(string name, int money, Inventory inventory) : base(name, inventory, money)
        {
            Inventory.Items.Add(new MeleeWeapon("Small axe", 200, 15, false, 100, 3));
            Inventory.Items.Add(new MeleeWeapon("Big sword", 300, 25, true, 200, 2));
            Inventory.Items.Add(new Food("Small potion", 100, 50, 10, 5));
        }
    }
}