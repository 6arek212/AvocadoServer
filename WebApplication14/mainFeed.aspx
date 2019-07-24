<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainFeed.aspx.cs" Inherits="WebApplication14.mainFeed" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>



            <asp:Button ID="addPost" runat="server" Text="add post" Style="position:absolute;    
                margin:16px; top: 12px;" OnClick="addPost_Click" CssClass="txtBox"></asp:Button>


            <asp:Repeater ID="Repeater1" runat="server">

                <ItemTemplate>

                    <div  class="postLayout">


                        <div>

                              <asp:Image ID="Image2" runat="server" ImageUrl='<%# Eval("User_profile_photo")%>'
                             CssClass="porfilePhoto"/>


                        <h3 class="h3"><%# Eval("User_first_name") %> <%# Eval("User_last_name") %></h3>


                    
                        </div>



                        <div class="postText">
                            <%# Eval("Post_text")%>
                       </div>

                        <div style="margin:0 auto; text-align:center;">
                       <%-- <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Post_image_path")%>' 
                          style="width:150px; height:150px;  "/>--%>
                      </div>

                    </div>
                </ItemTemplate>

            </asp:Repeater>






        </div>
    </form>
</body>
</html>
