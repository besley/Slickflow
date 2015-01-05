<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowCanvas.aspx.cs" Inherits="Slickflow.WebDemoV2._0.Slickflows.FlowCanvas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>流程步骤</title>
    <link href="../Skin/default.css" rel="stylesheet" />
    <script src="../js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="../js/excanvas.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="Main.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>流程步骤</span>
        </div>
        <!--/导航栏-->

        <!--内容-->
        <div class="tab-content">
            <dl>
                <dt>流程GUID</dt>
                <dd>
                    <input runat="server" type="text" value="" id="txtProcessGUID" class="input normal" disabled="disabled" />
                    <div style="display: none;">
                        <textarea id="wfxml" style="width: 400px; height: 200px;" runat="server"></textarea>
                    </div>
                </dd>
            </dl>

            <dl>
                <dt>流程图</dt>
                <dd>
                    <canvas id="canvas" width="800" height="1500" style="border: 1px solid #f00;"></canvas>
                </dd>
            </dl>
        </div>
        <!--/内容-->
    </form>
    <script src="../js/flow.canvas.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            drawWf();
        });
    </script>
</body>
</html>
