using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ModernRestaurantManagementSystem
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadAllMenuItems();
            }
        }
        protected void rptSearchResults_ItemCommand(object source, RepeaterCommandEventArgs e)
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
        private void LoadAllMenuItems()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT MenuItemId, Name, Price, ImagePath
            FROM MenuItem
            WHERE IsAvailable = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    rptSearchResults.DataSource = reader;
                    rptSearchResults.DataBind();
                }
            }
        }

        protected void btnFilterToggle_Click(object sender, EventArgs e)
        {
            pnlFilter.Visible = !pnlFilter.Visible;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchAndFilterMenuItems();
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            SearchAndFilterMenuItems();
        }

       
          protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            rblPrice.ClearSelection();
            cblCategories.ClearSelection(); 

            LoadAllMenuItems();
        }

        private void SearchAndFilterMenuItems()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT MenuItemId, Name, Price, ImagePath
            FROM MenuItem
            WHERE IsAvailable = 1";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // Search by keyword
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    query += " AND Name LIKE @Keyword";
                    cmd.Parameters.AddWithValue("@Keyword", "%" + txtSearch.Text.Trim() + "%");
                }

                // Price filter
                if (!string.IsNullOrEmpty(rblPrice.SelectedValue))
                {
                    if (rblPrice.SelectedValue == "under5000")
                    {
                        query += " AND Price < 5000";
                    }
                    else if (rblPrice.SelectedValue == "under10000")
                    {
                        query += " AND Price < 10000";
                    }
                    else if (rblPrice.SelectedValue == "over10000")
                    {
                        query += " AND Price > 10000";
                    }
                    else if (rblPrice.SelectedValue == "over15000")
                    {
                        query += " AND Price > 15000";
                    }
                }

                // Category checkbox filter
                List<string> selectedCategoryIds = new List<string>();

                foreach (ListItem item in cblCategories.Items)
                {
                    if (item.Selected)
                    {
                        selectedCategoryIds.Add(item.Value);
                    }
                }

                if (selectedCategoryIds.Count > 0)
                {
                    query += " AND CategoryId IN (" + string.Join(",", selectedCategoryIds) + ")";
                }

                cmd.CommandText = query;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                rptSearchResults.DataSource = reader;
                rptSearchResults.DataBind();
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT CategoryId, CategoryName FROM Category WHERE IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    cblCategories.DataSource = reader;
                    cblCategories.DataTextField = "CategoryName";
                    cblCategories.DataValueField = "CategoryId";
                    cblCategories.DataBind();
                }
            }
        }
    }
}