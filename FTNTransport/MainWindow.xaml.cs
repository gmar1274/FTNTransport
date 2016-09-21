using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MyEncryption;
namespace FTNTransport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadDB();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu_item = (MenuItem)e.Source;
            if (menu_item.Name.Equals("Exit"))
            {
                closing();

            }
        }
        private void loadDB()
        {
            try
            {
                Console.WriteLine("HERE");
                Console.WriteLine(Encryption.encrypt("acba"));

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string, string>("pqpUserName", "admin"),
        new KeyValuePair<string, string>("password", "test@123")
    };

                var content = new FormUrlEncodedContent(pairs);

                var response = client.PostAsync("youruri", content).Result;

                if (response.IsSuccessStatusCode)
                {


                }
            }
            catch {

            }
        }

        public void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (closing() == false) e.Cancel = true;
        }
        public bool closing()
        {
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

      
    }
}
