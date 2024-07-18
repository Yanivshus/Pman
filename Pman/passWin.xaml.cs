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
    /// Interaction logic for passWin.xaml
    /// </summary>
    public partial class passWin : Window
    {
        string username;
        string password;
        byte[] key;
        byte[] iv;
        public passWin(string usernameA, string passwordA, byte[] saltA)
        {
            InitializeComponent();
            username = usernameA;
            password = passwordA;
            key = Encryption.DeriveEncryptionKey(passwordA, saltA);
            iv = Encryption.DeriveEncryptionIV(passwordA, saltA);
        }

        private void addPasword(object sender, RoutedEventArgs e)
        {
            addPassW addW = new addPassW(username, key, iv);
            addW.Show();
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Left = this.Left;
            main.Top = this.Top;
            main.Show();
            this.Close();
        }

        private void refresh(object sender, RoutedEventArgs e)
        {

        }

        private void refreshFeed()
        {

        }
    }
}
