using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace trader
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly DataBaseStatemans _dataBaseStatemans = new DataBaseStatemans();
        private readonly MainWindow _mainWindow;
        public Login(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var user = new
            {
                username = UsernameTextBox.Text,
                password = PasswordBox.Password
            };
            MessageBox.Show(_dataBaseStatemans.loginuser(user).ToString());
        }
        private void RegLink_click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartWindow.Navigate(new RegisterPage(_mainWindow));
        }
    }
}
