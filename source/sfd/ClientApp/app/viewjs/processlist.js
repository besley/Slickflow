﻿import slick from '../script/slick.event.js'
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
    processlist.afterSubProcessSelected = new slick.Event();

    processlist.getProcessList = function () {
        /* $('#loading-indicator').show();*/
        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetProcessListSimple', null, function (result) {
            if (result.Status === 1) {
                var divProcessGrid = document.querySelector('#myProcessGrid');
                $(divProcessGrid).empty();

                var gridOptions = {
                    theme: themeBalham,
                    columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 80 },
                        { headerName: kresource.getItem('processid'), field: 'ProcessID', width: 240 },
                        { headerName: kresource.getItem('processname'), field: 'ProcessName', width: 200 },
                        { headerName: kresource.getItem('version'), field: 'Version', width: 80 },
                        { headerName: kresource.getItem('processcode'), field: 'ProcessCode', width: 200 },
                        { headerName: kresource.getItem('packagetype'), field: 'PackageType', width: 80, cellRenderer: onPackageTypeRenderer },
                        { headerName: kresource.getItem('packageid'), field: 'PackageID', width: 80 },
                        { headerName: kresource.getItem('status'), field: 'IsUsing', width: 80, cellRenderer: onIsUsingCellRenderer },
                        { headerName: kresource.getItem('starttype'), field: 'StartType', width: 80, cellRenderer: onStartTypeCellRenderer },
                        { headerName: kresource.getItem('startexpression'), field: 'StartExpression', width: 140 },
                        { headerName: kresource.getItem('endtype'), field: 'EndType', width: 80, cellRenderer: onEndTypeCellRenderer },
                        { headerName: kresource.getItem('endexpression'), field: 'EndExpression', width: 140 },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 220 },
                    ],
                    rowSelection: {
                        mode: 'singleRow',
                        checkboxes: false,
                        enableClickSelection: true
                    },
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

                gridOptions.rowData = result.Entity;
                const gridApi = createGrid(divProcessGrid, gridOptions);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridApi.getSelectedRows();

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
            $("#txtProcessID").val(entity.ProcessID);
            $("#txtProcessName").val(entity.ProcessName);
            $("#txtProcessCode").val(entity.ProcessCode);
            $("#txtVersion").val(entity.Version);
            $("#ddlIsUsing").val(entity.IsUsing);
            $("#txtDescription").val(entity.Description);
        } else {
            var strProcessGUID = jshelper.getUUID();
            $("#txtProcessGUID").val(strProcessGUID);
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
            //The lane process can only be edited using the main process
            if (process.PackageType === 2) {
                kmsgbox.warn(kresource.getItem('processpoolopendiagramwarnmsg'));
            } else {
                //单一流程编辑页面
                //Single process editing page
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
    //First, drag the control to create a process graphic on the main interface, and then save it
    processlist.saveDiagram = function () {
        if ($.trim($("#txtProcessName").val()) === ""
            || $.trim($("#txtProcessCode").val()) === ""
            || $.trim($("#txtVersion").val()) === "") {
            kmsgbox.warn(kresource.getItem('processsavewarnmsg'));
            return false;
        }
        var entity = {
            "ProcessID": kmain.mxSelectedProcessEntity.ProcessID,
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

    processlist.updateUsingState = function () {
        var entity = processlist.pselectedProcessEntity;
        if (entity === null) {
            kmsgbox.warn(kresource.getItem('processselectedwarnmsg'));
            return false;
        } else {
            var msgConfirm = '';
            var usingState = 0;
            if (processlist.pselectedProcessEntity.IsUsing === 0) {
                usingState = 1;
                msgConfirm = kresource.getItem('processusingenablemsg');
            } else {
                usingState = 0;
                msgConfirm = kresource.getItem('processusingunablemsg');
            }

            kmsgbox.confirm(msgConfirm, function () {
                var entity = {
                    "ProcessID": processlist.pselectedProcessEntity.ProcessID,
                    "Version": processlist.pselectedProcessEntity.Version,
                    "IsUsing": usingState
                };
                processapi.updateUsingState(entity);
                return;
            });
        }
    }

    processlist.refreshProcess = function () {
        processlist.getProcessList();
    }

    processlist.createFromTemplate = function () {
        if ($.trim($("#txtProcessName").val()) === ""
            || $.trim($("#txtProcessCode").val()) === ""
            || $.trim($("#txtVersion").val()) === "") {
            kmsgbox.warn(kresource.getItem('processsavewarnmsg'));
            return false;
        }

        var entity = {
            "ProcessID": $("#txtProcessID").val(),
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
                                if (processlist.afterOpened) {
                                    slick.trigger(processlist.afterOpened, {
                                        "ProcessEntity": result.Entity
                                    });
                                }

                                if (kmain.templateDialog !== undefined) kmain.templateDialog.close();
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
        var process = processlist.pselectedProcessEntity;
        if (process.PackageType === 1) {
            content = kresource.getItem('processpooldeletemsg');
        }

        kmsgbox.confirm(content, function () {
            var entity = {
                "ProcessID": processlist.pselectedProcessEntity.ProcessID,
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
                onDiagramOpen(process);
            }
        }
    }

    //sub process selected
    processlist.subSure = function () {
        var process = processlist.pselectedProcessEntity;
        if (process !== null) {
            if (processlist.afterSubProcessSelected) {
                slick.trigger(processlist.afterSubProcessSelected, {
                    "ProcessEntity": process
                });
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
                allowedExtensions: ['xml', 'txt', 'bpmn', 'bpmn2', 'xpdl'],
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

    //import xml file
    processlist.initMxGraphXmlImport = function () {
        var restrictedUploader = new qq.FineUploader({
            element: document.getElementById("fine-uploader-validation"),
            template: 'qq-template-validation',
            request: {
                endpoint: kconfig.webApiUrl + 'api/FineUploadMxGrp/import',
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
                allowedExtensions: ['xml', 'txt', 'bpmn', 'bpmn2', 'xpdl'],
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