namespace BookStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>()
            {
                new Book("Lord Of The Ring", 1954, "Tolkien"),
                new Book("Game development for beginners", 2014, "Roman"),
                new Book("Spin of Shapoklyak", 2022, "Uspensky"),
                new Book("Vinnipuh: reload", 2022, "Alan")
            };
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
        public List<Book> Books { get; private set; }

        public DataBase(List<Book> books)
        {
            Books = books;
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

            if (Console.ReadLine() == "1")
            {
                Books.RemoveAt(index);
                Console.WriteLine("Book was successfully removed");
            }
            else
            {
                Console.WriteLine("Canceled.");
            }
        }

        public List<int> FindIndexes()
        {
            const string FindByBookName = "1";
            const string FindByAuthorName = "2";
            const string FindByYear = "3";
            const string FindByIndex = "4";
            const string Cancel = "0";
            List<int> indexes = new List<int>();

            Console.Write($"\n{FindByBookName} - Find book by name\n{FindByAuthorName} - Find book by author name" +
                $"\n{FindByYear} - Find book by year\n{FindByIndex} - Find book by index\n{Cancel} - Cancel" +
                $"\n\nEnter number: ");
            
            switch (Console.ReadLine())
            {
                case FindByBookName:
                    indexes = ReturnIndexByBookName();
                    break;
                case FindByAuthorName:
                    indexes = ReturnIndexByAuthorName();
                    break;
                case FindByYear:
                    indexes = ReturnIndexByYear();
                    break;
                case FindByIndex:
                    indexes.Add(ReadNumber("book index") - 1);
                    break;
                case Cancel:
                    break;
            }

            return indexes;
        }

        public List<int> ReturnIndexByYear()
        {
            int year = ReadNumber("year of publishig");
            List<int> indexes = new List<int>();

            Console.WriteLine();

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].PublicationYear == year)
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        public List<int> ReturnIndexByBookName()
        {
            string bookName = ReadName("book");
            List<int> indexes = new List<int>();

            Console.WriteLine();

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].Name.ToLower() == bookName.ToLower())
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        public List<int> ReturnIndexByAuthorName()
        {
            string authorName = ReadName("author");
            List<int> indexes = new List<int>();

            Console.WriteLine();

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].AuthorName.ToLower() == authorName.ToLower())
                {
                    indexes.Add(i);
                }
            }

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

            for (int i = 0; i < Books.Count; i++)
            {
                ShowBook(i);
            }
        }

        public void ShowBook(int index)
        {
            if (index < 0 || index >= Books.Count)
            {
                Console.WriteLine("No results were found with your request.");
            }
            else
            {
                Console.WriteLine($"{index + 1}. Book name: {Books[index].Name} | Author: {Books[index].AuthorName} | Published: {Books[index].PublicationYear}");
            }
        }

        public void AddBook(Book newBook)
        {
            Books.Add(newBook);
            Console.WriteLine("Book was successfully added");
        }
    }
}