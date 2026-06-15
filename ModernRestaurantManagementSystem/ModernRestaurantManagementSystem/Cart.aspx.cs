using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModernRestaurantManagementSystem
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadCartItems();
            }

        }
        private void LoadCartItems()
        {
            string sessionId = Session.SessionID;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT 
                c.CartItemId,
                m.Name,
                c.Quantity,
                c.Price,
                c.Subtotal
            FROM CartItem c
            INNER JOIN MenuItem m ON c.MenuItemId = m.MenuItemId
            WHERE c.SessionId = @SessionId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SessionId", sessionId);

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    rptCartItems.DataSource = reader;
                    rptCartItems.DataBind();

                    reader.Close();
                }

                string totalQuery = @"
            SELECT ISNULL(SUM(Subtotal), 0)
            FROM CartItem
            WHERE SessionId = @SessionId";

                using (SqlCommand totalCmd = new SqlCommand(totalQuery, conn))
                {
                    totalCmd.Parameters.AddWithValue("@SessionId", sessionId);

                    decimal total = Convert.ToDecimal(totalCmd.ExecuteScalar());

                    lblTotal.Text = "Total: " + total.ToString("N0") + " Ks";
                }
            }
        }
    }
}