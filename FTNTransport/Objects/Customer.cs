using System;

public class Customer

{

    private Order last_order { get; set; }
    private string name { get; set; }
    private long id { get; set; }
	public Customer()
	{
	}
    public Customer(string name) {
        this.name = name;
    }
    public Order getLastOrder()
    {
        Order o = new Order();
        //get most recent order from DB
        return o;
    }

}
