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
            SELECT MenuItemId, Name, Price, ImagePath
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