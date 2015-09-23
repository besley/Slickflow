
var accountManager;
if (!accountManager) accountManager = {};

(function () {
    var isValidated = false;

    accountManager.loadRoles = function () {
        var query = {
            "ProcessGUID": gProductOrderProcessGUID,
            "Version": "1"
        }

        jshelper.ajaxPost('/SfDemoApi/api/Wf/QueryProcessRoles', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var roleList = result.Entity;
                $.each(roleList, function (i, role) {
                    $('#ddlRoles').append($('<option>', {
                        value: role.ID,
                        text: role.RoleName
                    }));
                });
            }
        });
    }

    accountManager.fillUsers = function (roleId) {
        //清空第一项之外的options
        $("#ddlUsers").find("option:gt(0)").remove();

        //重新加载
        jshelper.ajaxGet('/SfDemoApi/api/Wf/GetUserByRole/' + roleId, null, function (result) {
            if (result.Status === 1) {
                var performerList = result.Entity;
                $.each(performerList, function (i, performer) {
                    $('#ddlUsers').append($('<option>', {
                        value: performer.UserID,
                        text: performer.UserName
                    }));
                });
            }
        });
    }

    accountManager.login = function () {
        //remove temp cacahe data
        lsm.removeTempStorage();

        //检查角色用户是否选中
        var selectedText = $('#ddlUsers').find(":selected").text();
        var selectedValue = $('#ddlUsers').find(":selected").val();

        if (selectedValue > 0) {
            var user = {};
            user.UserID = selectedValue;
            user.UserName = selectedText;

            lsm.saveUserIdentity(user);

            //读取用户授权信息
            accountManager.readAuthData(user.UserID);

            accountManager.isValidated = true;
        } else {
            alert("请选择角色和用户！")
        }
    }

    accountManager.readAuthData = function (userID){
        //重新加载
        jshelper.ajaxGetSyn('/SfDemoApi/api/Auth/GetResourceByUser/' + userID, null, false, function (result) {
            if (result.Status === 1) {
                var resourceList = result.Entity;
                lsm.saveUserAuthData(resourceList);
            }
        });
    }
})()