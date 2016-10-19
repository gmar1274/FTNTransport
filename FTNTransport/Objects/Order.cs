using FTNTransport;
using FTNTransport.Objects;
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
        this.driver_name = Utils.getDriver(mw,driver);
        this.truck_name = Utils.getTruck(mw,truck);
        this.start_dest_name = Utils.getDestination(mw,sd);
        this.end_dest_name = Utils.getDestination(mw,ed);
        this.customer_name = Utils.getCustomer(mw,cust);
        this.order_created = oc;
        this.out_for_delivery = shipped;
        this.delivery_confirmation = delivered;
        this.lfd = lfd;
        this.status = status;
        this.container = con;
        this.size = size;
        this.driver_commission = comm;
        this.terminal = Utils.getDestination(mw,terminal);
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

   
}
