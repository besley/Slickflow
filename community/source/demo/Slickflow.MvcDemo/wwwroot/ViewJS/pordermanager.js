var pordermanager = (function () {
    function pordermanager() {
    }

    pordermanager.mProductOrderProcessGUID = config.ProcessGUID;
    pordermanager.mcurrentProcessInstanceID = 0;
    pordermanager.mcurrentAppName = "生产订单";

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
    pordermanager.selectedProductOrderID = 0;

    var selectedRow = null;
    var selectedRowIndex = 0;

    //#region current logon user
    pordermanager.getCurrentLogonUser = function () {
        var user = lsm.getUserIdentity();

        if (user === undefined) {
            $.msgBox({
                title: "MvcDemo / Order",
                content: "请选择用户，重新登录！",
                type: "alert"
            });
            return;
        }

        var runner = {
            "UserID": user.UserID,
            "UserName": user.UserName,
            "AppName": "生产订单",
            "AppInstanceID": pordermanager.selectedProductOrderID.toString(),
            "ProcessGUID": pordermanager.mProductOrderProcessGUID,
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

        var statusTextArray = ["准备", "等待派单", "等待打样", "等待生产", "等待质检", "等待称重", "等待发货", "完成"];
        //grid column define
        var columnProductOrder = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "OrderCode", name: "订单标号", field: "OrderCode", width: 80, cssClass: "bg-gray" },
                    { id: "ProductName", name: "产品名称", field: "ProductName", width: 90, cssClass: "bg-gray" },
                    { id: "Status", name: "状态", field: "Status", width: 70, cssClass: "bg-gray", formatter: Slick.Formatters.EnmuabledText, enumabledTextArray: statusTextArray },
                    { id: "Progress", name: "进度", field: "Status", width: 60, cssClass: "bg-gray", formatter: Slick.Formatters.PercentStatusBar, maxStatus: statusTextArray.length },
                    { id: "UnitPrice", name: "单价", field: "UnitPrice", width: 60, cssClass: "bg-gray" },
                    { id: "Quantity", name: "数量", field: "Quantity", width: 60, cssClass: "bg-gray" },
                    { id: "TotalPrice", name: "总价", field: "TotalPrice", width: 80, cssClass: "bg-gray" },
                    { id: "CreatedTime", name: "订单时间", field: "CreatedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "CustomerName", name: "客户名称", field: "CustomerName", width: 120, cssClass: "bg-gray" },
                    { id: "Address", name: "客户地址", field: "Address", width: 120, cssClass: "bg-gray" },
                    { id: "Mobile", name: "手机", field: "Mobile", width: 100, cssClass: "bg-gray" },
                    { id: "LastUpdatedTime", name: "最后更新时间", field: "LastUpdatedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
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
        dvProductOrder.setItems(dsProductOrder, "ID");
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
                pordermanager.selectedProductOrderID = row.ID;
                pordermanager.selectedProductOrderCode = row.OrderCode;

                pordermanager.getReadyActivityInstance(row.ID.toString(), pordermanager.mProductOrderProcessGUID);
                pordermanager.getAppFlowList(row.ID);

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
        pordermanager.mdvProductOrder.updateItem(row.ID, row);
        pordermanager.mgridProductOrder.render();
    }

    pordermanager.syncorder = function () {
        jshelper.ajaxPost('api/productorder/syncorder',
            null,
            function (result) {
                if (result.Status === 1) {
                    $.msgBox({
                        title: "MvcDemo / Order",
                        content: "新的订单数据同步成功！",
                        type: "info"
                    });

                    //重新加载数据
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
            "UserID": user.UserID,
            "UserName": user.UserName,
            "AppName": pordermanager.mcurrentAppName,
            "AppInstanceID": pordermanager.selectedProductOrderID.toString(),
            "ProcessGUID": pordermanager.mProductOrderProcessGUID,
            "Version": "1"
        };

        var productOrder = {
            "ID": pordermanager.selectedProductOrderID,
            "OrderCode": pordermanager.selectedProductOrderCode,
        };

        if (step === "dispatch") {
            //判断是否是重复派单
            jshelper.ajaxGet("api/productorder/checkdispatched/" + pordermanager.selectedProductOrderID,
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
    pordermanager.getReadyActivityInstance = function (appInstanceID, processGUID) {
        var query = {};
        query.AppInstanceID = appInstanceID.toString();
        query.ProcessGUID = processGUID;

        window.console.log(query);

        jshelper.ajaxPost('api/Wf/QueryReadyActivityInstance',
            JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnReadyActivityInstance = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "AppInstanceID", name: "订单ID", field: "AppInstanceID", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: "当前节点", field: "ActivityName", width: 80, cssClass: "bg-gray" },
                    { id: "CreatedDateTime", name: "创建时间", field: "CreatedDateTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "AssignedToUserNames", name: "任务接收人", field: "AssignedToUserNames", width: 320, cssClass: "bg-gray" },
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
                dvReadyActivityInstance.setItems(dsReadyActivityInstance, "ID");
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

    pordermanager.getAppFlowList = function (appInstanceID) {
        var query = {
            "PageIndex": 0,
            "PageSize": 20,
            "AppInstanceID": appInstanceID.toString()
        };

        jshelper.ajaxPost('api/AppFlow/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnAppFlow = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: "完成节点", field: "ActivityName", width: 80, cssClass: "bg-gray" },
                    { id: "ChangedTime", name: "完成时间", field: "ChangedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "ChangedUserName", name: "完成人", field: "ChangedUserName", width: 120, cssClass: "bg-gray" },
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
                dvAppFlow.setItems(dsAppFlow, "ID");
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