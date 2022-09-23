using LoginPage.Data;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        IEnumerable<MenuItem> MenuList;
        MenuItem menuItem;
        User CurrentUser = null;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
            MenuList = _db.MenuItems;
        }

        public IActionResult DisplaySingleProduct(int id, int uid = 0 )
        {   
            CurrentUser = _db.Users.FirstOrDefault(u => u.Id == id);
            menuItem = MenuList.FirstOrDefault(x => x.Id == id);
            if (menuItem != null)
            {
                ViewData["uid"] = uid;
                return View(menuItem);
            }
            return RedirectToAction("Index", "Home");
        }

        

        [HttpPost]
        public IActionResult PurOrderFor(IFormCollection col)
        {
            if (col == null)
            {
                return Content("Error occured: !!!!");
            }
            else
            {
                menuItem = _db.MenuItems.Where(x=>x.Id == Convert.ToInt32(col["id"])).FirstOrDefault();

                ViewData["order-qty"] = col["qty"];
                ViewData["order-id"] = col["id"];
                ViewData["order-address"] = col["address"];
                ViewData["uid"] = col["uid"];

                OrderItem order = new OrderItem();
                order.Address = col["address"];
                order.Cost = Convert.ToInt32(col["cost"]) * Convert.ToInt32(col["qty"]);
                order.Name = menuItem.Name;
                order.OrderStatus = OrderItem.Order_put;
                order.Qty = Convert.ToInt32(col["qty"]);
                order.UserName = col["uid"];

                return RedirectToAction("Order", order);
            }

            //          return Content(col["id"] + "|" + col["qty"]);
        }

        [HttpGet]
        public IActionResult Order(OrderItem o)
        {
           // Console.WriteLine(o.OrderStatus);
            return View(o);
        }

        [HttpPost]
        public IActionResult ConfirmOrder(IFormCollection col)
        {

            OrderItem order = new OrderItem();
            order.Address = col["address"];
            order.Cost = Convert.ToInt32(col["cost"]) * Convert.ToInt32(col["qty"]);
            order.Name = col["MenuItemName"];
            order.OrderStatus = OrderItem.Order_put;
            order.Qty = Convert.ToInt32(col["qty"]);
            order.UserName = col["uid"];

            _db.Orders.Add(order);
            _db.SaveChanges();
            return Content("OrderPlaced Successfully");

        }


    }
}
