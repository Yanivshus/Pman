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

        const string connectionString = "Data Source=mainDb.db";
        private SQLiteConnection conn;

        //singleton pattern.
        private static dbConnection _instance = null;
        public static dbConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new dbConnection();
                    
            }
            return _instance;
        }
        public dbConnection()
        {
            conn = new SQLiteConnection(connectionString);
            conn.Open();
            createTables();
        }
        ~dbConnection()
        {
            conn.Close();
        }


        /// <summary>
        /// create tables of db if not already exists
        /// </summary>
        public void createTables()
        {
            string query = "CREATE TABLE IF NOT EXISTS 'users' ('username' TEXT,'password' TEXT, 'email' TEXT,'salt' TEXT, PRIMARY KEY('username'));";
            var command = new SQLiteCommand(query, conn);
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
                curr.salt = Convert.FromBase64String(reader["salt"].ToString());
            }
            return curr;
        }

        public int addUser(string username, string password , string email)
        {
            user check = getUserDetailsByUsername (username);
            //check if user already exists.
            if(check.username != null)
            {
                return 0;
            }

            string query = "INSERT INTO users (username, password, email, salt) VALUES(@username, @password, @email, @salt);";
            var command = new SQLiteCommand(query, conn);
            byte[] userSalt = Encryption.generateSalt();
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", Encryption.HashPass(password, userSalt));
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@salt", Convert.ToBase64String(userSalt));

            var result = command.ExecuteNonQuery();
            return result;
        }

        public bool AuthenticateUser(string username, string password)
        {
            user check = getUserDetailsByUsername(username);
            //check if user already exists.
            if (check.username == null)
            {
                return false;
            }

            if(Encryption.HashPass(password, check.salt) == check.password)
            {
                return true;
            }

            return false;
        }
    }
    
    
}
