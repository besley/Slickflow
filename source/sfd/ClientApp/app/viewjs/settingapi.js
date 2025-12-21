import jshelper from '../script/jshelper.js'
import kconfig from '../config/kconfig.js'
import kmsgbox from '../script/kmsgbox.js';

// AI model setting API
const settingapi = (function () {
    function settingapi() {
    }

    // Get all AI model configurations
    settingapi.getList = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetModelList',
            null,
            function (result) {
                callback(result);
            })
    }

    // Get AI model configuration by Id
    settingapi.getById = function (id, callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetModel/' + id,
            null,
            function (result) {
                callback(result);
            })
    }

    // Save or update AI model configuration
    settingapi.save = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Ai/SaveModel',
            JSON.stringify(entity),
            function (result) {
                callback(result);
            });
    }

    // Delete AI model configuration
    settingapi.delete = function (id, callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/DeleteModel/' + id,
            null,
            function (result) {
                callback(result);
            });
    }

    // Test model connection
    settingapi.testConnection = function (baseUrl, apiKey, modelProvider, apiUUID, callback) {
        var request = {
            BaseUrl: baseUrl,
            ApiKey: apiKey,
            ModelProvider: modelProvider,
            ApiUUID: apiUUID || null
        };
        jshelper.ajaxPost(
            kconfig.webApiUrl + 'api/Wf2Ai/TestModelConnection',
            JSON.stringify(request),
            function (result) {
                callback(result);
            },
            function (xhr, status, error) {
                callback({
                    Status: 0,
                    Success: false,
                    Message: 'Connection test failed: ' + (xhr?.responseText || error || status || 'Unknown error')
                });
            });
    }

    return settingapi;
})()

export default settingapi

