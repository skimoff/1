using System.Data.SQLite;

namespace TypingTest.Core;

public class Database
{
    private readonly string _connection = "Data Source=users.db;Version=3;";

    public Database()
    {
        using var connection = new SQLiteConnection(_connection);
        connection.Open();

        string createTable = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT UNIQUE NOT NULL,
                Password TEXT NOT NULL
            );";
        new SQLiteCommand(createTable, connection).ExecuteNonQuery();
    }

    public bool AddUser(string username, string password)
    {
        using var connection = new SQLiteConnection(_connection);
        connection.Open();

        string insert = @"INSERT INTO Users (Username, Password) VALUES (@u, @p)";
        try
        {
            using var command = new SQLiteCommand(insert, connection);
            command.Parameters.AddWithValue("@u", username);
            command.Parameters.AddWithValue("@p", password);
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public bool CheckUser(string username, string password)
    {
        using var connection = new SQLiteConnection(_connection);
        connection.Open();

        string query = "SELECT COUNT(*) FROM Users WHERE Username=@u AND Password=@p";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@u", username);
        command.Parameters.AddWithValue("@p", password);
        return (long)command.ExecuteScalar() > 0;
    }
}