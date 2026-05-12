using System;
using Dapper;
using LibraryApp;

class Program
{
    static void Main(string[] args)
    {
        Database.Init();

        while (true)
        {
            Console.WriteLine("1 - Додати книгу");
            Console.WriteLine("2 - Всі книги");
            Console.WriteLine("3 - Пошук книги");
            Console.WriteLine("4 - Видалити книгу за ID");
            Console.WriteLine("5 - Оновити книгу за ID");
            Console.WriteLine("0 - Вихід");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") AddBook();
            else if (choice == "2") ShowAll();
            else if (choice == "3") SearchMenu();
            else if (choice == "4") DeleteBook();
            else if (choice == "5") UpdateBook();
            else if (choice == "0")
            {
                break;
            }
            else Console.WriteLine("невірний вибір");

            Console.WriteLine();
        }
    }

    static void AddBook()
    {
        Console.WriteLine("0 - назад");
        Console.WriteLine();

        Console.Write("Назва: ");
        string title = Console.ReadLine();
        if (title == "0") return;

        Console.Write("Автор: ");
        string author = Console.ReadLine();
        if (author == "0") return;

        var con = Database.GetConnection();
        con.Open();

        con.Execute(
            "INSERT INTO Books (Title, Author) VALUES (@Title, @Author)",
            new { Title = title, Author = author }
        );

        con.Close();

        Console.WriteLine("додано");
    }

    static void ShowAll()
    {
        while (true)
        {
            var con = Database.GetConnection();
            con.Open();

            var books = con.Query<Book>("SELECT * FROM Books").AsList();
            con.Close();

            PrintBooks(books);

            Console.WriteLine("0 - назад");
            Console.Write(">> ");
            string choice = Console.ReadLine();
            if (choice == "0") return;

            Console.WriteLine();
        }
    }

    static void SearchMenu()
    {
        while (true)
        {
            Console.WriteLine("1 - за назвою");
            Console.WriteLine("2 - за автором");
            Console.WriteLine("3 - за ID");
            Console.WriteLine("0 - назад");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") SearchByTitle();
            else if (choice == "2") SearchByAuthor();
            else if (choice == "3") SearchById();
            else if (choice == "0") return;
            else Console.WriteLine("невірний вибір");

            Console.WriteLine();
        }
    }

    static void SearchByTitle()
    {
        Console.Write("Назва: ");
        string title = Console.ReadLine();

        var con = Database.GetConnection();
        con.Open();

        var books = con.Query<Book>(
            "SELECT * FROM Books WHERE Title = @Title",
            new { Title = title }
        ).AsList();

        con.Close();

        PrintBooks(books);
    }

    static void SearchByAuthor()
    {
        Console.Write("Автор: ");
        string author = Console.ReadLine();

        var con = Database.GetConnection();
        con.Open();

        var books = con.Query<Book>(
            "SELECT * FROM Books WHERE Author = @Author",
            new { Author = author }
        ).AsList();

        con.Close();

        PrintBooks(books);
    }

    static void SearchById()
    {
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());

        var con = Database.GetConnection();
        con.Open();

        var books = con.Query<Book>(
            "SELECT * FROM Books WHERE Id = @Id",
            new { Id = id }
        ).AsList();

        con.Close();

        PrintBooks(books);
    }

    static void DeleteBook()
    {
        Console.WriteLine("0 - назад");
        Console.WriteLine();

        Console.Write("ID: ");
        string input = Console.ReadLine();
        if (input == "0") return;

        int id = int.Parse(input);

        var con = Database.GetConnection();
        con.Open();

        int rows = con.Execute(
            "DELETE FROM Books WHERE Id = @Id",
            new { Id = id }
        );

        con.Close();

        if (rows == 0) Console.WriteLine("не знайдено");
        else Console.WriteLine("видалено");
    }

    static void UpdateBook()
    {
        Console.WriteLine("0 - назад");
        Console.WriteLine();

        Console.Write("ID: ");
        string input = Console.ReadLine();
        if (input == "0") return;

        int id = int.Parse(input);

        var con = Database.GetConnection();
        con.Open();

        var book = con.QueryFirstOrDefault<Book>(
            "SELECT * FROM Books WHERE Id = @Id",
            new { Id = id }
        );

        con.Close();

        if (book == null)
        {
            Console.WriteLine("не знайдено");
            return;
        }

        Console.WriteLine("Поточна назва: " + book.Title);
        Console.Write("Нова назва: ");
        string newTitle = Console.ReadLine();

        Console.WriteLine("Поточний автор: " + book.Author);
        Console.Write("Новий автор: ");
        string newAuthor = Console.ReadLine();

        if (newTitle != "") book.Title = newTitle;
        if (newAuthor != "") book.Author = newAuthor;

        con = Database.GetConnection();
        con.Open();

        con.Execute(
            "UPDATE Books SET Title = @Title, Author = @Author WHERE Id = @Id",
            new { Title = book.Title, Author = book.Author, Id = id }
        );

        con.Close();

        Console.WriteLine("оновлено");
    }

    static void PrintBooks(System.Collections.Generic.List<Book> books)
    {
        if (books.Count == 0)
        {
            Console.WriteLine("не знайдено");
            Console.WriteLine();
            return;
        }

        Console.WriteLine("ID | Назва | Автор");
        Console.WriteLine("------------------");

        foreach (var b in books)
        {
            Console.WriteLine(b.Id + " | " + b.Title + " | " + b.Author);
        }

        Console.WriteLine();
    }
}
