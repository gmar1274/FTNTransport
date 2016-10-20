using MyStates;
using System;

using System.Globalization;

using System.Windows;
using System.Windows.Controls;


namespace FTNTransport.Windows
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window

    {
        public MainWindow mw;
        public Order order;
        public OrderWindow(MainWindow mw,Order o)
        {
            this.mw = mw;
            this.order = o;
            InitializeComponent();
           
            MyWebServices.WebService.loadTripDB(mw,this,this.order.order_number);
            listView_order.Items.Add(this.order);
            populateHarcodedItems();
            // updates the ListView with the Order Object
        }
        private async void populateHarcodedItems()
        {
            string[] arr = { "20ST", "40ST", "40HC", "45HC" };
            for (int i = 0; i < arr.Length; ++i)
            {
                this.comboBox_size.Items.Add(arr[i]);
            }
            string[] cargo = { "Loaded", "Empty" };
            foreach (string s in cargo)
            {
                this.comboBox_cargo.Items.Add(s);
            }
            string[] sl = { "APL", "MSK", "MSC", "KLINE", "NYK", "EVE", "COSCO", "WHL", "UA", "OOCL", "SEALAND", "HMM", "PIL", "YML", "HAPAG", "MOL" };
            Array.Sort(sl);
            foreach (string line in sl)
            {
                this.comboBox_shippingLine.Items.Add(line);
            }
            for (int i = 0; i < 60; ++i)
            {
                string format = i.ToString("0#");
                if (i < 24)
                {
                    this.comboBox_time_hours.Items.Add(format);//fill hours
                }

                this.comboBox_time_min.Items.Add(format);

            }
            foreach(string id in this.mw.dictionary_destination.Keys)
            {
                Destination d = this.mw.dictionary_destination[id];
                this.comboBox_destination.Items.Add(d.name);
                this.comboBox_pickup.Items.Add(d.name);
            }
            foreach (string id in this.mw.dictionary_drivers.Keys)
            {
                Driver d = this.mw.dictionary_drivers[id];
                this.comboBox_driver.Items.Add(d.ToString());
            }
            foreach (string id in this.mw.dictionary_trucks.Keys)
            {
                Truck t = this.mw.dictionary_trucks[id];
                this.comboBox_truck.Items.Add(t.name);
            }


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
       
        private void button_createLeg_Click(object sender, RoutedEventArgs e)
        {
            try
            {

               // string lfd = null;
              //  if (this.datepicker_lfd.SelectedDate != null)
              //  {
              //      lfd = this.datepicker_lfd.SelectedDate.Value.ToShortDateString();

             //   }
               // string cust = this.comboBox_customer.SelectedValue.ToString();
               // string container = this.comboBox_container.Text;///might have to fix this
                string start_dest = this.comboBox_pickup.SelectedValue.ToString();//start dest
               // string size = this.comboBox_size.SelectedValue.ToString();

                string end_dest = this.comboBox_destination.SelectedValue.ToString();
                string driver = this.comboBox_driver.SelectedValue.ToString();
                string truck = this.comboBox_truck.SelectedValue.ToString();
                string driver_comm = this.textBox_driverCommission_dollars.Text.ToString().Replace(",", "") + "." + this.textBox_driverCommission_cents.Text;
               // string amount = this.textBox_order_dollars.Text.Replace(",", "") + "." + this.textBox_order_cents.Text;

                //string shipping_line = this.comboBox_shippingLine.SelectedValue.ToString();
                string cargo = this.comboBox_cargo.SelectedValue.ToString();
                string hour = this.comboBox_time_hours.SelectedValue.ToString();
                string min = this.comboBox_time_min.SelectedValue.ToString();
                DateTime pickup_datetime = DateTime.Parse(this.datepicker_pickup.SelectedDate.Value.ToShortDateString() + " " + hour + ":" + min + ":00", new CultureInfo("en-US"));
                /// string pickup_sku = this.comboBox_pickup_number.Text;
                /// string delivery_sku = this.comboBox_delivery_number.Text;

                string[] arr = new string[] { this.order.order_number.ToString(), mw.user_id.ToString(), start_dest, end_dest, pickup_datetime.ToString("yyyy-MM-dd HH:mm:ss").ToString(),null, null, driver, driver_comm, truck,cargo,"pending..."};//this.order.pickup_sku, this.order.delivery_sku };
                for (int i = 0; i < arr.Length; ++i)//absolutely no logic checking...
                {
                    string s = arr[i];
                    if (i == 5 || i==6) continue;
                    if (s == null || s.Length == 0)
                    {

                        mw.orderError();
                        return;
                    }
                }
            MyWebServices.WebService.insertTripDB(mw,this,arr);
            }
            catch (Exception ee)
            {
                mw.orderError();
                Console.WriteLine(ee);
            }

        }
    }
}
