using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTNTransport.Objects
{
    public class Utils
    {
        public static string getCustomer(MainWindow mw, long cust)
        {
            Customer c = null;
            foreach (string id in mw.dictionary_customers.Keys)
            {
                c = mw.dictionary_customers[id];
                if (c.id == cust) return c.name;
            }
            return null;
        }

        public static string getDestination(MainWindow mw, long dest)
        {
            Destination d = null;
            foreach (string id in mw.dictionary_destination.Keys)
            {
                d = mw.dictionary_destination[id];
                if (d.id == dest) return d.name;
            }
            return null;
        }
public static string getTruck(MainWindow mw, long truck)
        {
            Truck t = null;
            foreach (string id in mw.dictionary_trucks.Keys)
            {
                t = mw.dictionary_trucks[id];
                if (t.id == truck) return t.name;
            }
            return null;
        }

        public static string getDriver(MainWindow mw, long driver)
        {
            Driver d = null;
            foreach (string id in mw.dictionary_drivers.Keys)
            {
                d = mw.dictionary_drivers[id];
                if (d.id == driver) return d.name;
            }
            return null;
        }
    }
}
