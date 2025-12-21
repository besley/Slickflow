var sfmain = (function () {
    function sfmain() {
    }

    sfmain.WfAppRunner = {};

    sfmain.ready = function () {
        pordermanager.getProductOrderList(true);
    }

    sfmain.initWfAppRunner = function () {
        var user = lsm.getUserIdentity();

        sfmain.WfAppRunner.UserId = user.UserId;
        sfmain.WfAppRunner.UserName = user.UserName;
    }

    sfmain.checkWfAppRunner = function () {
        var isOk = false;
        var user = lsm.getUserIdentity();
        if (user && $.trim(user.UserId) != "" && $.trim(user.UserName) != "") isOk = true;
        return isOk;
    }

    function initializeControls() {
        $(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd' });
    }

    //显示流程图
    //Display flowchart
    sfmain.showKGraph = function () {
        if (pordermanager.mProductOrderProcessId === "") {
            $.msgBox({
                title: "DynamicFlow / KGraph",
                content: "Please select a form with process information！",
                type: "alert"
            });
            return false;
        } else {
        	window.open('/sfd/Diagram?AppInstanceId=' + pordermanager.selectedProductOrderId.toString()
				+ '&ProcessId=' + pordermanager.mProductOrderProcessId + '&Mode=' + 'READONLY');
        }
    }

    return sfmain;
})();