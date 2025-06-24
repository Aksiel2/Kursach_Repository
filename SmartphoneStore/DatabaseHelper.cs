using System.Data.SQLite;
using Dapper;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.Generic;
public static class DatabaseHelper
{
    private static string dbFilePath = "smartphones.db";
    private static string connectionString = $"Data Source={dbFilePath};Version=3;";

    public static void InitializeDatabase()
    {
        bool isNewDatabase = !File.Exists(dbFilePath);

        if (isNewDatabase)
        {
            SQLiteConnection.CreateFile(dbFilePath);
        }

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Smartphones (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Brand TEXT NOT NULL,
                    Model TEXT NOT NULL,
                    Price DECIMAL NOT NULL,
                    Storage INTEGER NOT NULL,
                    RAM INTEGER NOT NULL,
                    Processor TEXT NOT NULL,
                    ScreenSize REAL NOT NULL,
                    BatteryCapacity INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL
                )");

            if (isNewDatabase)
            {
                connection.Execute(@"
                    INSERT INTO Smartphones 
                    (Brand, Model, Price, Storage, RAM, Processor, ScreenSize, BatteryCapacity, Quantity)
                    VALUES 
                    ('Samsung', 'Galaxy S21', 799.99, 128, 8, 'Exynos 2100', 6.2, 4000, 10),
                    ('Apple', 'iPhone 13', 999.00, 256, 4, 'A15 Bionic', 6.1, 3240, 15),
                    ('Xiaomi', 'Redmi Note 10', 249.99, 64, 4, 'Snapdragon 678', 6.43, 5000, 20)");
            }
        }
    }

    public static DataTable GetAllSmartphones()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            var table = new DataTable();
            using (var adapter = new SQLiteDataAdapter("SELECT * FROM Smartphones", connection))
            {
                adapter.Fill(table);
            }
            return table;
        }
    }

    public static void AddSmartphone(Smartphone smartphone)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Execute(
                @"INSERT INTO Smartphones 
                (Brand, Model, Price, Storage, RAM, Processor, ScreenSize, BatteryCapacity, Quantity) 
                VALUES (@Brand, @Model, @Price, @Storage, @RAM, @Processor, @ScreenSize, @BatteryCapacity, @Quantity)",
                smartphone);
        }
    }

    public static void UpdateSmartphone(Smartphone smartphone)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Execute(
                @"UPDATE Smartphones SET 
                Brand = @Brand, 
                Model = @Model, 
                Price = @Price, 
                Storage = @Storage, 
                RAM = @RAM, 
                Processor = @Processor, 
                ScreenSize = @ScreenSize, 
                BatteryCapacity = @BatteryCapacity,
                Quantity = @Quantity
                WHERE Id = @Id",
                smartphone);
        }
    }

    public static void DeleteSmartphone(int id)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Execute("DELETE FROM Smartphones WHERE Id = @Id", new { Id = id });
        }
    }

    public static DataTable SearchSmartphones(string searchTerm)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            string query;
            object parameters;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Если поиск пустой - возвращаем все записи
                query = "SELECT * FROM Smartphones";
                parameters = null;
            }
            else
            {
                query = @"SELECT * FROM Smartphones 
                     WHERE Brand LIKE @SearchTerm 
                     OR Model LIKE @SearchTerm 
                     OR Processor LIKE @SearchTerm";
                parameters = new { SearchTerm = $"%{searchTerm}%" };
            }

            var table = new DataTable();
            using (var adapter = new SQLiteDataAdapter(query, connection))
            {
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                }
                adapter.Fill(table);
            }
            return table;
        }
    }
}