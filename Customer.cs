using System;

public class Customer

{
    private string name;
    private Order last_order;
    public Order last_order{
        get { return this.last_order; }
        set
        {
            this.last_order =value;
        }
    }
    public string name {
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
