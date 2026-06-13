
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ModernRestaurantManagementSystem
{
    public class DatabaseHelper
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;
            return new SqlConnection(connectionString); /*DatabaseHelper က database connection ကို page တိုင်းမှာပြန်သုံးနိုင်အောင်လုပ်ထားတာပါ။
            Menu.aspx.cs, Search.aspx.cs, Cart.aspx.cs တွေက ဒီ GetConnection() ကိုခေါ်ပြီး database ချိတ်မယ်။*/
        }
    }
}