using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            refreshFeed();
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
            refreshFeed();
        }

        private void refreshFeed()
        {
            List<passEntry> dbEntries = dbConnection.GetInstance().getPassEntryByUsername(username, key, iv);
            ObservableCollection<listViewPassEntry> entries = new ObservableCollection<listViewPassEntry>();
            foreach (passEntry entry in dbEntries)
            {
                entries.Add(new listViewPassEntry
                {
                    Username = entry.webuser,
                    Password = entry.webpass,
                    Website = entry.website
                });
            }

            PasswordListView.ItemsSource = entries;
        }
    }
    public class listViewPassEntry
    {
        public string Website { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
    }
}
