using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MyEncryption;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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
        private async void loadDB()
        {
            try
            {
                Console.WriteLine("HERE");
                Console.WriteLine(Encryption.encrypt("acbadriveracba"));

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("driver", Encryption.encrypt("acbadriveracba")),
        new KeyValuePair<string, string>("pqpUserName", "admin"),
        new KeyValuePair<string, string>("password", "test@123"),
         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/driver.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("AYY GOOd");
                    // HttpContent stream = response.Content;
                    //Task<string> data = stream.ReadAsStringAsync();
                    //JObject.Parse(data.);
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.ToString());
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

        private void phone_format(object sender, TextChangedEventArgs e)
        {
            if (textBox_phone.Text.Length <= 0) return;
            char c = textBox_phone.Text.ToCharArray()[textBox_phone.Text.ToCharArray().Length - 1];
            try
            {
                int x = Int32.Parse(c + "");
            }
            catch (Exception ee)
            {
                int end = textBox_phone.Text.ToCharArray().Length - 1;
                string s = textBox_phone.Text.Substring(0, end);
                textBox_phone.Text = s;
                textBox_phone.SelectionStart = end;

                // textBox_phone.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
