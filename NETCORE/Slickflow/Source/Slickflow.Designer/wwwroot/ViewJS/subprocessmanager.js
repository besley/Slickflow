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

var subprocessmanager;
if (!subprocessmanager) subprocessmanager = {};

(function () {
    var msubprocessguid = null;
    var msubprocessname = null;

    subprocessmanager.load = function () {
        var activity = kmain.mxSelectedDomElement.Element;

        if (activity !== null && activity.subId !== "") {
            $("#txtProcessGUID").val(activity.subId);
            subprocessmanager.getProcess(activity.subId);
        }
        subprocessmanager.getProcessList();
    }

    //get process records
    subprocessmanager.getProcessList = function () {
        $("#spinner").show();
        jshelper.ajaxPost('api/Wf2Xml/GetProcessListSimple', null, function (result) {
        	if (result.Status === 1) {
        		var divProcessGrid = document.querySelector('#mySubProcessGrid');
        		var gridOptions = {
        			columnDefs: [
						{ headerName: 'ID', field: 'ID', width: 50 },
						{ headerName: '流程GUID', field: 'ProcessGUID', width: 120 },
						{ headerName: '流程名称', field: 'ProcessName', width: 160 },
						{ headerName: '版本', field: 'Version', width: 40 },
						{ headerName: '状态', field: 'IsUsing', width: 60 },
						{ headerName: '创建日期', field: 'CreatedDateTime', width: 120 }
        			],
        			rowSelection: 'single',
        			onSelectionChanged: onSelectionChanged,
        			onRowDoubleClicked: onRowDoubleClicked
        		};

        		new agGrid.Grid(divProcessGrid, gridOptions);
        		gridOptions.api.setRowData(result.Entity);

        		$('#loading-indicator').hide();

        		function onSelectionChanged() {
        			var selectedRows = gridOptions.api.getSelectedRows();
        			var selectedProcessID = 0;
        			selectedRows.forEach(function (selectedRow, index) {
        				selectedProcessID = selectedRow.ID;
        				msubprocessguid = selectedRow.ProcessGUID;      //marked and returned selected row info
        				msubprocessname = selectedRow.ProcessName;
        			});
        		}

        		function onRowDoubleClicked(e, args) {
        			//reattachSubProcess();
        		}
            }
            else {
                $.msgBox({
                    title: "Designer / SubProcess",
                    content: result.Message,
                    type: "error"
                });
            }
        });

        function datetimeFormatter(row, cell, value, columnDef, dataContext) {
            if (value != null && value != "") {
                return value.substring(0, 10);
            }
        }
    }

    subprocessmanager.getProcess = function (processGUID) {
        if (processGUID !== null 
            && processGUID !== undefined) {
            var query = { "ProcessGUID": processGUID };
            jshelper.ajaxPost('api/Wf2Xml/GetProcess', JSON.stringify(query), function (result) {
                if (result.Status == 1) {
                    var entity = result.Entity;

                    $("#txtProcessName").val(entity.ProcessName);
                }
            });
        }
    }

    subprocessmanager.saveSubProcess = function () {
    	reattachSubProcess();
    }

    function reattachSubProcess() {
    	$.msgBox({
    		title: "Are You Sure",
    		content: "请确认要将当前选中记录设置为子流程吗？！",
    		type: "confirm",
    		buttons: [{ value: "Yes" }, { value: "Cancel" }],
    		success: function (result) {
    			if (result == "Yes") {
    				$("#txtProcessGUID").val(msubprocessguid);
    				$("#txtProcessName").val(msubprocessname);
                   
                    var activity = kmain.mxSelectedDomElement.Element;
                    if (activity) {
                        activity.subId = msubprocessguid;
                        //update node user object
                        kmain.setVertexValue(activity);
                    } 
    				return;
    			}
    		}
    	});
    }

    return subprocessmanager;
})()