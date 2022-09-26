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

var kvariable = (function () {
    function kvariable() {
    }

    kvariable.getVariableList = function () {
        $('#loading-indicator').show();

        var query = { "ProcessInstanceID": processlist.pselectedTaskEntity.ProcessInstanceID };
        jshelper.ajaxPost('api/Wf2Xml/GetProcessVariableList', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var divProcessVariableGrid = document.querySelector('#myProcessVariableGrid');
                $(divProcessVariableGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 50 },
                        { headerName: kresource.getItem('appinstanceid'), field: 'AppInstanceID', width: 100 },
                        { headerName: kresource.getItem('variabletype'), field: 'VariableType', width: 100 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 100 },
                        { headerName: kresource.getItem('variablename'), field: 'Name', width: 120 },
                        { headerName: kresource.getItem('variablevalue'), field: 'Value', width: 200 },
                        { headerName: kresource.getItem('lastupdateddatetime'), field: 'LastUpdatedDateTime', width: 120 },
                    ],
                    rowSelection: 'single',
                    onSelectionChanged: onSelectionChanged,
                    onRowDoubleClicked: onRowDoubleClicked
                };


                new agGrid.Grid(divProcessVariableGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridOptions.api.getSelectedRows();
                    selectedRows.forEach(function (selectedRow, index) {
                        kvariable.pselectedProcessVariableEntity = selectedRow;
                        $("#txtVariableNameUpdate").val(selectedRow.Name);
                        $("#txtVariableValueUpdate").val(selectedRow.Value);
                    });
                }

                function onRowDoubleClicked(e, args) {
                    kvariable.editProcessVariable();
                }
            }
            else {
                kmsgbox.error(kresource.getItem("processvariablelistloaderrormsg"), result.Message);
            }
        });
    }

    kvariable.load = function () {
        var entity = kvariable.pselectedProcessVariableEntity;
        if (entity !== null) {
            $("#txtVariableName").val(entity.Name);
            $("#txtVariableValue").val(entity.Value);
            $("#ddlVariableType").val(entity.VariableType);
        }
    }

    kvariable.insert = function () {
        kvariable.pselectedProcessVariableEntity = null;
        var entity = processlist.pselectedTaskEntity;
        if (entity !== null) {
            BootstrapDialog.show({
                title: kresource.getItem('edit'),
                message: $('<div></div>').load('variable/edit'),
                draggable: true
            });
        } else {
            kmsgbox.warn(kresource.getItem('processvariableopenmsg'));
            return false;
        }
    }

    kvariable.update = function () {
        var entity = processlist.pselectedTaskEntity;
        if (entity !== null) {
            BootstrapDialog.show({
                title: kresource.getItem('edit'),
                message: $('<div></div>').load('variable/edit'),
                draggable: true
            });
        } else {
            kmsgbox.warn(kresource.getItem('processvariableopenmsg'));
            return false;
        }
    }

    kvariable.save = function () {
        var name = $("#txtVariableName").val();
        var value = $("#txtVariableValue").val();
        var variableType = $("#ddlVariableType").val();
        var entity = {
            "VariableType": variableType,
            "Name": name,
            "Value": value
        };

        if (kvariable.pselectedProcessVariableEntity === null) {
            entity["TaskID"] = processlist.pselectedTaskEntity.TaskID;

            jshelper.ajaxPost('api/Wf2Xml/InsertProcessVariable',
                JSON.stringify(entity),
                function (result) {
                    if (result.Status === 1) {
                        kmsgbox.info(kresource.getItem('processvariablesaveokmsg'));
                        kvariable.getVariableList();
                    } else {
                        kmsgbox.error(kresource.getItem('processvariablesaveerrormsg'), result.Message);
                    }
                });
        } else {
            entity["ID"] = kvariable.pselectedProcessVariableEntity.ID;
            jshelper.ajaxPost('api/Wf2Xml/UpdateProcessVariable',
                JSON.stringify(entity),
                function (result) {
                    if (result.Status === 1) {
                        kmsgbox.info(kresource.getItem('processvariablesaveokmsg'));
                        kvariable.getVariableList();
                    } else {
                        kmsgbox.error(kresource.getItem('processvariablesaveerrormsg'), result.Message);
                    }
                });
        }
    }

    kvariable.delete = function () {
        kmsgbox.confirm(kresource.getItem('processvariabledeletemsg'), function (result) {
            if (result === "Yes") {
                var entity = kvariable.pselectedProcessVariableEntity;
                if (entity !== null) {
                    jshelper.ajaxPost('api/Wf2Xml/DeleteProcessVariable',
                        JSON.stringify(entity),
                        function (result) {
                            if (result.Status === 1) {
                                kmsgbox.info(kresource.getItem('processvariabledeleteokmsg'));
                                kvariable.getVariableList();
                            } else {
                                kmsgbox.error(kresource.getItem('processvariabledeleteerrormsg'), result.Message);
                            }
                        });
                }
                return;
            }
        });
    }
    return kvariable;
})();