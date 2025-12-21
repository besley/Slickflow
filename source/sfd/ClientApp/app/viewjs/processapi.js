import jshelper from '../script/jshelper.js'
import kconfig from '../config/kconfig.js'
import kmsgbox from '../script/kmsgbox.js';

//process api
const processapi = (function () {
    function processapi() {
    }

    processapi.getListSimple = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetProcessListSimple', null, function (result) {
            if (result.Status === 1) {
                if (callback !== null) {
                    callback(result);
                }
            } else {
                kmsgbox.error(kresource.getItem("processlisterrormsg"));
            }
        })
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
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/WfTrial/ExecuteProcessGraph',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    callback(result);
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

    processapi.updateUsingState = function (entity) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/UpdateProcessUsingState',
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
                    //kmsgbox.info(result.Message);
                    //refresh
                    processlist.getProcessList();
                } else {
                    kmsgbox.error(result.Message);
                }
            });
    }

    processapi.deleteById = function (id, callback) {
        //delete the selected row
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/DeleteProcessById/' + id,
            null,
            function (result) {
                callback(result);
            });
    }

    processapi.InitNewBPMNFile = function (callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/InitNewBPMNFile', null,
            function (result) {
                callback(result);
            }
        );
    }

    processapi.queryProcessFile = function (entity, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/GetProcessFile',
            JSON.stringify(query),
            function (result) {
                callback(result);
            }
        );
    }

    processapi.getFirstStepInfo = function (query, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/GetFirstActivity',
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

    processapi.queryProcessFileById = function (id, callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetProcessFile/' + id,
            null,
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

    processapi.createByTemplate = function (entity, isPopupMessage, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/CreateProcessByTemplate',
            JSON.stringify(entity),
            function (result) {
                if (result.Status === 1) {
                    if (isPopupMessage === true) {
                        //kmsgbox.info(result.Message);
                    } else {
                        kmsgbox.error(result.Message);
                    }
                }
                //execute render in processlist
                callback(result);
            });
    }

    processapi.loadTemplate = function (option, callback) {
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/LoadProcessTemplateCode/' + option, null, function (result) {
            if (result.Status === 1) {
                var template = result.Entity;
                //callback function
                callback(template);
            } else {
                kmsgbox.warn(result.Message);
            }
        });
    }

    //流程运行时接口
    processapi.start = function (query, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/StartProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.run = function (query, callback) {
        jshelper.ajaxPost(kconfig.webApiUrl + 'api/Wf2Xml/RunProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    return processapi;
})()

export default processapi