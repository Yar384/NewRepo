using System;
using System.Linq;
using MovieApp;

class Program
{
    static void Main(string[] args)
    {
        MovieContext db = new MovieContext();
        db.Database.EnsureCreated();
        db.Dispose();

        while (true)
        {
            Console.WriteLine("1. Добавить юзера");
            Console.WriteLine("2. Добавить фильм");
            Console.WriteLine("3. Все юзеры");
            Console.WriteLine("4. Все фильмы");
            Console.WriteLine("0. Выход");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") AddUser();
            else if (choice == "2") AddMovie();
            else if (choice == "3") ShowUsers();
            else if (choice == "4") ShowMovies();
            else if (choice == "0") break;
            else Console.WriteLine("");

            Console.WriteLine();
        }
    }

    static void AddUser()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        MovieContext db = new MovieContext();

        bool exists = db.Users.Any(u => u.Username == username || u.Email == email);

        if (exists)
        {
            Console.WriteLine("такой уже имеется");
            db.Dispose();
            return;
        }

        User user = new User();
        user.Username = username;
        user.Email = email;
        user.Password = password;

        db.Users.Add(user);
        db.SaveChanges();
        db.Dispose();

        Console.WriteLine("добавлен");
    }

    static void AddMovie()
    {
        MovieContext db = new MovieContext();

        var users = db.Users.ToList();

        if (users.Count == 0)
        {
            Console.WriteLine("сначало юзера");
            db.Dispose();
            return;
        }

        Console.WriteLine("Выбери юзера:");
        foreach (var u in users)
        {
            Console.WriteLine(u.Id + " - " + u.Username);
        }

        Console.Write("ID: ");
        int userId = int.Parse(Console.ReadLine());

        Console.Write("Название: ");
        string title = Console.ReadLine();

        Console.Write("Год: ");
        int year = int.Parse(Console.ReadLine());

        Console.Write("Дескриптион: ");
        string description = Console.ReadLine();

        Movie movie = new Movie();
        movie.Title = title;
        movie.Year = year;
        movie.Description = description == "" ? null : description;
        movie.UserId = userId;
        movie.AddedDate = DateTime.Now;

        db.Movies.Add(movie);
        db.SaveChanges();
        db.Dispose();

        Console.WriteLine("добавлен");
    }

    static void ShowUsers()
    {
        MovieContext db = new MovieContext();
        var users = db.Users.ToList();
        db.Dispose();

        if (users.Count == 0)
        {
            Console.WriteLine("нету");
            return;
        }

        Console.WriteLine("ID | Username | Email");
        Console.WriteLine("---------------------");

        foreach (var u in users)
        {
            Console.WriteLine(u.Id + " | " + u.Username + " | " + u.Email);
        }
    }

    static void ShowMovies()
    {
        MovieContext db = new MovieContext();
        var movies = db.Movies.ToList();
        db.Dispose();

        if (movies.Count == 0)
        {
            Console.WriteLine("нету");
            return;
        }

        Console.WriteLine("ID | Название | Год | Дескриптион | Дата | ID");
        Console.WriteLine("-------------------------------------------------------------");

        foreach (var m in movies)
        {
            Console.WriteLine(m.Id + " | " + m.Title + " | " + m.Year + " | " + m.Description + " | " + m.AddedDate + " | " + m.UserId);
        }
    }
}
