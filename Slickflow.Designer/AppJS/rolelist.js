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
                        $("#role-list-controller").scope().selectedParticipantType = "role";
                        $("#role-list-controller").scope().selectedParticipantItem = row;
                    }
                });
            }
        });
    }
})()