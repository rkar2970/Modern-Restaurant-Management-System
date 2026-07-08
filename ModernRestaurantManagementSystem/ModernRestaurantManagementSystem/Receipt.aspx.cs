using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ModernRestaurantManagementSystem
{
    public partial class Receipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReceipt();
            }
        }
        protected void backToMenu_Click(Object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }
        private void LoadOrderInfo(int orderId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT 
                o.ReceiptNo,
                rt.TableNumber,
                o.OrderDate,
                o.PaymentMethod,
                o.TotalAmount
            FROM Orders o
            INNER JOIN RestaurantTable rt ON o.TableId = rt.TableId
            WHERE o.OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblReceiptNo.Text = reader["ReceiptNo"].ToString();
                        lblTableNumber.Text = reader["TableNumber"].ToString();

                        DateTime orderDate = Convert.ToDateTime(reader["OrderDate"]);
                        lblOrderDate.Text = orderDate.ToString("dd-MM-yyyy hh:mm tt");

                        lblPaymentMethod.Text = reader["PaymentMethod"].ToString();

                        decimal totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                        lblTotalAmount.Text = totalAmount.ToString("N0") + " Ks";
                    }
                    else
                    {
                        lblMessage.Text = "Order not found.";
                    }

                    reader.Close();
                }
            }
        }
        private void LoadReceiptItems(int orderId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT 
                m.Name,
                od.Quantity,
                od.Price,
                od.Subtotal
            FROM OrderDetail od
            INNER JOIN MenuItem m ON od.MenuItemId = m.MenuItemId
            WHERE od.OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    rptReceiptItems.DataSource = reader;
                    rptReceiptItems.DataBind();

                    reader.Close();
                }
            }
        }
        private void LoadReceipt()
        {
            if (Session["OrderId"] == null)
            {
                lblMessage.Text = "Receipt not found.";
                return;
            }

            int orderId = Convert.ToInt32(Session["OrderId"]);

            LoadOrderInfo(orderId);
            LoadReceiptItems(orderId);
        }
    }
}