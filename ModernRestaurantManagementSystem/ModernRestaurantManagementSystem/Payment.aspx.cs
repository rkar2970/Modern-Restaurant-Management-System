using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ModernRestaurantManagementSystem
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckTableNumber();
                LoadPaymentItems();
            }
        }
        private decimal GetCartTotal(string sessionId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT ISNULL(SUM(Subtotal),0) FROM CartItem WHERE SessionId =@SessionId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SessionId", sessionId);
                    conn.Open();
                    return Convert.ToDecimal(cmd.ExecuteScalar()); 
                }
            }
        }
        private string GenerateReceiptNo()
        {
            return "RCP-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        private int GetTableId(int tableNumber)
        {
            using(SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT TableId FROM RestaurantTable WHERE TableNumber = @TableNumber AND isActive=1";
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
        private int SaveOrder(int tableId,decimal totalAmount,string paymentMethod,string receiptNo)
        {
            using(SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO Orders (TableId,OrderDate, TotalAmount, PaymentMethod, ReceiptNo)
                                OUTPUT INSERTED.OrderId VALUES (@TableId,GETDATE(), @TotalAmount, @PaymentMethod, @ReceiptNo)";
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@TableId", tableId);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.Parameters.AddWithValue("@ReceiptNo", receiptNo);

                    conn.Open();    

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        private void SaveOrderDetails(int orderId, string sessionId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO OrderDetail (OrderId, MenuItemId, Quantity, Price, Subtotal)
                                SELECT @OrderId,MenuItemId,Quantity,Price,Subtotal FROM CartItem WHERE SessionId = @SessionId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@SessionId", sessionId);

                    conn.Open();

                    cmd.ExecuteNonQuery();

                }
            }
        }
        private void ClearCart(string sessionId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM CartItem WHERE SessionId = @SessionId ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SessionId",sessionId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        protected void btnPayNow_Click(Object sender, EventArgs e)
        {
            if (Session["TableNumber"] == null)
            {
                lblMessage.Text = "Table Number not found. Please Scan QR code Again";
                return;
            }
            if (string.IsNullOrEmpty(rblPaymentMethod.SelectedValue))
            {
                lblMessage.Text = "Please select a payment Method.";
                return;
            }
            string sessionId = Session.SessionID;
            int tableNumber = Convert.ToInt32(Session["TableNumber"]);
            int tableId = GetTableId(tableNumber);

            if (tableId == 0)
            {
                lblMessage.Text = "Invalid Table Number.";
                return;
            }
            decimal totalAmount = GetCartTotal(sessionId);

            if (totalAmount <= 0)
            {
                lblMessage.Text = "Your Cart is empty.";
                return;
            }
            string paymentMethod = rblPaymentMethod.SelectedValue;
            string receiptNo = GenerateReceiptNo();

            int orderId = SaveOrder(tableId, totalAmount, paymentMethod, receiptNo);

            SaveOrderDetails(orderId, sessionId);

            ClearCart(sessionId);

            Session["ReceiptNo"] = receiptNo;
            Session["OrderId"] = orderId;

            Response.Redirect("Receipt.aspx");
        }
        private void LoadPaymentItems()
        {
            string sessionId = Session.SessionID;
            using(SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT c.CartItemID, m.Name, c.Quantity, c.Price, c.Subtotal 
                                FROM CartItem c INNER JOIN MenuItem m ON c.MenuItemID = m.MenuItemID
                                WHERE c.SessionId =@SessionId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SessionId", sessionId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    rptPaymentItems.DataSource= reader;
                    rptPaymentItems.DataBind();
                    reader.Close();
                }
                string totalQuery = @"SELECT ISNULL(SUM(Subtotal),0) FROM CartItem WHERE SessionId = @SessionId";
                using (SqlCommand totalcmd = new SqlCommand(totalQuery, conn))
                {
                    totalcmd.Parameters.AddWithValue("@SessionId", sessionId);
                    decimal total = Convert.ToDecimal(totalcmd.ExecuteScalar());

                    lblTotal.Text = "Total Amount: " + total.ToString("N0") + "Ks";
                    if(total == 0)
                    {
                        lblMessage.Text = "Your cart is empty.";
                        btnPayNow.Enabled = false;
                    }
                }
            }
        }
        private void CheckTableNumber()
        {
            if (Session["TableNumber"] != null)
            {
                lblTableNumber.Text = "Table No:" + Session["TableNumber"].ToString();
            }
            else
            {
                lblMessage.Text = "Table Number Not Found. Please Sacn the QR code Again.";
                btnPayNow.Enabled= false;
            }
        }
    }
}