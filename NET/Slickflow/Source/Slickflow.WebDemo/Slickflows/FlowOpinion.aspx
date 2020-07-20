<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowOpinion.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.FlowOpinion" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Skin/default.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--列表-->

        <table width="100%" border="1" cellspacing="0" cellpadding="0" class="ltable">
            <tr>
                <th align="center" width="12%">步骤(Step)</th>
                <th align="center" width="8%">办理人(User)</th>
                <th align="center" width="10%">办理意见(Opinion)</th>
                <th align="center" width="10%">操作时间(ApprovingDate)</th>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td align="left"><%#Eval("ActivityName") %></td>
                        <td align="left"><%#Eval("ChangedUserName") %></td>
                        <td align="left"><%#Eval("Remark") %></td>
                        <td align="left"><%#Eval("ChangedTime") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>


    </form>
</body>
</html>
