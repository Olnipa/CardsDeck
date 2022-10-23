namespace AnarchyInTheHospital
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hospital hospital = new Hospital();
            hospital.StartWork();
        }
    }

    enum Diseases
    {
        Cold,
        Covid19,
        Asthma,
        AcuteRespiratoryInfections,
        Pneumonia
    }

    class Hospital
    {
        private List<Patient> _patients = new List<Patient>();

        public Hospital()
        {
            _patients.Add(new Patient("Ginny", UserUtils.GetRandomNumber(), Diseases.Cold));
            _patients.Add(new Patient("Severus", UserUtils.GetRandomNumber(), Diseases.Pneumonia));
            _patients.Add(new Patient("Nikolos", UserUtils.GetRandomNumber(), Diseases.Covid19));
            _patients.Add(new Patient("Draco", UserUtils.GetRandomNumber(), Diseases.Covid19));
            _patients.Add(new Patient("Minerva", UserUtils.GetRandomNumber(), Diseases.Asthma));
            _patients.Add(new Patient("Albus", UserUtils.GetRandomNumber(), Diseases.AcuteRespiratoryInfections));
            _patients.Add(new Patient("Lupin", UserUtils.GetRandomNumber(), Diseases.Covid19));
            _patients.Add(new Patient("Ron", UserUtils.GetRandomNumber(), Diseases.Covid19));
            _patients.Add(new Patient("Germiona", UserUtils.GetRandomNumber(), Diseases.Cold));
            _patients.Add(new Patient("Harry", UserUtils.GetRandomNumber(), Diseases.AcuteRespiratoryInfections));
            _patients.Add(new Patient("VolanDeMort", UserUtils.GetRandomNumber(), Diseases.Covid19));
        }

        public void StartWork()
        {
            bool isWorking = true;
            Console.WriteLine("Welcome to the Hogwartspital!\n\nWhat you want to do?");

            while (isWorking)
            {
                const string SortingByName = "1";
                const string SortingByAge = "2";
                const string ChooseDisease = "3";
                const string Exit = "0";
                Console.Write($"{SortingByName} - Sort by name\n{SortingByAge} - Sort by age\n{ChooseDisease} - Show patients with \n{Exit} - Exit\n\nChoosen Menu:");
                string ChoosenMenu = Console.ReadLine();
                Console.WriteLine();

                switch (ChoosenMenu)
                {
                    case SortingByName:
                        SortByName();
                        break;
                    case SortingByAge:
                        SortByAge();
                        break;
                    case ChooseDisease:
                        FilterPatientsByDisease();
                        break;
                    case Exit:
                        isWorking = false;
                        break;
                    default:
                        break;
                }

                Console.Write("\nPress any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
            }

            Console.WriteLine("Goodbye!");
        }

        private void ShowPatients(List<Patient> patients)
        {
            foreach (Patient patient in patients)
            {
                Console.WriteLine($"Name: {patient.Name}. Age: {patient.Age}. Disease: {patient.Disease}.");
            }
        }

        private void SortByName()
        {
            List<Patient> filteredPatients = _patients.OrderBy(_patients => _patients.Name).ToList();
            ShowPatients(filteredPatients);
        }

        private void SortByAge()
        {
            List<Patient> filteredPatients = _patients.OrderBy(_patients => _patients.Age).ToList();
            ShowPatients(filteredPatients);
        }

        private void FilterPatientsByDisease()
        {
            Console.Write("Write disease:");
            string choosenDisease = Console.ReadLine().ToLower();
            List<Patient> filteredPatients = _patients.Where(_patients => _patients.Disease.ToString().ToLower() == choosenDisease).ToList();
            Console.WriteLine();

            if (filteredPatients.Count > 0)
            {
                ShowPatients(filteredPatients);
            }
            else
            {
                Console.WriteLine($"No patients with disease \"{choosenDisease}\"");
            }
        }
    }

    class Patient
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public Diseases Disease { get; private set; }

        public Patient(string name, int age, Diseases disease)
        {
            Name = name;
            Age = age;
            Disease = disease;
        }
    }

    class UserUtils
    {
        public static int GetRandomNumber(int minNumber = 16, int maxNumber = 110)
        {
            Random random = new Random();
            return random.Next(minNumber, maxNumber);
        }
    }
}