using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModernRestaurantManagementSystem
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //TestDatabaseConnection();
                ReadTableNumber();
                //TestCategoryCount();
                LoadCategories();
                LoadMenuItems();
            }
        }

        protected void rptMenuItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int menuItemId = Convert.ToInt32(e.CommandArgument);
                AddToCart(menuItemId);
            }
        }

        private decimal GetMenuItemPrice(int menuItemId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT Price 
            FROM MenuItem 
            WHERE MenuItemId = @MenuItemId 
            AND IsAvailable = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MenuItemId", menuItemId);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToDecimal(result);
                    }
                }
            }

            return 0;
        }
        private int GetTableId(int tableNumber)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT TableId 
            FROM RestaurantTable 
            WHERE TableNumber = @TableNumber 
            AND IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableNumber", tableNumber);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            return 0;
        }
        private void AddToCart(int menuItemId)
        {
            string sessionId = Session.SessionID;

            if (Session["TableNumber"] == null)
            {
                lblMessage.Text = "Table number not found. Please scan QR code again.";
                return;
            }

            int tableNumber = Convert.ToInt32(Session["TableNumber"]);
            int tableId = GetTableId(tableNumber);

            if (tableId == 0)
            {
                lblMessage.Text = "Invalid table number.";
                return;
            }

            decimal price = GetMenuItemPrice(menuItemId);

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = @"
            SELECT CartItemId, Quantity 
            FROM CartItem
            WHERE SessionId = @SessionId 
            AND TableId = @TableId 
            AND MenuItemId = @MenuItemId";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@SessionId", sessionId);
                    checkCmd.Parameters.AddWithValue("@TableId", tableId);
                    checkCmd.Parameters.AddWithValue("@MenuItemId", menuItemId);

                    using (SqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int cartItemId = Convert.ToInt32(reader["CartItemId"]);
                            int quantity = Convert.ToInt32(reader["Quantity"]) + 1;

                            reader.Close();

                            string updateQuery = @"
                        UPDATE CartItem
                        SET Quantity = @Quantity,
                            Subtotal = @Subtotal
                        WHERE CartItemId = @CartItemId";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                                updateCmd.Parameters.AddWithValue("@Subtotal", price * quantity);
                                updateCmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            reader.Close();

                            string insertQuery = @"
                        INSERT INTO CartItem
                        (SessionId, TableId, MenuItemId, Quantity, Price, Subtotal, CreatedAt)
                        VALUES
                        (@SessionId, @TableId, @MenuItemId, @Quantity, @Price, @Subtotal, GETDATE())";

                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@SessionId", sessionId);
                                insertCmd.Parameters.AddWithValue("@TableId", tableId);
                                insertCmd.Parameters.AddWithValue("@MenuItemId", menuItemId);
                                insertCmd.Parameters.AddWithValue("@Quantity", 1);
                                insertCmd.Parameters.AddWithValue("@Price", price);
                                insertCmd.Parameters.AddWithValue("@Subtotal", price);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            lblMessage.Text = "Item added to cart successfully.";
        }
        private void ReadTableNumber()
        {
            string tableNo = Request.QueryString["table"]; //URLထဲမှာပါလာတဲ့"table"ဆိုတဲ့parameterရဲ့တန်ဖိုးကိုယူ

            if (!string.IsNullOrEmpty(tableNo)) //tableNo မှာ value ရှိမရှိ စစ်ဆေး
            {
                Session["TableNumber"] = tableNo; //Session ထဲမှာ သိမ်း , နောက်ထပ် page တွေသွားရင်လည်း ဒီနံပါတ်ကို ဆက်သုံး
                lblTableNumber.Text = "Table No: " + tableNo;
            }
            else if (Session["TableNumber"] != null)
            {
                lblTableNumber.Text = "Table No: " + Session["TableNumber"].ToString(); //ession ထဲမှာ အရင်က သိမ်းထားတဲ့ နံပါတ် ရှိမရှိ စစ် , ရှိ yin Session ထဲက နံပါတ်ကိုပဲ Label မှာ
            }
            else
            {
                lblTableNumber.Text = "Table number not found.";
            }
        }


        private void LoadCategories()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())   //DatabaseHelper ဆိုသည့် Class ထဲမှ connection method ကိုခေါ်
            {
                string query = "SELECT CategoryId, CategoryName FROM Category WHERE IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();  //Database မှ အချက်အလက်များကို တစ်ခုချင်းစီ ဖတ်ရှုနိုင်ရန် SqlDataReader ကို အသုံးပြုသည်။

                    rptCategories.DataSource = reader; //ဖတ်လိုက်ရသော data များကို rptCategories (Repeater) ထဲသို့ ထည့်ပေးလိုက်ပြီး DataBind() ဖြင့် screen ပေါ်တွင် ပေါ်လာအောင် အသက်သွင်းလိုက်သည်။
                    rptCategories.DataBind();
                }
            }
        }

        private void LoadMenuItems()
        {
            string categoryId = Request.QueryString["categoryId"];

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT MenuItemId, Name, Description, Price, ImagePath
            FROM MenuItem
            WHERE IsAvailable = 1";

                if (!string.IsNullOrEmpty(categoryId))   //categoryId မပါလာလျှင် (သို့မဟုတ် အားလုံးကို ပြချင်လျှင်) Menu အားလုံးကို ပြသပေးသည်။
                {
                    query += " AND CategoryId = @CategoryId";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(categoryId))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId); // for security
                    }

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    rptMenuItems.DataSource = reader;
                    rptMenuItems.DataBind();
                }
            }
        }



























        //private void TestDatabaseConnection()
        //{
        //    try
        //    {
        //        using (SqlConnection conn = DatabaseHelper.GetConnection())
        //        {
        //            conn.Open();
        //            lblTest.Text = "Database connection successful.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblTest.Text = "Database connection failed: " + ex.Message;
        //    }
        //}

        //private void TestCategoryCount()
        //{
        //    try
        //    {
        //        using (SqlConnection conn = DatabaseHelper.GetConnection())
        //        {
        //            conn.Open();

        //            string query = "SELECT COUNT(*) FROM Category";

        //            using (SqlCommand cmd = new SqlCommand(query, conn))
        //            {
        //                int count = (int)cmd.ExecuteScalar();
        //                Label1.Text = "Database connected. Category count: " + count;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Label1.Text = "Error: " + ex.Message;
        //    }
        //}
    }
}