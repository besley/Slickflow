/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
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

        jshelper.ajaxGet('api/Wf2Xml/GetProcessListSimple', null, function (result) {
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
            jshelper.ajaxGet('api/Wf2Xml/GetProcess/' + processGUID, null, function (result) {
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