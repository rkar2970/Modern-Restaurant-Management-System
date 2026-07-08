<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Information.aspx.cs" Inherits="ModernRestaurantManagementSystem.Information" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="ContentFolder/StyleSheet.css" rel="stylesheet" />


</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">

    <div class="info-header">
        <h2 class="page-title">Information</h2>
        <p class="page-subtitle">About our restaurant and ordering system.</p>
    </div>

    <div class="info-card">
        <h3> Le Château Table</h3>
        <p>
            Welcome to  Le Château Table. Customers can order food easily by scanning the QR code on the table.
        </p>
    </div>

    <div class="info-card">
        <h3>Opening Hours</h3>
        <p>Monday - Sunday</p>
        <p>9:00 AM - 9:00 PM</p>
    </div>

    <div class="info-card">
        <h3>How to Order</h3>
        <ol>
            <li>Scan the QR code on your table.</li>
            <li>Choose food items from the menu.</li>
            <li>Add items to your cart.</li>
            <li>Review your cart and click Order Now.</li>
            <li>Select payment method and click Pay Now.</li>
            <li>Receive your receipt.</li>
        </ol>
    </div>

    <div class="info-card">
        <h3>Payment Methods</h3>
        <p>Cash, KBZPay, and WavePay are available.</p>
    </div>

    <div class="info-card">
        <h3>Contact</h3>
        <p>Phone: 09</p>
        <p>Address: </p>
    </div>

</div>

<div class="bottom-nav">
    <a href="Menu.aspx">Menu</a>
    <a href="Search.aspx">Search</a>
    <a href="Cart.aspx">Cart</a>
    <a href="Information.aspx" class="active">Information</a>
</div>
    </form>
</body>
</html>
