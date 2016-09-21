using System;

public class Order
{
    private long order_number,driver_id,truck_id,start_dest_id,end_dest_id,customer_id;
    private DateTime order_created, out_for_delivery, delivery_confirmation,lfd;
    private string status,container;
    private int size;
    private double driver_commission;

	public Order()
	{
	}
}
