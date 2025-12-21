var pordermanager = (function () {
    function pordermanager() {
    }

    pordermanager.mProductOrderProcessId = config.ProcessId;
    pordermanager.mcurrentProcessInstanceId = 0;
    pordermanager.mcurrentAppName = "ProductOrder";

    var porderStatus = {};
    porderStatus.Ready = 1;
    porderStatus.Dispatched = 2;
    porderStatus.Sampled = 3;
    porderStatus.Manufactured = 4;
    porderStatus.QCChecked = 5,
    porderStatus.Weighted = 6,
    porderStatus.Deliveried = 7;

    pordermanager.checkUserPermission = function () {
        var buttonResource = $("button[rescode]");

        for (var i = 0; i < buttonResource.length; i++) {
            var rescode = $(buttonResource[i]).attr("rescode");
            var isPermitted = lsm.checkUserPermission(rescode);
            if (isPermitted === false) {
                $(buttonResource[i]).prop('disabled', true);
            }
        }
    }

    pordermanager.mgridProductOrder = null;
    pordermanager.mdvProductOrder = null;
    pordermanager.selectedProductOrderId = 0;

    var selectedRow = null;
    var selectedRowIndex = 0;

    //#region current logon user
    pordermanager.getCurrentLogonUser = function () {
        var user = lsm.getUserIdentity();

        if (user === undefined) {
            $.msgBox({
                title: "MvcDemo / Order",
                content: "Please select a user and login again！",
                type: "alert"
            });
            return;
        }

        var runner = {
            "UserId": user.UserId,
            "UserName": user.UserName,
            "AppName": "ProductOrder",
            "AppInstanceId": pordermanager.selectedProductOrderId.toString(),
            "ProcessId": pordermanager.mProductOrderProcessId,
            "Version": "1"
        };
        return runner;
    }
    //#endregion

    //#region Product Order DataGrid
    pordermanager.getProductOrderList = function (isRoleKept) {
        showProgressBar();

        var query = {
            "PageIndex": 0,
            "PageSize": 1000            //retrieve top 1000 
        };

        //add role data limit
        if (isRoleKept === true && lsm.getUserRole() !== null) query.RoleCode = lsm.getUserRole().RoleCode;

        jshelper.ajaxPost('api/ProductOrder/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                fillOrderGrid(result.Entity);
            } else {
                $.msgBox({
                    title: "MvcDemo / Order",
                    content: result.Message,
                    type: "error"
                });
            }
        });
    }

    pordermanager.queryByStatus = function () {
        var query = {
            "PageIndex": 0,
            "PageSize": 1000,            //retrieve top 1000 
            "Status": $("#ddlStatus").val() 
        };

        jshelper.ajaxPost('api/ProductOrder/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                fillOrderGrid(result.Entity);
            } else {
                $.msgBox({
                    title: "MvcDemo / Order",
                    content: result.Message,
                    type: "error"
                });
            }
        });
    }

    function fillOrderGrid(dsProductOrder) {

        var statusTextArray = ["Ready", "toDispatch", "toSample", "toManufature", "toQCCheck", "toWeight", "toDeliverty", "Completed"];
        //grid column define
        var columnProductOrder = [
                    { id: "Id", name: "Id", field: "Id", width: 40, cssClass: "bg-gray" },
                    { id: "OrderCode", name: "OrderCode", field: "OrderCode", width: 80, cssClass: "bg-gray" },
                    { id: "ProductName", name: "OrderName", field: "ProductName", width: 90, cssClass: "bg-gray" },
                    { id: "Status", name: "Status", field: "Status", width: 70, cssClass: "bg-gray", formatter: Slick.Formatters.EnmuabledText, enumabledTextArray: statusTextArray },
                    { id: "Progress", name: "Progress", field: "Status", width: 60, cssClass: "bg-gray", formatter: Slick.Formatters.PercentStatusBar, maxStatus: statusTextArray.length },
                    { id: "UnitPrice", name: "Price", field: "UnitPrice", width: 60, cssClass: "bg-gray" },
                    { id: "Quantity", name: "Quantity", field: "Quantity", width: 60, cssClass: "bg-gray" },
                    { id: "TotalPrice", name: "Price", field: "TotalPrice", width: 80, cssClass: "bg-gray" },
                    { id: "CreatedTime", name: "CreatedTime", field: "CreatedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "CustomerName", name: "CustomerName", field: "CustomerName", width: 120, cssClass: "bg-gray" },
                    { id: "Address", name: "Address", field: "Address", width: 120, cssClass: "bg-gray" },
                    { id: "Mobile", name: "Mobile", field: "Mobile", width: 100, cssClass: "bg-gray" },
                    { id: "UpdatedTime", name: "UpdatedTime", field: "UpdatedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
        ];

        var optionsProductOrder = {
            editable: true,
            enableCellNavigation: true,
            enableColumnReorder: false,
            asyncEditorLoading: true,
            forceFitColumns: false,
            topPanelHeight: 25
        };

        var dvProductOrder = pordermanager.mdvProductOrder = new Slick.Data.DataView({ inlineFilters: true });
        var gridProductOrder = pordermanager.mgridProductOrder = new Slick.Grid("#myGridProductOrder",
            dvProductOrder, columnProductOrder, optionsProductOrder);

        dvProductOrder.onRowsChanged.subscribe(function (e, args) {
            gridProductOrder.invalidateRows(args.rows);
            gridProductOrder.render();
        });

        dvProductOrder.onRowCountChanged.subscribe(function (e, args) {
            gridProductOrder.updateRowCount();
            gridProductOrder.render();
        });

        dvProductOrder.beginUpdate();
        dvProductOrder.setItems(dsProductOrder, "Id");
        gridProductOrder.setSelectionModel(new Slick.RowSelectionModel());
        gridProductOrder.autosizeColumns();
        dvProductOrder.endUpdate();

        //rows change event
        gridProductOrder.onSelectedRowsChanged.subscribe(function (e, args) {
            var selectedRowIndex = args.rows[0];
            var row = dvProductOrder.getItemByIdx(selectedRowIndex);
            if (row) {
                pordermanager.selectedRow = row;
                pordermanager.selectedRowIndex = selectedRowIndex;
                pordermanager.selectedProductOrderId = row.Id;
                pordermanager.selectedProductOrderCode = row.OrderCode;

                pordermanager.getReadyActivityInstance(row.Id.toString(), pordermanager.mProductOrderProcessId);
                pordermanager.getAppFlowList(row.Id);

                var status = row.Status;
                if (status === "1") {
                    $("#divCondition").show();
                } else {
                    $("#divCondition").hide();
                }

            }
        });
    }

    function showProgressBar() {
        $('.progress .bar').progressbar({
            transition_delay: 100
        });
        var $modal = $('.js-loading-bar'),
            $bar = $modal.find('.bar');

        $modal.modal('show');

        setTimeout(function () {
            $modal.modal('hide');
        }, 300);
    }
    //#endregion

    //#region product order
    pordermanager.updateRowCell = function (status){
        var row = pordermanager.selectedRow;
        var rowIndex = pordermanager.selectedRowIndex;

        row.Status = status;
        pordermanager.mgridProductOrder.invalidateRow(row);
        pordermanager.mgridProductOrder.invalidateRow(rowIndex);
        row[pordermanager.mgridProductOrder.getColumns()[3].id] = status;
        row[pordermanager.mgridProductOrder.getColumns()[4].id] = status;
        pordermanager.mdvProductOrder.updateItem(row.Id, row);
        pordermanager.mgridProductOrder.render();
    }

    pordermanager.syncorder = function () {
        jshelper.ajaxPost('api/productorder/syncorder',
            null,
            function (result) {
                if (result.Status === 1) {
                    $.msgBox({
                        title: "MvcDemo / Order",
                        content: "New order data synchronized successfully！",
                        type: "info"
                    });

                    //refresh data
                    pordermanager.getProductOrderList(false);
                } else {
                    $.msgBox({
                        title: "MvcDemo / Order",
                        content: result.Message,
                        type: "error"
                    });
                }
            });
    }

    pordermanager.runAppFlow = function (step) {
        var user = pordermanager.getCurrentLogonUser();
        if (user === undefined) return false;

        var runner = {
            "UserId": user.UserId,
            "UserName": user.UserName,
            "AppName": pordermanager.mcurrentAppName,
            "AppInstanceId": pordermanager.selectedProductOrderId.toString(),
            "ProcessId": pordermanager.mProductOrderProcessId,
            "Version": "1"
        };

        var productOrder = {
            "Id": pordermanager.selectedProductOrderId,
            "OrderCode": pordermanager.selectedProductOrderCode,
        };

        if (step === "dispatch") {
            //判断是否是重复派单
            //Determine whether it is a duplicate dispatch
            jshelper.ajaxGet("api/productorder/checkdispatched/" + pordermanager.selectedProductOrderId,
                        null,
                        function (result) {
                            if (result.Status === 1) {
                                runner.Conditions = {};
                                runner.Conditions["CanUseStock"] = $("#chkStorage").is(':checked') ? "true" : "false";
                                runner.Conditions["IsHavingWeight"] = $("#chkWeight").is(':checked') ? "true" : "false";

                                var entity = {
                                    "ProductOrderEntity": productOrder,
                                    "WfAppRunner": runner
                                };

                                $('#modelNextStepForm').modal('show');

                                nextactivitytree.showNextActivityTree(entity, step);
                            } else {
                                $.msgBox({
                                    title: "DynFlow / GoNext",
                                    content: result.Message,
                                    type: "alert"
                                });
                            }
                        });


        } else {
            var entity = {
                "ProductOrderEntity": productOrder,
                "WfAppRunner": runner
            };

            $('#modelNextStepForm').modal('show');

            nextactivitytree.showNextActivityTree(entity, step);
        }
    }

    

    //#endregion

    //#region 业务流程数据记录
    pordermanager.getReadyActivityInstance = function (appInstanceId, processId) {
        var query = {};
        query.AppInstanceId = appInstanceId.toString();
        query.ProcessId = processId;

        jshelper.ajaxPost('api/Wf/QueryReadyActivityInstance',
            JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnReadyActivityInstance = [
                    { id: "Id", name: "Id", field: "Id", width: 40, cssClass: "bg-gray" },
                    { id: "AppInstanceId", name: "OrderId", field: "AppInstanceId", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: "ActivityName", field: "ActivityName", width: 80, cssClass: "bg-gray" },
                    { id: "CreatedDateTime", name: "CreatedDateTiem", field: "CreatedDateTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "AssignedToUserNames", name: "AssignedToUser", field: "AssignedToUserNames", width: 320, cssClass: "bg-gray" },
                ];

                var optionsReadyActivityInstance = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: false,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsReadyActivityInstance = result.Entity;

                var dvReadyActivityInstance = new Slick.Data.DataView({ inlineFilters: true });
                var gridReadyActivityInstance = new Slick.Grid("#myGridReadyActivityInstance", dvReadyActivityInstance, columnReadyActivityInstance, optionsReadyActivityInstance);

                dvReadyActivityInstance.onRowsChanged.subscribe(function (e, args) {
                    gridReadyActivityInstance.invalidateRows(args.rows);
                    gridReadyActivityInstance.render();
                });

                dvReadyActivityInstance.onRowCountChanged.subscribe(function (e, args) {
                    gridReadyActivityInstance.updateRowCount();
                    gridReadyActivityInstance.render();
                });

                dvReadyActivityInstance.beginUpdate();
                dvReadyActivityInstance.setItems(dsReadyActivityInstance, "Id");
                gridReadyActivityInstance.setSelectionModel(new Slick.RowSelectionModel());
                gridReadyActivityInstance.autosizeColumns();
                dvReadyActivityInstance.endUpdate();
            } else {
                $.msgBox({
                    title: "MvcDemo / AppFlow",
                    content: result.Message,
                    type: "error"
                });
            }
        });
    }

    pordermanager.getAppFlowList = function (appInstanceId) {
        var query = {
            "PageIndex": 0,
            "PageSize": 20,
            "AppInstanceId": appInstanceId.toString()
        };

        jshelper.ajaxPost('api/AppFlow/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnAppFlow = [
                    { id: "Id", name: "Id", field: "Id", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: "ActivityName", field: "ActivityName", width: 80, cssClass: "bg-gray" },
                    { id: "ChangedTime", name: "ChangedTime", field: "ChangedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "ChangedUserName", name: "ChangedUser", field: "ChangedUserName", width: 120, cssClass: "bg-gray" },
                ];

                var optionsAppFlow = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: false,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsAppFlow = result.Entity;

                var dvAppFlow = new Slick.Data.DataView({ inlineFilters: true });
                var gridAppFlow = new Slick.Grid("#myGridAppFlow", dvAppFlow, columnAppFlow, optionsAppFlow);

                dvAppFlow.onRowsChanged.subscribe(function (e, args) {
                    gridAppFlow.invalidateRows(args.rows);
                    gridAppFlow.render();
                });

                dvAppFlow.onRowCountChanged.subscribe(function (e, args) {
                    gridAppFlow.updateRowCount();
                    gridAppFlow.render();
                });

                dvAppFlow.beginUpdate();
                dvAppFlow.setItems(dsAppFlow, "Id");
                gridAppFlow.setSelectionModel(new Slick.RowSelectionModel());
                gridAppFlow.autosizeColumns();
                dvAppFlow.endUpdate();
            } else {
                $.msgBox({
                    title: "MvcDemo / AppFlow",
                    content: result.Message,
                    type: "error"
                });
            }
        });
    }
    //#endregion

    return pordermanager;
})()