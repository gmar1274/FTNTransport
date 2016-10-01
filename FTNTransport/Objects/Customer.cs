using System;

public class Customer

{

    public Order last_order { get; set; }
    public string name { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public string routing_number { get; set; }
    public string account_number { get; set; }
    public long id { get; set; }
	public Customer()
	{
	}
    public Customer(long id,string name,string email, string phone,string rn,string an) {
        this.name = name;
        this.id = id;
        this.email = email;
        this.phone = phone;
        this.routing_number = rn;
        this.account_number = an;
    }
    public Order getLastOrder()
    {
        Order o = new Order();
        //get most recent order from DB
        return o;
    }
    public override string ToString()
    {
        return this.name;
    }

}
