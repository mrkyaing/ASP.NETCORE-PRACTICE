using Microsoft.AspNetCore.Mvc;
using System;
using WorkOutMVC.Models;
namespace WorkOutMVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string TellMeDateTime()
        {
            return DateTime.Now.ToString();
        }

        public IActionResult MakeOrder()=>View();

        [HttpPost]
        public JsonResult MakeOrder(OrderModel orderModel)
        {
            orderModel.OrderDate=DateTime.Now.ToString();
            //ViewBag.Order = $"Order Info:{orderModel.Id} and your order status is {orderModel.Status} and your order date is :{orderModel.OrderDate}";
            return Json(orderModel);
        }
    }
}
