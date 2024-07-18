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
        byte[] salt;
        public passWin(string usernameA, string passwordA, byte[] saltA)
        {
            InitializeComponent();
            username = usernameA;
            password = passwordA;
            salt = saltA;
        }

        private void addPasword(object sender, RoutedEventArgs e)
        {

        }
    }
}
