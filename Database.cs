using Microsoft.Data.Sqlite;
using Dapper;

namespace LibraryApp
{
    public class Database
    {
        static string connectionString = "Data Source=library.db";

        public static void Init()
        {
            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            con.Execute(
                "CREATE TABLE IF NOT EXISTS Books (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Title TEXT," +
                "Author TEXT)"
            );

            con.Close();
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
