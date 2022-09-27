using Admin.Models;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        private const string ConnectionString = "Data Source=HJSZL000057;Initial Catalog=CustomerDb;Integrated Security=True;";

        private static SqlConnection _conn = new SqlConnection(ConnectionString);
        private static List<AdminUser> _AdminUsers = new List<AdminUser>();
        private static List<OrderItem> _Orders = new List<OrderItem>();

        public IActionResult Index()
        {

            string CmdText = "select * from AdminUsers";
            SqlCommand cmd = new SqlCommand(CmdText, _conn);
            _conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();

            while (sr.Read())
            {
                AdminUser user = new AdminUser(
                    Convert.ToInt32(sr["Id"]),
                    sr["Name"].ToString(),
                    sr["Password"].ToString());
                _AdminUsers.Add(user);
            }
            _conn.Close();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DisplayAllOrders()
        {
            _Orders.Clear();

            const string CmdText = "select * from Orders";
            SqlCommand cmd = new SqlCommand(CmdText, _conn);
            _conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();
            
            while (sr.Read())
            {

                OrderItem item = new OrderItem();
                item.Id = Convert.ToInt32(sr["Id"]);
                item.Name = sr["Name"].ToString();
                item.Qty = sr.GetInt32(2);
                item.Cost = sr.GetInt32(3);
                item.UserName = sr.GetString(4);
                item.Address = sr.GetString(5);
                item.OrderStatus = sr.GetString(6);

                _Orders.Add(item);
            }

            _conn.Close();

            return View(_Orders);
        }

        [HttpPost]
        public IActionResult LoginAdmin(AdminUser admin)
        {

            if(admin == null)
            {
                throw new ArgumentNullException("Admin is null");
            }

            if((_AdminUsers.FirstOrDefault(x => x.Id == admin.Id && x.Name == admin.Name) == null)){ 
                
                    return RedirectToAction("DisplayAllOrders");

            }

           /* foreach (AdminUser a in AdminUsers)
            {

                if (a.Name == admin.Name && a.Password == admin.Password)
                {

                }
            }*/
            return Content("Error");
        }


        public IActionResult ChangeStatusToProcessing(int OrderId)
        {

            _conn = new SqlConnection(ConnectionString);

            string CmdText = "UPDATE Orders SET OrderStatus = '" + OrderItem.Order_put + "' WHERE Id = " + OrderId;
            SqlCommand cmd = new SqlCommand(CmdText, _conn);

            _conn.Open();
            int NoOfRowsAffexted = cmd.ExecuteNonQuery();
            _conn.Close();
            
            if (NoOfRowsAffexted < 0)
            {
                return Content("Error: No Update Performed");
            }
            
            return RedirectToAction("DisplayAllOrders");
        }
        public IActionResult ChangeStatusToDelivered(int OrderId)
        {

            _conn = new SqlConnection(ConnectionString);


            string CmdText = "UPDATE Orders SET OrderStatus = '" + OrderItem.Order_del + "' WHERE Id = " + OrderId;
            SqlCommand cmd = new SqlCommand(CmdText, _conn);
            _conn.Open();
            int NoOfRowsAffexted = cmd.ExecuteNonQuery();
            _conn.Close();
            if (NoOfRowsAffexted < 0)
            {
                return Content("Error: No Update Performed");
            }
            return RedirectToAction("DisplayAllOrders");

        }
        public IActionResult ChangeStatusToCancelled(int OrderId)
        {
            _conn = new SqlConnection(ConnectionString);


            string CmdText = "UPDATE Orders SET OrderStatus = '" + OrderItem.Order_cancelled + "' WHERE Id = " + OrderId;
            SqlCommand cmd = new SqlCommand(CmdText, _conn);
            _conn.Open();
            int NoOfRowsAffexted = cmd.ExecuteNonQuery();
            _conn.Close();
            if (NoOfRowsAffexted < 0)
            {
                return Content("Error: No Update Performed");
            }
            return RedirectToAction("DisplayAllOrders");
        }



    }
}