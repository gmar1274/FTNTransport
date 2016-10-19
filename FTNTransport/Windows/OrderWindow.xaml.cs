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
            listView_leg.Items.Add(this.order);

            // updates the ListView with the Order Object
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
                string start_dest = this.comboBox_terminal.SelectedValue.ToString();//start dest
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
                DateTime start_datetime = DateTime.Parse(this.datepicker_pickup.SelectedDate.Value.ToShortDateString() + " " + hour + ":" + min + ":00", new CultureInfo("en-US"));
                /// string pickup_sku = this.comboBox_pickup_number.Text;
                /// string delivery_sku = this.comboBox_delivery_number.Text;

                string[] arr = new string[] { this.order.order_number.ToString(), mw.user_id.ToString(), start_dest, end_dest, start_datetime.ToString("yyyy-MM-dd HH:mm:ss").ToString(), null, driver, driver_comm, truck,cargo,"out for delivery..."};//this.order.pickup_sku, this.order.delivery_sku };
                for (int i = 0; i < arr.Length; ++i)//absolutely no logic checking...
                {
                    string s = arr[i];
                    if (i == 5) continue;
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
