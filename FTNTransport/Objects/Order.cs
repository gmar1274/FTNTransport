using System;

public class Order
{
    private long order_number { get; set; }
    private long driver_id { get; set; }
    private long truck_id { get; set; }
    private long start_dest_id { get; set; }
    private long end_dest_id { get; set; }
    private long customer_id { get; set; }
    private DateTime order_created { get; set; }
    private DateTime out_for_delivery { get; set; }
    private DateTime delivery_confirmation { get; set; }
    private DateTime lfd { get; set; }
    private string status { get; set; }
    private string container { get; set; }
    private int size { get; set; }
    private double driver_commission { get; set; }

    public Order()
	{
	}
}
