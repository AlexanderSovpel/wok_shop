using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WokShop.Models;
using System.Net.Mail;
using System.Net;

namespace WokShop.Controllers
{
    //[Authorize(Roles = "admin, operator")]
    public class OrderController : Controller
    {
        FoodContext db = new FoodContext();
        List<FoodsInOrder> CartList = new List<FoodsInOrder>();

        public ActionResult MakeOrder(string person, string email, string phone, string address, string deliveryType, string payBy)
        {
            DateTime now = DateTime.Now.Date;
            if (Session["CartList"] != null)
                CartList = (List<FoodsInOrder>)Session["CartList"];

            Order newOrder = new Order(person, phone, address, deliveryType, payBy, now);
            db.Orders.Add(newOrder);
            db.SaveChanges();

            int lastOrderId = newOrder.OrderId;

            for (int i = 0; i < CartList.Count; ++i)
            {
                CartList[i].OrderId = lastOrderId;
                db.FoodsInOrder.Add(CartList[i]);
            }

            db.SaveChanges();

            MailMessage message = new MailMessage(
                "alexico1407@gmail.com",
                email);
            message.Subject = "Информация о заказе";
            message.Body = "<h1>Ваш заказ</h1><ul>";

            List<FoodsInOrder> fio = db.FoodsInOrder.ToList();
            List<Food> foods = db.Foods.ToList();
            foreach (FoodsInOrder f in fio)
            {
                if (f.OrderId == newOrder.OrderId)
                {
                    foreach (Food food in foods)
                    {
                        if (food.Id == f.FoodId)
                        {
                            message.Body += "<li>" + food.Name + ", " + f.Count + " шт.</li>";  
                        }
                    }
                }
            }
            message.Body += "</ul>";
            message.IsBodyHtml = true;

            /*SmtpClient mySmtpClient = new SmtpClient("smtp.gmail.com", 587); //???
            mySmtpClient.EnableSsl = true;
            mySmtpClient.Credentials = new NetworkCredential("alexico1407", "Google_Fun");
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.Send(message);*/


            CartList.Clear();
            Session["FoodList"] = null; // надо ли?
            Session["CartList"] = null;

            return Content("Заказ успешно принят. Ждите звонка оператора");
        }

        private bool SendMessage(string email, Order order)
        {
            MailMessage message = new MailMessage(
                "info@wokshop.by",
                email);
            message.Subject = "Информация о заказе";
            message.Body = "<h1>Ваш заказ</h1>" + 
                "";
            message.IsBodyHtml = true;
            

            return true;
        }


        public ActionResult Orders()
        {
            User user = (User)Session["user"];
            if (user == null)
                return Redirect("/Account/Login");
            else if (user.Role == "admin" || user.Role == "operator")
            {
                ViewBag.Orders = db.Orders.ToArray();
                ViewBag.FoodsInOrder = db.FoodsInOrder.ToArray();
                ViewBag.Foods = db.Foods.ToArray();

                return View();
            }
            else
                return Redirect("/Account/Login");
        }
    }
}