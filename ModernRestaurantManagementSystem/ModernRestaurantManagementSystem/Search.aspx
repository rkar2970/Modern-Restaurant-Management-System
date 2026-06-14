<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="ModernRestaurantManagementSystem.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
              <h2>Search Page</h2>
              <p>This is Search Page</p>

            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search food name..."></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

            <asp:Button ID="btnFilterToggle" runat="server" Text="Filter ▼" OnClick="btnFilterToggle_Click" />

<asp:Panel ID="pnlFilter" runat="server" Visible="false">
    <h4>Price</h4>

    <asp:RadioButtonList ID="rblPrice" runat="server">
        <asp:ListItem Text="Under 5000 Ks" Value="under5000"></asp:ListItem>
        <asp:ListItem Text="Under 10000 Ks" Value="under10000"></asp:ListItem>
        <asp:ListItem Text="Over 10000 Ks" Value="over10000"></asp:ListItem>
        <asp:ListItem Text="Over 15000 Ks" Value="over15000"></asp:ListItem>
    </asp:RadioButtonList>

    <h4>Categories</h4>

    <asp:CheckBoxList ID="cblCategories" runat="server"></asp:CheckBoxList>

    <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" OnClick="btnApplyFilter_Click" />
    <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click" />
</asp:Panel>

        <asp:Repeater ID="rptSearchResults" runat="server">
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
<asp:Label ID="lblMessage" runat="server"></asp:Label>
            
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
