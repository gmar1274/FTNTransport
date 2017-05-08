using FTNTransport.Interfaces;
using System;
using System.ComponentModel;
using System.Windows;
using FTNTransport.Interfaces;
using System.Threading.Tasks;
/***
TEST AUTHENTICATION
username: 9092822545 
password: admin123
*/
namespace FTNTransport
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, ILogin
    {
        private long user_id;
        private string username, password;
        public LoginWindow()
        {
            this.user_id = -1;
            this.username = null;
            this.password = null;
            InitializeComponent();
        }


        //Window listener
        public void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (closing() == false) e.Cancel = true;
        }
        //Message that will display before exiting the program
        public bool closing()
        {
            if (this.user_id >= 0) return true;
            string msg = "Are you sure you want to exit?";
            MessageBoxResult result =
              MessageBox.Show(
                msg,
                "ACBA Dispatch Program",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                Environment.Exit(0);
                return true;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(this.textBox_username.Text.Length==0 || this.passwordBox.Password.Length == 0)
            {
                MessageBox.Show(
                "Username and Password are required.",
                "ACBA Dispatch Program",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                return;
            }
            MyWebServices.WebService.Login(this);
        }

        public string getUsername()
        {
            return this.textBox_username.Text;
        }

        public string getPassword()
        {
            return MyEncryption.Encryption.encrypt(this.passwordBox.Password);
        }

        public long getID()
        {
            return this.user_id;
        }

        public bool isLoggedIn()
        {
            return this.user_id > 0;
        }

        public Task login()
        {
            Task task = new Task(() => MyWebServices.WebService.Login(this));
            task.Start();
            return task;
        }

        public void setUsername(string username)
        {
            this.username = username;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        public void setID(long id)
        {
            this.user_id = id;
        }
     
        public void closeLoginWindow()
        {
            this.Close();
        }
    }
}
