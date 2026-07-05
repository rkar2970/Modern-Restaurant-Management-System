<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="ModernRestaurantManagementSystem.Receipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/site.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
              <h2 class="page-title">Receipt</h2>

<asp:Label ID="lblMessage" runat="server"></asp:Label>

<div class="receipt-box">
    <p><strong>Receipt No:</strong> <asp:Label ID="lblReceiptNo" runat="server"></asp:Label></p>
    <p><strong>Table No:</strong> <asp:Label ID="lblTableNumber" runat="server"></asp:Label></p>
    <p><strong>Order Date:</strong> <asp:Label ID="lblOrderDate" runat="server"></asp:Label></p>
    <p><strong>Payment Method:</strong> <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label></p>

    <hr />

    <h3>Ordered Items</h3>

    <asp:Repeater ID="rptReceiptItems" runat="server">
        <ItemTemplate>
            <div class="receipt-item">
                <h4><%# Eval("Name") %></h4>
                <p>Quantity: <%# Eval("Quantity") %></p>
                <p>Price: <%# Eval("Price") %> Ks</p>
                <p>Subtotal: <%# Eval("Subtotal") %> Ks</p>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <hr />

    <h3>
        <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
    </h3>

    <p>Thank you for your order.</p>
    <p>Receipt ရရှိပြီးနောက် food ကိုစတင်ပြင်ဆင်ပါမည်။</p>
</div>
            <asp:Button  class="btn-main" ID="backToMenu" runat="server" Text="Back To Menu" OnClick="backToMenu_Click" />


        </div>
    </form>
</body>
</html>
