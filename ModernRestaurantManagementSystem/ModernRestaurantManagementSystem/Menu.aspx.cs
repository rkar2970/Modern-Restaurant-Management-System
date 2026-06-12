using System;
using System.Collections.Generic;
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
            if (!IsPostBack) //work only when start first time
            {
                string tableNo = Request.QueryString["table"]; //get value of table which was in Url
                if (!string.IsNullOrEmpty(tableNo)) // if table no has value?
                {
                    Session["TableNumber"] = tableNo; //store in session
                }

            }
        }
    }
}