using Admin.Models;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        private const string ConnectionString = "Data Source=HJSZL000057;Initial Catalog=CustomerDb;Integrated Security=True;";
        public SqlConnection conn = new SqlConnection(ConnectionString);
        public static List<AdminUser> adminUsers = new List<AdminUser>();
        public static List<OrderItem> OrderList = new List<OrderItem>();

        public IActionResult Index()
        {

            SqlCommand cmd = new SqlCommand("select * from AdminUsers", conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();
            while (sr.Read())
            {
                AdminUser user = new AdminUser(Convert.ToInt32(sr["Id"]),sr["Name"].ToString(), sr["Password"].ToString());
                adminUsers.Add(user);
            }
            conn.Close();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DisplayOrders()
        {
            OrderList.Clear();
            SqlCommand cmd = new SqlCommand("select * from Orders", conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();
            while (sr.Read())
            {
                int index = 0;
                /*OrderItem user = new OrderItem(sr["id"], 
                    sr["Name"], sr["qty"], sr["cost"], sr["userName"], sr["address"],);*/

                OrderItem item = new OrderItem();  
                item.Id = Convert.ToInt32(sr["Id"]);
                item.Name = sr["Name"].ToString();
                item.Qty = sr.GetInt32(2);
                item.Cost = sr.GetInt32(3);
                item.UserName = sr.GetString(4);
                item.Address = sr.GetString(5);
                item.OrderStatus = sr.GetString(6);
                
                OrderList.Add(item);
            }
            conn.Close();

            return View(OrderList);
        }

        [HttpPost]
        public IActionResult LoginAdmin(AdminUser admin)
        {
            foreach (AdminUser a in adminUsers)
            {

                if( a.Name == admin.Name && a.Password == admin.Password)
                {
                    return RedirectToAction("DisplayOrders");  

                }
            }
            return Content("Error");
        }


        public IActionResult ChangeStatusToProcessing(int id)
        {

            conn = new SqlConnection(ConnectionString);


            string query = "UPDATE Orders SET OrderStatus = '"+OrderItem.Order_put+"' WHERE Id = "+id;
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            int NoOfRowsAffexted = cmd.ExecuteNonQuery();
            conn.Close();
            if (NoOfRowsAffexted < 0)
            {
                return Content("Error: No Update Performed");
            }
            return RedirectToAction("DisplayOrders");
        }
        public IActionResult ChangeStatusToDelivered(int id)
        {

            conn = new SqlConnection(ConnectionString);


            string query = "UPDATE Orders SET OrderStatus = '" + OrderItem.Order_del + "' WHERE Id = " + id;
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            int NoOfRowsAffexted = cmd.ExecuteNonQuery();
            conn.Close();
            if (NoOfRowsAffexted < 0)
            {
                return Content("Error: No Update Performed");
            }
            return RedirectToAction("DisplayOrders");

        }
        public IActionResult ChangeStatusToCancelled(int id)
        {
            conn = new SqlConnection(ConnectionString);


            string query = "UPDATE Orders SET OrderStatus = '" + OrderItem.Order_cancelled + "' WHERE Id = " + id;
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            int NoOfRowsAffexted = cmd.ExecuteNonQuery();
            conn.Close();
            if (NoOfRowsAffexted < 0)
            {
                return Content("Error: No Update Performed");
            }
            return RedirectToAction("DisplayOrders");
        }



    }
}