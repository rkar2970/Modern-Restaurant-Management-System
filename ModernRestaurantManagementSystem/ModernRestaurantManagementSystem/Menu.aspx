<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="ModernRestaurantManagementSystem.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content Folder/site.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div>
           
           

            <asp:Label ID="lblTableNumber" runat="server"></asp:Label>
            <a href="Menu.aspx">All</a>
            <asp:Repeater ID="rptCategories" runat="server">
                <ItemTemplate>
                    <a href='Menu.aspx?categoryId=<%# Eval("CategoryId") %>'>
                     <%# Eval("CategoryName") %>
                    </a>
                </ItemTemplate>
            </asp:Repeater>

    <asp:Repeater ID="rptMenuItems" runat="server">
    <ItemTemplate>
        <div class="food-card">
            <img src='<%# Eval("ImagePath") %>' class="food-img" />

            <h3><%# Eval("Name") %></h3>

            <p><%# Eval("Price") %> Ks</p>

            <asp:Button 
                ID="btnAddToCart" 
                runat="server" 
                Text="Add to Cart"
                CommandArgument='<%# Eval("MenuItemId") %>' />
        </div>
    </ItemTemplate>
</asp:Repeater>

            <h2>Menu Page</h2>
            <p>This is menu Page</p>

            <div class="buttom-nav">
                <a href="Menu.aspx">Menu</a>
                <a href="Search.aspx">Search</a>
                <a href="Cart.aspx">Cart</a>
                <a href="Information.aspx">Information</a>
            </div>
        </div>
    </form>
</body>
</html>
