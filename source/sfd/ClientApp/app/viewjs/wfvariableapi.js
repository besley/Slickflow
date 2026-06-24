/**
 * 流程变量定义 API（wf_variable 表）
 * 与 api/WfVariable 控制器对应
 */
import jshelper from '../script/jshelper.js';
import kconfig from '../config/kconfig.js';

const wfvariableapi = (function () {
    function wfvariableapi() {}

    wfvariableapi.getList = function (processId, version, activityId, callback, errorCallback) {
        var url = kconfig.webApiUrl + 'api/WfVariable/GetList?processId=' + encodeURIComponent(processId || '') + '&version=' + encodeURIComponent(version || '1') + '&activityId=' + encodeURIComponent(activityId || '');
        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: function (result) {
                callback(result);
            },
            error: errorCallback || function (xhr, status, error) {
                var msg = (xhr && xhr.responseJSON && xhr.responseJSON.Message) ? xhr.responseJSON.Message : (error || status || 'Request failed');
                if (typeof callback === 'function') {
                    callback({ Status: -1, Message: msg });
                }
            }
        });
    };

    wfvariableapi.saveList = function (processId, version, activityId, list, callback, errorCallback) {
        var url = kconfig.webApiUrl + 'api/WfVariable/SaveList?processId=' + encodeURIComponent(processId || '') + '&version=' + encodeURIComponent(version || '1') + '&activityId=' + encodeURIComponent(activityId || '');
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify(list || []),
            success: function (result) {
                callback(result);
            },
            error: errorCallback || function (xhr, status, error) {
                var msg = (xhr && xhr.responseJSON && xhr.responseJSON.Message) ? xhr.responseJSON.Message : (error || status || 'Request failed');
                if (typeof callback === 'function') {
                    callback({ Status: -1, Message: msg });
                }
            }
        });
    };

    return wfvariableapi;
})();

export default wfvariableapi;
