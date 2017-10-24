using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WokShop.Models
{
    public class FoodDbInitializer: CreateDatabaseIfNotExists<FoodContext>
    //public class FoodDbInitializer : DropCreateDatabaseAlways<FoodContext>
    {
        protected override void Seed(FoodContext context)
        {
            context.Foods.Add(new Food {
                Category = "hot",
                Name = "Пад Тай с курицей",
                Description = "Обжаренная в соусе лапша Пад Тай - блюдо, которое сами тайцы считают основным в тайской кухне",
                Image = "/Images/hot.png",
                Weight = 400,
                Price = 90000
            });

            context.Foods.Add(new Food
            {
                Category = "wok",
                Name = "Курица в соусе терияки с лапшой удон",
                Description = "Нежные замаринованные кусочки курицы в легком соусе терияки",
                Image = "/Images/wok.png",
                Weight = 400,
                Price = 90000
            });

            context.Foods.Add(new Food
            {
                Category = "soup",
                Name = "Тайский суп том-ям",
                Description = "Традиционный тайский суп с морепродуктами",
                Image = "/Images/soup.png",
                Weight = 300,
                Price = 60000
            });

            base.Seed(context);
        }
    }
}