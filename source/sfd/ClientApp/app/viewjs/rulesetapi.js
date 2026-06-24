const rulesetapi = (function () {
    function rulesetapi() {
    }

    function base() {
        return (window.kconfig && window.kconfig.webApiUrl) ? window.kconfig.webApiUrl : '';
    }

    rulesetapi.getList = function (callback) {
        jshelper.ajaxGet(base() + "api/RuleSet/GetList", null, callback);
    }

    rulesetapi.get = function (ruleSetCode, callback) {
        jshelper.ajaxGet(base() + "api/RuleSet/Get", { ruleSetCode: ruleSetCode }, callback);
    }

    rulesetapi.save = function (entity, callback, errFn) {
        jshelper.ajaxPost(base() + "api/RuleSet/Save", JSON.stringify(entity), callback, errFn);
    }

    rulesetapi.delete = function (ruleSetCode, callback) {
        $.ajax({
            url: base() + "api/RuleSet/Delete?ruleSetCode=" + encodeURIComponent(ruleSetCode || ''),
            type: 'DELETE',
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: callback
        });
    }

    return rulesetapi;
})()

window.rulesetapi = rulesetapi;
export default rulesetapi;

