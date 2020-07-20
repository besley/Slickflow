<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HrsLeaveInfo.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.HrsLeaveInfo" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请假流程办理(LeaveInfo)</title>
    <link href="../Skin/default.css?v=1.1" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="Main.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>流程管理(FlowInfo)</span>
            <i class="arrow"></i>
            <span>请假信息(LeaveInfo))</span>
        </div>

        <!--/导航栏-->

        <!--内容-->
        <div class="tab-content">
            <dl>
                <dt>请假类型(LeaveType)</dt>
                <dd>
                    <div class="rule-single-select">
                        <select id="selectLeaveType" runat="server">
                            <option value="0">请选择请假类型(SelectLeaveType)...</option>
                            <option value="1">病假(Sick)</option>
                            <option value="2">事假(Personal)</option>
                            <option value="3">丧假(FuneralLeave)</option>
                            <option value="4">产假(MaternityLeave)</option>
                            <option value="5">工伤假(InjuryLeave)</option>
                            <option value="6">婚假(MaritalLeave)</option>
                            <option value="7">年休假(AnnualLeave)</option>
                            <option value="8">其他假(Others)</option>
                        </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>请假天数(Days)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDays" class="input normal" disabled="disabled" />

                </dd>
            </dl>
            <dl>
                <dt>请假开始日期(From)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtFromDate" class="input normal" disabled="disabled" />
                </dd>
            </dl>
            <dl>
                <dt>请假结束日期(To)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtToDate" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>部门经理意见(DeptOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDepmanagerRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl class="none">
                <dt>主管总监意见(DirectorOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDirectorRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl  class="none">
                <dt>副总经理意见(DuptyOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDeputyGeneralRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>总经理意见(CEOOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtGeneralManagerRemark" class="input normal" disabled="disabled" /></dd>
            </dl>

            <dl>
                <dt>创建人(Applicant)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtCreatedByUserName" class="input normal" disabled="disabled" />
                </dd>
            </dl>

            <dl>
                <dt>创建日期(ApplyDate)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtCreatedDateTime" class="input normal" disabled="disabled" />
                </dd>
            </dl>

        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-list">
                <input name="btnReturn" type="button" value="返回上一页(Return)" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
            <div class="clear"></div>
        </div>
        <!--/工具栏-->
    </form>

</body>

</html>
