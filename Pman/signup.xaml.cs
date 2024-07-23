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
using System.Windows.Shapes;

namespace Pman
{
    /// <summary>
    /// Interaction logic for signup.xaml
    /// </summary>
    public partial class signup : Window
    {
        public signup()
        {
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        { 
            var main = new MainWindow();
            main.Left = this.Left;
            main.Top = this.Top;
            main.Show();
            this.Close();
        }

        private void addUser(object sender, RoutedEventArgs e)
        {
            if(usernameB.Text.ToString().Length <= 0 ||
                passwordB.Password.ToString().Length <= 0 ||
                emailB.Text.ToString().Length <= 0) 
            {
                this.errB.Text = "err";
            }

            dbConnection db = dbConnection.GetInstance();
            var res = db.addUser(usernameB.Text.ToString(),
                passwordB.Password.ToString(),
                emailB.Text.ToString());
            
            if(res == 0) {
                errB.Text = "Failed to add user";
            }
            else
            {
                errB.Text = "user entered succefully";
            }
        }
    }
}
