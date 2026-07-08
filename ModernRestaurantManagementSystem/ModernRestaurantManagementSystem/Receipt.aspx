<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="ModernRestaurantManagementSystem.Receipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/StyleSheet.css" rel="stylesheet" />


</head>
<body>
    <form id="form1" runat="server">
       <div class="page-container">

    <div class="receipt-box">

        <div class="receipt-success">
            <div class="success-icon">✓</div>
            <h2>Order Confirmed</h2>
            <p>Your receipt has been generated.</p>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="message-text"></asp:Label>

        <div class="receipt-info">
            <div>
                <span>Receipt No</span>
                <strong><asp:Label ID="lblReceiptNo" runat="server"></asp:Label></strong>
            </div>

            <div>
                <span>Table No</span>
                <strong><asp:Label ID="lblTableNumber" runat="server"></asp:Label></strong>
            </div>

            <div>
                <span>Order Date</span>
                <strong><asp:Label ID="lblOrderDate" runat="server"></asp:Label></strong>
            </div>

            <div>
                <span>Payment</span>
                <strong><asp:Label ID="lblPaymentMethod" runat="server"></asp:Label></strong>
            </div>
        </div>

        <h3 class="section-title">Ordered Items</h3>

        <asp:Repeater ID="rptReceiptItems" runat="server">
            <ItemTemplate>
                <div class="receipt-item-row">
                    <div>
                        <strong><%# Eval("Name") %></strong>
                        <p>Qty: <%# Eval("Quantity") %></p>
                    </div>

                    <span><%# Eval("Subtotal") %> Ks</span>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div class="receipt-total">
            <span>Total Amount</span>
            <strong>
                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
            </strong>
        </div>

        <div class="thank-you-box">
            <p>Thank you for your order.</p>
            <p>Receipt ရရှိပြီးနောက် food ကိုစတင်ပြင်ဆင်ပါမည်။</p>
        </div>
        <asp:Button  class="btn-main" ID="backToMenu" runat="server" Text="Back To Menu" OnClick="backToMenu_Click" />
    </div>

</div>
    </form>
</body>
</html>
