using System;

public class Driver
{
    public long id { get; set; }
    public string email { get; set; }
    public string fname { get; set; }
    public string mname { get; set; }
    public string lname { get; set; }
    public string phone { get; set; }
    public string routing_number { get; set; }
    public string account_number { get; set; }
    public string name { get { return getNameString(); }  }
    public string bank { get; set; }
    public string cdl_expr { get; set; }
    public string medical_expr { get; set; }
    public string date_commissioned { get { return created.ToShortDateString(); } }
    public DateTime created { set; get; }
    public Driver()
	{
	}
    public Driver(long id,string fname,string mname,string lname,string email,string phone,string rn, string an,string bank,string cdl,string med,DateTime created) {
        this.id = id;
        this.fname = fname;
        this.mname = mname;
        this.lname = lname;
        this.email = email;
        this.phone = phone;
        this.routing_number = rn;
        this.account_number = an;
        this.bank = bank;
        this.cdl_expr = cdl;
        this.medical_expr = med;
        this.created = created;
    }
    private string getNameString()
    {
        if (mname.Length == 0) return fname + " " + lname;
        return fname + " " + mname + " " + lname;
    }
    public override string ToString()
    {
       
        return this.getNameString();
    }
}
