using FTNTransport;
using System;
using System.Globalization;

public class Order
{
    public long order_number { get; set; }
    public string driver_name { get; set; }
    public string truck_name { get; set; }
    public string start_dest_name { get; set; }
    public string end_dest_name { get; set; }
    public string customer_name { get; set; }
    public DateTime order_created { get; set; }
    public DateTime? out_for_delivery { get; set; }
    public DateTime? delivery_confirmation { get; set; }
    public DateTime? lfd { get; set; }
    public string status { get; set; }
    public string terminal { get; set; }
    public string container { get; set; }
    public string size { get; set; }
    public decimal amount { get; set; }
    public decimal driver_commission { get; set; }

    public string shipping_line { get; set; }
    public string cargo { get; set; }
    public DateTime? pickup_datetime { get; set; }
    public string pickup_sku { get; set; }
    public string delivery_sku { get; set; }


    public string formatted_commission { get; set; }
    public string formatted_amount { get; set; }

    public Order(MainWindow mw,long on,long driver,long truck,long sd,long ed, long cust,DateTime oc, DateTime? shipped, DateTime? delivered
        ,DateTime? lfd, string status,int terminal,string con,string size, decimal comm,decimal amount,
        string sl, string cargo, DateTime? pickup,string p_sku,string d_sku
        )
	{
        this.order_number=on;
        this.driver_name = getDriver(mw,driver);
        this.truck_name = getTruck(mw,truck);
        this.start_dest_name = getDestination(mw,sd);
        this.end_dest_name = getDestination(mw,ed);
        this.customer_name = getCustomer(mw,cust);
        this.order_created = oc;
        this.out_for_delivery = shipped;
        this.delivery_confirmation = delivered;
        this.lfd = lfd;
        this.status = status;
        this.container = con;
        this.size = size;
        this.driver_commission = comm;
        this.terminal = getDestination(mw,terminal);
        this.amount = amount;
        formatted_amount = amount.ToString("C", new CultureInfo("en-US"));
        this.formatted_commission=comm.ToString("C", new CultureInfo("en-US"));

        this.shipping_line = sl;
        this.cargo = cargo;
        this.pickup_datetime = pickup;
        this.pickup_sku = p_sku;
        this.delivery_sku = d_sku;
    }

   

    public Order() { }

    private string getCustomer(MainWindow mw, long cust)
    {
        Customer c = null;
        foreach (string id in mw.dictionary_customers.Keys) {
            c = mw.dictionary_customers[id];
            if (c.id == cust) return c.name;
        }
        return null;
    }

    private string getDestination(MainWindow mw, long dest)
    {
        Destination d = null;
        foreach (string id in mw.dictionary_destination.Keys)
        {
            d = mw.dictionary_destination[id];
            if (d.id == dest) return d.name;
        }
        return null;
    }

    private string getTruck(MainWindow mw,long truck)
    {
        Truck t = null;
        foreach (string id in mw.dictionary_trucks.Keys)
        {
           t = mw.dictionary_trucks[id];
            if (t.id == truck) return t.name;
        }
        return null;
    }

    private string getDriver(MainWindow mw, long driver)
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
