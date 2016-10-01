﻿using MyEncryption;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FTNTransport;
using System.Windows;

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
                        long id = Int64.Parse(item.GetValue("id").ToString());
                        string fname = item.GetValue("fname").ToString();
                        string mname = item.GetValue("mname").ToString();
                        string lname = item.GetValue("lname").ToString();
                        string email = item.GetValue("email").ToString();
                        string phone = item.GetValue("phone").ToString();
                        string rn = item.GetValue("routing_number").ToString();
                        string an = item.GetValue("account_number").ToString();

                        Driver d = new Driver(id, fname, mname, lname, email, phone,rn,an);
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
            }
            catch (Exception eee)
            {

                MessageBox.Show("Error. No data has been saved.");
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

                MessageBox.Show("Error. No data has been saved.");
                Console.WriteLine(eee.ToString());
            }
        }
        //POST data to driver.php 
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
         new KeyValuePair<string, string>("insert","true")// Company.Name)
    };

                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = await client.PostAsync("http://www.acbasoftware.com/ftntransport/destination.php", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Success. Destination has been added!");
                    loadDestinationDB(mw);
                }
            }
            catch (Exception eee)
            {

                MessageBox.Show("Error. No data has been saved.");
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
                        Destination d = new Destination(id, name,addr,city,state,zip);
                        if (!mw.dictionary_destination.ContainsKey(d.name))
                        {
                            mw.dictionary_destination.Add(d.name, d);

                            mw.comboBox_destination.Items.Add(d.ToString());
                           

                            //d = null;
                        }
                    }
                }
               // mw.populateDestComboBoxes();
            }
            catch (Exception eee)
            {

                MessageBox.Show("Error. No data has been saved.");
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
          new KeyValuePair<string, string>("cargo", arr[2]),
          new KeyValuePair<string, string>("mpg", arr[3]),
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
                MessageBox.Show("Error. No data has been saved.");
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
                        long id = Int64.Parse(item.GetValue("id").ToString());
                        string name = item.GetValue("name").ToString();
                        string lp = item.GetValue("license_plate").ToString();
                        string cargo = item.GetValue("cargo_capacity").ToString();
                        int mpg = Int32.Parse(item.GetValue("mpg").ToString());
                        Truck t = new Truck(id, name, lp, cargo,mpg);
                        if (!mw.dictionary_trucks.ContainsKey(t.name))
                        {
                            mw.dictionary_trucks.Add(t.name, t);
                            mw.listView_truck.Items.Add(t);
                        }
                    }
                    mw.populateComboBoxes();
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("Error. No data has been saved.");
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
                MessageBox.Show("Error. No data has been saved.");
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
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("Error. No data has been saved.");
                Console.WriteLine(eee.ToString());
            }
        }
    }///end namespace
}//end class
