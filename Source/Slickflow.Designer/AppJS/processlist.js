var processManager;
if (!processManager) processManager = {};

(function () {
    processManager.loadProcess = function (entity) {
        $("#txtProcessName").val(entity.ProcessName);
        $("#txtAppType").val(entity.AppType);
        $("#txtProcessFileName").val(entity.XmlFileName);
        $("#txtDescription").val(entity.Description);
    }

    processManager.createProcess = function () {
        var entity = {
            "ProcessName": $("#txtProcessName").val(),
            "XmlFileName": $("#txtProcessFileName").val(),
            "AppType": $("#txtAppType").val(),
            "Description": $("#txtDescription").val()
        };

        jshelper.ajaxPost('/SfApi/api/Wf2Xml/CreateProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status == 1) {
                    alert("流程成功创建！");

                    //refresh
                    $("#process-list-controller").scope().getProcessInfo();
                    $("#process-list-controller").scope().isSelectedNew = false;
                } else {
                    alert(result.Message);
                }
            });
    }

    processManager.updateProcess = function () {
        var processGUID = $("#process-list-controller").scope().selectedProcessGUID;
        var entity = {
            "ProcessGUID": processGUID,
            "ProcessName": $("#txtProcessName").val(),
            "XmlFileName": $("#txtProcessFileName").val(),
            "AppType": $("#txtAppType").val(),
            "Description": $("#txtDescription").val()
        };

        jshelper.ajaxPost('/SfApi/api/Wf2Xml/UpdateProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status == 1) {
                    alert("流程成功保存！")
                } else {
                    alert(result.Message);
                }
            });
    }

    processManager.deleteProcess = function () {
        var r = confirm("确实要删除流程定义记录吗？");
        if (r == true) {
            var processGUID = $("#process-list-controller").scope().selectedProcessGUID;
            var entity = {
                "ProcessGUID": processGUID
            };

            //delete the selected row
            jshelper.ajaxPost('/SfApi/api/Wf2Xml/DeleteProcess',
                JSON.stringify(entity),
                function (result) {
                    if (result.Status == 1) {
                        alert("流程记录已经删除！");

                        //refresh
                        $("#process-list-controller").scope().getProcessInfo();
                        $("#process-list-controller").scope().isSelectedNew = false;
                    } else {
                        alert(result.Message);
                    }
                });
        }
    }
})()