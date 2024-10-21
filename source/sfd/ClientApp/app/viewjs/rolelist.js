import kconfig from '../config/kconfig.js';

const rolelist = (function () {
    function rolelist() {
    }
    rolelist.afterPartakerSelected = new slick.Event();

    var pselectedPartakerType = "";
    var pselectedPartakerItem = null;

    rolelist.mxElement = null;
    rolelist.mxBpmnFactory = null;
    rolelist.mxCommandStack = null;


    rolelist.getRoleList = function () {
        jshelper.ajaxPost(kconfig.webApiUrl + "api/Wf2Xml/GetRoleAll", null, function (result) {
        	if (result.Status === 1) {
        		var divRoleGrid = document.querySelector('#myRoleGrid');
        		var gridOptions = {
        			columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 60 },
                        { headerName: kresource.getItem('rolename'), field: 'RoleName', width: 200 },
                        { headerName: kresource.getItem('rolecode'), field: 'RoleCode', width: 200 }
        			],
        			rowSelection: 'single',
        			onSelectionChanged: onSelectionChanged,
                    onRowDoubleClicked: onRowDoubleClicked
        		};

        		new agGrid.Grid(divRoleGrid, gridOptions);
        		gridOptions.api.setRowData(result.Entity);

        		function onSelectionChanged() {
        			var selectedRows = gridOptions.api.getSelectedRows();
        			selectedRows.forEach(function (selectedRow, index) {
        				pselectedPartakerType = "Role";
        				pselectedPartakerItem = selectedRow;
        			});
                }

                function onRowDoubleClicked(e, args) {
                    rolelist.sure();
                }
            }
        });
    }

    rolelist.sure = function () {
        if (pselectedPartakerType != ""
            && pselectedPartakerItem != null) {
            slick.trigger(rolelist.afterPartakerSelected, {
                "PartakerType": pselectedPartakerType,
                "PartakerItem": pselectedPartakerItem
            });
        } else {
            kmsgbox.warn(kresource.getItem('rolelistselectwarnmsg'));
        }
    }
    return rolelist;
})()

export default rolelist