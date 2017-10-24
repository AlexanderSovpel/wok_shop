using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WokShop.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }

        /*private int count = 1;
        public int Count {
            get { return count; }
            set { count = value; }
        }*/
    }

    public class FoodComparer: IEqualityComparer<Food>
    {
        public bool Equals(Food f1, Food f2)
        {
            if (f2 == null && f1 == null)
                return true;
            else if (f1 == null | f2 == null)
                return false;
            else if (f1.Id == f2.Id)
                return true;
            else
                return false;
        }

        public int GetHashCode(Food obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    
}