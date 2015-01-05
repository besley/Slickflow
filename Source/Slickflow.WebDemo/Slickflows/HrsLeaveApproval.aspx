<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HrsLeaveApproval.aspx.cs" Inherits="Slickflow.WebDemoV2._0.Slickflows.HrsLeaveApproval" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请假流程办理</title>
    <link href="../Skin/default.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="Main.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>流程管理</span>
            <i class="arrow"></i>
            <span>请假流程办理</span>
        </div>

        <!--/导航栏-->

        <!--内容-->
        <div class="tab-content">
            <dl>
                <dt>请假流程GUID</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtProcessGUID" class="input normal" />
                </dd>
            </dl>

            <dl>
                <dt>请假类型</dt>
                <dd>
                    <div class="rule-single-select">
                        <select id="selectLeaveType" runat="server">
                            <option value="0">请选择请假类型...</option>
                            <option value="1">病假</option>
                            <option value="2">事假</option>
                            <option value="3">丧假</option>
                            <option value="4">产假</option>
                            <option value="5">工伤假</option>
                            <option value="6">婚假</option>
                            <option value="7">年休假</option>
                            <option value="8">其他假</option>
                        </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>请假天数</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDays" class="input normal" disabled="disabled" />

                </dd>
            </dl>
            <dl>
                <dt>请假开始日期</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtFromDate" class="input normal" disabled="disabled" />
                </dd>
            </dl>
            <dl>
                <dt>请假结束日期</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtToDate" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>部门经理意见</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDepmanagerRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>主管总监意见</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDirectorRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>副总经理意见</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDeputyGeneralRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>总经理意见</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtGeneralManagerRemark" class="input normal" disabled="disabled" /></dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-list">
                <input type="hidden" id="hiddenStepGuid" value="" runat="server" />
                <input type="hidden" id="hiddenStepUser" value="" runat="server" />
                <input type="hidden" id="hiddenInstanceId" value="" runat="server" />
                <input type="hidden" id="hiddenNextFlowIsOK" value="" runat="server" />
                <input type="hidden" id="hiddenPerformField" value="" runat="server" />
                <input type="hidden" id="hiddenActivityInstanceID" value="" runat="server" />
                <input name="btnSelectFlowStep" type="button" value="选择送下一步信息" class="btn" onclick="SeleteFlowInfo()" />
                <asp:Button ID="btnSendNext" runat="server" Text="送往下一步" CssClass="btn yellow" OnClick="btnSendNext_Click" Style="display: none" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
            <div class="clear"></div>
        </div>
        <!--/工具栏-->
    </form>
    <script type="text/javascript">
        function SeleteFlowInfo() {
            var ProcessGUID = $("#txtProcessGUID").val();
            var Days = $("#txtDays").val();
            var InstanceId = $("#hiddenInstanceId").val();

            if (ProcessGUID == null || ProcessGUID == "") {
                alert("请填写流程GUID");
                return;
            }

            if (Days == null || Days == "" || parseInt(Days) <= 0) {
                alert("请填写请假天数");
                return;
            }

            $("#hiddenNextFlowIsOK").val("");
            $.layer({
                type: 2,
                closeBtn: [0, true],
                shadeClose: false,
                shade: [0],
                border: [5, 0.3, '#000', true],
                offset: ['10px', ''],
                area: ['420px', '410px'],
                title: "转下一步.选择步骤及办理人",
                iframe: { src: 'FlowStepSelect.aspx?Step=task&ProcessGUID=' + ProcessGUID + "&instanceId=" + InstanceId + "&condition=" + "days-" + parseInt(Days) },
                close: function (index) {
                    layer.close(index);
                },
                beforeClose: function (index) {
                    var selectOK = layer.getChildFrame('#hiddenOK', index).val();
                    if (selectOK == "OK") {
                        var _stepGuid = layer.getChildFrame('#hiddenStepGuid', index).val();//选中的步骤ID
                        var _stepMember = layer.getChildFrame('#hiddenStepUser', index).val();//步骤办理人员ID

                        if (_stepGuid != undefined && _stepGuid != null && _stepGuid != "") {
                            $("#hiddenStepGuid").val(_stepGuid);
                            $("#hiddenStepUser").val(_stepMember);
                        }
                        var nextActivityGuid = $("#hiddenStepGuid").val();
                        var nextActivityMemberId = $("#hiddenStepUser").val();
                        if (nextActivityGuid != undefined && nextActivityGuid != null && nextActivityGuid != "") {
                            $("#hiddenNextFlowIsOK").val("OK");
                            $("#btnSendNext").click();
                        } else {
                            $("#hiddenNextFlowIsOK").val("");
                            $("#hiddenStepGuid").val("");
                            $("#hiddenStepUser").val("");
                        }
                    }
                },
                end: function () {

                }
            });
        }

    </script>
</body>

</html>
