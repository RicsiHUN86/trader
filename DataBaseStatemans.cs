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
        public bool Loginuser(object user)
        {
            try
            {
                conn._connection.Open();

                string sql = "SELECT * FROM users WHERE UserName = @username;";

                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                var logUser = user.GetType().GetProperties();

                cmd.Parameters.AddWithValue("@username", logUser[0].GetValue(user));

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader.GetString(3);
                    string storedSalt = reader.GetString(4);
                    string computeHash = computerHmacSha256(logUser[1].GetValue(user).ToString(), storedSalt);
                    conn._connection.Close();

                    return storedHash == computeHash;
                }
                conn._connection.Close();
                return false;
            }
            catch
            {
                return false;
            }

        }

        public DataView userlist()
        {
            try
            {
                conn._connection.Open();
                string sql = "SELECT `UserName`,`FullName`,`Email`,`RegTime` FROM `users` ";

                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn._connection);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                conn._connection.Close();

                return dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public object deleteuser(object id)
        {
            try
            {
                conn._connection.Open();

                string sql = "DELETE FROM `users` WHERE Id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                conn._connection.Close();
                return new { message = "Sikeres törlés" };
            }
            catch (Exception ex)
            {
                conn._connection.Close();
                return new { message = ex.Message };
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
