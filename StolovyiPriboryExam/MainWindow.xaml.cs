using Library_project.Models;
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

namespace Library_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void UserAutorization()
        {
            using (LibraryContext db = new LibraryContext())
            {
                var currentUser = db.Users
                    .FirstOrDefault(user =>
                        user.Login == LoginBox.Text &&
                        user.Password == PasswordBox.Password);

                if (currentUser != null)
                {
                    UserWindow window = new UserWindow(currentUser);
                    window.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль");
                }
            }
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            UserAutorization();
        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}

