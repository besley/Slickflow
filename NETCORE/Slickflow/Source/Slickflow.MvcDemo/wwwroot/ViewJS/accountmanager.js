var accountmanager = (function () {
    function accountmanager() {
    }
    accountmanager.mrolelist = null;
    accountmanager.misValidated = false;

    accountmanager.loadRoles = function (jsonString) {
        $('#loading-indicator').show();

        var query = {};
        query.ProcessGUID = "5c5041fc-ab7f-46c0-85a5-6250c3aea375";
        query.Version = "1";

        jshelper.ajaxPost('api/wf/QueryProcessRoleUserList',
            JSON.stringify(query),
            function (result) {
                if (result.Status === 1) {
                    var rolelist = result.Entity;
                    accountmanager.mrolelist = rolelist;

                    $.each(rolelist, function (i, role) {
                        $('#ddlRoles').append($('<option>', {
                            value: role.RoleCode,
                            text: role.RoleName
                        }));
                    });

                    $('#ddlRoles').on('change', function (e) {
                        var optionSelected = $("option:selected", this);
                        var valueSelected = this.value;     //roleId
                        accountmanager.fillUsers(valueSelected);
                    });

                    $('#loading-indicator').hide();
                } else {
                    $.msgBox({
                        title: "MvcDemo / Account",
                        content: result.Message,
                        type: "error"
                    });
                }
            });
    }

    accountmanager.fillUsers = function (roleCode) {
        //清空第一项之外的options
        $("#ddlUsers").find("option:gt(0)").remove();
        var items = jQuery.grep(accountmanager.mrolelist, function (x) {
            return x.RoleCode === roleCode;
        });
        var userlist = items[0].UserList;
        //从新加载
        $.each(userlist, function (i, user) {
            $('#ddlUsers').append($('<option>', {
                value: user.UserID,
                text: user.UserName
            }));
        });
    }

    accountmanager.login = function () {
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

            var selectedRoleText = $("#ddlRoles").find(":selected").text();
            var selectedRoleValue = $("#ddlRoles").find(":selected").val();

            var role = {};
            role.RoleCode = selectedRoleValue;
            role.RoleName = selectedRoleText;
            lsm.saveUserRole(role);

            accountmanager.misValidated = true;

            $("#spnLogonUser").text(user.UserName);

            sfmain.ready();
            sfmain.initWfAppRunner();

            $("#modelLoginForm").modal("hide");
        } else {
            $.msgBox({
                title: "DynamicFlow / Login",
                content: "请选择角色和用户！",
                type: "alert"
            });
        }
    }

    return accountmanager;
})()

