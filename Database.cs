using Microsoft.Data.Sqlite;
using Dapper;

namespace DogApp
{
    public class Database
    {
        static string connectionString = "Data Source=dogs.db";

        public static void Init()
        {
            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            con.Execute(
                "CREATE TABLE IF NOT EXISTS Dogs (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Name TEXT," +
                "Age INTEGER," +
                "Breed TEXT," +
                "IsAdopted INTEGER DEFAULT 0)"
            );

            con.Close();
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
