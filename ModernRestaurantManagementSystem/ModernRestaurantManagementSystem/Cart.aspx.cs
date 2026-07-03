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
                CheckTableNumber();
                LoadCartItems();
            }

        }

        private void CheckTableNumber()
        {
            if (Session["TableNumber"] != null)
            {
                lblTableNumber.Text = "Table No: " + Session["TableNumber"].ToString();
            }
            else
            {
                lblMessage.Text = "Table number not found. Please scan QR code again.";
                btnOrder.Enabled = false;
            }
        }
        
        protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int cartItemId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Increase")
            {
                IncreaseQuantity(cartItemId);
            }
            else if (e.CommandName == "Decrease")
            {
                DecreaseQuantity(cartItemId);
            }
            else if (e.CommandName == "Remove")
            {
                RemoveCartItem(cartItemId);
            }

            LoadCartItems();
        }
        private void IncreaseQuantity(int cartItemId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            UPDATE CartItem
            SET Quantity = Quantity + 1,
                Subtotal = Price * (Quantity + 1)
            WHERE CartItemId = @CartItemId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void DecreaseQuantity(int cartItemId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT Quantity FROM CartItem WHERE CartItemId = @CartItemId";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                    int quantity = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (quantity <= 1)
                    {
                        string deleteQuery = "DELETE FROM CartItem WHERE CartItemId = @CartItemId";

                        using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                        {
                            deleteCmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                            deleteCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updateQuery = @"
                    UPDATE CartItem
                    SET Quantity = Quantity - 1,
                        Subtotal = Price * (Quantity - 1)
                    WHERE CartItemId = @CartItemId";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private void RemoveCartItem(int cartItemId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM CartItem WHERE CartItemId = @CartItemId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Item removed from cart.";
        }
        protected void btnOrder_Click(object sender,EventArgs e)
        {
            if (Session["TableNumber"] == null)
            {
                lblMessage.Text = "Table Number Not Found.Please Scan The QR code again";

                return;

            }
            Response.Redirect("Payment.aspx");
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

                    if (total == 0)
                    {
                        lblMessage.Text = "Your cart is empty.";
                        btnOrder.Enabled = false;
                    }
                    else
                    {
                        btnOrder.Enabled = true;
                    }
                }
            }
        }
    }
}