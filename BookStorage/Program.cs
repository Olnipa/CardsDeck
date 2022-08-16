namespace BookStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>();
            DataBase booksData = new DataBase(books);
            bool isWorking = true;

            Console.WriteLine($"Welkome to the Library!");

            while (isWorking)
            {
                const string AddBook = "1";
                const string ShowBooks = "2";
                const string RemoveBook = "3";
                const string SearchBook = "4";
                const string Exit = "0";

                Console.Write($"\nChoose what you want to do:\n\n{AddBook} - Add book to storage\n{ShowBooks} - Show all books in storage" +
                    $"\n{RemoveBook} - Remove book from storage\n{SearchBook} - Find book by...\n{Exit} - Exit\n\nEnter number: ");
                string choosenMenu = Console.ReadLine();

                switch (choosenMenu)
                {
                    case AddBook:
                        booksData.AddBook(new Book(booksData.ReadName("book"), booksData.ReadNumber("year of publishing"), booksData.ReadName("author")));
                        break;
                    case ShowBooks:
                        booksData.ShowAllBooks();
                        break;
                    case RemoveBook:
                        booksData.RemoveBook();
                        break;
                    case SearchBook:
                        booksData.ShowSomeBooks(booksData.FindIndexes());
                        break;
                    case Exit:
                        isWorking = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    class Book
    {
        public string Name { get; private set; }
        public int PublicationYear { get; private set; }
        public string AuthorName { get; private set; }

        public Book(string name, int publicationYear, string authorName)
        {
            Name = name;
            PublicationYear = publicationYear;
            AuthorName = authorName;
        }
    }

    class DataBase
    {
        private List<Book> _books;

        public DataBase(List<Book> books)
        {
            books.AddRange(new List<Book> {
                new Book("Lord Of The Ring", 1954, "Tolkien"),
                new Book("Game development for beginners", 2014, "Roman"),
                new Book("Spin of Shapoklyak", 2022, "Uspensky"),
                new Book("Vinnipuh: reload", 2022, "Alan")
            });
            _books = books;
        }

        public void RemoveBook()
        {
            const string DeleteBook = "1";
            const string Cancel = "0";

            Console.WriteLine("\nChoose book which you need to remove.");
            int index = ReadNumber("book index") - 1;

            Console.WriteLine("Are you shure, you want to remove below book?\n");
            ShowBook(index);
            Console.Write($"\n{DeleteBook} - remove this book\n{Cancel} - cancel\n\nEnter value: ");
            string choosenMenu = Console.ReadLine();

            if (choosenMenu == DeleteBook)
            {
                _books.RemoveAt(index);
                Console.WriteLine("Book was successfully removed");
            }
            else
            {
                Console.WriteLine("Canceled.");
            }
        }

        public List<int> FindIndexes()
        {
            List<int> indexes = new List<int>();
            indexes = ReturnIndex();
            return indexes;
        }

        public int ReadNumber(string valueNameForFind)
        {
            int value;

            Console.Write($"Enter {valueNameForFind}: ");

            while (int.TryParse(Console.ReadLine(), out value) == false)
            {
                Console.Write($"Entered value is incorrect. You can use only numbers.\nEnter {valueNameForFind}: ");
            }

            return value;
        }

        public string ReadName(string text)
        {
            string value = "";
            int minLength = 2;

            while (value.Length < minLength)
            {
                Console.Write("Enter " + text + " name (value must include at least " + minLength + " symbols): ");
                value = Console.ReadLine();
            }

            return value;
        }

        public void ShowSomeBooks(List<int> indexes)
        {
            if (indexes.Count == 0)
            {
                Console.WriteLine("Books was not found.");
            }
            else
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    ShowBook(indexes[i]);
                }
            }
        }

        public void ShowAllBooks()
        {
            Console.WriteLine();

            for (int i = 0; i < _books.Count; i++)
            {
                ShowBook(i);
            }
        }

        public void AddBook(Book newBook)
        {
            _books.Add(newBook);
            Console.WriteLine("Book was successfully added");
        }

        private List<int> ReturnIndex()
        {
            string searchFrase = ReadName("year of publishing, author or book");
            List<int> indexes = new List<int>();

            Console.WriteLine();

            for (int i = 0; i < _books.Count; i++)
            {
                if (_books[i].Name.ToLower() == searchFrase.ToLower() || Convert.ToString(_books[i].PublicationYear).ToLower() == searchFrase.ToLower() || _books[i].AuthorName.ToLower() == searchFrase.ToLower())
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        private void ShowBook(int index)
        {
            if (index < 0 || index >= _books.Count)
            {
                Console.WriteLine("No results were found with your request.");
            }
            else
            {
                Console.WriteLine($"{index + 1}. Book name: {_books[index].Name} | Author: {_books[index].AuthorName} | Published: {_books[index].PublicationYear}");
            }
        }
    }
}