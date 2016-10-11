using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace FTNTransport.Windows
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
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

        }
    }
}
