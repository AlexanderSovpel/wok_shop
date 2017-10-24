using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WokShop.Models
{
    public class CartController : Controller
    {
        FoodContext db = new FoodContext();
        List<Food> FoodList = new List<Food>();
        List<FoodsInOrder> CartList = new List<FoodsInOrder>();

        public ActionResult Cart()
        {
            CartList = (List<FoodsInOrder>)Session["CartList"];
            FoodList = (List<Food>)Session["FoodList"];
            if (CartList != null && FoodList != null)
            {
                int count = 0;
                int total = 0;
                for (int i = 0; i < CartList.Count; ++i)
                {
                    total = total + FoodList[i].Price * CartList[i].Count;
                    count = count + CartList[i].Count;
                }
                ViewBag.Total = total;
                ViewBag.Count = count;
                ViewBag.FoodList = FoodList;
                ViewBag.CartList = CartList;
            }

            return View();
        }

        public int GetCartCount()
        {
            CartList = (List<FoodsInOrder>)Session["CartList"];
            FoodList = (List<Food>)Session["FoodList"];
            int count = 0;
            if (CartList != null && FoodList != null)
            {
                for (int i = 0; i < CartList.Count; ++i)
                {
                    count = count + CartList[i].Count;
                }
            }
            return count;
        }

        public int GetCartTotal()
        {
            CartList = (List<FoodsInOrder>)Session["CartList"];
            FoodList = (List<Food>)Session["FoodList"];
            int total = 0;
            if (CartList != null && FoodList != null)
            {
                for (int i = 0; i < CartList.Count; ++i)
                {
                    total = total + FoodList[i].Price * CartList[i].Count;
                }
            }
            return total;
        }
    }
}