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
                curr.salt =  ObjectToByteArray(reader["salt"]);
            }
        }

        public int addUser(string username, string password , string email, byte[] salt)
        {
            return 0;
        }

        public bool AuthenticateUser(string username, string password)
        {

        }
        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
    
    
}
