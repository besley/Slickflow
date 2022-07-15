
//local storage maanger
var lsm;
if (!lsm) lsm = {};

(function () {

    lsm.getStorage = function (key) {
        localStorage.getItem(key);
    }

    lsm.saveStorage = function (key, item) {
        if (item !== null)
            localStorage.setItem(key, item)
    }

    lsm.deleteStorage = function (key) {
        localStorage.removeItem(key);
    }

    lsm.saveUserIdentity = function (user) {
        var item = JSON.stringify(user);
        if (item !== null && item !== '') {
            localStorage.setItem("slickflowuser", item);
        }
    }

    lsm.getUserIdentity = function () {
        var userStr = localStorage.getItem("slickflowuser");
        if (userStr !== null && userStr !== '')
            return JSON.parse(userStr);
        else
            return null;
    }

    lsm.removeUserIdentity = function () {
        localStorage.removeItem("slickflowuser");
    }

    lsm.saveUserAuthData = function (authData) {
        var item = JSON.stringify(authData);
        if (item !== null && item !== '') {
            localStorage.setItem("slickflowauth", item);
        }
    }

    lsm.getUserAuthData = function () {
        var authStr = localStorage.getItem("slickflowauth");
        if (authStr !== null && authStr !== '')
            return JSON.parse(authStr);
        else
            return null;
    }

    lsm.removeUserAuthData = function () {
        localStorage.removeItem("slickflowauth");
    }

    lsm.saveUserRole = function (role) {
        var item = JSON.stringify(role);
        if (item !== null && item !== '') {
            localStorage.setItem("slickflowuserrole", item);
        }
    }

    lsm.getUserRole = function () {
        var roleStr = localStorage.getItem("slickflowuserrole");
        if (roleStr !== null && roleStr !== '')
            return JSON.parse(roleStr);
        else
            return null;
    }

    lsm.removeUserRole = function () {
        localStorage.removeItem("slickflowuserrole");
    }

    lsm.removeTempStorage = function () {
        lsm.removeUserIdentity();
        lsm.removeUserAuthData();
        lsm.removeUserRole();
    }

    lsm.checkUserPermission = function (resourceCode) {
        var isPermitted = false;
        var resourceList = lsm.getUserAuthData();

        for (var i = 0; i < resourceList.length; i++) {
            if (resourceList[i].ResourceCode == resourceCode) {
                isPermitted = true;
                break;
            }
        }

        return isPermitted;
    }

})(lsm);

