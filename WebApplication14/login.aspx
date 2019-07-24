<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication14.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="loginStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div class="layoutMain">
                <asp:TextBox ID="mEmail" runat="server" placeholder="Email..." CssClass="txtBox"></asp:TextBox>

               

                <asp:TextBox ID="mPassword" runat="server"  placeholder="Password..." CssClass="txtBox"></asp:TextBox>


                <asp:Button ID="mLogin" runat="server" Text="login" OnClick="mLogin_Click" CssClass="txtBox"/>


            </div>
        </div>
    </form>
</body>
</html>

