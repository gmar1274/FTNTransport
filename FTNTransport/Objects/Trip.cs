
using FTNTransport;
using FTNTransport.Objects;
using System;
public class Trip
    {
    public long order_number { set; get; }
    public long user_id { set; get; }
    public string start_destination { set; get; }
    public string end_destination { set; get; }
    public DateTime pickup { set; get; }
    public DateTime? start_datetime { set; get; }
    public DateTime? end_datetime { set; get; }
    public string driver { set; get; }
    public Decimal driver_commission { set; get; }
    public string truck { set; get; }
    public string cargo { set; get; }
    public string status { set; get; }
    public string delivery_sku { set; get; }
    public string pickup_sku{ set; get; }


    public Trip(MainWindow mw,long on, long uid,long sd,long ed, DateTime pkup,DateTime? sdt, DateTime? edt,long did,Decimal driver_comm,long tid,string cargo,string status,string dsku,string pkupsku)
        {
        this.order_number = on;
        this.user_id = uid;
        this.start_destination = Utils.getDestination(mw,sd);
        this.end_destination = Utils.getDestination(mw, ed);
        this.pickup = pkup;
        this.start_datetime = sdt;
        this.end_datetime = edt;
        this.driver = Utils.getDriver(mw, did);
        this.driver_commission = driver_comm;
        this.truck = Utils.getTruck(mw, tid);
        this.cargo = cargo;
        this.status = status;
        this.delivery_sku = dsku;
        this.pickup_sku = pkupsku;
        }
    }

