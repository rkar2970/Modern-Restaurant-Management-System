<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="ModernRestaurantManagementSystem.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">

            <div class="search-header">
              <h2 class="page-title">Search Food</h2>
                <p class="page-subtitle">Find Your Favourate Dishes easily.</p>


            </div>
            <asp:Label ID="lblMessage" runat="server" class="message-text"></asp:Label>


            <div class="search-box-wrapper">
                 <asp:TextBox ID="txtSearch" runat="server" placeholder="Search food name..." CssClass="search-input"></asp:TextBox>

                <asp:Button  CssClass="btn-main search-btn" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

            </div>
           

            <asp:Button  CssClass="btn-secondary filter-toggle" ID="btnFilterToggle" runat="server" Text="Filter Options" OnClick="btnFilterToggle_Click" />
<asp:Panel ID="pnlFilter" runat="server" Visible="false" CssClass="filter-panel">

    <h3 class="section-title">Price</h3>

    <asp:RadioButtonList 
        ID="rblPrice" 
        runat="server"
        CssClass="filter-list">
        <asp:ListItem Text="Under 5000 Ks" Value="under5000"></asp:ListItem>
        <asp:ListItem Text="Under 10000 Ks" Value="under10000"></asp:ListItem>
        <asp:ListItem Text="Over 10000 Ks" Value="over10000"></asp:ListItem>
        <asp:ListItem Text="Over 15000 Ks" Value="over15000"></asp:ListItem>
    </asp:RadioButtonList>

    <h3 class="section-title">Categories</h3>

    <asp:CheckBoxList 
        ID="cblCategories" 
        runat="server"
        CssClass="filter-list">
    </asp:CheckBoxList>

    <div class="filter-actions">
        <asp:Button 
            ID="btnApplyFilter" 
            runat="server" 
            Text="Apply Filter" 
            CssClass="btn-main"
            OnClick="btnApplyFilter_Click" />

        <asp:Button 
            ID="btnClearFilter" 
            runat="server" 
            Text="Clear" 
            CssClass="btn-secondary"
            OnClick="btnClearFilter_Click" />
    </div>

</asp:Panel>
            <h3 class="section-title">
                Search Results
            </h3>

        <asp:Repeater ID="rptSearchResults" runat="server" OnItemCommand="rptSearchResults_ItemCommand">
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
                <a href="Menu.aspx">Menu</a>
                <a href="Search.aspx" class="active" >Search</a>
                <a href="Cart.aspx">Cart</a>
                <a href="Information.aspx">Information</a>
              </div>
        </div>
    </form>
</body>
</html>
