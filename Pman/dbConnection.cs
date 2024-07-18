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
            //main users table
            string query = "CREATE TABLE IF NOT EXISTS 'users' ('username' TEXT,'password' TEXT, 'email' TEXT,'salt' TEXT, PRIMARY KEY('username'));";
            var command1 = new SQLiteCommand(query, conn);
            command1.ExecuteNonQuery();

            //main users passwords table.
            query = "CREATE TABLE IF NOT EXISTS 'mainPasswords' ('username' TEXT,'website' TEXT,'webpass' TEXT,'webuser' TEXT, FOREIGN KEY('username') REFERENCES 'users'('username'));";
            var command2 = new SQLiteCommand(query, conn);
            command2.ExecuteNonQuery();
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
            user check = getUserDetailsByUsername(username);
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

        public int addPassword(string username, string webuser,string password, string website, byte[] key, byte[] iv)
        {
            string query = "INSERT INTO mainPasswords (username, website, webpass, webuser) VALUES(@username, @website, @webpass, @webuser);";
            var command = new SQLiteCommand(query, conn);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@website", website);
            //encrypt both username and password using precreated key and iv.
            command.Parameters.AddWithValue("@webpass",Convert.ToBase64String(Encryption.Encrypt(password, key, iv)));
            command.Parameters.AddWithValue("@webuser", Convert.ToBase64String(Encryption.Encrypt(webuser, key, iv)));

            var result = command.ExecuteNonQuery();
           
            return result;
        }

        public List<passEntry> getPassEntryByUsername(string username, byte[] key, byte[] iv)
        {
            string query = "SELECT * FROM mainPasswords WHERE username='" + username + "';";
            var command = new SQLiteCommand(query, conn);
            var reader = command.ExecuteReader();
            List<passEntry> pList = new List<passEntry>();

            //reading all the result.
            while (reader.Read())
            {
                passEntry curr = new passEntry();
                curr.username = reader["username"].ToString();
                curr.website = reader["website"].ToString();
                curr.webuser = Encryption.Decrypt(Convert.FromBase64String(reader["webuser"].ToString()), key, iv);
                curr.webpass = Encryption.Decrypt(Convert.FromBase64String(reader["webpass"].ToString()), key, iv);

                pList.Add(curr);
            }

            return pList;
        }
    }
    
    
}
