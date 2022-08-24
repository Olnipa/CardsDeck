namespace PassangerTrainConfigurator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CityDataBase cities = new CityDataBase();
            WagonTypeDataBase wagonType = new WagonTypeDataBase();
            TrainConfigurator trainConfigurator = new TrainConfigurator(cities, new List<Route>(), wagonType, new List<Train>());
            bool isWorking = true;
            Console.WriteLine($"------------ Welcome to the Train Configurator ------------");
            trainConfigurator.ReadAnyKey();
            Console.Clear();

            while (isWorking)
            {
                const string StartTrainConfigurating = "1";
                const string Exit = "0";

                trainConfigurator.ShowCurrentRouteInformation();

                Console.Write($"Choose What you want to do:\n{StartTrainConfigurating} - Start Train Configurating" +
                    $"\n{Exit} - Exit\n\nEnter number: ");
                string choosenMenu = Console.ReadLine();
                Console.WriteLine();

                switch (choosenMenu)
                {
                    case StartTrainConfigurating:
                        trainConfigurator.CreateNewTrainPlan();
                        break;
                    case Exit:
                        isWorking = false;
                        break;
                    default:
                        break;
                }

                trainConfigurator.ReadAnyKey();
                Console.Clear();
            }
        }
    }

    class City
    {
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
        }
    }

    class CityDataBase
    {
        private List<City> _cities;

        public CityDataBase()
        {
            _cities = new List<City>() { new City("Vancouver"), new City("Calgary"), new City("Winnipeg"), new City("Ottawa"), new City("Toronto") };
        }

        public string GetCityName(int index)
        {
            return _cities[index].Name;
        }

        public void ShowCities()
        {
            for (int i = 0; i < _cities.Count; i++)
            {
                ShowCity(i);
            }
        }

        public int GetCount()
        {
            return _cities.Count;
        }
        private void ShowCity(int index)
        {
            Console.WriteLine($"Index {index + 1}: {_cities[index].Name}.");
        }
    }

    class Route
    {
        private City _cityDeparture;
        private City _cityDestination;

        public Route(City cityDeparture, City cityDestination)
        {
            _cityDeparture = cityDeparture;
            _cityDestination = cityDestination;
        }

        public string GetDepartureCityName()
        {
            return _cityDeparture.Name;
        }

        public string GetDestinationCityName()
        {
            return _cityDestination.Name;
        }
    }

    class TrainConfigurator
    {
        private CityDataBase _cityDataBase;
        private List<Route> _routes;
        private WagonTypeDataBase _wagonsType;
        private List<Train> _trains;

        public TrainConfigurator(CityDataBase cityDataBase, List<Route> routes, WagonTypeDataBase wagonsType, List<Train> trains)
        {
            _cityDataBase = cityDataBase;
            _routes = routes;
            _wagonsType = wagonsType;
            _trains = trains;
        }

        public void ShowCurrentRouteInformation()
        {
            string currentRouteInformation = "No current routes";
            Console.WriteLine("====================================================\n");

            if (_trains.Count > 0 )
            {
                bool atLeastOneTrainIsSent = false;

                for (int i = 0; i < _trains.Count; i++)
                {
                    if (_trains[i].IsSent == true)
                    {
                        atLeastOneTrainIsSent = true;
                        string currentRoute = _routes[i].GetDepartureCityName() + " - " + _routes[i].GetDestinationCityName();
                        currentRouteInformation = $"{currentRoute}.\nTrain consist of {_trains[i].GetCountOfWagons()} wagons." +
                            $"\nQuantity of passengers: {_trains[i].Passengers} \\ {_trains[i].GetSeatsAmount()}";
                        Console.WriteLine($"Current route of train {i + 1}: {currentRouteInformation}.\n");
                    }
                }

                if (atLeastOneTrainIsSent == false)
                {
                    Console.WriteLine($"Current route: {currentRouteInformation}.\n");
                }
            }
            else
            {
                Console.WriteLine($"Current route: {currentRouteInformation}.\n");
            }

            Console.WriteLine("====================================================\n");
        }

        public void CreateNewTrainPlan()
        {
            Console.Clear();
            ShowCurrentRouteInformation();
            CreateRoute();
            string departureCity = _routes[^1].GetDepartureCityName();
            string destinationCity = _routes[^1].GetDestinationCityName();
            Console.WriteLine($"Route {departureCity} - {destinationCity} successfully created.");
            ReadAnyKey();

            Console.Clear();
            ShowCurrentRouteInformation();
            int ticketsSold = GenerateAmountOfPassengers();
            Console.WriteLine($"{ticketsSold} tickets were sold.");
            ReadAnyKey();

            Console.Clear();
            ShowCurrentRouteInformation();
            CreateNewTrain(ticketsSold);
            Console.WriteLine($"\nTrain successfully formed.");
            ReadAnyKey();

            Console.Clear();
            ShowCurrentRouteInformation();
            SendTrain();
            Console.WriteLine($"\nTrain successfully sent.");
        }
        public void ReadAnyKey(string text = "\nPress any key to continue...")
        {
            Console.Write(text);
            Console.ReadKey(true);
        }

        private void SendTrain()
        {
            Console.WriteLine("\n--------------- (Step 4 \\ 4) Send the train on its way ---------------");
            ReadAnyKey("Press any key to send train...");
            _trains[^1].SendTrain();
        }

        private void CreateNewTrain(int amountOfPassengers)
        {
            Console.WriteLine("\n--------------- (Step 3 \\ 4) Train formation ---------------");
            int amountOfSeatsInTrain = 0;

            Train train = new Train(new List<Wagon>(), amountOfPassengers);

            while (amountOfPassengers > amountOfSeatsInTrain)
            {
                int routeIndex = _routes.Count - 1;
                string departureCity = _routes[routeIndex].GetDepartureCityName();
                string destinationeCity = _routes[routeIndex].GetDestinationCityName();

                Console.WriteLine("\nNot enough seats. Need to add more wagons.");
                Console.WriteLine($"\nChoose type of wagon № {train.GetCountOfWagons() + 1} from below list for train {departureCity} - {destinationeCity}.");
                _wagonsType.ShowAllWagonTypes();
                int wagonTypeIndex = ReadIndex("\nWrite wagon type index to add it in train: ");

                FirstClassWagon firstClassWagon = new FirstClassWagon();
                SecondClassWagon secondClassWagon = new SecondClassWagon();

                if (_wagonsType.GetTypeName(wagonTypeIndex) == firstClassWagon.GetTypeName())
                {
                    train.AddWagon(new Wagon(firstClassWagon));
                }
                else if (_wagonsType.GetTypeName(wagonTypeIndex) == secondClassWagon.GetTypeName())
                {
                    train.AddWagon(new Wagon(secondClassWagon));
                }

                amountOfSeatsInTrain = train.GetSeatsAmount(); 
                Console.Write($"\nTrain {departureCity} - {destinationeCity} " +
                    $"consist of {train.GetCountOfWagons()} wagons.\nCurrent train capacity is {amountOfSeatsInTrain} \\ {amountOfPassengers}.");
                ReadAnyKey();
            }

            _trains.Add(train);
        }
        
        private int GenerateAmountOfPassengers()
        {
            Console.WriteLine("\n--------------- (Step 2 \\ 4) Ticket selling --------------- \n");
            int minTicketsSolded = 70;
            int maxTicketsSolded = 200;
            Random random = new Random();
            ReadAnyKey("Press any key to start selling tickets\n");
            int passengers = random.Next(minTicketsSolded, maxTicketsSolded);
            return passengers;
        }

        private void CreateRoute()
        {
            Console.WriteLine("--------------- (Step 1 \\ 4) Menu of Route Creation ---------------\n\nAvailabel cities:");
            _cityDataBase.ShowCities();
            int departureCityIndex = ReadIndex("\nWrite index of departure city:");
            int destinationCityIndex = ReadIndex("Write index of destination city:");
            string departureCityName = _cityDataBase.GetCityName(departureCityIndex);
            string destinationCityName = _cityDataBase.GetCityName(destinationCityIndex);
            City departureCity = new City(departureCityName);
            City destinationCity = new City(destinationCityName);
            _routes.Add(new Route(departureCity, destinationCity));
        }

        private int ReadIndex(string text)
        {
            bool indexIsOutOfRange = true;
            int index = -1;

            while (indexIsOutOfRange)
            {
                Console.Write(text);
                string choosenIndex = Console.ReadLine();

                if (int.TryParse(choosenIndex, out index))
                {
                    index--;

                    if (index >= 0 & index < _cityDataBase.GetCount())
                    {
                        indexIsOutOfRange = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error. Index is out of range.");
                    }
                }
                else
                {
                    Console.WriteLine("Error. Index can include only numbers.");
                }
            }

            return index;
        }
    }

    class Wagon
    {
        public int MaxCapacity { get; protected set; }
        public string Type { get; protected set; }
        
        public Wagon(Wagon wagon)
        {
            MaxCapacity = wagon.MaxCapacity;
            Type = wagon.Type;
        }

        protected Wagon() { }

        public int GetCapacity()
        {
            return MaxCapacity;
        }

        public string GetTypeName()
        {
            return Type;
        }
    }

    class FirstClassWagon : Wagon
    {
        public FirstClassWagon()
        {
            MaxCapacity = 36;
            Type = "First Class";
        }
    }

    class SecondClassWagon : Wagon
    {
        public SecondClassWagon()
        {
            MaxCapacity = 52;
            Type = "SecondClass";
        }
    }

    class WagonTypeDataBase
    {
        private List<Wagon> _wagons;

        public WagonTypeDataBase()
        {
            _wagons = new List<Wagon>() { new FirstClassWagon(), new SecondClassWagon() };
        }

        public string GetTypeName(int index)
        {
            return _wagons[index].GetTypeName();
        }

        public void ShowAllWagonTypes()
        {
            for (int i = 0; i < _wagons.Count; i++)
            {
                Console.WriteLine($"Index {i + 1}. Wagon type - {_wagons[i].GetTypeName()}. Capacity - {_wagons[i].GetCapacity()} passengers.");
            }
        }
    }

    class Train
    {
        private List<Wagon> _train;
        public bool IsSent { get; private set; }
        public int Passengers { get; private set; }

        public Train(List<Wagon> wagons, int passengers)
        {
            _train = wagons;
            Passengers = passengers;
            IsSent = false;
        }

        public int GetCountOfWagons()
        {
            return _train.Count;
        }

        public void AddWagon(Wagon wagon)
        {
            _train.Add(wagon);
        }

        public int GetWagonCapacity(int index)
        {
            return _train[index].GetCapacity();
        }

        public int GetSeatsAmount()
        {
            int amountOfSeatsInTrain = 0;

            for (int i = 0; i < GetCountOfWagons(); i++)
            {
                amountOfSeatsInTrain += GetWagonCapacity(i);
            }

            return amountOfSeatsInTrain;
        }

        public void SendTrain()
        {
            IsSent = true;
        }
    }
}