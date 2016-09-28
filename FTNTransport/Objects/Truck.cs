﻿
  public  class Truck
    {
    public long id { set; get; }
    public string name { set; get; }
    public string cargo_capacity { set; get; }
    public int mpg { set; get; }
    public string license_plate { set; get; }
    public Truck(long id,string name,string lp, string cargo,int mpg) {
        this.id = id;
        this.name = name;
        this.license_plate = lp;
        this.cargo_capacity = cargo;
        this.mpg = mpg;
    }
    public override string ToString()
    {
        return "ID: "+id+" Name: "+name;
    }
}
