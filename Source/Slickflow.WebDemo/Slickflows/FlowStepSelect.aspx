<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowStepSelect.aspx.cs" Inherits="Slickflow.WebDemoV2._0.Slickflows.FlowStepSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Skin/default.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>

    <script type="text/javascript">

        function SelectOK() {
            $("#hiddenOK").val("");
            var step = $(':radio[name="radioStep"]:checked').val();
            var member = $(':radio[name="radioMember"]:checked').val();
            if (step == undefined || step == null || step == "" || step == "0") {
                alert("请选择办理步骤");
                return;
            }

            var memberLength = $(':radio[name="radioMember"]').length;
            if (memberLength > 0) {
                if (member == undefined || member == null || member == "" || member == "0") {
                    alert("请选择办理步骤人员");
                    return;
                }
            }

            $("#hiddenStepGuid").val(step);
            $("#hiddenStepUser").val(member);
            $("#hiddenOK").val("OK");
            CloseWindowPage();
        }

        //关闭窗口
        function CloseWindowPage() {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }

    </script>


</head>
<body>
    <form id="form1" runat="server">

        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
            <tr valign="middle" style="background: #EEEEEE">
                <td align="left" width="50%">转到步骤</td>
                <td align="left" width="50%">办理人员</td>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td>
                            <input name="radioStep" id='radioStep_<%#Eval("ActivityGUID") %>' type="radio" value="<%#Eval("ActivityGUID") %>" /><%#Eval("ActivityName") %>
                        </td>
                        <td>
                            <asp:Repeater ID="Repeater2" runat="server">
                                <ItemTemplate>
                                    <input name="radioMember" id='radioMember_<%#Eval("ID") %>' type="radio" value="<%#Eval("ID") %>" /><%#Eval("UserName")%>
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td colspan="2">
                    <asp:Literal ID="LiteralMSG" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>

        <div style="margin-top: 20px; float: right; text-align: right; line-height: 30px;">
            <input type="hidden" id="hiddenStepGuid" value="" runat="server" />
            <input type="hidden" id="hiddenStepUser" value="" runat="server" />
            <input type="hidden" id="hiddenOK" value="" runat="server" />
            <input type="hidden" id="hiddenIsSelectMember" value="" runat="server" />

            <input type="button" value="确定" class="btn" onclick="SelectOK();" />&nbsp;&nbsp;
                <input type="button" value="取消" class="btn" onclick="CloseWindowPage();" />
        </div>

    </form>

</body>
</html>
