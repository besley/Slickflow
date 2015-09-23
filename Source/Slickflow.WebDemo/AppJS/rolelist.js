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

var roleManager;
if (!roleManager) roleManager = {};

(function () {
    roleManager.getRoleList = function () {
        var url = "/SfApi/api/Wf2Xml/GetRoleAll";
        jshelper.ajaxGet(url, null, function (result) {
            if (result.Status == 1) {
                var list = result.Entity;
                var columnRole = [
                    { id: "id", name: "id", field: "ID", width: 60, cssClass: "bg-gray" },
                    { id: "RoleName", name: "角色名称", field: "RoleName", width: 200, cssClass: "bg-gray" },
                    { id: "RoleCode", name: "角色代码", field: "RoleCode", width: 200, cssClass: "bg-gray" }
                ];

                var optionsRole = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: true,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsRole = result.Entity;

                var dvRole = new Slick.Data.DataView({ inlineFilters: true });
                var gridRole = new Slick.Grid("#myGridRole", dvRole, columnRole, optionsRole);

                dvRole.onRowsChanged.subscribe(function (e, args) {
                    gridRole.invalidateRows(args.rows);
                    gridRole.render();
                });

                dvRole.onRowCountChanged.subscribe(function (e, args) {
                    gridRole.updateRowCount();
                    gridRole.render();
                });

                dvRole.beginUpdate();
                dvRole.setItems(dsRole, "ID");
                gridRole.setSelectionModel(new Slick.RowSelectionModel());
                dvRole.endUpdate();

                //rows change event
                gridRole.onSelectedRowsChanged.subscribe(function (e, args) {
                    var selectedRowIndex = args.rows[0];
                    var row = dvRole.getItemByIdx(selectedRowIndex);
                    if (row) {
                        kmain.selectedParticipantType = "role";
                        kmain.selectedParticipantItem = row;
                    }
                });
            }
        });
    }
})()