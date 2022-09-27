using LoginPage.Data;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{


    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IEnumerable<User> _UserList;
        private IEnumerable<MenuItem> _MenuList;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {

            _MenuList = _db.MenuItems;
            ViewData["MenuList"] = _MenuList;
            return View();
        }

        public IActionResult Index(User u)
        {
            _MenuList = _db.MenuItems;
            ViewData["MenuList"] = _MenuList;
            ViewData["User"] = u;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            int id = ValidateUser(u);

            if (id == -1)
            {   //user not found
                ViewData["login-message"] = "Error: Invalid Credentials";
                return View();

            }else{
                u.Id = id;
                //Console.WriteLine("----------------id:"+u.Id);
                //Console.WriteLine("----------------Name:" + u.Name);
                TempData["userId"] = u.Id;
                TempData["userName"] = u.Name;

                /*HttpContext.Session.SetInt32("uid", u.Id);
                HttpContext.Session.SetString("uid", u.Name);*/

                return RedirectToAction("Index");
            }
        }


        private int ValidateUser(User u)
        {

            _UserList = _db.Users;

            foreach (User user in _UserList)
            {
                if (user.Name == u.Name && user.Password == u.Password)
                {
                    return user.Id;
                }
            }
            return -1;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(string Name, string Password, string cnfPassword)
        {
            _UserList = _db.Users;
            if (string.IsNullOrEmpty(Name))
            {
                ViewData["ErrorMessage"] = "Name/User ID Cannot be empty";
                return View("Register");
            }
            else if ((_UserList.FirstOrDefault(u => u.Name == Name)) != null)
            {
                ViewData["ErrorMessage"] = "Name/User Already Exist";
                return View("Register");
            }
            else if (string.IsNullOrEmpty(Password))
            {
                ViewData["ErrorMessage"] = "Password cannot be empty";
                return View("Register");
            }
            else if (string.IsNullOrEmpty(cnfPassword) || !cnfPassword.Equals(Password))
            {
                ViewData["ErrorMessage"] = "Confirm password doesnt match";
                return View("Register");
            }
            else
            {
                User u = new User()
                {
                    Name = Name,
                    Password = Password
                };
                _db.Users.Add(u);
                _db.SaveChanges();

                ViewData["Greeting1"] = "Log In For";
                ViewData["Greeting2"] = "Better Experince";

                TempData["message"] = "Account Created Successfully";
                return RedirectToAction("Login");
            }
        }


        public IActionResult DisplaySingleProduct(int id)
        {

            MenuItem menuItem = _MenuList.FirstOrDefault(x => x.Id == id);
            if (menuItem != null)
            {
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
                MenuItem menuItem = _MenuList.FirstOrDefault(x => x.Id == col["id"]);

                ViewData["order-qty"] = col["qty"];
                ViewData["order-id"] = col["id"];
                ViewData["order-id"] = col["address"];

                return RedirectToAction("Order");
            }

            //  return Content(col["id"] + "|" + col["qty"]);
        }

        public IActionResult Order()
        {
            return View();
        }


        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }

        //get {id = userId}
        //userName in order is user's Id
        public IActionResult DisplayOrders(int id)
        {
            if (id == 0)
            {
                ViewData["Message"] = "Log in first";
                return View("DisplayError");
            }
            var orderlist = _db.Orders.Where(x => x.UserName == id.ToString()).ToList();
            return View(orderlist);
        }


        public IActionResult CancelOrder(int id)
        {

            OrderItem order = _db.Orders.Where(x => x.Id == id).FirstOrDefault();

            if (order == null)
            {
                ViewData["Message"] = "Error: Unable to find order with id :" + id;
                return View("DisplayError");
            }
            else
            {
                order.OrderStatus = OrderItem.Order_cancelled;

                _db.Orders.Where(x => x.Id == id).FirstOrDefault().OrderStatus = OrderItem.Order_cancelled;
                _db.SaveChanges();

                return Redirect("DisplayOrders?id=" + order.UserName);
            }

        }

    }
}