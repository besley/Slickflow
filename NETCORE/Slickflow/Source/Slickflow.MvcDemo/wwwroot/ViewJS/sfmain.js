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

    //show process diagram
    sfmain.showKGraph = function () {
        if (pordermanager.mProductOrderProcessGUID == "") {
            $.msgBox({
                title: "DynamicFlow / KGraph",
                content: kresource.getItem("showprocessgraphwarnmsg"),
                type: "alert"
            });
            return false;
        } else {
        	window.open('/sfd2c/Diagram?AppInstanceID=' + pordermanager.selectedProductOrderID
				+ '&ProcessGUID=' + pordermanager.mProductOrderProcessGUID + '&Mode=' + 'READONLY');
        }
    }

    sfmain.changeLang = function (lang) {
        kresource.setLang(lang);
        location.reload();
    }
    return sfmain;
})();