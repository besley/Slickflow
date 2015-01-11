var app = angular.module("canvasApp", ['kGraphApp', 'processApp']);

app.service('prompt', function () {
    return prompt;
});


app.controller('canvasCtrl', ['$scope', 'prompt', 'processAPIService',
    function canvasCtrl($scope, prompt, processAPI) {

    var deleteKeyCode = 46;
    var ctrlKeyCode = 65;
    var ctrlDown = false;
    var aKeyCode = 17;
    var escKeyCode = 27;
    var nextNodeID = 10;
    $scope.processXMLContent = null;
    $scope.selectedProcessGUID = null;
    $scope.selectedProcessRecord = null;
    $scope.currentPackageData = null;
    $scope.isSelectedNew = false;

    //#region UI mouse and key event
    $scope.loadSvgPanel = function () {
        //window.console.log("svg panel load...");
    }

    $scope.mouseDown = function (evt) {
        //window.console.log("mouse down...");
    }

    $scope.mouseMove = function (evt) {
        //window.console.log("mouse move...");
    }

    $scope.mouseUp = function (evt) {
        //window.console.log("mouse up...");
    }

    $scope.keyDown = function (evt) {
        if (evt.keyCode === ctrlKeyCode) {
            ctrlDown = true;
            evt.stopPropagation();
            evt.preventDefault();
        }
    }

    $scope.keyUp = function (evt) {
        if (evt.keyCode === deleteKeyCode) {
            $scope.graphView.deleteSelected();
        }

        if (evt.keyCode == aKeyCode && ctrlDown) {
            $scope.graphView.selectAll();
        }

        if (evt.keyCode === escKeyCode) {
            $scope.graphView.deselectAll();
        }

        if (evt.keyCode === ctrlKeyCode) {
            ctrlDown = false;
            evt.stopPropagation();
            evt.preventDefault();
        }
    }
    //#endregion

    $scope.deleteSelected = function () {
        $scope.graphView.deleteSelected();
    }

    $scope.hello = function () {
        alert("hi, how are you!");
    }

    $scope.onSureClick = function () {
        alert("line dialog form");
    }

    //#region process dialog
    $scope.loadProcess = function () {
        $scope.isSelectedNew = false;
        $scope.selectedProcessGUID = null;
        $scope.selectedProcessRecord = null;

        var processDialog = $("#divProcessDialog")
            .load("/sfd/views/processlist.html",
            function () {
                var dialogOptions = {
                    title: "流程定义数据",
                    width: 700,
                    height: 500,
                    modal: true,
                    autoOpen: false,
                    beforeClose: function (evt, ui) {
                        var processGUID = $scope.selectedProcessGUID;
                        if ($scope.isSelectedNew && processGUID) {
                            //load process xml file
                            processAPI.getProcessFile(processGUID)
                                .success(function (result){
                                    if (result.Status == 1) {
                                        var processFileEntity = result.Entity;
                                        $scope.graphView = krender.load(processFileEntity);
                                        $scope.currentPackageData = $scope.graphView.packageData;
                                    } else {
                                        alert(result.Message);
                                    }
                                });
                        }
                    },
                    close: function (event, ui) {
                        $(this).children().remove();
                        $(this).dialog("destroy");
                    }
                };

                processDialog
                    .dialog(dialogOptions)
                    .dialog('open');
            }
        );
    }
    //#endregion

    //#region Process DataGrid
    $scope.getProcessInfo = function () {
        $("#spinner").show();

        //get wfprocess records
        processAPI.getProcess().success(function (result) {
            if (result.Status == 1) {
                var columnProcess = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "ProcessGUID", name: "流程GUID", field: "ProcessGUID", width: 120, cssClass: "bg-gray" },
                    { id: "ProcessName", name: "流程名称", field: "ProcessName", width: 160, cssClass: "bg-gray" },
                    { id: "AppType", name: "App类型", field: "AppType", width: 120, cssClass: "bg-gray" },
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
                var gridProcess = new Slick.Grid("#myGridProcess", dvProcess, columnProcess, optionsProcess);

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
                        $scope.selectedProcessGUID = row.ProcessGUID;      //marked and returned selected row info
                        $scope.selectedProcessRecord = row;
                        $scope.isSelectedNew = true;

                        processManager.loadProcess(row);
                    }
                });
            }

            $("#spinner").hide();
        });
    }
    //#endregion

    //create new graph
    $scope.createNew = function () {
        processAPI.getProcessById("6a171d68-3fe7-482a-a727-27eaf93bc42c")       //测试流程
            .success(function (result) {
                if (result.Status == 1) {
                    var processEntity = result.Entity;
                    $scope.graphView = krender.createNew(processEntity);
                }
            });
    }
    
    //save graph
    $scope.saveProcessFile = function () {
        var processGUID = $scope.graphView.processGUID;
        var packageData = $scope.graphView.packageData;     //include participants and process 
        var processFileEntity = krender.serialize(processGUID, packageData);

        processAPI.saveProcessFile(processFileEntity).success(function (result) {
            if (result.Status == 1) {
                alert("保存成功！");
            } else {
                alert(result.Message);
            }
        });
    }

    //#region save activity and transtion info
    $scope.saveActivityInfo = function (activityInfo) {

    }

    $scope.saveTransitionInfo = function(transitionInfo){
        alert(transitionInfo.description);
    }
    //#endregion

    //#region Process, Activity, Transition dialog click event stop propagation
    $scope.activityDialogKeyUp = function (e) {
        if (e.keyCode === deleteKeyCode) {
            e.stopPropagation();
        }
    }

    $scope.transitionDialogKeyUp = function (e) {
        if (e.keyCode === deleteKeyCode) {
            e.stopPropagation();
        }
    }

    $scope.processDialogKeyUp = function (e) {
        if (e.keyCode === deleteKeyCode) {
            e.stopPropagation();
        }
    }
    //#endregion
}]);
