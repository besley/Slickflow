import jshelper from '../script/jshelper.js'
import kconfig from '../config/kconfig.js'
import kmsgbox from '../script/kmsgbox.js';

//process api
const axconfigapi = (function () {
    function axconfigapi() {
    }

    axconfigapi.getByProcessVersionActivity = function (processId, version, activityId, callback, errorCallback) {
        var url = kconfig.webApiUrl + 'api/Wf2Ai/GetAxConfig?processId=' + encodeURIComponent(processId) + '&version=' + encodeURIComponent(version) + '&activityId=' + encodeURIComponent(activityId);
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
    }

    axconfigapi.save = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Ai/SaveAxConfig',
            JSON.stringify(entity),
            function (result) {
                callback(result);
            });
    }

    axconfigapi.delete = function (processId, version, activityId, callback) {
        //delete the selected row
        var url = kconfig.webApiUrl + 'api/Wf2Ai/DeleteAxConfig?processId=' + encodeURIComponent(processId) + '&version=' + encodeURIComponent(version) + '&activityId=' + encodeURIComponent(activityId);
        jshelper.ajaxGet(url,
            null,
            function (result) {
                callback(result);
            });
    }

    axconfigapi.getModelList = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetModelList',
            null,
            function (result) {
                callback(result);
            });
    }

    axconfigapi.getEmbeddingModelList = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetModelListByType?modelType=vector_model',
            null,
            function (result) {
                callback(result);
            });
    }

    axconfigapi.getTableNames = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetTableNames',
            null,
            function (result) {
                callback(result);
            });
    }

    return axconfigapi;
})()

export default axconfigapi