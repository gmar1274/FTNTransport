using System;

public class Customer

{
    private string name;
    private Order last_order;
    public Order get_last_order{
        get { return this.last_order; }
        set
        {
            this.last_order =value;
        }
    }
    public string get_name {
        get { return this.name; }
        set { this.name = value; }
    }
	public Customer()
	{
	}
    public Customer(string name) {
        this.name = name;
    }
    public Order getLastOrder()
    {
        Order o = new Order();
        return o;
    }

}
