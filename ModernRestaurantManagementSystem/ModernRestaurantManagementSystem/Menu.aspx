<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="ModernRestaurantManagementSystem.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
       <div class="page-container">

    <div class="menu-header">
        <div>
            <h2 class="restaurant-name">Le Château Table</h2>
            <p class="page-subtitle">What would you like to order?</p>
        </div>

        <div class="table-badge">
            <asp:Label ID="lblTableNumber" runat="server"></asp:Label>
        </div>
        <br />


    </div>
                       <asp:Label ID="lblMessage" runat="server" class="message-text"></asp:Label>

    <div class="category-section">
        <a href="Menu.aspx" class="category-pill">All</a>

        <asp:Repeater ID="rptCategories" runat="server">
            <ItemTemplate>
                <a class="category-pill" href='Menu.aspx?categoryId=<%# Eval("CategoryId") %>'>
                    <%# Eval("CategoryName") %>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <asp:Repeater ID="rptMenuItems" runat="server" OnItemCommand="rptMenuItems_ItemCommand">
        <ItemTemplate>
            <div class="food-card">
                <img src='<%# Eval("ImagePath") %>' class="food-img" />

                <div class="food-info">
                    <h3 class="food-name"><%# Eval("Name") %></h3>

                    <p class="food-description">
                        <%# Eval("Description") %>
                    </p>

                    <div class="food-bottom">
                        <span class="food-price"><%# Eval("Price") %> Ks</span>

                        <asp:Button 
                            ID="btnAddToCart" 
                            runat="server" 
                            Text="Add"
                            CssClass="btn-main"
                            CommandName="AddToCart"
                            CommandArgument='<%# Eval("MenuItemId") %>' />
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
            <div class="bottom-nav">
     <a href="Menu.aspx" class="active">Menu</a>
     <a href="Search.aspx" >Search</a>
     <a href="Cart.aspx">Cart</a>
     <a href="Information.aspx">Information</a>
   </div>

</div>
    </form>
</body>
</html>
