<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HrsLeaveApply.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.HrsLeaveApply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请假流程发起(LeaveInfoSubmit)</title>
    <link href="../Skin/default.css?v=1.1" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layout.js" type="text/javascript"></script>
    <link href="../js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" />
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        function setToDate() {
            var date = $("#txtFromDate").val();
            var d = new Date(date);
            var days = Math.ceil($("#txtDays").val());
            d.setDate(d.getDate() + parseInt(days));
            var t = formatDate(d.toString(), "YYYY-MM-DD");
            $("#txtToDate").val(t);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="Main.aspx" class="home"><i></i><span>首页(Home)</span></a>
            <i class="arrow"></i>
            <span>流程管理(FlowInfo)</span>
            <i class="arrow"></i>
            <span>请假流程发起(LeaveInfoSubmit)</span>
        </div>

        <!--/导航栏-->

        <!--内容-->
        <div class="tab-content">
            <dl>
                <dt>请假流程GUID(ProcessGUID)</dt>
                <dd>
                    <input runat="server" type="text" value="2acffb20-6bd1-4891-98c9-c76d022d1445" disabled="disabled" id="txtProcessGUID" class="input normal" />
                </dd>
            </dl>

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
                    <input runat="server" type="text" value="" id="txtDays" class="input normal" />

                </dd>
            </dl>
            <dl>
                <dt>请假开始日期(From)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtFromDate" class="input normal Wdate" onfocus="WdatePicker({minDate:'%y-%M-%d',onpicked:setToDate})" style="cursor: pointer" />
                </dd>
            </dl>
            <dl>
                <dt>请假结束日期(To)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtToDate" class="input normal Wdate" onfocus="WdatePicker({minDate:'#F{$dp.$D(\'txtFromDate\')}',maxDate:'2020-10-01'})" style="cursor: pointer" /></dd>
            </dl>
            <dl>
                <dt>部门经理意见(DeptOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDepmanagerRemark" class="input normal disabled" disabled="disabled" /></dd>
            </dl>
            <dl class="none">
                <dt>主管总监意见(DirectorOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDirectorRemark" class="input normal disabled" disabled="disabled" /></dd>
            </dl>
            <dl class="none">
                <dt>副总经理意见(Duptypinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtDeputyGeneralRemark" class="input normal disabled" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dt>总经理意见(CEOOpinion)</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtGeneralManagerRemark" class="input normal disabled" disabled="disabled" /></dd>
            </dl>
            <dl>
                <dd>
                    <span>流程说明：</span><br />
                    <span>1、普通员工请假三天以内的（含三天），请假单由部门负责人批准,人事备档；</span><br />
                    <span>2、三天以上的经总经理同意后，再由人事备档；</span><br />
                    <span>备注：其他角色及条件的请自行定义流程；</span><br />
                </dd>
            </dl>

        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-list">
                <input type="hidden" id="hiddenNextActivityPerformers" value="" runat="server" />
                <input type="hidden" id="hiddenNextFlowIsOK" value="" runat="server" />
                <input name="btnSelectFlowStep" type="button" value="选择送下一步信息(NextStep)" class="btn" onclick="SeleteFlowInfo()" />
                <asp:Button ID="btnSave" runat="server" Text="提交保存(Save)" CssClass="btn yellow" OnClick="btnSave_Click" Style="display: none" />
                <input name="btnReturn" type="button" value="返回上一页(Return)" class="btn yellow" onclick="javascript: history.back(-1);" />

                <span id="span_flow_step"></span>
            </div>
            <div class="clear"></div>
        </div>
        <!--/工具栏-->
    </form>
    <script type="text/javascript">
        function SeleteFlowInfo() {
            $("#hiddenNextFlowIsOK").val("");
            var ProcessGUID = $("#txtProcessGUID").val();
            var Days = $("#txtDays").val();
            var LeaveType = $("#selectLeaveType").val();
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();

            if (ProcessGUID == null || ProcessGUID == "") {
                alert("请填写流程GUID--processguid required");
                return false;
            }

            if (LeaveType == null || LeaveType == "undefined" || LeaveType == "" || LeaveType == "0") {
                alert("请选择请假类型--leave type required");
                return false;
            }

            if (Days == null || Days == "" || parseInt(Days) <= 0) {
                alert("请填写请假天数--days required");
                return false;
            }

            if (FromDate == null || FromDate == "" || FromDate.length <= 0) {
                alert("请选择请假开始日期--fromdate required");
                return false;
            }

            if (ToDate == null || ToDate == "" || ToDate.length <= 0) {
                alert("请选择请假结束日期--todate required");
                return false;
            }


            $.layer({
                type: 2,
                closeBtn: [0, true],
                shadeClose: false,
                shade: [0],
                border: [5, 0.3, '#000', true],
                offset: ['10px', ''],
                area: ['420px', '410px'],
                title: "转下一步.选择步骤及办理人--please select next step and user",
                iframe: { src: 'FlowStepSelect.aspx?Step=start&ProcessGUID=' + ProcessGUID + "&condition=" + "days-" + Days },
                close: function (index) {
                    layer.close(index);
                },
                beforeClose: function (index) {
                    $("#hiddenNextFlowIsOK").val("");
                    var selectOK = layer.getChildFrame('#hiddenOK', index).val();
                    if (selectOK == "OK") {
                        var _hiddenNextActivityPerformers = layer.getChildFrame('#hiddenNextActivityPerformers', index).val();//选中的步骤人员
                        if (_hiddenNextActivityPerformers != undefined && _hiddenNextActivityPerformers != null && _hiddenNextActivityPerformers != "") {
                            $("#hiddenNextActivityPerformers").val(_hiddenNextActivityPerformers);
                        }
                        var hiddenNextActivityPerformers = $("#hiddenNextActivityPerformers").val();
                        if (hiddenNextActivityPerformers != undefined && hiddenNextActivityPerformers != null && hiddenNextActivityPerformers != "") {
                            $("#hiddenNextFlowIsOK").val("OK");
                            $("#btnSave").click();
                        } else {
                            $("#hiddenNextFlowIsOK").val("");
                            $("#hiddenNextActivityPerformers").val("");
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
