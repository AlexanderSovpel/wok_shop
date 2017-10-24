using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WokShop.Models
{
    public class FoodsInOrder
    {
        //public int FoodOrderId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int FoodId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int OrderId { get; set; }

        public int Count { get; set; }
    }

    public class FoodsInOrderComparer : IEqualityComparer<FoodsInOrder>
    {
        //TODO:???
        public bool Equals(FoodsInOrder f1, FoodsInOrder f2)
        {
            if (f2 == null && f1 == null)
                return true;
            else if (f1 == null | f2 == null)
                return false;
            else if (f1.FoodId == f2.FoodId)
                return true;
            else
                return false;
        }

        public int GetHashCode(FoodsInOrder obj)
        {
            return obj.FoodId.GetHashCode();
        }
    }
}