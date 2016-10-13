using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace FTNTransport
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public long user_id { set; get; }
        public LoginWindow()
        {
            this.user_id = -1;//not logged in (id < 0); logged in otherwise
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
            MyWebServices.WebService.Login(this);
        }
    }
}
