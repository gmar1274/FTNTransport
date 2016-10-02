using System;

public class Destination
{
    public string name { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zipcode { get; set; }
    public long id { get; set; }
    public bool isTerminal { get; set; }
    public Destination(long id, string name, string addr, string city, string state, string zip)
    {
        this.name = name;
        this.address = addr;
        this.city = city;
        this.state = state;
        this.id = id;
        this.zipcode = zip;
        isTerminal = false;
    }

    public override string ToString() {
        return this.name;
    }
}

