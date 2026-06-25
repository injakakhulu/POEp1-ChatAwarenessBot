using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using POEp1.Models;

namespace POEp1.Services
{
    public static class DatabaseService
    {
        // Adjust server configurations to point directly to your database setup.
        private static readonly string _connectionString = "server=127.0.0.1;port=3306;uid=root;pwd=;database=cyberbot_db;";

        static DatabaseService()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    // Automatically provision operational data schemas on startup if missing.
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS cybersecurity_tasks (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            title VARCHAR(255) NOT NULL,
                            description TEXT,
                            reminder_days INT DEFAULT 0,
                            is_completed BOOLEAN DEFAULT FALSE,
                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (var cmd = new MySqlCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database Init Error: {ex.Message}");
            }
        }

        public static bool AddTask(string title, string desc, int reminderDays)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO cybersecurity_tasks (title, description, reminder_days) VALUES (@title, @desc, @remind);";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@desc", desc);
                        cmd.Parameters.AddWithValue("@remind", reminderDays);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public static List<CyberTask> GetAllTasks()
        {
            var list = new List<CyberTask>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM cybersecurity_tasks ORDER BY id DESC;";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new CyberTask
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Title = reader["title"].ToString(),
                                Description = reader["description"].ToString(),
                                ReminderDays = Convert.ToInt32(reader["reminder_days"]),
                                IsCompleted = Convert.ToBoolean(reader["is_completed"])
                            });
                        }
                    }
                }
            }
            catch { }
            return list;
        }

        public static bool MarkTaskCompleted(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "UPDATE cybersecurity_tasks SET is_completed = TRUE WHERE id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public static bool DeleteTask(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM cybersecurity_tasks WHERE id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }
    }
}