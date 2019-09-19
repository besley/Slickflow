/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。

The Slickflow Designer project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

var processlist = (function () {
	function processlist() {
	}

    processlist.pselectedProcessEntity = null;
    processlist.mprocessTemplateType = '';
    processlist.afterCreated = new slick.Event();
    processlist.afterOpened = new slick.Event();

    processlist.init = function () {
        kresource.localize();
    }

	//#region Process DataGrid
	processlist.getProcessList = function () {
		$('#loading-indicator').show();

        jshelper.ajaxPost('api/Wf2Xml/GetProcessListSimple', null, function (result) {
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
                        { headerName: kresource.getItem('status'), field: 'IsUsing', width: 80, cellRenderer: onIsUsingCellRenderer },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 220 },
                        { headerName: kresource.getItem('starttype'), field: 'StartType', width: 80, cellRenderer: onStartTypeCellRenderer },
                        { headerName: kresource.getItem('startexpression'), field: 'StartExpression', width: 140},
					],
					rowSelection: 'single',
					onSelectionChanged: onSelectionChanged,
					onRowDoubleClicked: onRowDoubleClicked
                };

                function onIsUsingCellRenderer(params) {
                    return params.value == 1 ? kresource.getItem('active') : kresource.getItem('unactive');
                }

                function onStartTypeCellRenderer(params) {
                    var startType = '';
                    if (params.value == 1)
                        startType = kresource.getItem('timer');
                    else if (params.value == 2)
                        startType = kresource.getItem('email');
                    return startType;
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
                    processlist.editProcess();
                }
            }
            else {
                $.msgBox({
                    title: "Designer / Process",
                    content: kresource.getItem("processlisterrormsg"),
                    type: "error"
                });
            }
		});
	}

	processlist.createProcess = function () {
        processlist.pselectedProcessEntity = null;
        processlist.mProcessEditDialog = BootstrapDialog.show({
            title: kresource.getItem('processcreate'),
            message: $('<div></div>').load('process/edit'),
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
		var entity = processlist.pselectedProcessEntity;
		if (entity == null) {
			$.msgBox({
                title: "Designer / Process",
                content: kresource.getItem('processselectedwarnmsg'),
				type: "alert"
			});
			return false;
		}

        BootstrapDialog.show({
            title: kresource.getItem('processedit'),
            message: $('<div></div>').load('process/edit'),
            draggable: true
		});
    }

    processlist.refreshProcess = function () {
        processlist.getProcessList();
    }

	processlist.saveProcess = function () {
		if ($("#txtProcessName").val() == ""
			|| $("#txtVersion").val() == "") {
					$.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processsavewarnmsg'),
						type: "alert"
					});
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
			processapi.create(entity, function (result) {
				if (result.Status == 1) {
                    processlist.pselectedProcessEntity = result.Entity;
                    //render process into graph canvas
					if (processlist.afterCreated) {
						slick.trigger(processlist.afterCreated, {
                            "ProcessEntity": result.Entity
                        });
                    }
                    if (processlist.mProcessEditDialog !== null) processlist.mProcessEditDialog.close();
				}
			});
		}
		else
			processapi.update(entity);
	}

	processlist.deleteProcess = function () {
		$.msgBox({
            title: "Are You Sure",
            content: kresource.getItem('processdeletemsg'),
			type: "confirm",
			buttons: [{ value: "Yes" }, { value: "Cancel" }],
			success: function (result) {
				if (result == "Yes") {
					var entity = {
						"ProcessGUID": processlist.pselectedProcessEntity.ProcessGUID,
						"Version": processlist.pselectedProcessEntity.Version
					};
					processapi.delete(entity);
					return;
				}
			}
		});
	}

	processlist.sure = function () {
		//render process into graph canvas
		if (processlist.pselectedProcessEntity !== null) {
            onDiagramOpen(processlist.pselectedProcessEntity);
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
                if (processlist.afterOpened) {
                    slick.trigger(processlist.afterOpened, {
                        "ProcessEntity": result.Entity
                    });
                }
            } else {
                $.msgBox({
                    title: "Designer / Process",
                    content: kresource.getItem('processopenerrormsg') + result.Message,
                    type: "error"
                });
            }
        });
    }

    //import xml file
    processlist.initXmlImport = function(){
        var restrictedUploader = new qq.FineUploader({
            element: document.getElementById("fine-uploader-validation"),
            template: 'qq-template-validation',
            request: {
                endpoint: 'api/FineUpload/import',
                params: {
                    extraParam1: "1",
                    extraParam2: "2"
                }
            },
            thumbnails: {
                placeholders: {
                    waitingPath: 'Content/fineuploader/waiting-generic.png',
                    notAvailablePath: 'Content/fineuploader/not_available-generic.png'
                }
            },
            validation: {
                allowedExtensions: ['xml', 'txt'],
                itemLimit: 1,
                sizeLimit: 51200 // 50 kB = 50 * 1024 bytes
            },
            callbacks: {
                onComplete: function (id, fileName, result) {
                    if (result.success === true) {
                        $.msgBox({
                            title: "Designer / Process",
                            content: kresource.getItem("processxmlimportokmsg"),
            			    type: "info",
            			    buttons: [{ value: "Ok" }],
            		    });
                    }
                    else {
            		    $.msgBox({
                            title: "Designer / Process",
                            content: kresource.getItem("processxmlimporterrormsg") + result.ExceptionMessage,
            			    type: "error",
            			    buttons: [{ value: "Ok" }],
            		    });
                    }
                }
            }
        });
    }

	return processlist;
})()

//process api
var processapi = (function () {
	function processapi() {
	}

	processapi.create = function (entity, callback) {
		jshelper.ajaxPost('api/Wf2Xml/CreateProcess',
            JSON.stringify(entity),
            function (result) {
            	if (result.Status == 1) {
            		$.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processsaveokmsg'),
            			type: "info"
            		});
            	} else {
            		$.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processsaveerrormsg') + result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}

            	//execute render in processlist
            	callback(result);
            });
    }

    processapi.createProcessGraph = function (entity, callback) {
        jshelper.ajaxPost('api/Wf2Xml/CreateProcessGraph',
            JSON.stringify(entity),
            function (result) {
                if (result.Status == 1) {
                    $.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processcreateokmsg'),
                        type: "info"
                    });
                } else {
                    $.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processcreateerrormsg') + result.Message,
                        type: "error",
                        buttons: [{ value: "Ok" }],
                    });
                }

                //execute render in processlist
                callback(result);
            });
    }

	processapi.update = function (entity) {
		jshelper.ajaxPost('api/Wf2Xml/UpdateProcess',
            JSON.stringify(entity),
            function (result) {
            	if (result.Status == 1) {
            		$.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processsaveokmsg'),
            			type: "info"
            		});
            	} else {
            		$.msgBox({
                        title: "Ooops",
                        content: kresource.getItem('processsaveerrormsg') + result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}
            });
	}

	processapi.delete = function (entity) {
		//delete the selected row
		jshelper.ajaxPost('api/Wf2Xml/DeleteProcess',
            JSON.stringify(entity),
            function (result) {
            	if (result.Status == 1) {
            		$.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processdeleteokmsg'),
            			type: "info"
            		});

            		//refresh
            		processlist.getProcessList();
            	} else {
            		$.msgBox({
                        title: "Ooops",
                        content: kresource.getItem('processdeleteerrormsg') + result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}
            });
	}

	processapi.queryProcessFile = function (query, callback) {
		jshelper.ajaxPost('api/Wf2Xml/QueryProcessFile',
            JSON.stringify(query),
            function (result) {
            	callback(result);
            }
        );
    }

    processapi.queryProcessFileByID = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/QueryProcessFileByID',
            JSON.stringify(query),
            function (result) {
                callback(result);
            }
        );
    }

	processapi.saveProcessFile = function (entity) {
		jshelper.ajaxPost('api/Wf2Xml/SaveProcessFile', JSON.stringify(entity), function (result) {
			if (result.Status == "1") {
				$.msgBox({
                    title: "Designer / Index",
                    content: kresource.getItem('processxmlsaveokmsg'),
					type: "info"
				});
			} else {
				$.msgBox({
                    title: "Designer / Index",
                    content: kresource.getItem('processxmlsaveerrormsg') + result.Message,
					type: "info"
				});
			}
		});
    }

    processapi.getSchedule = function () {

    }

	return processapi;
})()