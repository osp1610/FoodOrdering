using LoginPage.Data;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IEnumerable<MenuItem> _MenuList;
        private MenuItem _MenuItem;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
            _MenuList = _db.MenuItems;
        }

        public IActionResult DisplaySingleProduct(int id, int uid = 0 )
        {   
 //           CurrentUser = _db.Users.FirstOrDefault(u => u.Id == id);
            _MenuItem = _MenuList.FirstOrDefault(x => x.Id == id);
            if (_MenuItem != null)
            {
                ViewData["uid"] = uid;
                return View(_MenuItem);
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ContinueWithOrder(IFormCollection col)
        {

            _MenuItem = _db.MenuItems.Where(x => x.Id == Convert.ToInt32(col["id"])).FirstOrDefault();

            if( _MenuItem == null)
            {
                throw new Exception("Unable to get MenuItem of id: "+col["id"]);
            }

            ViewData["order-qty"] = col["qty"];
            ViewData["order-id"] = col["id"];
            ViewData["order-address"] = col["address"];
            ViewData["uid"] = col["uid"];

            OrderItem order = new OrderItem();
            order.Address = col["address"];
            order.Cost = Convert.ToInt32(col["cost"]) * Convert.ToInt32(col["qty"]);
            order.Name = _MenuItem.Name;
            order.OrderStatus = OrderItem.Order_put;
            order.Qty = Convert.ToInt32(col["qty"]);
            order.UserName = col["uid"];

            return View("ConfirmOrderDetails", order);
        }

        //called at makepayment submit
        [HttpPost]
        public IActionResult SubmitOrder(IFormCollection col)
        {
            if (col == null)
            {
                throw new ArgumentNullException("Order Data is empty!!1");
            }
            else
            {

                OrderItem order = new OrderItem
                {
                    Name = col["name"],
                    OrderStatus = col["orderStatus"],
                    Qty = Convert.ToInt32(col["qty"]),
                    UserName = col["userName"],
                    Cost = Convert.ToInt32(col["cost"]),
                    Address = col["address"]
                };

                _db.Orders.Add(order);
                _db.SaveChanges();

                ViewData["Message"] = "Order Placed Successfully";
                return View("DisplayError");
            }

        }

        [HttpGet]
        public IActionResult ConfirmOrderDetails(OrderItem o)
        {
           // Console.WriteLine(o.OrderStatus);
            return View(o);
        }

        /* [HttpPost]
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

         }*/


/*
        public IActionResult MakePayment(OrderItem o)
        {
            _db.Orders.Add(o);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

*/

            // called in cnf order page submit
        
        [HttpPost]
        public IActionResult MakePayment(IFormCollection col)
        {

            OrderItem order = new OrderItem
            {
                Address = col["address"],
                Cost = Convert.ToInt32(col["cost"]) * Convert.ToInt32(col["qty"]),
                Name = col["MenuItemName"],
                OrderStatus = OrderItem.Order_put,
                Qty = Convert.ToInt32(col["qty"]),
                UserName = col["uid"]
            };

            return View(order);

        }


        //displaySingleProduct -> confirm order
        //confirmOrder -> order View
        //order view -> make payment view
        //make payment --> pur order for

    }
}
