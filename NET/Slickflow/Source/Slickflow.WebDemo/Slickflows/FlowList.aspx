<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowList.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.FlowList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Skin/default.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">

        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页(Return)</span></a>
            <a href="Main.aspx" class="home"><i></i><span>首页(Home)</span></a>
            <i class="arrow"></i>
            <span>流程列表(ProcessList)</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div class="toolbar-wrap">
            <div id="floatHead_1" class="toolbar">
                <div class="l-list">
                    <ul class="icon-list">
                        <li><a onclick="javascript:void(0);" id="btnFlowApply" class="add" href="HrsLeaveApply.aspx"><i></i><span>发起请假流程(Apply)</span></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--内容-->
        <div class="content-tab-wrap">
            <div id="floatHead" class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a href="javascript:;" onclick="tabs(this);" class="selected">待办流程(ToDo)</a></li>
                        <li><a href="javascript:;" onclick="tabs(this);">我发起的流程(Done)</a></li>
                        <li><a href="javascript:;" onclick="tabs(this);">所有流程(All)</a></li>
                    </ul>
                </div>
            </div>
        </div>


        <!--代办流程-->
        <div class="tab-content">
            <table width="100%" border="1" cellspacing="0" cellpadding="0" class="ltable">
                <tr>
                    <th align="center">实例名称(ProcessName)</th>
                    <th align="center">活动名称(ActivityName)</th>
                    <th align="center">活动状态(ActivityState)</th>
                    <th align="center">任务状态(TaskState)</th>
                    <th align="center">创建时间(CreatedTime)</th>
                    <th align="center">操作(User)</th>
                </tr>
                <asp:Repeater ID="Repeater2" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="left"><%#Eval("AppName") %></td>
                            <td align="left"><%#Eval("ActivityName") %></td>
                            <td align="left"><%#Slickflow.WebDemo.Common.EnumHelper.GetDescription(typeof(Slickflow.WebDemo.Common.ActivityStateEnum),Convert.ToInt32(Eval("ActivityState"))) %></td>
                            <td align="left"><%#Slickflow.WebDemo.Common.EnumHelper.GetDescription(typeof(Slickflow.WebDemo.Common.TaskStateEnum),Convert.ToInt32(Eval("TaskState"))) %></td>
                            <td align="left"><%#Eval("CreatedDateTime") %></td>
                            <td align="center">
                                <a href="javascript:ShowFlowOpinion(<%#Eval("AppInstanceID") %>)">流程信息(FlowInfo)</a>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <a href="/sfd/Diagram?ProcessGUID=<%#Eval("ProcessGUID") %>&Version=<%#Eval("Version") %>&AppInstanceID=<%#Eval("AppInstanceID") %>" target="_blank">流程图信息(Diagram)</a>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <a href="HrsLeaveApproval.aspx?ProcessGUID=<%#Eval("ProcessGUID") %>&AppInstanceID=<%#Eval("AppInstanceID") %>&ActivityInstanceID=<%#Eval("ActivityInstanceID") %>">办理(Approving)</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!--/代办流程-->

        <!--我发起的流程列表-->
        <div class="tab-content" style="display: none;">
            <table width="100%" border="1" cellspacing="0" cellpadding="0" class="ltable">
                <tr>
                    <th align="center">流程名称(ProcessName)</th>
                    <th align="center">发起人(Applicant)</th>
                    <th align="center">创建时间(CreateTime)</th>
                    <th align="center">运行状态(Status)</th>
                    <th align="center">当前步骤(CurrentStep)</th>
                    <th align="center">操作(User)</th>
                </tr>
                <asp:Repeater ID="RepeaterMyApply" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="left"><%#Eval("AppName") %></td>
                            <td align="left"><%#Eval("CreatedByUserName") %></td>
                            <td align="left"><%#Eval("CreatedDateTime") %></td>
                            <td align="left"><%#Slickflow.WebDemo.Common.EnumHelper.GetDescription(typeof(Slickflow.WebDemo.Common.ProcessStateEnum),Convert.ToInt32(Eval("ProcessState"))) %></td>
                            <td align="left"><%#Eval("CurrentActivityText") %></td>
                            <td align="center">
                                <a href="javascript:ShowFlowOpinion(<%#Eval("AppInstanceID") %>)">流程信息(FlowInfo)</a>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <a href="/sfd/Diagram?ProcessGUID=<%#Eval("ProcessGUID") %>&Version=<%#Eval("Version") %>&AppInstanceID=<%#Eval("AppInstanceID") %>" target="_blank">流程图信息(Diagram)</a>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <a href="HrsLeaveInfo.aspx?AppInstanceID=<%#Eval("AppInstanceID") %>">申请信息(ApplyInfo)</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!--/我发起的流程列表-->

        <!--所有流程-->
        <div class="tab-content" style="display: none;">
            <table width="100%" border="1" cellspacing="0" cellpadding="0" class="ltable">
                <tr>
                    <th align="center">流程名称(ProcessName)</th>
                    <th align="center">发起人(Applicant)</th>
                    <th align="center">创建时间(CreateTime)</th>
                    <th align="center">运行状态(Status)</th>
                    <th align="center">当前步骤(CurrentStep)</th>
                    <th align="center">操作(User)</th>
                </tr>
                <asp:Repeater ID="RepeaterALL" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="left"><%#Eval("AppName") %></td>
                            <td align="left"><%#Eval("CreatedByUserName") %></td>
                            <td align="left"><%#Eval("CreatedDateTime") %></td>
                            <td align="left"><%#Slickflow.WebDemo.Common.EnumHelper.GetDescription(typeof(Slickflow.WebDemo.Common.ProcessStateEnum),Convert.ToInt32(Eval("ProcessState"))) %></td>
                            <td align="left"><%#Eval("CurrentActivityText") %></td>
                            <td align="center">
                                <a href="javascript:ShowFlowOpinion(<%#Eval("AppInstanceID") %>)">流程信息(FlowInfo)</a>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <a href="/sfd0/Diagram?ProcessGUID=<%#Eval("ProcessGUID") %>&Version=<%#Eval("Version") %>&AppInstanceID=<%#Eval("AppInstanceID") %>" target="_blank">流程图信息(Diagram)</a>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <a href="HrsLeaveInfo.aspx?AppInstanceID=<%#Eval("AppInstanceID") %>">申请信息(ApplyInfo)</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!--/列表-->

    </form>
</body>
</html>
<script type="text/javascript">
    function ShowFlowOpinion(AppInstanceID) {
        $.layer({
            type: 2,
            closeBtn: [0, true],
            shadeClose: false,
            shade: [0],
            border: [5, 0.3, '#000', true],
            offset: ['10px', ''],
            area: ['720px', '500px'],
            title: "查看流程详细办理步骤(ViewDetail)",
            iframe: { src: 'FlowOpinion.aspx?AppInstanceID=' + AppInstanceID },
            close: function (index) {
                layer.close(index);
            },
            beforeClose: function (index) {
            },
            end: function () {

            }
        });
    }

</script>
