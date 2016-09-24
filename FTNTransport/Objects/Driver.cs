using System;

public class Driver
{
    public long id { get; set; }
    public string email { get; set; }
    public string fname { get; set; }
    public string mname { get; set; }
    public string lname { get; set; }
    public string phone { get; set; }
    public string name { get { return getNameString(); }  }

	public Driver()
	{
	}
    public Driver(long id,string fname,string mname,string lname,string email,string phone) {
        this.id = id;
        this.fname = fname;
        this.mname = mname;
        this.lname = lname;
        this.email = email;
        this.phone = phone;
    }
    private string getNameString()
    {
        if (mname.Length == 0) return fname + " " + lname;
        return fname + " " + mname + " " + lname;
    }
    public string toString()
    {
        return this.getNameString();
    }
}
