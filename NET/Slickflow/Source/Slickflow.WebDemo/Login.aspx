<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Slickflow.WebDemo.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="Skin/default.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>
</head>
<body class="loginbody">
    <form id="form1" runat="server">
        <div class="login-screen">
            <div class="login-icon">LOGO</div>
            <div class="login-form">
                <h1>系统管理登录(Login)</h1>
                <div class="control-group">
                    <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" Width="200"></asp:DropDownList>
                </div>
                <div class="control-group">
                    <asp:DropDownList ID="ddlUser" runat="server" Width="200"></asp:DropDownList>
                </div>
                <div>
                    <asp:Button ID="btnLogin" CssClass="btn-login" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
    </form> 
</body>
</html>
