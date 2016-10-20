using MyEncryption;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FTNTransport;
using System.Windows;
using System.Globalization;
using FTNTransport.Windows;

namespace MyWebServices
{
    public class WebService
    {
        //POST call to driver.php web response expecting JSON response
        public static async void loadDriverDB(MainWindow mw)
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("driver", Encryption.encrypt("acbadriveracba")),

 new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/driver.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    JArray a = JArray.Parse(json);
                    // Console.WriteLine("Count:: " + a.Count);
                    //Console.WriteLine(json);
                    //Console.WriteLine(a.ToString());
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long id = long.Parse(item.GetValue("id").ToString());
                        string fname = item.GetValue("fname").ToString();
                        string mname = item.GetValue("mname").ToString();
                        string lname = item.GetValue("lname").ToString();
                        string email = item.GetValue("email").ToString();
                        string phone = item.GetValue("phone").ToString();
                        string rn = item.GetValue("routing_number").ToString();
                        string an = item.GetValue("account_number").ToString();
                        string bank = item.GetValue("bank").ToString();
                        string cdl_expr = item.GetValue("cdl_expr").ToString();
                        string medical_expr = item.GetValue("medical_expr").ToString();
                        DateTime created = DateTime.Parse(item.GetValue("created").ToString());
                        Driver d = new Driver(id, fname, mname, lname, email, phone,rn,an,bank,cdl_expr,medical_expr,created);
                        if (!mw.dictionary_drivers.ContainsKey(d.name))
                        {
                            mw.dictionary_drivers.Add(d.name, d);

                            mw.listView_driver.Items.Add(d);
                            
                            //d = null;
                        }
                    }
                    //foreach (JObject o in a.Children<JObject>())
                    //{
                    //    //foreach (JProperty p in o.Properties())
                    //    //{
                    //    //    string name = p.Name;
                    //    //    //string value = (string)p.Value;
                    //    //    Console.WriteLine(name);// + " -- " + value);
                    //    //}
                    //    Console.WriteLine(o.ToString());
                    //}

                    // Console.WriteLine(JObject.Parse(json));
                }
                mw.populateComboBoxes();
                mw.drivers_loaded = true;
            }
            catch (Exception eee)
            {

               Error();
                Console.WriteLine(eee.ToString());
            }
        }

        //POST data to driver.php 
        public static async void insertDriverDB(MainWindow mw,string[] arr)
        {
            try
            {
                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("driver", Encryption.encrypt("acbadriveracba")),
        new KeyValuePair<string, string>("fname", arr[0]),
        new KeyValuePair<string, string>("mname", arr[1]),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
          new KeyValuePair<string, string>("lname", arr[2]),
        new KeyValuePair<string, string>("email",arr[3] ),
        new KeyValuePair<string, string>("phone", arr[4]),
        new KeyValuePair<string, string>("routing_number", arr[5]),
        new KeyValuePair<string, string>("account_number", arr[6]),
        new KeyValuePair<string, string>("bank", arr[7]),
        new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("cdl_expr", arr[8]),
          new KeyValuePair<string, string>("medical_expr", arr[9]),
           new KeyValuePair<string, string>("created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new KeyValuePair<string, string>("password",Encryption.encrypt(arr[10])),

                    new KeyValuePair<string, string>("insert","true")// Company.Name)

    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/driver.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Driver successfully has been added!");
                    loadDriverDB(mw);
                }
            }
            catch (Exception eee)
            {

               Error();
                Console.WriteLine(eee.ToString());
            }
        }

       

        //POST data to destination.php 
        public static async void insertDestinationDB(MainWindow mw, string[] arr)
        {
            try
            {
                var client = new HttpClient();
                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("destination", Encryption.encrypt("acbadestinationacba")),
        new KeyValuePair<string, string>("name", arr[0]),
        new KeyValuePair<string, string>("address", arr[1]),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
          new KeyValuePair<string, string>("city", arr[2]),
        new KeyValuePair<string, string>("state",arr[3] ),
        new KeyValuePair<string, string>("zipcode", arr[4]),
        new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("insert","true")// Company.Name)
    };
                bool isterminal = false;
                if (arr.Length == 6 && arr[5].ToLower().Contains("terminal"))
                {
                    isterminal = true;
                   pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("destination", Encryption.encrypt("acbadestinationacba")),
        new KeyValuePair<string, string>("name", arr[0]),
        new KeyValuePair<string, string>("address", arr[1]),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
          new KeyValuePair<string, string>("city", arr[2]),
        new KeyValuePair<string, string>("state",arr[3] ),
        new KeyValuePair<string, string>("zipcode", arr[4]),
         new KeyValuePair<string, string>("insert","true"),// Company.Name),
         new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("terminal","true")// Company.Name)
    };
                }
                

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/destination.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    if (isterminal) {
                        MessageBox.Show("Success. Terminal has been added!");
                    }
                    else {
                        MessageBox.Show("Success. Destination has been added!");
                    }
                    loadDestinationDB(mw);
                }
            }
            catch (Exception eee)
            {

               Error();
                Console.WriteLine(eee.ToString());
            }
        }

        public static async void loadDestinationDB(MainWindow mw)
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("destination", Encryption.encrypt("acbadestinationacba")),
new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/destination.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    JArray a = JArray.Parse(json);
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long id = Int64.Parse(item.GetValue("id").ToString());
                        string name = item.GetValue("name").ToString();
                        string addr = item.GetValue("address").ToString();
                        string city = item.GetValue("city").ToString();
                        string state = item.GetValue("state").ToString();
                        string zip = item.GetValue("zipcode").ToString();
                        bool isterminal = item.GetValue("isterminal").ToString().Contains("1");//is true if one
                        Destination d = new Destination(id, name,addr,city,state,zip);
                        d.isTerminal = isterminal;
                        if (!mw.dictionary_destination.ContainsKey(d.name))
                        {
                            mw.dictionary_destination.Add(d.name, d);

                            if (d.isTerminal) {
                                mw.comboBox_terminal.Items.Add(d.ToString());
                            } else {
                                mw.comboBox_destination.Items.Add(d.ToString());
                            }

                            //d = null;
                        }
                    }
                }
                // mw.populateDestComboBoxes();
                mw.destinations_loaded = true;
            }
            catch (Exception eee)
            {

               Error();
                Console.WriteLine(eee.ToString());
            }
        }

        //POST data to truck.php 
        public static async void insertTruckDB(MainWindow mw, string[] arr)
        {
            try
            {
                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("truck", Encryption.encrypt("acbatruckacba")),
        new KeyValuePair<string, string>("name", arr[0]),
        new KeyValuePair<string, string>("license_plate", arr[1]),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
          new KeyValuePair<string, string>("vin", arr[2]),
          new KeyValuePair<string, string>("mpg", arr[3]),
          new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
          new KeyValuePair<string, string>("rfid",arr[4]),
          new KeyValuePair<string, string>("created",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
         new KeyValuePair<string, string>("insert","true")
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/truck.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Success. Truck has been added!");
                    loadTruckDB(mw);
                    mw.populateComboBoxes();
                }
            }
            catch (Exception eee)
            {
               Error();
                Console.WriteLine(eee.ToString());
            }
        }
        public static async void loadTruckDB(MainWindow mw)
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
                    new KeyValuePair<string, string>("truck", Encryption.encrypt("acbatruckacba")),

         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/truck.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    JArray a = JArray.Parse(json);
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long id = long.Parse(item.GetValue("id").ToString());
                        string name = item.GetValue("name").ToString();
                        string lp = item.GetValue("license_plate").ToString();
                        string vin = item.GetValue("vin").ToString();
                        int mpg = int.Parse(item.GetValue("mpg").ToString());
                        DateTime created = DateTime.Parse(item.GetValue("created").ToString());
                        string rfid = item.GetValue("rfid").ToString();
                        Truck t = new Truck(id, name, lp, vin,mpg,created,rfid);
                        if (!mw.dictionary_trucks.ContainsKey(t.name))
                        {
                            mw.dictionary_trucks.Add(t.name, t);
                            mw.listView_truck.Items.Add(t);
                        }
                    }
                    mw.populateComboBoxes();
                    mw.trucks_loaded = true;
                }
            }
            catch (Exception eee)
            {
               Error();
                Console.WriteLine(eee.ToString());
            }
        }
        //POST data to customer.php 
        //arr[0]=name
        public static async void insertCustomerDB(MainWindow mw, string[] arr)
        {
            try
            {
                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
                    new KeyValuePair<string, string>("customer", Encryption.encrypt("acbacustomeracba")),
        new KeyValuePair<string, string>("name", arr[0]),
         new KeyValuePair<string, string>("email", arr[1]),
          new KeyValuePair<string, string>("phone", arr[2]),
         new KeyValuePair<string, string>("routing_number", arr[3]),
          new KeyValuePair<string, string>("account_number", arr[4]),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
         new KeyValuePair<string, string>("insert","true")
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/customer.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Success. Customer has been added!");
                    loadCustomerDB(mw);
                    mw.populateComboBoxes();
                }
            }
            catch (Exception eee)
            {
               Error();
                Console.WriteLine(eee.ToString());
            }
        }
        public static async void loadCustomerDB(MainWindow mw)
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("customer", Encryption.encrypt("acbacustomeracba")),
new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/customer.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    JArray a = JArray.Parse(json);
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long id = Int64.Parse(item.GetValue("id").ToString());
                        string name = item.GetValue("name").ToString();
                        string email = item.GetValue("email").ToString();
                        string phone = item.GetValue("phone").ToString();
                        string rn = item.GetValue("routing_number").ToString();
                        string an = item.GetValue("account_number").ToString();

                        Customer c = new Customer(id, name,email,phone,rn,an);
                        if (!mw.dictionary_customers.ContainsKey(c.name))
                        {
                            mw.dictionary_customers.Add(c.name, c);
                            mw.listView_customer.Items.Add(c);
                        }
                    }
                    mw.populateComboBoxes();
                    mw.customers_loaded = true;
                }
            }
            catch (Exception eee)
            {
               Error();
                Console.WriteLine(eee.ToString());
            }
        }
        //POST data to order.php 
        public static async void insertOrderDB(MainWindow mw, string[] arr)
        {
            try
            {
                var client = new HttpClient();
                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("order", Encryption.encrypt("acbaorderacba")),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
         //string[] arr = new string[] { driver, terminal, end_dest, driver_comm, truck, "pending...", cust, container, size, terminal, lfd };
        new KeyValuePair<string, string>("driver_id", mw.dictionary_drivers[ arr[0]].id.ToString()),//get the id of the driver
        new KeyValuePair<string, string>("start_dest_id",mw.dictionary_destination[arr[1]].id.ToString()),//start pos
        new KeyValuePair<string, string>("end_dest_id",mw.dictionary_destination[arr[2]].id.ToString()),
        new KeyValuePair<string, string>("driver_commission",arr[3] ),
        new KeyValuePair<string, string>("truck_id", mw.dictionary_trucks[arr[4]].id.ToString()),
        new KeyValuePair<string, string>("status",arr[5] ),
        new KeyValuePair<string, string>("customer_id", mw.dictionary_customers[arr[6]].id.ToString()),
        new KeyValuePair<string, string>("container", arr[7]),

new KeyValuePair<string, string>("size",arr[8] ),
        new KeyValuePair<string, string>("terminal_dest_id",mw.dictionary_destination[arr[9]].id.ToString()),
        new KeyValuePair<string, string>("lfd",arr[10]),
        new KeyValuePair<string, string>("created",DateTime.Now.ToString()),

         new KeyValuePair<string, string>("amount",arr[11]),


         new KeyValuePair<string, string>("shipping_line",arr[12]),
        new KeyValuePair<string, string>("cargo",arr[13]),
        new KeyValuePair<string, string>("pickup_datetime",arr[14]),

         new KeyValuePair<string, string>("pickup_sku",arr[15]),
          new KeyValuePair<string, string>("delivery_sku",arr[16]),
          new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
          new KeyValuePair<string, string>("insert","true")
    };
                var content = new FormUrlEncodedContent(pairs);
                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/order.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                   
                        MessageBox.Show("Success. Order has been added!");
                         mw.orderConfirmation();
                    loadOrderDB(mw);
                }
            }
            catch (Exception eee)
            {

                Error();
                Console.WriteLine(eee.ToString());
            }
        }
        /// <summary>
        /// SELECT * FROM `order` O, `driver` D, `destination` DD, `destination` DDD, `truck` T WHERE O.driver_id=D.id AND O.start_destination_id=DD.id AND O.end_destination_id=DDD.id AND O.truck_id=T.id 
        /// </summary>
        /// <param name="mw"></param>
        public static async void loadOrderDB(MainWindow mw)
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("order", Encryption.encrypt("acbaorderacba")),
                    new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/order.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                   // MessageBox.Show(json);
                    JArray a = JArray.Parse(json);
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long order_number = long.Parse(item.GetValue("order_number").ToString());
                        long driver_id = long.Parse(item.GetValue("driver_id").ToString());
                        long start_dest_id = long.Parse(item.GetValue("start_destination_id").ToString());
                        long end_dest_id = long.Parse(item.GetValue("end_destination_id").ToString());
                        
                        DateTime oc = DateTime.Parse(item.GetValue("order_created_datetime").ToString());//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        string ofd = item.GetValue("out_for_delivery_datetime").ToString();
                        string od = item.GetValue("delivery_confirmation_datetime").ToString();
                        DateTime? shipped = null;
                        if (ofd != null && ofd.Length>0)
                        {
                            shipped = DateTime.Parse(ofd);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        DateTime? delivered = null;
                        if (od != null && od.Length>0) {
                            delivered = DateTime.Parse(od);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        decimal comm = decimal.Parse(item.GetValue("driver_commission").ToString());
                        long truck_id = long.Parse(item.GetValue("truck_id").ToString());
                        string status = item.GetValue("status").ToString();
                        long cust_id = long.Parse(item.GetValue("customer_id").ToString());
                        string container = item.GetValue("container").ToString();
                        string size = item.GetValue("size").ToString();
                        int terminal = int.Parse(item.GetValue("terminal").ToString());
                        string l = item.GetValue("lfd").ToString();

                        DateTime? lfd = null;
                        if (l != null && l.Length > 0)
                        {
                            lfd = DateTime.Parse(l);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        decimal amount = decimal.Parse(item.GetValue("amount").ToString());
                        ///
                        string shipping_line = item.GetValue("shipping_line").ToString();
                        string cargo = item.GetValue("cargo").ToString();
                        string pkup = item.GetValue("pickup_datetime").ToString();
                        DateTime? picku_datetime = null;
                        if (pkup != null && pkup.Length > 0)
                        {
                            
                            picku_datetime = DateTime.Parse(pkup);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        string pickup_sku = (item.GetValue("pickup_sku").ToString());
                        string delivery_sku = (item.GetValue("delivery_sku").ToString());


                        Order o = new Order(mw,order_number,driver_id,truck_id,start_dest_id,end_dest_id,cust_id,oc,shipped,delivered
                            ,lfd,status,terminal,container,size,comm,amount,shipping_line,cargo,picku_datetime,
                            pickup_sku,delivery_sku);
                        //MessageBox.Show(o.container);
                        if (!mw.dictionary_orders.ContainsKey(o.order_number))
                        {
                            mw.dictionary_orders.Add(o.order_number, o);
                            mw.listView_order.Items.Add(o);
                        }
                    }
                    loadOrderDB(mw);
                }///////response from server was good

            }
            catch (Exception eee)
            {
                Console.WriteLine("HEREEEEEEEEE:: "+eee.ToString());
                Error();
            }
        }
        /// <summary>
        /// Just display an error. Later will implement logic checking like highlight the error..
        /// </summary>
        private static void Error()
        {
            MessageBox.Show(
                    "Error. No data has been saved.",
                    "ACBA Dispatch Program",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }
        public static async void Login(LoginWindow lw)
        {
            try
            {
                string username = lw.textBox_username.Text;
                string password = Encryption.encrypt(lw.passwordBox.Password);
               
                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("login", Encryption.encrypt("acbaloginacba")),
                      new KeyValuePair<string, string>("password", password),
                          new KeyValuePair<string, string>("username", username),


         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/login.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                   // MessageBox.Show(username+" "+password+" json: "+json );
                    long id = long.Parse(json);
                    lw.user_id = id;
                    lw.Close();
                        
                        
                }
                
            }
            catch (Exception eee)
            {
                MessageBox.Show(
                       "Username/password did not match.",
                       "ACBA Dispatch Program",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);

            }
        }
        /////////////////////
        //////////Trip.php
        //////////////////////
        //POST data to trip.php
        /***
        array index: orderno,user_id,start_dest,end_dest,start_dt,end_dt,driver,driver_comm,truck,cargo,status
        */ 
        public static async void insertTripDB(MainWindow mw,OrderWindow ow, string[] arr)
        {
            try
            {
                var client = new HttpClient();
                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("trip", Encryption.encrypt("acbatripacba")),
         new KeyValuePair<string, string>("company_name","ftntransport"),// Company.Name)
                                                                         //string[] arr = new string[] { driver, terminal, end_dest, driver_comm, truck, "pending...", cust, container, size, terminal, lfd };

        new KeyValuePair<string, string>("order_number", arr[0]),
          new KeyValuePair<string, string>("user_id", arr[1]),
        new KeyValuePair<string, string>("start_dest_id",mw.dictionary_destination[arr[2]].id.ToString()),//start pos
        new KeyValuePair<string, string>("end_dest_id",mw.dictionary_destination[arr[3]].id.ToString()),
        new KeyValuePair<string, string>("pickup_datetime",arr[4]),
        new KeyValuePair<string, string>("start_datetime",arr[5]),
        new KeyValuePair<string, string>("end_datetime",arr[6]),
        new KeyValuePair<string, string>("driver_id", mw.dictionary_drivers[ arr[7]].id.ToString()),//get the id of the driver
        new KeyValuePair<string, string>("driver_commission",arr[8] ),
        new KeyValuePair<string, string>("truck_id", mw.dictionary_trucks[arr[9]].id.ToString()),
        new KeyValuePair<string, string>("cargo",arr[10]),
        new KeyValuePair<string, string>("status",arr[11] ),
         new KeyValuePair<string, string>("delivery_sku",arr[12] ),
          new KeyValuePair<string, string>("pickup_sku",arr[13] ),
        // new KeyValuePair<string, string>("pickup_sku",arr[15]),
        //  new KeyValuePair<string, string>("delivery_sku",arr[16]),
          new KeyValuePair<string, string>("insert","true")
    };
                var content = new FormUrlEncodedContent(pairs);
                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/trip.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Success. Trip has been added!");
                    mw.tripConfirmation();
                    ow.listView_leg.Items.Clear();
                    loadTripDB(mw,ow,ow.order.order_number);
                }
            }
            catch (Exception eee)
            {
                Error();
                Console.WriteLine(eee.ToString());
            }
        }

        /// <summary>
        /// SELECT * FROM `order` O, `driver` D, `destination` DD, `destination` DDD, `truck` T WHERE O.driver_id=D.id AND O.start_destination_id=DD.id AND O.end_destination_id=DDD.id AND O.truck_id=T.id 
        /// </summary>
        /// <param name="mw"></param>
        public static async void loadTripDB(MainWindow mw, OrderWindow ow, long order_no)
        {
            try
            {

                var client = new HttpClient();

                var pairs = new List<KeyValuePair<string, string>>
    {
                    new KeyValuePair<string, string>("trip", Encryption.encrypt("acbatripacba")),
                    new KeyValuePair<string, string>("user_id", mw.user_id.ToString()),
                    new KeyValuePair<string, string>("order_number",order_no.ToString()),
         new KeyValuePair<string, string>("company_name","ftntransport")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/trip.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                   
                    string json = await response.Content.ReadAsStringAsync();
                    // MessageBox.Show(json);
                    JArray a = JArray.Parse(json);
                    for (int i = 0; i < a.Count; ++i)
                    {
                        var item = (JObject)a[i];
                        long order_number = long.Parse(item.GetValue("order_number").ToString());
                        long driver_id = long.Parse(item.GetValue("driver_id").ToString());
                        long start_dest_id = long.Parse(item.GetValue("start_destination_id").ToString());
                        long end_dest_id = long.Parse(item.GetValue("end_destination_id").ToString());

                        DateTime pickup_datetime = DateTime.Parse(item.GetValue("pickup_datetime").ToString());

                        string isshipped = item.GetValue("start_datetime").ToString();

                        DateTime? shipped = null;
                        if (isshipped != null && isshipped.Length > 0 && !isshipped.Contains("0000-00-00 00:00:00"))
                        {
                            shipped = DateTime.Parse(isshipped);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        //out for delivery
                        string delivered_date = item.GetValue("end_datetime").ToString();

                        DateTime? delivered = null;
                        if (delivered_date != null && delivered_date.Length > 0 && !delivered_date.Contains("0000-00-00 00:00:00"))
                        {
                            delivered = DateTime.Parse(delivered_date);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        decimal comm = decimal.Parse(item.GetValue("driver_commission").ToString());
                        long truck_id = long.Parse(item.GetValue("truck_id").ToString());
                        string status = item.GetValue("status").ToString();
                        string cargo = item.GetValue("cargo").ToString();
                        string pickup_sku = (item.GetValue("pickup_sku").ToString());
                        string delivery_sku = (item.GetValue("delivery_sku").ToString());
                      
                        Trip t = new Trip(mw, order_number, mw.user_id, start_dest_id, end_dest_id, pickup_datetime, shipped, delivered, driver_id, comm, truck_id, cargo, status, delivery_sku, pickup_sku);
                        //MessageBox.Show(o.container);


                        ow.listView_leg.Items.Add(t);

                    }
                    //loadTripDB(mw);
                }///////response from server was good

            }
            catch (Exception eee)
            {
                Console.WriteLine("HEREEEEEEEEE:: " + eee.ToString());
                Error();
            }
        }
    }///end namespace
}//end class
