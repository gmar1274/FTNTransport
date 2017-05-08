using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using FTNTransport.Interfaces;
using System.Windows.Controls;
using FTNTransport.Windows;
using System.Windows;
using MyStates;
using System.ComponentModel;
using System.Windows.Media;
using System.Globalization;

namespace FTNTransport.ViewModels
{
    /***
    View model class for MainWindow.xaml. This class contains all logic to then output calculations/api calls to MainWindow.xaml. 
    */
    class VMMainWindow:IDispatchRespository
    {
        private DispatcherTimer dispatcherTimer;
        public bool customers_loaded;
        public bool trucks_loaded;
        public bool drivers_loaded;
        public bool destinations_loaded;
        public long user_id = -1;

        public Dictionary<string, Driver> dictionary_drivers { set; get; }
        public Dictionary<long, Order> dictionary_orders { set; get; }
        public Dictionary<string, Customer> dictionary_customers { set; get; }
        public Dictionary<string, Destination> dictionary_destination { set; get; }
        public Dictionary<string, Truck> dictionary_trucks { set; get; }
        private MainWindow mainwindow;
        public VMMainWindow(MainWindow mw)
        {
            this.mainwindow = mw;
            LoginWindow lw = new LoginWindow();
            lw.ShowDialog();
            this.user_id = lw.getID();
            // MessageBox.Show(this.user_id.ToString());
            this.mainwindow.InitializeComponent();//GUI
            init();//Data structures
            this.loadDataFromDatabase(lw);
            //loadDB();//Web calls
        }

        /// <summary>
        /// Initialize all fields.
        /// </summary>
        public void init()
        {
            trucks_loaded = false;
            destinations_loaded = false;
            customers_loaded = false;
            drivers_loaded = false;
            dictionary_drivers = new Dictionary<string, Driver>();
            dictionary_orders = new Dictionary<long, Order>();
            dictionary_customers = new Dictionary<string, Customer>();
            dictionary_destination = new Dictionary<string, Destination>();
            dictionary_trucks = new Dictionary<string, Truck>();
            populateHarcodedItems();

        }
        /// <summary>
        /// Shipping_line,military_time,cargo
        /// </summary>
        private async void populateHarcodedItems()
        {
            string[] arr = { "20ST", "40ST", "40HC", "45HC" };
            for (int i = 0; i < arr.Length; ++i)
            {
                mainwindow.comboBox_size.Items.Add(arr[i]);
            }
            string[] cargo = { "Loaded", "Empty" };
            foreach (string s in cargo)
            {
                mainwindow.comboBox_cargo.Items.Add(s);
            }
            string[] sl = { "APL", "MSK", "MSC", "KLINE", "NYK", "EVE", "COSCO", "WHL", "UA", "OOCL", "SEALAND", "HMM", "PIL", "YML", "HAPAG", "MOL" };
            Array.Sort(sl);
            foreach (string line in sl)
            {
                mainwindow.comboBox_shippingLine.Items.Add(line);
            }
            for (int i = 0; i < 60; ++i)
            {
                string format = i.ToString("0#");
                if (i < 24)
                {
                    mainwindow.comboBox_time_hours.Items.Add(format);//fill hours
                }

                mainwindow.comboBox_time_min.Items.Add(format);

            }
            //fill in month for combo boxes in driver settings tab
            for (int i = 1; i <= 12; ++i)
            {
                string format = i.ToString("0#");
                mainwindow.comboBox_cdl_month.Items.Add(format);
                mainwindow.comboBox_medical_month.Items.Add(format);

            }
            //fill in days
            for (int i = 1; i <= 31; ++i)
            {
                string format = i.ToString("0#");
                mainwindow.comboBox_cdl_day.Items.Add(format);
                mainwindow.comboBox_medical_day.Items.Add(format);

            }
            //fill in year
            int year = DateTime.Now.Year;
            for (int i = year; i < year + 20; ++i)
            {
                string format = i.ToString("####");
                mainwindow.comboBox_cdl_year.Items.Add(format);
                mainwindow.comboBox_medical_year.Items.Add(format);

            }
            this.populateStates();
        }
        private async void populateStates()
        {
            foreach (State.CODES sc in Enum.GetValues(typeof(State.CODES)))
            {
                mainwindow.comboBox_state.Items.Add(sc.ToString());
            }
           mainwindow.comboBox_state.SelectedIndex = mainwindow.comboBox_state.Items.IndexOf("CA");
        }
        /**
        *Populates the comboboxes from the values in the dictionary data structures
        * That were filled from a DB call from loadDB().
        *
        */
        public async void populateComboBoxes()
        {
            if (this.dictionary_drivers != null)
            {
                fillComboBox(mainwindow.comboBox_driver, this.dictionary_drivers);
            }
            if (this.dictionary_trucks != null)
            {
                fillComboBox(mainwindow.comboBox_truck, this.dictionary_trucks);
            }
            if (this.dictionary_destination != null)
            {
                fillComboBox(mainwindow.comboBox_destination, this.dictionary_destination);
            }
            if (this.dictionary_customers != null)
            {
                fillComboBox(mainwindow.comboBox_customer, this.dictionary_customers);
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
        private async void fillComboBox(ComboBox cb, Dictionary<string, Customer> d)
        {
            foreach (string id in d.Keys)
            {
                if (!cb.Items.Contains(d[id].name))
                {
                    cb.Items.Add(d[id].name);
                }
            }
        }
        private async void fillComboBox(ComboBox cb, Dictionary<string, Driver> d)
        {
            foreach (string id in d.Keys)
            {
                if (!cb.Items.Contains(d[id].name))
                {
                    cb.Items.Add(d[id].name);
                }
            }
        }

        private async void fillComboBox(ComboBox cb, Dictionary<string, Destination> d)
        {
            foreach (string id in d.Keys)
            {
                Destination dest = d[id];
                if (dest.isTerminal && !mainwindow.comboBox_terminal.Items.Contains(dest.ToString()))
                {
                    mainwindow.comboBox_terminal.Items.Add(dest.ToString());
                }
                if (!dest.isTerminal && !cb.Items.Contains(d[id].ToString()))
                {
                    cb.Items.Add(d[id].ToString());
                }

            }
        }
        private async void fillComboBox(ComboBox cb, Dictionary<string, Truck> t)
        {
            foreach (string id in t.Keys)
            {
                if (!cb.Items.Contains(t[id].name))
                {
                    cb.Items.Add(t[id].name);
                }

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
            TextBox tb = (TextBox)sender;
            format_phone(tb);
        }
        private void format_phone(TextBox tb)
        {
            if (tb.Text.Length <= 0 && tb.IsFocused || tb.Text.Length <= 0) return;
            char c = tb.Text.ToCharArray()[tb.Text.ToCharArray().Length - 1];
            try
            {
                int x = Int32.Parse(c + "");
                // MessageBox.Show(tb.Text);
            }
            catch (Exception ee)
            {
                if (tb.Text.Length <= 1)
                {
                    tb.Text = "";
                    return;
                }
                int end = tb.Text.ToCharArray().Length - 1;
                string s = tb.Text.Substring(0, end);
                tb.Text = s;
                tb.SelectionStart = end;
                Console.WriteLine(ee.ToString());
                // textBox_phone.Background = System.Windows.Media.Brushes.Red;
            }
        }

        /// <summary>
        /// DRIVER SETTINGS INSERT TAB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            // var firstStackPanelInTabControl = FindVisualChildren<TextBox>(tabControl).ElementAt(3);
            bool good = true;
            foreach (TextBox tb in FindVisualChildren<TextBox>(mainwindow.tabControl))///msy cause problems. May check all textboxes not in tab control selection 3
            {
                if (tb == mainwindow.textBox_driver_rn || tb == mainwindow.textBox_driver_an) continue;
                if (tb.Text.Length == 0 && tb != mainwindow.textBox_mname)
                {
                    tb.Background = Brushes.Red;
                    good = false;
                }
                else if (tb == mainwindow.textBox_phone && mainwindow.textBox_phone.Text.Length < 10)
                {
                    mainwindow.textBox_phone.Background = Brushes.Red;
                    good = false;
                }
                else if (tb == mainwindow.textBox_email && (!tb.Text.Contains("@") || !tb.Text.Contains(".")))
                {
                    tb.Background = Brushes.Red;
                    good = false;
                }
                else {
                    tb.Background = Brushes.White;
                }
            }

            string pass = mainwindow.textBox_driver_password.Text;
            if (pass.Length == 0)
            {
                good = false;
                setError(mainwindow.textBox_driver_password);
            }
            else
            {
                mainwindow.textBox_driver_password.Background = Brushes.White;
            }
            string cdl = null;
            if (mainwindow.comboBox_cdl_month.SelectedIndex >= 0 && mainwindow.comboBox_cdl_day.SelectedIndex >= 0 && mainwindow.comboBox_cdl_year.SelectedIndex >= 0)
            {
                cdl = mainwindow.comboBox_cdl_year.SelectedItem + "-" + mainwindow.comboBox_cdl_month.SelectedItem + "-" + mainwindow.comboBox_cdl_day.SelectedItem;
            }
            else { good = false; }
            string medical = null;
            if (mainwindow.comboBox_medical_month.SelectedIndex >= 0 && mainwindow.comboBox_medical_day.SelectedIndex >= 0 && mainwindow.comboBox_medical_year.SelectedIndex >= 0)
            {
                medical = mainwindow.comboBox_medical_year.SelectedItem + "-" + mainwindow.comboBox_medical_month.SelectedItem + "-" + mainwindow.comboBox_medical_day.SelectedItem;
            }
            else
            {
                good = false;
            }
            if (good)
            {

                MyWebServices.WebService.insertDriverDB(mainwindow, new string[] { mainwindow.textBox_fname.Text, mainwindow.textBox_mname.Text, mainwindow.textBox_lname.Text, mainwindow.textBox_email.Text, mainwindow.textBox_phone.Text, mainwindow.textBox_driver_rn.Text, mainwindow.textBox_driver_an.Text, mainwindow.textBox_bankname.Text, cdl, medical, pass });
                ///will update
                //send db
                clearErrors();
            }
        }
        //clears all text in a tab and sets the background color to white
        //resets all text boxes to empty
        private void clearErrors()
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(mainwindow.tabControl))///msy cause problems. May check all textboxes not in tab control selection 3
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
            int size = mainwindow.textBox1_zipcode.Text.Length;
            if (size > 5)
            {
                mainwindow.textBox1_zipcode.Text = mainwindow.textBox1_zipcode.Text.Substring(0, size - 1);
                mainwindow.textBox1_zipcode.SelectionStart = size - 1;
            }
            try
            {
                int c = Int32.Parse(mainwindow.textBox1_zipcode.Text.ToCharArray()[size - 1].ToString());
                //int cc = Int32.Parse(textBox1_zipcode.Text);
            }
            catch (Exception ee)
            {
                if (size == 0)
                {
                    mainwindow.textBox1_zipcode.Text = "";
                }
                else {
                    mainwindow.textBox1_zipcode.Text = mainwindow.textBox1_zipcode.Text.Substring(0, size - 1);
                    mainwindow.textBox1_zipcode.SelectionStart = size - 1;
                }
            }
        }
        private void button_create_destination_Click(object sender, RoutedEventArgs e)
        {
            Button terminal = (Button)sender;
            bool terminal_dest = false;
            if (terminal.Name.ToLower().Contains("terminal"))
            {
                terminal_dest = true;
                //MessageBox.Show("AYY");
            }
            bool good = true;
            string name = mainwindow.textBox_DestinationName.Text;
            string addr = mainwindow.textBox1_address.Text;
            string zip = mainwindow.textBox1_zipcode.Text;
            string city = mainwindow.textBox1_city.Text;
            string state = mainwindow.comboBox_state.Text;
            if (name.Length == 0) { good = false; setError(mainwindow.textBox_DestinationName); }
            if (terminal_dest && !good) return;
            else if (terminal_dest && good)
            {
                MyWebServices.WebService.insertDestinationDB(mainwindow, new string[] { name, addr, city, state, zip, "terminal" });
                clearErrors();
                return;

                if (addr.Length == 0) { good = false; setError(mainwindow.textBox1_address); }
                if (zip.Length == 0) { good = false; setError(mainwindow.textBox1_zipcode); }
                if (city.Length == 0) { good = false; setError(mainwindow.textBox1_city); }

                if (good == false) return;

                MyWebServices.WebService.insertDestinationDB(mainwindow, new string[] { name, addr, city, state, zip });
                clearErrors();
                ///send to db
                ///I got to create a webservice for this
            }
        }



        private void setError(TextBox tb)
        {
            tb.Background = Brushes.Red;
        }

        private void button_truck_Click(object sender, RoutedEventArgs e)
        {
            bool good = true;
            if (mainwindow.textBox_truck_name.Text.Length == 0)
            {
                good = false; setError(mainwindow.textBox_truck_name);
            }
            if (mainwindow.textBox_truck_licenseplate.Text.Length == 0) { good = false; setError(mainwindow.textBox_truck_licenseplate); }
            if (mainwindow.textBox_truck_vin.Text.Length == 0) { good = false; setError(mainwindow.textBox_truck_vin); }
            if (mainwindow.textBox_truck_mpg.Text.Length == 0 || !isNumeric(mainwindow.textBox_truck_mpg.Text)) { good = false; setError(mainwindow.textBox_truck_mpg); }
            string rfid = mainwindow.textBox_truck_rfid.Text;
            if (rfid.Length == 0) { good = false; setError(mainwindow.textBox_truck_rfid); }
            if (good == false) return;
            MyWebServices.WebService.insertTruckDB(mainwindow, new string[] { mainwindow.textBox_truck_name.Text, mainwindow.textBox_truck_licenseplate.Text, mainwindow.textBox_truck_vin.Text, mainwindow.textBox_truck_mpg.Text, rfid });
            clearErrors();
            MyWebServices.WebService.loadTruckDB(mainwindow);
        }
        /// <summary>
        /// Just check if the string is a numeric
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool isNumeric(string text)
        {
            try
            {
                double.Parse(text);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void textBox_truck_licenseplate_TextChanged(object sender, TextChangedEventArgs e)
        {
            int size = mainwindow.textBox_truck_licenseplate.Text.Length;
            if (mainwindow.textBox_truck_licenseplate.Text.Length > 8)
            {
                mainwindow.textBox_truck_licenseplate.Text = mainwindow.textBox_truck_licenseplate.Text.Substring(0, size - 1);
                mainwindow.textBox_truck_licenseplate.SelectionStart = size - 1;
            }
        }

        private void textBox_truck_mpg_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        /**Create customer for DB
        Field validation
        */
        private void button_create_customer_Click(object sender, RoutedEventArgs e)
        {
            string name = mainwindow.textBox_customer_name.Text;///Order of array 0
            string email = mainwindow.textBox_customer_email.Text;//1
            string phone = mainwindow.textBox_customer_phone.Text;//2
            string routing = mainwindow.textBox_customer_routingNumber.Text;//3
            string account = mainwindow.textBox_customer_accountNumber.Text;//4
            bool good = true;//pretend all data is good
            string[] arr = new string[] { name, email, phone, routing, account };
            for (int i = 0; i < arr.Length; ++i)
            {
                // Console.WriteLine("i: "+i+" "+arr[i]);
                if (arr[i].Length == 0 || !isNumeric(phone))
                {
                    MessageBox.Show("No data has been saved. " + " Debug mode: index [" + i + "] ");
                    good = false;
                    return;
                }
            }
            if (good)
            {
                MyWebServices.WebService.insertCustomerDB(mainwindow, arr);
                clearErrors();
            }
        }

        private void textBox_customer_phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            ///implement numeric checker
        }

        private void comboBox_customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainwindow.comboBox_customer.SelectedItem == null) return;
            string cust_name = mainwindow.comboBox_customer.SelectedItem.ToString();
            ///fetch a DB call for Last recent ORDER if any and fill out the fields
            ///
            if (this.dictionary_customers.ContainsKey(cust_name))
            {
                mainwindow.checkBox_repeat_order.IsEnabled = true;
            }
        }

        private void checkBox_repeat_order_Checked(object sender, RoutedEventArgs e)
        {
            ///fill in last order of customer
            ///soo do a query to find the last order shipment
        }
        /// <summary>
        /// This method is the action handler for creating a new order.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_createOrder_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                string lfd = null;
                if (mainwindow.datepicker_lfd.SelectedDate != null)
                {
                    lfd = mainwindow.datepicker_lfd.SelectedDate.Value.ToShortDateString();

                }
                string cust = mainwindow.comboBox_customer.SelectedValue.ToString();
                string container = mainwindow.comboBox_container.Text;///might have to fix this
                string terminal = mainwindow.comboBox_terminal.SelectedValue.ToString();//start dest
                string size = mainwindow.comboBox_size.SelectedValue.ToString();

                string end_dest = mainwindow.comboBox_destination.SelectedValue.ToString();
                string driver = mainwindow.comboBox_driver.SelectedValue.ToString();
                string truck = mainwindow.comboBox_truck.SelectedValue.ToString();
                string driver_comm = mainwindow.textBox_driverCommission_dollars.Text.ToString().Replace(",", "") + "." + mainwindow.textBox_driverCommission_cents.Text;
                string amount = mainwindow.textBox_order_dollars.Text.Replace(",", "") + "." + mainwindow.textBox_order_cents.Text;

                string shipping_line = mainwindow.comboBox_shippingLine.SelectedValue.ToString();
                string cargo = mainwindow.comboBox_cargo.SelectedValue.ToString();
                string hour = mainwindow.comboBox_time_hours.SelectedValue.ToString();
                string min = mainwindow.comboBox_time_min.SelectedValue.ToString();
                DateTime dt = DateTime.Parse(mainwindow.datepicker_pickup.SelectedDate.Value.ToShortDateString() + " " + hour + ":" + min + ":00", new CultureInfo("en-US"));
                string pickup_sku = mainwindow.comboBox_pickup_number.Text;
                string delivery_sku = mainwindow.comboBox_delivery_number.Text;

                string[] arr = new string[] { driver, terminal, end_dest, driver_comm, truck, "pending...", cust, container, size, terminal, lfd, amount, shipping_line, cargo, dt.ToString("yyyy-MM-dd HH:mm:ss").ToString(), pickup_sku, delivery_sku };
                for (int i = 0; i < arr.Length; ++i)//absolutely no logic checking...
                {
                    string s = arr[i];
                    if (i == 10) continue;
                    if (s == null || s.Length == 0)
                    {

                        orderError();
                        return;
                    }
                }
                MyWebServices.WebService.insertOrderDB(mainwindow, arr);
            }
            catch (Exception ee)
            {
                orderError();
                Console.WriteLine(ee);
            }

        }
        public void orderConfirmation()
        {///refreshes fields
            foreach (ComboBox cb in FindVisualChildren<ComboBox>(mainwindow.tabControl))///msy cause problems. May check all textboxes not in tab control selection 3
            {

                cb.SelectedIndex = -1;
                cb.Text = "";
            }
            foreach (TextBox tb in FindVisualChildren<TextBox>(mainwindow.tabControl))///msy cause problems. May check all textboxes not in tab control selection 3
            {
                tb.Text = "";
            }

        }
        /// <summary>
        /// Just display an error. Later will implement logic checking like highlight the error..
        /// </summary>
        public void orderError()
        {
            MessageBox.Show(
                    "Error. Recheck your fields.\nNo data has been saved.",
                    "ACBA Dispatch Program",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }






        private void button_highlight_containers_Click(object sender, RoutedEventArgs e)
        {


            // string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Window w = new Window();
            // w.Width = 500;
            //w.Height=600;
            //var window = new Window();
            //var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            //stackPanel.Children.Add(new Label { Content = "Label" });
            //stackPanel.Children.Add(new Button { Content = "Button" });
            //window.Content = stackPanel;
            //window.Show();
            string con = "";
            foreach (long id in this.dictionary_orders.Keys)
            {
                con += this.dictionary_orders[id].container + Environment.NewLine;
            }
            System.Windows.Clipboard.SetText(con);
            MessageBox.Show("Text has been copied!");
        }

        private void textBox_driverCommission_dollars_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length <= 0 || !tb.IsFocused) return;
            format_money(tb);
        }

        private void format_money(TextBox tb)
        {
            var dollars = tb;

            long amount = 0;
            try
            {
                amount = long.Parse(dollars.Text.Replace(",", ""));
                if (dollars.Text.Length == 1) { dollars.Text = amount.ToString("#,##0", new CultureInfo("en-US")); }
                else {
                    dollars.Text = amount.ToString("#,#00", new CultureInfo("en-US"));
                }
                dollars.SelectionStart = dollars.Text.Length;

            }
            catch (Exception ee)

            {
                Console.WriteLine(ee.ToString());
                dollars.Text = "";

            }
        }

        private void listview_onDoubleClick(object sender, RoutedEventArgs e)
        {
            // o.listView_leg.Items[0]= ((DataRowView)((ListView)sender).SelectedItem)["Order No."].ToString();
            // long on = long.Parse(((DataRowView)((ListView)sender).SelectedItem)["Order No."].ToString());
            //  o.listView_leg.Items.Add(this.dictionary_orders[on]);
            ListView l = (ListView)sender;
            Order or = (Order)l.SelectedItem;
            // MessageBox.Show(or.order_number+" "+or.driver_name);
            OrderWindow o = new OrderWindow(mainwindow, or);
            o.ShowDialog();
            // Populate list
            //this.listView.Items.Add(new MyItem { Id = 1, Name = "David" });
            //((DataRowView)((ListView)sender).SelectedItem)["column_name"].ToString();
        }

        /// <summary>
        /// Attempts to dynamically format string to currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void tripConfirmation()
        {
            this.orderConfirmation();
        }

        /**Load all databases*/
        private void loadDB()
        {
            MyWebServices.WebService.loadDriverDB(mainwindow);//load Drivers from DB
            MyWebServices.WebService.loadDestinationDB(mainwindow); // load destinations
            MyWebServices.WebService.loadTruckDB(mainwindow);//load trucks
            MyWebServices.WebService.loadCustomerDB(mainwindow);//load customers 
                                                          //System.Threading.Thread.Sleep(5000);
                                                          // dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                                                          // dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                                                          //dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                                                          //dispatcherTimer.Start();
        }
        /// <summary>
        /// Wait till the orders are all loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (customers_loaded && trucks_loaded && drivers_loaded && destinations_loaded)
            {
                MyWebServices.WebService.loadOrderDB(mainwindow);//load orders
                dispatcherTimer.Stop();
            }
            // code goes here
        }
        public Task loadDataFromDatabase(ILogin loginWindow)
        {
            Task task = new Task(() => this.loadDB());
            task.Start();
            return task;
        }
    }
}