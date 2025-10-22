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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private readonly DataBaseStatemans db = new DataBaseStatemans();
        private readonly MainWindow _mainWindow;
        public RegisterPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == PasswordAgainBox.Password)
            {


                var user = new
                {
                    username = UsernameTextBox.Text,
                    Fullname = FullNameTextBox.Text,
                    password = PasswordBox.Password,
                    salt = "",
                    email = EmailTextBox.Text
                };
                MessageBox.Show(db.AddNewUser(user).ToString());
                _mainWindow.StartWindow.Navigate(new Login(_mainWindow));
            }
            else
            {
                MessageBox.Show("A jelszavak nem egyeznek");
            }
        }
    }
}
