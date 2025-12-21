import jshelper from '../script/jshelper.js'
import kconfig from '../config/kconfig.js'
import kmsgbox from '../script/kmsgbox.js';

//process api
const agentapi = (function () {
    function agentapi() {
    }

    agentapi.getById = function (id, callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetAgent/' + id, null, function (result) {
            callback(result);
        })
    }

    agentapi.getListSimple = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetAgentListSimple', null, function (result) {
            if (result.Status === 1) {
                if (callback !== null) {
                    callback(result);
                }
            } else {
                kmsgbox.error(kresource.getItem("agentlisterrormsg"));
            }
        })
    }

    agentapi.save = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Ai/SaveAgent',
            JSON.stringify(entity),
            function (result) {
                callback(result);
            });
    }

    agentapi.upgrade = function (entity) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Ai/UpgradeAgent',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(result.Message);
                    //refresh
                    agentlist.getAgentList();
                } else {
                    kmsgbox.error(result.Message);
                }
            });
    }

    agentapi.delete = function (id, callback) {
        //delete the selected row
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/DeleteAgent/' + id,
            null,
            function (result) {
                callback(result);
            });
    }

    return agentapi;
})()

export default agentapi