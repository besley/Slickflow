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

var rolelist = (function () {
    function rolelist() {
    }

    var pselectedParticipantType = "";
    var pselectedParticipantItem = null;

    rolelist.getRoleList = function () {
        var url = "api/Wf2Xml/GetRoleAll";
        jshelper.ajaxGet(url, null, function (result) {
        	if (result.Status == 1) {
        		var divRoleGrid = document.querySelector('#myRoleGrid');
        		var gridOptions = {
        			columnDefs: [
						{ headerName: 'ID', field: 'ID', width: 60 },
						{ headerName: '角色名称', field: 'RoleName', width: 200 },
						{ headerName: '角色代码', field: 'RoleCode', width: 200 }
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
            $.msgBox({
                title: "Designer / Role",
                content: "请选择角色记录！",
                type: "alert"
            });
        }
    }
    return rolelist;
})()