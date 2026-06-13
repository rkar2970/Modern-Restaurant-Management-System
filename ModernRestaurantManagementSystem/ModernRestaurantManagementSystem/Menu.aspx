<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="ModernRestaurantManagementSystem.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblTest" runat="server"></asp:Label>
            <asp:Label ID="Label1" runat="server"></asp:Label>

            <h2>Menu Page</h2>
            <p>This is menu Page</p>

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
