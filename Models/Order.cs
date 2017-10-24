using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WokShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        //public List<Food> FoodCollection { get; set; }
        public string DeliveryType { get; set; }
        public string PayBy { get; set; }
        public DateTime Date { get; set; }

        public Order(string person, string phone, string address, /*List<Food> foods,*/ string deliveryType, string payBy, DateTime date)
        {
            Person = person;
            Phone = phone;
            Address = address;
            //FoodCollection = foods;
            DeliveryType = deliveryType;
            PayBy = payBy;
            Date = date;
        }

        public Order()
        {

        }
    }
}