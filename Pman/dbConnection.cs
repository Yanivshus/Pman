using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pman
{
    internal class dbConnection
    {

        const string connectionString = "Data Source=mainDb.db;";
        private static SQLiteConnection conn;

        public dbConnection()
        {
            conn = new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// create tables of db if not already exists
        /// </summary>
        public void createTables()
        {
            string query = "CREATE TABLE IF NOT EXISTS 'users' ('username' TEXT,'password' TEXT, 'email' TEXT,'salt' TEXT, PRIMARY KEY('username'));";
            var command = new SQLiteCommand(query);
            command.ExecuteNonQuery();
        }

        public user getUserDetailsByUsername(string username)
        {
            string query = "SELECT * FROM users WHERE username='"+username+"';";
            var command = new SQLiteCommand(query, conn);
            var reader = command.ExecuteReader();
            user curr = new user();
            while (reader.Read())
            {
                curr.username = reader["username"].ToString();
                curr.password = reader["password"].ToString();
                curr.email = reader["email"].ToString();
                curr.salt = reader["salt"].ToString();
            }
            return curr;
        }

        public int addUser(string username, string password , string email)
        {
            string query = "INSERT INTO users(username, password, email, salt) VALUES(@username, @password, @email, @salt);";
            var command = new SQLiteCommand(query, conn);
            byte[] userSalt = Encryption.generateSalt();
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@salt", Convert.ToBase64String(userSalt));
            command.Parameters.AddWithValue("@password", Encryption.HashPass(password, userSalt));
            var result = command.ExecuteNonQuery();
            return result;
        }

        public bool AuthenticateUser(string username, string password)
        {
            return true;
        }
    }
    
    
}
