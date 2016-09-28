﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MyEncryption;
using MyStates;
using System.Windows.Media;

namespace FTNTransport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public Dictionary<string, Driver> dictionary_drivers { set; get; }
        public Dictionary<string, Order> dictionary_orders { set; get; }
        public Dictionary<string, Customer> dictionary_customers { set; get; }
        public Dictionary<string, Destination> dictionary_destination { set; get; }
        public Dictionary<string, Truck> dictionary_trucks { set; get; }

        public MainWindow()
        {
            InitializeComponent();
            intit();
            loadDB();
            populateStates();

        }

        private async void populateStates()
        {
            foreach (State.CODES sc in Enum.GetValues(typeof(State.CODES))) {
                comboBox_state.Items.Add(sc.ToString());
            }
            comboBox_state.SelectedIndex = comboBox_state.Items.IndexOf("CA");
        }

        public async void populateComboBoxes()
        {
            if (this.dictionary_drivers != null)
            {
                fillComboBox(this.comboBox_driver, this.dictionary_drivers);
            }
            if (this.dictionary_trucks != null) {
                fillComboBox(this.comboBox_truck, this.dictionary_trucks);
            }
            if (this.dictionary_destination != null) {
                fillComboBox(this.comboBox_destination, this.dictionary_destination);
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
        private async void fillComboBox(ComboBox cb, Dictionary<string, Driver> d)
        {
            foreach (string id in d.Keys)
            {

                cb.Items.Add(d[id].ToString());

            }
        }

        private async void fillComboBox(ComboBox cb, Dictionary<string, Destination> d)
        {
            foreach (string id in d.Keys)
            {

                cb.Items.Add(d[id]);
               
            }
        }
        private async void fillComboBox(ComboBox cb, Dictionary<string, Truck> t)
        {
            foreach (string id in t.Keys)
            {

                cb.Items.Add(t[id].name.ToUpper());

            }
        }
        /**Load all databases*/
        private void loadDB()
        {
            MyWebServices.WebService.loadDriverDB(this);//load Drivers from DB
            MyWebServices.WebService.loadDestinationDB(this); // load destinations
            MyWebServices.WebService.loadTruckDB(this);                                                //load customers 

            //load orders
        }

        private void intit()
        {
            dictionary_drivers = new Dictionary<string, Driver>();
            dictionary_orders = new Dictionary<string, Order>();
            dictionary_customers = new Dictionary<string, Customer>();
            dictionary_destination = new Dictionary<string, Destination>();

            dictionary_trucks = new Dictionary<string, Truck>();
            int[] arr = { 20, 40, 45 };
            for (int i = 0; i < arr.Length; ++i)
            {
                this.comboBox_size.Items.Add(arr[i]);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu_item = (MenuItem)e.Source;
            if (menu_item.Name.Equals("Exit"))
            {
                closing();

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

                MyWebServices.WebService.insertDriverDB(this,new string[] { textBox_fname.Text, textBox_mname.Text, textBox_lname.Text, textBox_email.Text, textBox_phone.Text });
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
            bool good = true;
            string name = this.textBox_companyname.Text;
            string addr = this.textBox1_address.Text;
            string zip = this.textBox1_zipcode.Text;
            string city = this.textBox1_city.Text;
            string state = this.comboBox_state.Text;
            if (name.Length==0){ good = false; setError(textBox_companyname); }
            if (addr.Length == 0) { good = false; setError(textBox1_address); }
            if (zip.Length == 0) { good = false;setError(textBox1_zipcode); }
            if (city.Length == 0) { good = false;setError(textBox1_city); }

            if (good == false) return;
           
            MyWebServices.WebService.insertDestinationDB(this,new string[] { name, addr, city, state, zip });
            clearErrors();
            ///send to db
            ///I got to create a webservice for this
        }

       

        private void setError(TextBox tb) {
            tb.Background = Brushes.Red;
        }

        private void button_truck_Click(object sender, RoutedEventArgs e)
        {
            bool good = true;
            if (this.textBox_truck_name.Text.Length == 0) { good = false; setError(this.textBox_truck_name);
            }
            if (this.textBox_truck_licenseplate.Text.Length == 0) { good = false; setError(this.textBox_truck_licenseplate); }
            if (this.textBox_truck_cargocapacity.Text.Length == 0) { good = false; setError(this.textBox_truck_cargocapacity); }
            if (this.textBox_truck_mpg.Text.Length == 0 || !isNumeric(this.textBox_truck_mpg.Text)) { good = false; setError(this.textBox_truck_mpg); }
            if (good == false) return;
            MyWebServices.WebService.insertTruckDB(this,new string[]{ this.textBox_truck_name.Text, this.textBox_truck_licenseplate.Text,this.textBox_truck_cargocapacity.Text,this.textBox_truck_mpg.Text });
            clearErrors();
            MyWebServices.WebService.loadTruckDB(this);
        }

        private bool isNumeric(string text)
        {
            try {
                Int32.Parse(text);
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        private void textBox_truck_licenseplate_TextChanged(object sender, TextChangedEventArgs e)
        {
            int size = this.textBox_truck_licenseplate.Text.Length;
            if (this.textBox_truck_licenseplate.Text.Length > 8)
            {
                this.textBox_truck_licenseplate.Text = this.textBox_truck_licenseplate.Text.Substring(0,size-1);
                this.textBox_truck_licenseplate.SelectionStart = size - 1;
            }
        }

        private void textBox_truck_mpg_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
