using System;
using System.Data.SQLite;

namespace UserApp
{
    class Program
    {
        static string connectionString = "Data Source=users.db;Version=3;";
        static void Main(string[] args)
        {
            CreateTable();

            while (true)
            {
                Console.WriteLine("1. Добавить пользователя");
                Console.WriteLine("2. Показать всех пользователей");
                Console.WriteLine("3. Поиск по username");
                Console.WriteLine("4. Поиск по email");
                Console.WriteLine("5. Удалить пользователя");
                Console.WriteLine("0. Выход");
                Console.Write(">> ");

                string choice = Console.ReadLine();

                if (choice == "1") AddUser();
                else if (choice == "2") ShowUsers();
                else if (choice == "3") SearchByUsername();
                else if (choice == "4") SearchByEmail();
                else if (choice == "5") DeleteUser();
                else if (choice == "0") break;
                else Console.WriteLine("неверный выбор");

                Console.WriteLine();
            }
        }

        static void CreateTable()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS Users (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Username TEXT," +
                "Email TEXT," +
                "BirthDate TEXT)", con);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        static void AddUser()
        {
            Console.Write("username: ");
            string username = Console.ReadLine();

            Console.Write("email: ");
            string email = Console.ReadLine();

            Console.Write("(гггг-мм-дд): ");
            string birthdate = Console.ReadLine();

            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "INSERT INTO Users (Username, Email, BirthDate) VALUES (@u, @e, @b)", con);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@b", birthdate);

            cmd.ExecuteNonQuery();
            con.Close();

            Console.WriteLine("добавлен");
        }

        static void ShowUsers()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Users", con);
            SQLiteDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("ID | Username | Email | BirthDate");
            Console.WriteLine("-----------------------------------");

            while (reader.Read())
            {
                Console.WriteLine(reader["Id"] + " | " + reader["Username"] + " | " + reader["Email"] + " | " + reader["BirthDate"]);
            }

            reader.Close();
            con.Close();
        }

        static void SearchByUsername()
        {
            Console.Write("username: ");
            string username = Console.ReadLine();

            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "SELECT * FROM Users WHERE Username = @u", con);

            cmd.Parameters.AddWithValue("@u", username);
            SQLiteDataReader reader = cmd.ExecuteReader();

            bool found = false;

            while (reader.Read())
            {
                found = true;
                Console.WriteLine(reader["Id"] + " | " + reader["Username"] + " | " + reader["Email"] + " | " + reader["BirthDate"]);
            }

            if (!found) Console.WriteLine("не найден");

            reader.Close();
            con.Close();
        }

        static void SearchByEmail()
        {
            Console.Write("email: ");
            string email = Console.ReadLine();

            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "SELECT * FROM Users WHERE Email = @e", con);

            cmd.Parameters.AddWithValue("@e", email);
            SQLiteDataReader reader = cmd.ExecuteReader();

            bool found = false;

            while (reader.Read())
            {
                found = true;
                Console.WriteLine(reader["Id"] + " | " + reader["Username"] + " | " + reader["Email"] + " | " + reader["BirthDate"]);
            }

            if (!found) Console.WriteLine("не найден");

            reader.Close();
            con.Close();
        }

        static void DeleteUser()
        {
            Console.Write("ID: ");
            string id = Console.ReadLine();

            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "DELETE FROM Users WHERE Id = @id", con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();

            Console.WriteLine("удалён");
        }
    }
}
