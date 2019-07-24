<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addingPost.aspx.cs" Inherits="WebApplication14.addingPost" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 0 auto; text-align: center; margin-top: 20px;">

            <asp:TextBox ID="postText" placeholder="text ..." runat="server"></asp:TextBox>

            <br />

            <asp:DropDownList ID="postType" runat="server"></asp:DropDownList>

            <br />

            <asp:FileUpload ID="FileUpload1" runat="server" />

            <br />

            <asp:Button ID="addPostBtn" runat="server" Text="add" OnClick="addPostBtn_Click" />
        </div>

    </form>
</body>
</html>
