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

    //get process records
    subprocessmanager.getProcessList = function () {
        $("#spinner").show();

        jshelper.ajaxGet('/SfApi/api/Wf2Xml/GetProcessList', null, function (result) {
            if (result.Status === 1) {
                var columnProcess = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "ProcessGUID", name: "流程GUID", field: "ProcessGUID", width: 120, cssClass: "bg-gray" },
                    { id: "ProcessName", name: "流程名称", field: "ProcessName", width: 160, cssClass: "bg-gray" },
                    { id: "AppType", name: "App标识", field: "AppType", width: 120, cssClass: "bg-gray" },
                    { id: "XmlFilePath", name: "文件路径", field: "XmlFilePath", width: 200, cssClass: "bg-gray" }
                ];

                var optionsProcess = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: true,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsProcess = result.Entity;

                var dvProcess = new Slick.Data.DataView({ inlineFilters: true });
                var gridProcess = new Slick.Grid("#myGridSubProcess", dvProcess, columnProcess, optionsProcess);

                dvProcess.onRowsChanged.subscribe(function (e, args) {
                    gridProcess.invalidateRows(args.rows);
                    gridProcess.render();
                });

                dvProcess.onRowCountChanged.subscribe(function (e, args) {
                    gridProcess.updateRowCount();
                    gridProcess.render();
                });

                dvProcess.beginUpdate();
                dvProcess.setItems(dsProcess, "ID");
                gridProcess.setSelectionModel(new Slick.RowSelectionModel());
                dvProcess.endUpdate();

                //rows change event
                gridProcess.onSelectedRowsChanged.subscribe(function (e, args) {
                    var selectedRowIndex = args.rows[0];
                    var row = dvProcess.getItemByIdx(selectedRowIndex);
                    if (row) {
                        msubprocessguid = row.ProcessGUID;      //marked and returned selected row info
                        msubprocessname = row.ProcessName;
                    }
                });
            }

            $("#spinner").hide();
        });
    }

    subprocessmanager.getProcess = function (processGUID) {
        if (processGUID !== null 
            && processGUID !== "undefined") {
            jshelper.ajaxGet('/SfApi/api/Wf2Xml/GetProcess/' + processGUID, null, function (result) {
                if (result.Status == 1) {
                    var entity = result.Entity;

                    $("#txtProcessName").val(entity.ProcessName);
                }
            });
        }
    }

    subprocessmanager.changeSubProcess = function () {
        $.msgBox({
            title: "Are You Sure",
            content: "请确认要将当前选中记录设置为子流程吗？！",
            type: "confirm",
            buttons: [{ value: "Yes" }, { value: "Cancel" }],
            success: function (result) {
                if (result == "Yes") {
                    $("#txtProcessGUID").val(msubprocessguid);
                    $("#txtProcessName").val(msubprocessname);

                    return;
                }
            }
        });
    }

    subprocessmanager.saveSubProcess = function () {
        var node = window.parent.$("#divActivityDialog").data("node");
        node.sdata.subid = msubprocessguid;

        window.parent.$('#divActivityDialog').dialog('close');
    }

})()