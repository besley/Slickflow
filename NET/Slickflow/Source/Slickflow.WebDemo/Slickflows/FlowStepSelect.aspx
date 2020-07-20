<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowStepSelect.aspx.cs" Inherits="Slickflow.WebDemo.Slickflows.FlowStepSelect" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width" />
    <title></title>
    <link href="../Skin/default.css?v=1.1" rel="stylesheet" />
    <link href="../Skin/layout.css?v=0.1" rel="stylesheet" />
    <link href="../js/zTree/zTreeStyle/zTreeStyle.css?v=0.1" rel="stylesheet" />
    <script src="../js/base.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.0.min.js?v=0.1" type="text/javascript"></script>
    <script src="../js/zTree/jquery.ztree.core-3.5.min.js?v=0.1" type="text/javascript"></script>
    <script src="../js/zTree/jquery.ztree.excheck-3.5.min.js?v=0.1" type="text/javascript"></script>

    <script type="text/javascript">
        function GetCheckedAll() {
            var nodesArray = [];
            var treeObj = $.fn.zTree.getZTreeObj("ztree_container");
            var nodes = treeObj.getCheckedNodes(true);
            if (nodes.length > 0) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    nodesArray.push(node.id);
                }
            }
            $("#hiddenNextActivityPerformers").val(nodesArray.join(','));
            var NextActivityPerformers = $("#hiddenNextActivityPerformers").val();
            if (step == undefined || step == null || step == "" || step == "0" || step.length < 1) {
                alert("请选择办理步骤或办理人员(Please select step and user)");
                return;
            }
        }

        function SelectOK() {
            $("#hiddenOK").val("");

            var nodesArray = [];
            var treeObj = $.fn.zTree.getZTreeObj("ztree_container");
            var nodes = treeObj.getCheckedNodes(true);
            if (nodes.length > 0) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    nodesArray.push(node.id);
                }
            }
            $("#hiddenNextActivityPerformers").val(nodesArray.join(','));
            var nextActivityPerformers = $("#hiddenNextActivityPerformers").val();
            if (nextActivityPerformers == undefined || nextActivityPerformers == null || nextActivityPerformers == "" || nextActivityPerformers == "0" || nextActivityPerformers.length < 1) {
                alert("请选择办理步骤或办理人员(Please select step and user)");
                return;
            }

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
        <div id="ztree_container" class="ztree">
        </div>
        <asp:Literal ID="LiteralMSG" runat="server"></asp:Literal>
        <div style="margin-top: 20px; float: right; text-align: right; line-height: 30px;">
            <input type="hidden" id="hiddenStepGuid" value="" runat="server" />
            <input type="hidden" id="hiddenStepUser" value="" runat="server" />
            <input type="hidden" id="hiddenOK" value="" runat="server" />

            <input type="hidden" id="hiddenNextActivityPerformers" value="" runat="server" />

            <input type="hidden" id="hiddenIsSelectMember" value="" runat="server" />

            <input type="button" value="确定(Sure)" class="btn" onclick="SelectOK();" />&nbsp;&nbsp;
                <input type="button" value="取消(Cancel)" class="btn" onclick="CloseWindowPage();" />
        </div>

    </form>


    <script type="text/javascript">
        var setting = {
            check: {
                enable: true,
                chkboxType: { "Y": "ps", "N": "ps" },
                chkStyle: "checkbox"
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            view: {
                showLine: true
            }
        };

        $(function () {
            $.getJSON('FlowStepSelect.aspx' + window.location.search + '&Action=InitStep', function (data) {
                $.fn.zTree.init($("#ztree_container"), setting, data);
            }, 'JSON');

        });
    </script>

</body>
</html>
