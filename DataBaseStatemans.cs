using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

                string sql = "INSERT INTO `users`(`UserName`, `FullName`, `PASSWORD`, `Salt`, `Email`) VALUES (@username,@fullname,@password,@salt,@email)";

                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                var newuser = user.GetType().GetProperties();

                cmd.Parameters.AddWithValue("@username", newuser[0].GetValue(user));
                cmd.Parameters.AddWithValue("@fullname", newuser[1].GetValue(user));
                cmd.Parameters.AddWithValue("@password", newuser[2].GetValue(user));
                cmd.Parameters.AddWithValue("@salt", newuser[3].GetValue(user));
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
            conn._connection.Open();
            string sql = "SELECT * FROM `users` WHERE UserName=@username AND PASSWORD=@password";
            MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

            var Loguser = user.GetType().GetProperties();

            cmd.Parameters.AddWithValue("@username", Loguser[0].GetValue(user));
            cmd.Parameters.AddWithValue("@password", Loguser[1].GetValue(user));

            MySqlDataReader reader = cmd.ExecuteReader();

            object isRegistered = reader.Read() ? new { message = "Regisztrált" } : new { message = "Nem regisztrált" };

            conn._connection.Close();
            return isRegistered;
        }
    }
}
