using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WokShop.Models;

namespace WokShop.Controllers
{
    public class FoodsController : Controller
    {
        FoodContext db = new FoodContext();
        User user;

        public ActionResult Foods(int id = 0)
        {
            user = (User)Session["user"];
            if (user == null || user.Role != "admin")
                return Redirect("/Account/Login");

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

        public ActionResult AddFood()
        {
            user = (User)Session["user"];
            if (user == null || user.Role != "admin")
                return Redirect("/Account/Login");

            return View();
        }

        [HttpPost]
        public ActionResult AddFood(string category, string name, string description, HttpPostedFileBase image, int weight, int price)
        {
            user = (User)Session["user"];
            if (user == null || user.Role != "admin")
                return Redirect("/Account/Login");

            string foodImageName = System.IO.Path.GetFileName(image.FileName);
            string fullName = Server.MapPath("~/Images/" + foodImageName);
            image.SaveAs(fullName);

            Food newFood = new Food {
                Category = category,
                Name = name,
                Description = description,
                Image = "/Images/" + foodImageName, //???
                Weight = weight,
                Price = price
            };

            db.Foods.Add(newFood);
            db.SaveChanges();

            return Redirect("/Foods/Foods");
            //return Content("\"" + name + "\"успешно добавлен в список доступных продуктов");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase image)
        {
            string foodImageName = System.IO.Path.GetFileName(image.FileName);
            image.SaveAs(Server.MapPath("~/Images/" + foodImageName));

            return View("Foods");
        }

        public ActionResult ChangeFood(int id)
        {
            user = (User)Session["user"];
            if (user == null || user.Role != "admin")
                return Redirect("/Account/Login");

            ViewBag.Food = db.Foods.Find(id);

            return View();
        }

        [HttpPost]
        public ActionResult ChangeFood(int id, string category, string name, string description, HttpPostedFileBase image, int weight, int price/*, string action*/)
        {
            user = (User)Session["user"];
            if (user == null || user.Role != "admin")
                return Redirect("/Account/Login");
            //if (action == "save")
            //{
            Food changingFood = db.Foods.Find(id);
                changingFood.Category = category;
                changingFood.Name = name;
                changingFood.Description = description;

                if (image != null)
                {
                    string foodImageName = System.IO.Path.GetFileName(image.FileName);
                    string fullName = Server.MapPath("~/Images/" + foodImageName);
                    image.SaveAs(fullName);
                    changingFood.Image = "/Images/" + foodImageName;
                }

                changingFood.Weight = weight;
                changingFood.Price = price;

                db.SaveChanges();

                return Redirect("/Foods/Foods");
            //return Content("Изменения успешно сохранены"); //TODO: мб сразу делать редирект с изменениями?
            /*}
            else
            {
                return Redirect("/Foods/Foods");
            }*/
        }

        public ActionResult DeleteFood(int id)
        {
            user = (User)Session["user"];
            if (user == null || user.Role != "admin")
                return Redirect("/Account/Login");

            Food deletingFood = db.Foods.Find(id);
            db.Foods.Remove(deletingFood);

            var deletingConnections = db.FoodsInOrder.Where(c => c.FoodId == id);
            foreach (var connection in deletingConnections)
            {
                db.FoodsInOrder.Remove(connection);
            }

            db.SaveChanges();

            return Redirect("/Foods/Foods");
            //return Content("Изменения успешно сохранены"); //Переделать в AJAX
        }
    }
}