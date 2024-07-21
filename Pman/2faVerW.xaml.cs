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
using MailKitSimplified.Sender.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKitSimplified.Sender;


namespace Pman
{
    /// <summary>
    /// Interaction logic for _2faVerW.xaml
    /// </summary>
    public partial class _2faVerW : Window
    {
        string username;
        string password;
        byte[] salt;
        string email;
        string codeCurr;
        EmailSettings emailSettings;
        public _2faVerW(string usernameA, string passwordA, byte[] saltA)
        {
            InitializeComponent();
            this.username = usernameA;
            this.password = passwordA;
            this.salt = saltA;
            this.email = dbConnection.GetInstance().getUserDetailsByUsername(usernameA).email;

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();

            sendCode();

        }

        private void verify(object sender, RoutedEventArgs e)
        {
            if(this.codeB.Text.ToString() == codeCurr)
            {
                passWin next = new passWin(username, password, salt);
                next.Left = this.Left;
                next.Top = this.Top;
                next.Show();
                this.Close();
            }
            else
            {
                MainWindow next = new MainWindow();
                next.Left = this.Left;
                next.Top = this.Top;
                next.Show();
                this.Close();
            }
        }
        private async void sendCode()
        {
            Random r = new Random();
            var x = r.Next(0, 1000000);
            string code = x.ToString("000000");
            codeCurr = code;

            var emailService = new EmailService(emailSettings);
            await emailService.SendEmailAsync(email, "Your verification code: ", code);
        }

        private void sendCodeAgain(object sender, RoutedEventArgs e)
        {
            sendCode();
        }
    }
}
