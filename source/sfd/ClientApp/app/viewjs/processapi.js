import jshelper from '../script/jshelper.js'
import kconfig from '../config/kconfig.js'

//process api
const processapi = (function () {
    function processapi() {
    }

    processapi.create = function (entity, isPopupMessage, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/CreateProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    if (isPopupMessage === true) {
                        kmsgbox.info(result.Message);
                    }
                } else {
                    kmsgbox.error(result.Message);
                }
                //execute render in processlist
                callback(result);
            });
    }

    processapi.executeProcessGraph = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/ExecuteProcessGraph',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(result.Message);
                    callback(result.Entity);
                } else {
                    kmsgbox.warn(result.Message);
                }
            });
    }

    processapi.update = function (entity) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/UpdateProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(result.Message);
                } else {
                    kmsgbox.error(result.Message);
                }
            });
    }

    processapi.upgrade = function (entity) {
        //delete the selected row
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/UpgradeProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(result.Message);
                    //refresh
                    processlist.getProcessList();
                } else {
                    kmsgbox.error(result.Message);
                }
            });
    }

    processapi.delete = function (entity) {
        //delete the selected row
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/DeleteProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(result.Message);
                    //refresh
                    processlist.getProcessList();
                } else {
                    kmsgbox.error(result.Message);
                }
            });
    }

    processapi.InitNewBPMNFile = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/InitNewBPMNFile', null,
            function (result) {
                callback(result);
            }
        );
    }

    processapi.queryProcessFile = function (query, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/QueryProcessFile',
            JSON.stringify(query),
            function (result) {
                callback(result);
            }
        );
    }

    processapi.checkProcessFile = function (query, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/CheckProcessFile',
            JSON.stringify(query),
            function (result) {
                callback(result);
            }
        );
    }

    processapi.queryProcessFileByID = function (query, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/QueryProcessFileByID',
            JSON.stringify(query),
            function (result) {
                callback(result);
            }
        );
    }

    processapi.saveProcessFile = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/SaveProcessFile', JSON.stringify(entity), function (result) {
            callback(result);
        });
    }

    processapi.saveProcessFilePool = function (entity) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/SaveProcessFilePool', JSON.stringify(entity), function (result) {
            if (result.Status === 1) {
                kmsgbox.info(result.Message);
            } else {
                kmsgbox.error(result.Message);
            }
        });
    }

    processapi.loadTemplate = function (option) {
        var content = processmodel.mprocessTemplate[option];

        if (content !== undefined && content !== "") {
            processmodel.mcodemirrorEditor.setValue(content);
            //$("#txtCode").val(content);
            return;
        }

        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/LoadProcessTemplate/' + option, null, function (result) {
            if (result.Status === 1) {
                var template = result.Entity;
                //$("#txtCode").val(template.Content);
                processmodel.mcodemirrorEditor.setValue(template.Content);
                processmodel.mprocessTemplate[option] = template.Content;
            } else {
                kmsgbox.warn(result.Message);
            }
        });
    }
    return processapi;
})()

export default processapi