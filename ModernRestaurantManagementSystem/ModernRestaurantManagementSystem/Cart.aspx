<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ModernRestaurantManagementSystem.Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>  
           <h2>Your Cart</h2>

<asp:Repeater ID="rptCartItems" runat="server">
    <ItemTemplate>
        <div class="cart-item">
            <h3><%# Eval("Name") %></h3>
            <p>Quantity: <%# Eval("Quantity") %></p>
            <p>Price: <%# Eval("Price") %> Ks</p>
            <p>Subtotal: <%# Eval("Subtotal") %> Ks</p>
        </div>
    </ItemTemplate>
</asp:Repeater>

<asp:Label ID="lblTotal" runat="server"></asp:Label>

             <div class="buttom-nav">
                <a href="Menu.aspx">Menu</a>
                <a href="Search.aspx">Search</a>
                <a href="Cart.aspx">Cart</a>
                <a href="Imformation.aspx">Imformation</a>
             </div>
        </div>
    </form>
</body>
</html>
