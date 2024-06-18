import slick from '../script/slick.event.js'
import kconfig from '../config/kconfig.js';
window.slick = slick;

import ClipboardJS from '../script/clipboard.min.js'
window.ClipboardJS = ClipboardJS;

const processlist = (function () {
    function processlist() {
    }

    processlist.pselectedProcessEntity = null;
    processlist.mprocessTemplateType = '';
    processlist.afterCreated = new slick.Event();
    processlist.afterOpened = new slick.Event();
    processlist.diagramCreated = new slick.Event();

    processlist.getProcessList = function () {
        /* $('#loading-indicator').show();*/
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetProcessListSimple', null, function (result) {
            if (result.Status === 1) {
                var divProcessGrid = document.querySelector('#myProcessGrid');
                $(divProcessGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 50 },
                        { headerName: kresource.getItem('processguid'), field: 'ProcessGUID', width: 120 },
                        { headerName: kresource.getItem('processname'), field: 'ProcessName', width: 200 },
                        { headerName: kresource.getItem('version'), field: 'Version', width: 80 },
                        { headerName: kresource.getItem('processcode'), field: 'ProcessCode', width: 200 },
                        { headerName: kresource.getItem('packagetype'), field: 'PackageType', width: 80, cellRenderer: onPackageTypeRenderer },
                        { headerName: kresource.getItem('packageprocessid'), field: 'PackageProcessID', width: 80 },
                        { headerName: kresource.getItem('status'), field: 'IsUsing', width: 80, cellRenderer: onIsUsingCellRenderer },
                        { headerName: kresource.getItem('starttype'), field: 'StartType', width: 80, cellRenderer: onStartTypeCellRenderer },
                        { headerName: kresource.getItem('startexpression'), field: 'StartExpression', width: 140 },
                        { headerName: kresource.getItem('endtype'), field: 'EndType', width: 80, cellRenderer: onEndTypeCellRenderer },
                        { headerName: kresource.getItem('endexpression'), field: 'EndExpression', width: 140 },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 220 },
                    ],
                    rowSelection: 'single',
                    onSelectionChanged: onSelectionChanged,
                    onRowDoubleClicked: onRowDoubleClicked
                };

                function onIsUsingCellRenderer(params) {
                    return params.value === 1 ? kresource.getItem('active') : kresource.getItem('unactive');
                }

                function onPackageTypeRenderer(params) {
                    if (params.value === 1)
                        return kresource.getItem('mainprocess');
                    else if (params.value === 2)
                        return kresource.getItem('poolprocess');
                    else
                        return '';
                }

                function onStartTypeCellRenderer(params) {
                    var startType = '';
                    if (params.value === 1)
                        startType = kresource.getItem('timer');
                    else if (params.value === 2)
                        startType = kresource.getItem('message');
                    return startType;
                }

                function onEndTypeCellRenderer(params) {
                    var endType = '';
                    if (params.value === 1)
                        endType = kresource.getItem('timer');
                    else if (params.value === 2)
                        endType = kresource.getItem('message');
                    return endType;
                }

                new agGrid.Grid(divProcessGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridOptions.api.getSelectedRows();
                    var selectedProcessID = 0;
                    selectedRows.forEach(function (selectedRow, index) {
                        processlist.pselectedProcessEntity = selectedRow;
                    });
                }

                function onRowDoubleClicked(e, args) {
                    processlist.sure();
                }
            }
            else {
                kmsgbox.error(kresource.getItem("processlisterrormsg"));
            }
        });
    }

    processlist.createProcess = function () {
        processlist.pselectedProcessEntity = null;
        processlist.mProcessEditDialog = BootstrapDialog.show({
            message: $('<div id="popupContent-create"></div>'),
            title: kresource.getItem("processcreate"),
            onshown: function () {
                $("#popupContent-create").load('pages/process/edit.html')
            },
            draggable: true
        });
    }

    processlist.loadProcess = function () {
        var entity = processlist.pselectedProcessEntity;
        if (entity !== null) {
            $("#txtProcessGUID").val(entity.ProcessGUID);
            $("#txtProcessName").val(entity.ProcessName);
            $("#txtProcessCode").val(entity.ProcessCode);
            $("#txtVersion").val(entity.Version);
            $("#ddlIsUsing").val(entity.IsUsing);
            $("#txtDescription").val(entity.Description);
        } else {
            $("#txtProcessGUID").val("");
            $("#txtProcessName").val("");
            $("#txtProcessCode").val("");
            $("#txtVersion").val("1");
            $("#ddlIsUsing").val();
            $("#txtDescription").val("");
        }
    }

    processlist.editProcess = function () {
        var process = processlist.pselectedProcessEntity;
        if (process === null) {
            kmsgbox.warn(kresource.getItem('processselectedwarnmsg'));
            return false;
        } else {
            //泳道流程只能用主流程编辑
            if (process.PackageType === 2) {
                kmsgbox.warn(kresource.getItem('processpoolopendiagramwarnmsg'));
            } else {
                //单一流程编辑页面
                BootstrapDialog.show({
                    message: $('<div id="popupContent-edit"></div>'),
                    title: kresource.getItem("processedit"),
                    onshown: function () {
                        $("#popupContent-edit").load('pages/process/edit.html')
                    },
                    draggable: true
                });
            }
        }
    }

    //先拖动控件，在主界面创建流程图形，然后保存
    processlist.saveDiagram = function () {
        if ($.trim($("#txtProcessName").val()) === ""
            || $.trim($("#txtProcessCode").val()) === ""
            || $.trim($("#txtVersion").val()) === "") {
            kmsgbox.warn(kresource.getItem('processsavewarnmsg'));
            return false;
        }
        var entity = {
            "ProcessGUID": kmain.mxSelectedProcessEntity.ProcessGUID,
            "ProcessName": $("#txtProcessName").val(),
            "ProcessCode": $("#txtProcessCode").val(),
            "Version": $("#txtVersion").val(),
            "IsUsing": $("#ddlIsUsing").val(),
            "Description": $("#txtDescription").val()
        };

        processapi.checkProcessFile(entity, function (result) {
            if (result.Status === 1) {
                if (result.Entity === null) {
                    processapi.create(entity, false, function (result) {
                        if (result.Status === 1) {
                            processlist.pselectedProcessEntity = result.Entity;
                            //trigger diagram created event
                            slick.trigger(processlist.diagramCreated, {
                                "ProcessEntity": entity
                            });
                        }
                    });
                } else {
                    kmsgbox.warn(kresource.getItem('processsavenotuniquewarnmsg'));
                }
            } else {
                kmsgbox.error(kresource.getItem('processcheckfileexisterrormsg'), result.Message);
            }
        })
    }

    processlist.upgradeProcess = function () {
        var entity = processlist.pselectedProcessEntity;
        if (entity === null) {
            kmsgbox.warn(kresource.getItem('processselectedwarnmsg'));
            return false;
        } else {
            kmsgbox.confirm(kresource.getItem('processupgrademsg'), function () {
                var entity = {
                    "ID": processlist.pselectedProcessEntity.ID
                };
                processapi.upgrade(entity);
                return;
            });
        }
    }

    processlist.refreshProcess = function () {
        processlist.getProcessList();
    }

    processlist.saveProcess = function () {
        if ($.trim($("#txtProcessName").val()) === ""
            || $.trim($("#txtProcessCode").val()) === ""
            || $.trim($("#txtVersion").val()) === "") {
            kmsgbox.warn(kresource.getItem('processsavewarnmsg'));
            return false;
        }

        var entity = {
            "ProcessGUID": $("#txtProcessGUID").val(),
            "ProcessName": $("#txtProcessName").val(),
            "ProcessCode": $("#txtProcessCode").val(),
            "Version": $("#txtVersion").val(),
            "IsUsing": $("#ddlIsUsing").val(),
            "Description": $("#txtDescription").val(),
            "TemplateType": processlist.mprocessTemplateType === '' ? "Blank" : processlist.mprocessTemplateType
        };

        if (processlist.pselectedProcessEntity === null) {
            processapi.checkProcessFile(entity, function (result) {
                if (result.Status === 1) {
                    if (result.Entity === null) {
                        //create a new process
                        processapi.create(entity, true, function (result) {
                            if (result.Status === 1) {
                                processlist.pselectedProcessEntity = result.Entity;
                                kmain.mxSelectedProcessEntity = result.Entity;
                                //render process into graph canvas
                                if (processlist.afterCreated) {
                                    slick.trigger(processlist.afterCreated, {
                                        "ProcessEntity": result.Entity
                                    });
                                }
                                if (processlist.mProcessEditDialog !== null) processlist.mProcessEditDialog.close();
                            }
                        });
                    } else {
                        kmsgbox.warn(kresource.getItem('processsavenotuniquewarnmsg'));
                    }
                } else {
                    kmsgbox.error(kresource.getItem('processcheckfileexisterrormsg'), result.Message);
                }
            })
        }
        else
            processapi.update(entity);
    }

    processlist.deleteProcess = function () {
        var content = kresource.getItem('processdeletemsg');
        //var process = processlist.pselectedProcessEntity;
        //if (process.PackageType === 1) {
        //    content = kresource.getItem('processpooldeletemsg');
        //}
        kmsgbox.confirm(content, function () {
            var entity = {
                "ProcessGUID": processlist.pselectedProcessEntity.ProcessGUID,
                "Version": processlist.pselectedProcessEntity.Version
            };
            processapi.delete(entity);
            return;
        });
    }

    processlist.sure = function () {
        //render process into graph canvas
        var process = processlist.pselectedProcessEntity;
        if (process !== null) {
            // swimpool process is edit by main process
            if (process.PackageType === 2) {
                kmsgbox.warn(kresource.getItem('processpoolopendiagramwarnmsg'));
            } else {
                onDiagramOpen(processlist.pselectedProcessEntity);
            }
        }
    }

    //render process into graph canvas
    function onDiagramOpen(processEntity) {
        if (processlist.afterOpened) {
            slick.trigger(processlist.afterOpened, {
                "ProcessEntity": processEntity
            });
        }
    }

    //open a diagram
    processlist.openProcessDiagram = function (processID) {
        var query = {
            "ID": processID
        };

        processapi.queryProcessFileByID(query, function (result) {
            if (result.Status === 1) {
                var process = result.Entity;
                //泳道流程只能用主流程编辑
                if (process.PackageType === 2) {
                    kmsgbox.warn(kresource.getItem('processpoolopendiagramwarnmsg'));
                } else {
                    if (processlist.afterOpened) {
                        slick.trigger(processlist.afterOpened, {
                            "ProcessEntity": process
                        });
                    }
                }
            } else {
                kmsgbox.error(kresource.getItem('processopenerrormsg'), result.Message);
            }
        });
    }

    //show process xml content
    processlist.showXmlContent = function () {

        kmain.mBpmnModeler.saveXML({ format: true }).then(function (result) {
            if (result && result.xml) {
                var xmlContent = result.xml;
                if ($.isEmptyObject(xmlContent) === false) {
                    $("#txtXmlContent").val(xmlContent);
                    //copy xml to clipboard
                    copyToClipboard();
                }
                else {
                    kmsgbox.warn(kresource.getItem('xmlpreviewwarnmsg'));
                }
            } else {
                kmsgbox.warn(kresource.getItem('xmlprevieexceptionmsg'), result.message);
            }
        });
    }

    function copyToClipboard() {
        var clipboard = new ClipboardJS('#btnCopy');
        clipboard.on('success', function () {
            //kmsgbox.info(kresource.getItem('xmlcopytoclipboardok'));
        });
        clipboard.on('error', function () {
            kmsgbox.warn(kresource.getItem('xmlcopytoclipboardwarn'));
        });
    }

    //import xml file
    processlist.initXmlImport = function () {
        var restrictedUploader = new qq.FineUploader({
            element: document.getElementById("fine-uploader-validation"),
            template: 'qq-template-validation',
            request: {
                endpoint: kconfig.webApiUrl + 'api/FineUpload/import',
                params: {
                    extraParam1: "1",
                    extraParam2: "2"
                }
            },
            thumbnails: {
                placeholders: {
                    waitingPath: 'vendor/fine-uploader/fine-uploader/placeholders/waiting-generic.png',
                    notAvailablePath: 'vendor/fine-uploader/fine-uploader/placeholders/not_available-generic.png'
                }
            },
            validation: {
                allowedExtensions: ['xml', 'txt'],
                itemLimit: 1,
                sizeLimit: 102400 // 100 kB = 100 * 1024 bytes
            },
            callbacks: {
                onComplete: function (id, fileName, result) {
                    if (result.success === true) {
                        kmsgbox.info(kresource.getItem("processxmlimportokmsg"));
                    }
                    else {
                        kmsgbox.error(kresource.getItem("processxmlimporterrormsg"), result.Message);
                    }
                }
            }
        });
    }

    return processlist;
})()

export default processlist