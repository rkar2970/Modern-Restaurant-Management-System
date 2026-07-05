<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="ModernRestaurantManagementSystem.Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/site.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
              <h2 class="page-title">Payment</h2>
            <asp:Label ID="lblTableNumber" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <h3>Order Summary</h3>
            <asp:Repeater ID="rptPaymentItems" runat="server">
                <ItemTemplate>
                    <div>
                        <h4><%#Eval("Name")%></h4>
                        <p>Quantity:<%#Eval("Quantity")%></p>
                        <p>Price:<%#Eval("Price")%>Ks</p>
                        <p>Subtotal:<%#Eval("Subtotal")%>Ks</p>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <hr />
            <asp:Label ID="lblTotal" runat="server"></asp:Label>
            <h3>Payment Method</h3>

            <asp:RadioButtonList ID="rblPaymentMethod" runat="server">

                <asp:ListItem Text="Cash" Value="Cash" ></asp:ListItem>
                <asp:ListItem Text="KBZPay" Value="KBZPay" ></asp:ListItem>
                <asp:ListItem Text="WavePay" Value="WavePay" ></asp:ListItem>


            </asp:RadioButtonList>
            <br />

            <asp:Button  class="btn-main" ID="btnPayNow" runat="server" Text="Pay Now" OnClick="btnPayNow_Click" />
              
        </div>
    </form>
</body>
</html>
