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

namespace Pman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dbConnection db = dbConnection.GetInstance();
        }

        private void signup(object sender, RoutedEventArgs e)
        {
            var signupW = new signup();
            signupW.Show();
            signupW.Left = this.Left;
            signupW.Top = this.Top;
            this.Close();
        }

        private void login(object sender, RoutedEventArgs e)
        {
            if (userB.Text.ToString().Length <= 0 ||
                passB.Password.ToString().Length <= 0)
            {
                //this.errB.Text = "err";
            }

            else
            {
                dbConnection db = dbConnection.GetInstance();
                var ret = db.AuthenticateUser(userB.Text.ToString(), passB.Password.ToString());
                if (ret)
                {
                    passWin next = new passWin(userB.Text.ToString(), passB.Password.ToString(), db.getUserDetailsByUsername(userB.Text.ToString()).salt);
                    next.Left = this.Left;
                    next.Top = this.Top;
                    next.Show();
                    this.Close();
                }
                else
                {
                    //errB.Text = "Login failed! username or password are wrong.";
                }
            }
        }
    }
}
