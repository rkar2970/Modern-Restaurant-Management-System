<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ModernRestaurantManagementSystem.Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/site.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">  
           <h2 class="page-title">Your Cart</h2>

<asp:Label ID="lblTableNumber" runat="server"></asp:Label>
<br />
<asp:Label ID="lblMessage" runat="server"></asp:Label>

<asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
    <ItemTemplate>
        <div class="cart-item">
            <h3><%# Eval("Name") %></h3>

            <p>Price: <%# Eval("Price") %> Ks</p>

            <p>
                Quantity:
                <asp:Button 
                    class="btn-main"
                    ID="btnDecrease" 
                    runat="server" 
                    Text="-"
                    CommandName="Decrease"
                    CommandArgument='<%# Eval("CartItemId") %>' />

                <%# Eval("Quantity") %>

                <asp:Button 
                    class ="btn-main"
                    ID="btnIncrease" 
                    runat="server" 
                    Text="+"
                    CommandName="Increase"
                    CommandArgument='<%# Eval("CartItemId") %>' />
            </p>

            <p>Subtotal: <%# Eval("Subtotal") %> Ks</p>

            <asp:Button 
                class="btn-main"
                ID="btnRemove" 
                runat="server" 
                Text="Remove"
                CommandName="Remove"
                CommandArgument='<%# Eval("CartItemId") %>' />
        </div>
    </ItemTemplate>
</asp:Repeater>

<hr />

<asp:Label ID="lblTotal" runat="server"></asp:Label>
<br /><br />

<asp:Button 
     class="btn-main"
    ID="btnOrder" 
    runat="server" 
    Text="Order"
    OnClick="btnOrder_Click" />
            

             
            <div class="bottom-nav"> 
                <a href="Menu.aspx" >Menu</a>
                <a href="Search.aspx">Search</a>
                <a href="Cart.aspx" class="active" >Cart</a>
                <a href="Information.aspx">Information</a>
            </div>
            
        </div>
    </form>
</body>
</html>
