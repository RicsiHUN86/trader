using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace trader
{
    internal class DataBaseStatemans
    {
        connect conn = new connect();
        public object AddNewUser(object user)
        {
            try
            {

                conn._connection.Open();
                var newuser = user.GetType().GetProperties();

                string salt = generateSalt();
                string hashedPassword = computerHmacSha256(newuser[2].GetValue(user).ToString(), salt);

                string sql = "INSERT INTO `users`(`UserName`, `FullName`, `PASSWORD`, `Salt`, `Email`) VALUES (@username,@fullname,@password,@salt,@email)";

                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);


                cmd.Parameters.AddWithValue("@username", newuser[0].GetValue(user));
                cmd.Parameters.AddWithValue("@fullname", newuser[1].GetValue(user));
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@email", newuser[4].GetValue(user));

                cmd.ExecuteNonQuery();

                conn._connection.Close();
                return new { message = "Sikeres hozzáadás" };
            }
            catch (Exception ex)
            {
                return new { message = ex.Message };
            }
        }
        public object loginuser(object user)
        {
           try
            {
                conn._connection.Open();
                string sql = "SELECT * FROM `users` WHERE UserName=@username AND PASSWORD=@password";
                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                var Loguser = user.GetType().GetProperties();

                cmd.Parameters.AddWithValue("@username", Loguser[0].GetValue(user));
                

                MySqlDataReader reader = cmd.ExecuteReader();

                object isRegistered = reader.Read() ? new { message = "Regisztrált" } : new { message = "Nem regisztrált" };
                while (reader.Read())
                {
                    string storedHashed = reader.GetString(3);
                    string storedSalt = reader.GetString(4);
                    string computedHash = computerHmacSha256(Loguser[2].GetValue(user).ToString(), storedSalt);

                    return storedHashed == computedHash;
                }

                conn._connection.Close();
            }
            catch(System.Exception ex)
            {
                return false;
            }
        }
        public DataView userlist()
        {
            try
            {
                conn._connection.Open();
                string sql = "SELECT * FROM `users`";
                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                conn._connection.Close();

                return dt.DefaultView;
            }
            catch (System.Exception)
            {
                return null;
            }

        }
        public string generateSalt()
        {
            byte[] salt = new byte[16];
            using(var rng =  RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
        public string computerHmacSha256(string password, string salt)
        {
            using(var hmac = new HMACSHA256(Convert.FromBase64String(salt)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
