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

var rolelist = (function () {
    function rolelist() {
    }

    var pselectedParticipantType = "";
    var pselectedParticipantItem = null;

    rolelist.getRoleList = function () {
        jshelper.ajaxPost("api/Wf2Xml/GetRoleAll", null, function (result) {
        	if (result.Status === 1) {
        		var divRoleGrid = document.querySelector('#myRoleGrid');
        		var gridOptions = {
        			columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 60 },
                        { headerName: kresource.getItem('rolename'), field: 'RoleName', width: 200 },
                        { headerName: kresource.getItem('rolecode'), field: 'RoleCode', width: 200 }
        			],
        			rowSelection: 'single',
        			onSelectionChanged: onSelectionChanged
        		};

        		new agGrid.Grid(divRoleGrid, gridOptions);
        		gridOptions.api.setRowData(result.Entity);

        		function onSelectionChanged() {
        			var selectedRows = gridOptions.api.getSelectedRows();
        			selectedRows.forEach(function (selectedRow, index) {
        				pselectedParticipantType = "role";
        				pselectedParticipantItem = selectedRow;
        			});
        		}
            }
        });
    }

    rolelist.sure = function () {
        if (pselectedParticipantType != ""
            && pselectedParticipantItem != null) {
            //sync activity performers
            activityproperty.syncActivityPerformers(pselectedParticipantType,
                pselectedParticipantItem);
        } else {
            kmsgbox.warn(kresource.getItem('rolelistselectwarnmsg'));
        }
    }
    return rolelist;
})()