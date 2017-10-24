using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WokShop.Models;

namespace WokShop.Controllers
{
    public class HomeController : Controller
    {
        FoodContext db = new FoodContext();
        List<Food> FoodList = new List<Food>();
        List<FoodsInOrder> CartList = new List<FoodsInOrder>();

        public ActionResult Index(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.SingleFood = db.Foods.Find(id);
            }
            else
            {
                ViewBag.Foods = db.Foods.ToArray();
                ViewBag.FoodsCount = db.Foods.Count();
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Delivery()
        {
            return View();
        }

        public ActionResult Hots(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.SingleFood = db.Foods.Find(id);
            }
            else
                ViewBag.HotFood = db.Foods.Where(food => food.Category.Equals("hot"));
            return View();
        }

        public ActionResult Woks(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.SingleFood = db.Foods.Find(id);
            }
            else
                ViewBag.Woks = db.Foods.Where(food => food.Category.Equals("wok"));
            return View();
        }

        public ActionResult Soups(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.SingleFood = db.Foods.Find(id);
            }
            else
                ViewBag.Soups = db.Foods.Where(food => food.Category.Equals("soup"));
            return View();
        }

        [HttpGet]
        public ActionResult AddToCart(int id, int count = 1)
        {
            if (Session["CartList"] != null)
                CartList = (List<FoodsInOrder>)Session["CartList"];

            if (Session["FoodList"] != null)
                FoodList = (List<Food>)Session["FoodList"];

            Food newFood = db.Foods.Find(id);
            FoodsInOrder newCartItem = new FoodsInOrder { FoodId = id };
            if (FoodList.Contains(newFood, new FoodComparer()))
            {
                FoodsInOrder existingCartItem = CartList.Find(c => c.FoodId == newFood.Id);
                existingCartItem.Count += count;
            }
            else
            {
                newCartItem.Count = count;
                CartList.Add(newCartItem);
                FoodList.Add(newFood);
            }
            Session["CartList"] = CartList;
            Session["FoodList"] = FoodList;

            return Content(newFood.Name + ", " + count + " шт. добавлен в корзину");
        }


        [HttpGet]
        public ActionResult DeleteSomeFromCart(int id, int count)
        {
            if (Session["FoodList"] != null)
                FoodList = (List<Food>)Session["FoodList"];
            if (Session["CartList"] != null)
                CartList = (List<FoodsInOrder>)Session["CartList"];

            Food deletingFood = db.Foods.Find(id);
            FoodsInOrder deletingCartItem = new FoodsInOrder { FoodId = id };
            if (FoodList.Contains(deletingFood, new FoodComparer()))
            {
                FoodsInOrder existingCartItem = CartList.Find(c => c.FoodId == deletingFood.Id);
                if (count < existingCartItem.Count)
                {
                    existingCartItem.Count -= count;
                    Session["CartList"] = CartList;
                    Session["FoodList"] = FoodList;
                }
                else
                    return Content("Нельзя удалить больше, чем есть");
                return Content(deletingFood.Name + ", " + count + " шт. удалён из корзины");
            }
            else
                return Content("Товар не найден в корзине");
        }

        [HttpGet]
        public ActionResult DeleteFromCart(int id)
        {
            if (Session["FoodList"] != null)
                FoodList = (List<Food>)Session["FoodList"];
            if (Session["CartList"] != null)
                CartList = (List<FoodsInOrder>)Session["CartList"];

            Food deletingFood = db.Foods.Find(id);
            if (FoodList.Contains(deletingFood, new FoodComparer()))
            {
                int deletingIndex = FoodList.FindIndex(f => f.Id == deletingFood.Id);
                FoodList.RemoveAt(deletingIndex);
                CartList.RemoveAt(deletingIndex);
                Session["FoodList"] = FoodList;
                Session["CartList"] = CartList;
                return Content(deletingFood.Name + "полностью удалён из корзины");
            }
            else
                return Content("Товар не найден в корзине");
        }


        [HttpGet]
        public ActionResult Add(int id)
        {
            ViewBag.FoodId = id;

            return View();
        }

        [HttpPost]
        public string MakeOrder(Order order)
        {
            order.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + order.Person + ", за покупку!";
        }
    }
}