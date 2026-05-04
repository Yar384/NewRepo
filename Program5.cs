using System;
using System.Collections.Generic;
using Dapper;
using DogApp;

class Program
{
    static void Main(string[] args)
    {
        Database.Init();

        while (true)
        {
            Console.WriteLine("1. Добавить собаку");
            Console.WriteLine("2. Все собаки");
            Console.WriteLine("3. Собаки в приюте");
            Console.WriteLine("4. Усыновленные собаки");
            Console.WriteLine("5. Поиск собаки");
            Console.WriteLine("0. Виход");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") AddDog();
            else if (choice == "2") ShowAll();
            else if (choice == "3") ShowInShelter();
            else if (choice == "4") ShowAdopted();
            else if (choice == "5") SearchMenu();
            else if (choice == "0")
            {
                break;
            }
            else Console.WriteLine("Неверный выбор");

            Console.WriteLine();
        }
    }

    static void AddDog()
    {
        Console.WriteLine("0. Назад");
        Console.WriteLine();

        Console.Write("Кличка: ");
        string name = Console.ReadLine();
        if (name == "0") return;

        Console.Write("Возрост: ");
        string ageInput = Console.ReadLine();
        if (ageInput == "0") return;
        int age = int.Parse(ageInput);

        Console.Write("Порода: ");
        string breed = Console.ReadLine();
        if (breed == "0") return;

        var con = Database.GetConnection();
        con.Open();

        con.Execute(
            "INSERT INTO Dogs (Name, Age, Breed, IsAdopted) VALUES (@Name, @Age, @Breed, 0)",
            new { Name = name, Age = age, Breed = breed }
        );

        con.Close();

        Console.WriteLine("добавлена");
    }

    static void ShowAll()
    {
        while (true)
        {
            var con = Database.GetConnection();
            con.Open();

            var dogs = con.Query<Dog>("SELECT * FROM Dogs").AsList();
            con.Close();

            PrintDogs(dogs);

            Console.WriteLine("0. Назад");
            Console.Write(">> ");
            string choice = Console.ReadLine();
            if (choice == "0") return;

            Console.WriteLine();
        }
    }

    static void ShowInShelter()
    {
        while (true)
        {
            var con = Database.GetConnection();
            con.Open();

            var dogs = con.Query<Dog>("SELECT * FROM Dogs WHERE IsAdopted = 0").AsList();
            con.Close();

            PrintDogs(dogs);

            Console.WriteLine("0. Назад");
            Console.Write(">> ");
            string choice = Console.ReadLine();
            if (choice == "0") return;

            Console.WriteLine();
        }
    }

    static void ShowAdopted()
    {
        while (true)
        {
            var con = Database.GetConnection();
            con.Open();

            var dogs = con.Query<Dog>("SELECT * FROM Dogs WHERE IsAdopted = 1").AsList();
            con.Close();

            PrintDogs(dogs);

            Console.WriteLine("0. Назад");
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
            Console.WriteLine("1. Поиск по кличке");
            Console.WriteLine("2. Поиск по ID");
            Console.WriteLine("3. Поиск по породе");
            Console.WriteLine("0. Назад");
            Console.Write(">> ");

            string choice = Console.ReadLine();

            if (choice == "1") SearchByName();
            else if (choice == "2") SearchById();
            else if (choice == "3") SearchByBreed();
            else if (choice == "0") return;
            else Console.WriteLine("Неверный выбор");

            Console.WriteLine();
        }
    }

    static void SearchByName()
    {
        Console.Write("Кличка: ");
        string name = Console.ReadLine();

        var con = Database.GetConnection();
        con.Open();

        var dogs = con.Query<Dog>(
            "SELECT * FROM Dogs WHERE Name = @Name",
            new { Name = name }
        ).AsList();

        con.Close();

        PrintDogs(dogs);
    }

    static void SearchById()
    {
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());

        var con = Database.GetConnection();
        con.Open();

        var dogs = con.Query<Dog>(
            "SELECT * FROM Dogs WHERE Id = @Id",
            new { Id = id }
        ).AsList();

        con.Close();

        PrintDogs(dogs);
    }

    static void SearchByBreed()
    {
        Console.Write("Порода: ");
        string breed = Console.ReadLine();

        var con = Database.GetConnection();
        con.Open();

        var dogs = con.Query<Dog>(
            "SELECT * FROM Dogs WHERE Breed = @Breed",
            new { Breed = breed }
        ).AsList();

        con.Close();

        PrintDogs(dogs);
    }

    static void PrintDogs(System.Collections.Generic.List<Dog> dogs)
    {
        if (dogs.Count == 0)
        {
            Console.WriteLine("нету");
            Console.WriteLine();
            return;
        }

        Console.WriteLine("ID | Кличка | Возрост | Порода | Усыновлений");
        Console.WriteLine("------------------------------------------");

        foreach (var d in dogs)
        {
            string adopted = d.IsAdopted ? "да" : "нет";
            Console.WriteLine(d.Id + " | " + d.Name + " | " + d.Age + " | " + d.Breed + " | " + adopted);
        }

        Console.WriteLine();
    }
}
