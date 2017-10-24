using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WokShop.Models
{
    public class FoodContext: DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FoodsInOrder> FoodsInOrder { get; set; }
        public DbSet<User> Users { get; set; }
    }
}