using System;

namespace CarService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AutoService autoService = new AutoService();
            autoService.StartWork();
        }
    }

    enum SparePartNames
    {
        Engine = 1,
        Turbine,
        FuelPump,
        Bulb,
        Headlight
    }

    class SparePart
    {
        public SparePartNames Name { get; private set; }
        public int Price { get; private set; }

        public SparePart(SparePartNames name, int price)
        {
            Name = name;
            Price = price;
        }
    }

    class SparePartStack
    {
        public SparePart SparePart { get; private set; }
        public int Quantity { get; private set; }
        public int ReplacementCost { get; private set; }

        public SparePartStack(SparePart sparePart, int quantity, int replacementCost)
        {
            SparePart = sparePart;
            Quantity = quantity;
            ReplacementCost = replacementCost;
        }

        public void IssueSparePart()
        {
            if (Quantity > 0)
            {
                Quantity--;
            }
            else
            {
                Console.WriteLine("Not enouth detail in warehouse.");
            }
        }
    }

    class AutoService
    {
        private int _money;
        private List<SparePartStack> _spareParts = new List<SparePartStack>();
        private Queue<Client> _clients = new Queue<Client>();

        public AutoService()
        {
            int minMoney = 1000;
            int maxMoney = 5000;
            int minDetailsPrice = 0;
            int maxDetailsPrice = 4;
            _money = UserUtils.GetNumber(minMoney, maxMoney);
            _spareParts.Add(new SparePartStack(new SparePart(SparePartNames.Engine, 500), UserUtils.GetNumber(minDetailsPrice, maxDetailsPrice), 1000));
            _spareParts.Add(new SparePartStack(new SparePart(SparePartNames.Turbine, 100), UserUtils.GetNumber(minDetailsPrice, maxDetailsPrice), 500));
            _spareParts.Add(new SparePartStack(new SparePart(SparePartNames.FuelPump, 70), UserUtils.GetNumber(minDetailsPrice, maxDetailsPrice), 150));
            _spareParts.Add(new SparePartStack(new SparePart(SparePartNames.Bulb, 10), UserUtils.GetNumber(minDetailsPrice, maxDetailsPrice), 100));
            _spareParts.Add(new SparePartStack(new SparePart(SparePartNames.Headlight, 50), UserUtils.GetNumber(minDetailsPrice, maxDetailsPrice), 200));
            GetClients();
        }

        public void StartWork()
        {
            bool isWorking = true;
            Console.WriteLine("Welcome to the work of your dream!\nPress any key to start work...");

            while (isWorking && _clients.Count > 0)
            {
                Console.ReadKey(true);
                Console.Clear();
                const string Exit = "exit";
                Console.WriteLine($"Car service money: {_money}\nThere are {_clients.Count} customers in queue. Press any key " +
                    $"to serve next client or write \"Exit\" to finish work.");
                string choosenMenu = Console.ReadLine();
                
                if (choosenMenu.ToLower() == Exit)
                {
                    isWorking = false;
                }
                else
                {
                    ServeNextClient();
                }
            }

            Console.WriteLine("No new clients for today, so you can go home.");
        }

        public void ShowSparePartsInfo()
        {
            for (int i = 0; i < _spareParts.Count; i++)
            {
                int totalCost = _spareParts[i].SparePart.Price + _spareParts[i].ReplacementCost;
                Console.WriteLine($"{i + 1}. {_spareParts[i].SparePart.Name} - {_spareParts[i].Quantity} pcs." +
                    $"\tPrice - {_spareParts[i].SparePart.Price}.\tReplacement cost - {_spareParts[i].ReplacementCost}. " +
                    $"\tTotal cost for repair - {totalCost}");
            }
        }

        private void ServeNextClient()
        {
            const string Refuse = "refuse";
            int penaltyForRefuse = 100;
            int moralDamage = 0;
            Client nextClient = _clients.Dequeue();
            nextClient.ShowInfo();
            Console.WriteLine($"\nList of spare parts in warehouse:");
            ShowSparePartsInfo();

            Console.WriteLine($"\nPress any key to serve client or write \"{Refuse}\" to refuse customer service...\nEntered value:");
            string choosenMenu = Console.ReadLine();

            if (choosenMenu.ToLower() == Refuse)
            {
                int moneyToPay = PayPenalty(penaltyForRefuse, moralDamage);
                nextClient.TakeMoney(penaltyForRefuse);
                Console.WriteLine($"You paid {moneyToPay} to client instead just tell him \"Sorry\". Car service money: {_money}.");
            }
            else
            {
                StartRepairing(nextClient);
            }
        }

        private void StartRepairing(Client client)
        {
            int choosenPartNumber = GetPartNumber();

            if (_spareParts[choosenPartNumber].Quantity > 0)
            {
                if (client.IsSolvent(_spareParts[choosenPartNumber]))
                {
                    int moneyFromClient = client.PayMoney(_spareParts[choosenPartNumber]);
                    _money += moneyFromClient;
                    Console.WriteLine($"Client payd {moneyFromClient}. Car service money: {_money}");
                    _spareParts[choosenPartNumber].IssueSparePart();

                    if (client.BrokenSparePart == _spareParts[choosenPartNumber].SparePart.Name)
                    {
                        Console.WriteLine("Client is happy.");
                        client.ConfirmRepair();
                    }
                    else
                    {
                        Console.WriteLine("\nClient is angry. His car is still not working! Press any key to pay him penalty and compensate cost...");
                        Console.ReadKey(true);
                        int moneyToPay = PayPenalty(moneyFromClient);
                        client.TakeMoney(moneyToPay);
                        Console.WriteLine($"You refund {moneyToPay} to the client as a penalty. Car service money: {_money}. Client left your service.");
                    }
                }
                else
                {
                    Console.WriteLine("Client does not have enouth money and left car service.");
                }
            }
            else
            {
                Console.WriteLine($"Not enouth quantity of spare part {_spareParts[choosenPartNumber].SparePart.Name}. Client left car service.");
            }
        }

        private int GetPartNumber()
        {
            int choosenPartNumber = -1;
            bool partNumberIsIncorrect = true;

            while (partNumberIsIncorrect)
            {
                choosenPartNumber = UserUtils.ReadNumber("\nEnter choosen spare part index for change:") - 1;

                if (choosenPartNumber >= 0 && choosenPartNumber < _spareParts.Count)
                    partNumberIsIncorrect = false;
                else
                    Console.WriteLine("Entered spare part index is out of range.");
            }

            return choosenPartNumber;
        }

        private int PayPenalty(int penalty, float moralDamagesPercent = 0.1f)
        {
            int penaltyToPay = Convert.ToInt32(moralDamagesPercent * penalty);
            int moneyToPay = penaltyToPay + penalty;

            if (moneyToPay <= _money)
            {
                _money -= moneyToPay;
            }
            else
            {
                moneyToPay = _money;
                _money = 0;
            }

            return moneyToPay;
        }

        private void GetClients(int minClientsCount = 1, int maxClientsCount = 12)
        {
            for (int i = 0; i < UserUtils.GetNumber(minClientsCount, maxClientsCount); i++)
            {
                _clients.Enqueue(new Client());
            }
        }
    }

    class Client
    {
        public SparePartNames BrokenSparePart { get; private set; }
        public bool CarIsRepaired { get; private set; }
        public int Money { get; private set; }

        public Client()
        {
            BrokenSparePart = GetSparePartName();
            CarIsRepaired = false;
            Money = UserUtils.GetNumber();
        }

        public void TakeMoney(int money)
        {
            if (money > 0)
            {
                Money += money;
            }
        }

        public void ShowInfo()
        {
            Console.WriteLine($"The customer's car has a broken {BrokenSparePart} (Part number {(int)BrokenSparePart})");
        }

        public bool IsSolvent(SparePartStack detailStack)
        {
            int repairCost = detailStack.ReplacementCost + detailStack.SparePart.Price;
            return Money >= repairCost;
        }

        public void ConfirmRepair()
        {
            CarIsRepaired = true;
        }

        public int PayMoney(SparePartStack detailStack)
        {
            int moneyToPay = 0;

            if (IsSolvent(detailStack))
            {
                moneyToPay = detailStack.ReplacementCost + detailStack.SparePart.Price;
                Money -= moneyToPay;
            }
            else
            {
                Console.WriteLine("Not enouth money.");
            }

            return moneyToPay;
        }

        private SparePartNames GetSparePartName()
        {
            int minDetailIndex = Enum.GetValues(typeof(SparePartNames)).Cast<int>().Min();
            int detailsCount = Enum.GetValues(typeof(SparePartNames)).Cast<int>().Max();
            int detailIndex = UserUtils.GetNumber(minDetailIndex, detailsCount + 1);
            return (SparePartNames)detailIndex;
        }

    }

    class UserUtils
    {
        public static int ReadNumber(string text = "Please, enter a number:")
        {
            bool isParsed = false;
            int number = 0;

            while (isParsed == false)
            {
                Console.Write(text);
                string value = Console.ReadLine();

                if (int.TryParse(value, out number))
                {
                    isParsed = true;
                }
                else
                {
                    Console.Write("Entered value is not a number. Please, write a number.");
                }
            }

            return number;
        }

        public static int GetNumber(int minNumber = 100, int maxNumber = 5000)
        {
            Random random = new Random();
            return random.Next(minNumber, maxNumber);
        }
    }
}