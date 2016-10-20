
using System;

public  class Truck
    {
    public long id { set; get; }
    public string name { set; get; }
    public string vin { set; get; }
    public int mpg { set; get; }
    public string license_plate { set; get; }

    public DateTime created { set; get; }
    public string rfid { set; get; }
    public string date_commissioned { get { return created.ToShortDateString(); } }
    public Truck(long id,string name,string lp, string vin,int mpg,DateTime created,string rfid) {
        this.id = id;
        this.name = name;
        this.license_plate = lp;
        this.vin = vin;
        this.mpg = mpg;
        this.created = created;
        this.rfid = rfid;
    }
    public override string ToString()
    {
        return "ID: "+id+" Name: "+name;
    }
}

