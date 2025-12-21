import jshelper from '../script/jshelper.js'
import kconfig from '../config/kconfig.js'
import kmsgbox from '../script/kmsgbox.js';

//process api
const axconfigapi = (function () {
    function axconfigapi() {
    }

    axconfigapi.getByUUID = function (uuid, callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/GetAxConfig/' + uuid,
            null,
            function (result) {
                callback(result);
            })
    }

    axconfigapi.save = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Ai/SaveAxConfig',
            JSON.stringify(entity),
            function (result) {
                callback(result);
            });
    }

    axconfigapi.delete = function (uuid, callback) {
        //delete the selected row
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Ai/DeleteAxConfig/' + uuid,
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

    return axconfigapi;
})()

export default axconfigapi