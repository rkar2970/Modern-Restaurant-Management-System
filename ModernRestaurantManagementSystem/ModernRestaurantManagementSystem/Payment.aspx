<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="ModernRestaurantManagementSystem.Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/StyleSheet.css" rel="stylesheet" />


</head>
<body>
    <form id="form1" runat="server">
           <div class="page-container">

    <div class="payment-header">
        <div>
            <h2 class="page-title">Payment</h2>
            <p class="page-subtitle">Confirm your order and payment method.</p>
        </div>

        <div class="table-badge">
            <asp:Label ID="lblTableNumber" runat="server"></asp:Label>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="message-text"></asp:Label>


           <div class="summary-card">
    <h3 class="section-title">Order Summary</h3>

    <asp:Repeater ID="rptPaymentItems" runat="server">
        <ItemTemplate>
            <div class="summary-row">
                <div>
                    <strong><%# Eval("Name") %></strong>
                    <p>Qty: <%# Eval("Quantity") %></p>
                </div>

                <span><%# Eval("Subtotal") %> Ks</span>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
            <hr />
            <div class="payment-total-box">
    <span>Total Amount</span>
    <strong>
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </strong>
</div>
           <div class="payment-method-card">
    <h3 class="section-title">Payment Method</h3>

    <asp:RadioButtonList 
        ID="rblPaymentMethod" 
        runat="server"
        CssClass="payment-method-list">

        <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
        <asp:ListItem Text="KBZPay" Value="KBZPay"></asp:ListItem>
        <asp:ListItem Text="WavePay" Value="WavePay"></asp:ListItem>
    </asp:RadioButtonList>
</div>
            <br />

            <asp:Button  class="btn-main btn-full" ID="btnPayNow" runat="server" Text="Pay Now" OnClick="btnPayNow_Click" />
              
        </div>
    </form>
</body>
</html>
