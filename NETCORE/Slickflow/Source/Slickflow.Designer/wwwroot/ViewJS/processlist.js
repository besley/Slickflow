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
    processlist.diagramCreated = new slick.Event();

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
                    return params.value == 1 ? kresource.getItem('active') : kresource.getItem('unactive');
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
                    if (params.value == 1)
                        startType = kresource.getItem('timer');
                    else if (params.value == 2)
                        startType = kresource.getItem('message');
                    return startType;
                }

                function onEndTypeCellRenderer(params) {
                    var endType = '';
                    if (params.value == 1)
                        endType = kresource.getItem('timer');
                    else if (params.value == 2)
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
        var process = processlist.pselectedProcessEntity;
        if (process == null) {
            $.msgBox({
                title: "Designer / Process",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "alert"
            });
            return false;
        } else {
            //泳道流程只能用主流程编辑
            if (process.PackageType === 2) {
                $.msgBox({
                    title: "Designer / Process",
                    content: kresource.getItem('processpoolopendiagramwarnmsg'),
                    type: "warn"
                });
            } else {
                //单一流程编辑页面
                BootstrapDialog.show({
                    title: kresource.getItem('processedit'),
                    message: $('<div></div>').load('process/edit'),
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
            $.msgBox({
                title: "Designer / Process",
                content: kresource.getItem('processsavewarnmsg'),
                type: "alert"
            });
            return false;
        }

        var entity = {
            "ProcessGUID": jshelper.getUUID(),
            "ProcessName": $("#txtProcessName").val(),
            "ProcessCode": $("#txtProcessCode").val(),
            "Version": $("#txtVersion").val(),
            "IsUsing": $("#ddlIsUsing").val(),
            "Description": $("#txtDescription").val()
        };

        processapi.checkProcessFile(entity, function (result) {
            if (result.Status === 1) {
                if (result.Entity === null) {
                    if (processlist.diagramCreated) {
                        //把流程实体对象传递到主界面，然后进行XML保存操作
                        slick.trigger(processlist.diagramCreated, {
                            "ProcessEntity": entity
                        });
                    }
                } else {
                    $.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processsavenotuniquewarnmsg'),
                        type: "alert"
                    });
                }
            } else {
                $.msgBox({
                    title: "Designer / Process",
                    content: kresource.getItem('processsavewarnmsg'),
                    type: "alert"
                });
            }
        })
    }

    processlist.upgradeProcess = function () {
        var entity = processlist.pselectedProcessEntity;
        if (entity == null) {
            $.msgBox({
                title: "Designer / Process",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "alert"
            });
            return false;
        } else {
            $.msgBox({
                title: "Are You Sure",
                content: kresource.getItem('processupgrademsg'),
                type: "confirm",
                buttons: [{ value: "Yes" }, { value: "Cancel" }],
                success: function (result) {
                    if (result == "Yes") {
                        var entity = {
                            "ID": processlist.pselectedProcessEntity.ID
                        };
                        processapi.upgrade(entity);
                        return;
                    }
                }
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
        var content = kresource.getItem('processdeletemsg');
        var process = processlist.pselectedProcessEntity;
        if (process.PackageType === 1) {
            content = kresource.getItem('processpooldeletemsg');
        }

		$.msgBox({
            title: "Are You Sure",
            content: content,
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
        var process = processlist.pselectedProcessEntity;
        if (process !== null) {
            //泳道流程只能用主流程编辑
            if (process.PackageType === 2) {
                $.msgBox({
                    title: "Designer / Process",
                    content: kresource.getItem('processpoolopendiagramwarnmsg'),
                    type: "warn"
                });
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
                    $.msgBox({
                        title: "Designer / Process",
                        content: kresource.getItem('processpoolopendiagramwarnmsg'),
                        type: "warn"
                    });
                } else {
                    if (processlist.afterOpened) {
                        slick.trigger(processlist.afterOpened, {
                            "ProcessEntity": process
                        });
                    }
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

    //show process xml content
    processlist.showXmlContent = function () {
        var result = kloader.serialize2Xml(kmain.mxSelectedParticipants);
        if (result.status === 1) {
            var xmlContent = result.xmlContent;
            if ($.isEmptyObject(xmlContent) === false) {
                $("#txtXmlContent").val(xmlContent);
            }
            else {
                $.msgBox({
                    title: "Designer / Index",
                    content: kresource.getItem('xmlpreviewwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Designer / Index",
                content: kresource.getItem('xmlprevieexceptionmsg') + result.message,
                type: "warn"
            });
        }
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
                        content: result.Message,
            			type: "info"
            		});
            	} else {
            		$.msgBox({
                        title: "Designer / Process",
                        content: result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}

            	//execute render in processlist
            	callback(result);
            });
    }

    processapi.executeProcessGraph = function (entity, callback) {
        jshelper.ajaxPost('api/Wf2Xml/ExecuteProcessGraph',
            JSON.stringify(entity),
            function (result) {
                if (result.Status == 1) {
                    $.msgBox({
                        title: "Designer / Process",
                        content: result.Message,
                        type: "info"
                    });
                    callback(result.Entity);
                } else {
                    $.msgBox({
                        title: "Designer / Process",
                        content: result.Message,
                        type: "warn",
                        buttons: [{ value: "Ok" }],
                    });
                }
            });
    }

	processapi.update = function (entity) {
		jshelper.ajaxPost('api/Wf2Xml/UpdateProcess',
            JSON.stringify(entity),
            function (result) {
            	if (result.Status == 1) {
            		$.msgBox({
                        title: "Designer / Process",
                        content: result.Message,
            			type: "info"
            		});
            	} else {
            		$.msgBox({
                        title: "Ooops",
                        content: result.Message,
            			type: "error",
            			buttons: [{ value: "Ok" }],
            		});
            	}
            });
    }

    processapi.upgrade = function (entity) {
        //delete the selected row
        jshelper.ajaxPost('api/Wf2Xml/UpgradeProcess',
            JSON.stringify(entity),
            function (result) {
                if (result.Status == 1) {
                    $.msgBox({
                        title: "Designer / Process",
                        content: result.Message,
                        type: "info"
                    });

                    //refresh
                    processlist.getProcessList();
                } else {
                    $.msgBox({
                        title: "Ooops",
                        content: result.Message,
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
                        content: result.Message,
            			type: "info"
            		});

            		//refresh
            		processlist.getProcessList();
            	} else {
            		$.msgBox({
                        title: "Ooops",
                        content: result.Message,
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

    processapi.checkProcessFile = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/CheckProcessFile',
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
                    content: result.Message,
					type: "info"
				});
			} else {
				$.msgBox({
                    title: "Designer / Index",
                    content: result.Message,
					type: "info"
				});
			}
		});
    }

    processapi.saveProcessFilePool = function (entity) {
        jshelper.ajaxPost('api/Wf2Xml/SaveProcessFilePool', JSON.stringify(entity), function (result) {
            if (result.Status == "1") {
                $.msgBox({
                    title: "Designer / Index",
                    content: result.Message,
                    type: "info"
                });
            } else {
                $.msgBox({
                    title: "Designer / Index",
                    content: result.Message,
                    type: "info"
                });
            }
        });
    }

    processapi.loadTemplate = function (option) {
        var content = processmodel.mprocessTemplate[option];

        if (content !== undefined && content !== "") {
            processmodel.mcodemirrorEditor.setCode(content);
            //$("#txtCode").val(content);
            return;
        }

        jshelper.ajaxGet('api/Wf2Xml/LoadProcessTemplate/' + option, null, function (result) {
            if (result.Status == "1") {
                var template = result.Entity;
                //$("#txtCode").val(template.Content);
                processmodel.mcodemirrorEditor.setCode(template.Content);
                processmodel.mprocessTemplate[option] = template.Content;
            } else {
                $.msgBox({
                    title: "Process / Template",
                    content: result.Message,
                    type: "warn"
                });
            }
        });
    }
	return processapi;
})()