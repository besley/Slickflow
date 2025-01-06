var sfmain = (function () {
    function sfmain() {
    }

    sfmain.WfAppRunner = {};

    sfmain.ready = function () {
        pordermanager.getProductOrderList(true);
    }

    sfmain.initWfAppRunner = function () {
        var user = lsm.getUserIdentity();

        sfmain.WfAppRunner.UserID = user.UserID;
        sfmain.WfAppRunner.UserName = user.UserName;
    }

    sfmain.checkWfAppRunner = function () {
        var isOk = false;
        var user = lsm.getUserIdentity();
        if (user && $.trim(user.UserID) != "" && $.trim(user.UserName) != "") isOk = true;
        return isOk;
    }

    function initializeControls() {
        $(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd' });
    }

    //显示流程图
    //Display flowchart
    sfmain.showKGraph = function () {
        if (pordermanager.mProductOrderProcessGUID === "") {
            $.msgBox({
                title: "DynamicFlow / KGraph",
                content: "Please select a form with process information！",
                type: "alert"
            });
            return false;
        } else {
        	window.open('/sfd/Diagram?AppInstanceID=' + pordermanager.selectedProductOrderID.toString()
				+ '&ProcessGUID=' + pordermanager.mProductOrderProcessGUID + '&Mode=' + 'READONLY');
        }
    }

    return sfmain;
})();