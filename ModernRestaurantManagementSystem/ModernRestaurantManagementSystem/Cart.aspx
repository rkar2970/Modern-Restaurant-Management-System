<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ModernRestaurantManagementSystem.Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="ContentFolder/StyleSheet.css" rel="stylesheet" />


</head>
<body>
    <form id="form1" runat="server">
       <div class="page-container">

    <div class="cart-header">
        <div>
            <h2 class="page-title">Your Cart</h2>
            <p class="page-subtitle">Review your selected items.</p>
        </div>

        <div class="table-badge">
            <asp:Label ID="lblTableNumber" runat="server"></asp:Label>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="message-text"></asp:Label>

<asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
    <ItemTemplate>
        <div class="cart-card">

            <div class="cart-item-top">
                <div>
                    <h3 class="cart-food-name"><%# Eval("Name") %></h3>
                    <p class="cart-price">Price: <%# Eval("Price") %> Ks</p>
                </div>

                <asp:Button 
                    ID="btnRemove" 
                    runat="server" 
                    Text="Remove"
                    CssClass="btn-danger btn-small"
                    CommandName="Remove"
                    CommandArgument='<%# Eval("CartItemId") %>' />
            </div>

            <div class="quantity-row">
                <span>Quantity</span>

                <div class="quantity-control">
                    <asp:Button 
                        ID="btnDecrease" 
                        runat="server" 
                        Text="-"
                        CssClass="qty-btn"
                        CommandName="Decrease"
                        CommandArgument='<%# Eval("CartItemId") %>' />

                    <span class="quantity-number"><%# Eval("Quantity") %></span>

                    <asp:Button 
                        ID="btnIncrease" 
                        runat="server" 
                        Text="+"
                        CssClass="qty-btn"
                        CommandName="Increase"
                        CommandArgument='<%# Eval("CartItemId") %>' />
                </div>
            </div>

            <div class="subtotal-row">
                <span>Subtotal</span>
                <strong><%# Eval("Subtotal") %> Ks</strong>
            </div>

        </div>
    </ItemTemplate>
</asp:Repeater>

<hr />


<div class="cart-summary">
    <span>Total Amount</span>
    <strong>
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </strong>
</div>

<asp:Button 
     class="btn-main btn-full"
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
