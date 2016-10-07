﻿using FTNTransport;
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
    public DateTime lfd { get; set; }
    public string status { get; set; }
    public string terminal { get; set; }
    public string container { get; set; }
    public int size { get; set; }
    public decimal amount { get; set; }
    public string formatted_amount { get; set; }
    public double driver_commission { get; set; }

    public Order(MainWindow mw,long on,long driver,long truck,long sd,long ed, long cust,DateTime oc, DateTime? shipped, DateTime? delivered
        ,DateTime lfd, string status,int terminal,string con,int size, double comm,decimal amount)
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
