using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trader
{
    internal class connect
    {
        public MySqlConnection _connection;

        private string _host;
        private string _db;
        private string _user;
        private string _password;

        private string _connectString;

        
        public connect()
        {
            _host = "localhost";
            _db = "trader";
            _user = "root";
            _password = "";

            _connectString = $"server={_host};database={_db};user={_user};password={_password}; SslMode=None;";

            _connection = new MySqlConnection(_connectString);
        }
       
    }
}
