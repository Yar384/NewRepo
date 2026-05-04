using System;
using System.Linq;
using MoviesApp;

class Program
{
    static void Main(string[] args)
    {
        MoviesContext db = new MoviesContext();
        db.Database.EnsureCreated();
        db.Dispose();

        while (true)
        {
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Посмотреть юзеров");
            Console.WriteLine("0. Выход");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") RegisterUser();
            else if (choice == "2") ShowUsers();
            else if (choice == "0")
            {
                break;
            }
            else Console.WriteLine("неверный выбо");

            Console.WriteLine();
        }
    }

    static void RegisterUser()
    {
        while (true)
        {
            Console.WriteLine("0. Назад");
            Console.WriteLine();

            Console.Write("Имя: ");
            string name = Console.ReadLine();

            if (name == "0") return;

            Console.Write("Логин: ");
            string login = Console.ReadLine();

            if (login == "0") return;

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            if (password == "0") return;

            MoviesContext db = new MoviesContext();

            bool exists = db.Users.Any(u => u.Login == login);

            if (exists)
            {
                Console.WriteLine("этот уже занят");
                db.Dispose();
                continue;
            }

            User user = new User();
            user.Name = name;
            user.Login = login;
            user.Password = password;

            db.Users.Add(user);
            db.SaveChanges();
            db.Dispose();

            Console.WriteLine("зарегестрирован");
            Console.WriteLine();
            return;
        }
    }

    static void ShowUsers()
    {
        while (true)
        {
            MoviesContext db = new MoviesContext();
            var users = db.Users.ToList();
            db.Dispose();

            if (users.Count == 0)
            {
                Console.WriteLine("нету");
            }
            else
            {
                Console.WriteLine("ID | Имя | Логин");
                Console.WriteLine("------------------");

                foreach (var u in users)
                {
                    Console.WriteLine(u.Id + " | " + u.Name + " | " + u.Login);
                }
            }

            Console.WriteLine();
            Console.WriteLine("0. Назад");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "0") return;
            else Console.WriteLine(">> ");

            Console.WriteLine();
        }
    }
}
