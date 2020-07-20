<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tool.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.Tool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>工具箱(ToolBox)</title>
    <link href="../Skin/default.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="Main.aspx" class="home"><i></i><span>首页(Home)</span></a>
            <i class="arrow"></i>
            <span>工具箱(ToolBox)</span>

        </div>

        <!--/导航栏-->

        <!--内容-->
        <div class="tab-content">
            <dl>
                <dt>
                    <asp:Button ID="btnCreateGUID" runat="server" Text="生成新GUID(NewGUID)" CssClass="btn" OnClick="btnCreateGUID_Click"   /></dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtNewGuid" class="input normal" />
                </dd>
            </dl>

        </div>
        <!--/内容-->
        
    </form>


</body>


</html>
