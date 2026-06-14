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