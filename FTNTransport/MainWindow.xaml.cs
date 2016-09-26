using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MyEncryption;
using Newtonsoft.Json.Linq;
using MyStates;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Media;

namespace FTNTransport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, Driver> dictionary_drivers;
        public Dictionary<string, Order> dictionary_orders;
        public Dictionary<string, Customer> dictionary_customers;
        public Dictionary<string, Destination> dictionary_destination;
        public MainWindow()
        {
            intit();
            InitializeComponent();
            loadDB();
            loadStates();

        }

        private async void loadStates()
        {
            foreach (State.CODES sc in Enum.GetValues(typeof(State.CODES))) {
                comboBox_state.Items.Add(sc.ToString());
            }
            comboBox_state.SelectedIndex = comboBox_state.Items.IndexOf("CA");
        }

        private void loadComboBoxes()
        {
            if (this.dictionary_drivers != null)
            {
                fillComboBox<Driver>(this.comboBox_driver, this.dictionary_drivers);
            }
        }

        /// <summary>
        /// //Dont know c#generics soo we can optimize this later.....
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cb"></param>
        /// <param name="d"></param>
      /**  private void fillComboBox<T>(ComboBox cb, Dictionary<long, T> d)
        {
            foreach (long id in d.Keys)
            {
              
                cb.Items.Add(d[id]);
              //  Console.WriteLine(" Obj::: " + d[id].GetType());
            }
        }*/
        private void fillComboBox<T>(ComboBox cb, Dictionary<string, Driver> d)
        {
            foreach (string id in d.Keys)
            {

                cb.Items.Add(d[id].ToString());

            }
        }

        private void fillComboBox(ComboBox cb, Dictionary<long, Destination> d)
        {
            foreach (long id in d.Keys)
            {

                cb.Items.Add(d[id]);
               
            }
        }
        /**Load all databases*/
        private void loadDB()
        {
            loadDriverDB();

            //load customers 
            // load destinations
            //load orders
        }

        private void intit()
        {
            dictionary_drivers = new Dictionary<string, Driver>();
            dictionary_orders = new Dictionary<string, Order>();
            dictionary_customers = new Dictionary<string, Customer>();
            dictionary_destination = new Dictionary<string, Destination>();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu_item = (MenuItem)e.Source;
            if (menu_item.Name.Equals("Exit"))
            {
                closing();

            }
        }
        private async void insertDriverDB(string[] arr)
        {
            try
            {
                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("driver", Encryption.encrypt("acbadriveracba")),
        new KeyValuePair<string, string>("fname", arr[0]),
        new KeyValuePair<string, string>("mname", arr[1]),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
          new KeyValuePair<string, string>("lname", arr[2]),
        new KeyValuePair<string, string>("email",arr[3] ),
        new KeyValuePair<string, string>("phone", arr[4]),
         new KeyValuePair<string, string>("insert","true")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/driver.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    MessageBox.Show("Driver successfully has been added!");
                    loadDriverDB();
                }

            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.ToString());
            }
        }
        private async void loadDriverDB()
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("driver", Encryption.encrypt("acbadriveracba")),

         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/driver.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    JArray a = JArray.Parse(json);
                    // Console.WriteLine("Count:: " + a.Count);
                    //Console.WriteLine(json);
                    //Console.WriteLine(a.ToString());
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long id = Int64.Parse(item.GetValue("id").ToString());
                        string fname = item.GetValue("fname").ToString();
                        string mname = item.GetValue("mname").ToString();
                        string lname = item.GetValue("lname").ToString();
                        string email = item.GetValue("email").ToString();
                        string phone = item.GetValue("phone").ToString();
                        Driver d = new Driver(id, fname, mname, lname, email, phone);
                        if (!this.dictionary_drivers.ContainsKey(d.name))
                        {
                            this.dictionary_drivers.Add(d.name, d);

                            this.listView_driver.Items.Add(d);
                            //d = null;
                        }
                    }
                    //foreach (JObject o in a.Children<JObject>())
                    //{
                    //    //foreach (JProperty p in o.Properties())
                    //    //{
                    //    //    string name = p.Name;
                    //    //    //string value = (string)p.Value;
                    //    //    Console.WriteLine(name);// + " -- " + value);
                    //    //}
                    //    Console.WriteLine(o.ToString());
                    //}

                    // Console.WriteLine(JObject.Parse(json));
                }
                loadComboBoxes();
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.ToString());
            }
        }
        //Window listener
        public void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (closing() == false) e.Cancel = true;
        }
        //Message that will display before exiting the program
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
        /**Method will only allow 10 numerical characters. All other input is ignored.
        */
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
                Console.WriteLine(ee.ToString());
                // textBox_phone.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // var firstStackPanelInTabControl = FindVisualChildren<TextBox>(tabControl).ElementAt(3);
            bool good = true;
            foreach (TextBox tb in FindVisualChildren<TextBox>(tabControl))///msy cause problems. May check all textboxes not in tab control selection 3
            {
                if (tb.Text.Length == 0 && tb != textBox_mname)
                {
                    tb.Background = Brushes.Red;
                    good = false;
                }
                else if (tb == textBox_phone && textBox_phone.Text.Length < 10)
                {
                    textBox_phone.Background = Brushes.Red;
                    good = false;
                }
                else if (tb == textBox_email && (!tb.Text.Contains("@") || !tb.Text.Contains(".")))
                {
                    tb.Background = Brushes.Red;
                    good = false;
                }
                else {
                    tb.Background = Brushes.White;
                }
            }
            if (good)
            {

                insertDriverDB(new string[] { textBox_fname.Text, textBox_mname.Text, textBox_lname.Text, textBox_email.Text, textBox_phone.Text });
                ///will update
                //send db
                clearErrors();
            }
        }
      //clears all text in a tab and sets the background color to white
        private void clearErrors()
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(tabControl))///msy cause problems. May check all textboxes not in tab control selection 3
            {
                tb.Background = Brushes.White;
                tb.Text = "";
            }
        }
        //Will find all <T> controls in a given window
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject rootObject) where T : DependencyObject
        {
            if (rootObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(rootObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(rootObject, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private void zipCodeFormatter(object sender, TextChangedEventArgs e)
        {
            int size = textBox1_zipcode.Text.Length;
            Console.WriteLine("SIZE is: "+size);
            if (size > 5)
            {
                textBox1_zipcode.Text = textBox1_zipcode.Text.Substring(0, size - 1);
                textBox1_zipcode.SelectionStart = size - 1;
            }
            try {
                int c = Int32.Parse(textBox1_zipcode.Text.ToCharArray()[size-1].ToString());
                //int cc = Int32.Parse(textBox1_zipcode.Text);
            } catch (Exception ee) {
                if (size == 0)
                {
                    textBox1_zipcode.Text = "";
                }
                else {
                    textBox1_zipcode.Text = textBox1_zipcode.Text.Substring(0, size - 1);
                    textBox1_zipcode.SelectionStart = size-1;
                }
            }
        }

        private void button_create_destination_Click(object sender, RoutedEventArgs e)
        {
            ///send to db
            ///I got to create a webservice for this
        }
    }
}
