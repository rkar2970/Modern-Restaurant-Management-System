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
                TestDatabaseConnection();
                ReadTableNumber();
                TestCategoryCount();
            }
        }

      
        private void ReadTableNumber()
        {

            string tableNo = Request.QueryString["table"];
            if (!string.IsNullOrEmpty(tableNo))
            {
                Session["TableNumber"] = tableNo;
            }
        }
        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    lblTest.Text = "Database connection successful.";
                }
            }
            catch (Exception ex)
            {
                lblTest.Text = "Database connection failed: " + ex.Message;
            }
        }

        private void TestCategoryCount()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Category";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        Label1.Text = "Database connected. Category count: " + count;
                    }
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Error: " + ex.Message;
            }
        }
    }
}