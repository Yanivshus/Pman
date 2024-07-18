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
    /// Interaction logic for addPassW.xaml
    /// </summary>
    public partial class addPassW : Window
    {
        string username;
        byte[] key;
        byte[] iv;
        public addPassW(string usernameA, byte[] keyA, byte[] ivA)
        {
            InitializeComponent();
            username = usernameA;
            key = keyA;
            iv = ivA;
        }

        private void add(object sender, RoutedEventArgs e)
        {
            dbConnection db = dbConnection.GetInstance();
            var res = db.addPassword(username, this.usernameB.Text.ToString(),this.passwordB.Text.ToString() ,this.websiteB.Text.ToString(), key, iv);
            if(res == 0)
            {
                this.errB.Text = "failed to add password";
            }
            else
            {
                this.errB.Text = "Password added";
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
