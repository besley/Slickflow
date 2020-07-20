<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SwitchLoginUser.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.SwitchLoginUser" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Skin/default.css?v=1.1" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
</head>
<body class="loginbody" style="background-color: #EEF3FA">
    <form id="form1" runat="server">
        <div class="login-screen" style="margin-left:-185px">
            <div class="login-form" style="margin-left:0px;">
                <h1>用户切换(SwitchUser)</h1>
                <div class="control-group">
                    <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" Width="200"></asp:DropDownList>
                </div>
                <div class="control-group">
                    <asp:DropDownList ID="ddlUser" runat="server" Width="200"></asp:DropDownList>
                </div>
                <div>
                    <asp:Button ID="btnSwitchUser" CssClass="btn-login" runat="server" Text="切换(Switch)" OnClick="btnSwitchUser_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
