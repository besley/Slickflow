var pordermanager = (function () {
    function pordermanager() {
    }

    pordermanager.mProductOrderProcessGUID = '5c5041fc-ab7f-46c0-85a5-6250c3aea375';
    pordermanager.mcurrentProcessInstanceID = 0;
    pordermanager.mcurrentAppName = 'ProductOrder';

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
            if (isPermitted == false) {
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

        if (user == undefined) {
            $.msgBox({
                title: "MvcDemo / Order",
                content: kresource.getItem("reloginwarnmsg"),
                type: "alert"
            });
            return;
        }

        var runner = {
            "UserID": user.UserID,
            "UserName": user.UserName,
            "AppName": pordermanager.mcurrentAppName,
            "AppInstanceID": pordermanager.selectedProductOrderID,
            "ProcessGUID": pordermanager.mProductOrderProcessGUID
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
        var statusTextArray = [
            kresource.getItem("ready"),
            kresource.getItem("waitassignorder"),
            kresource.getItem("waitsampleorder"),
            kresource.getItem("waitproductorder"),
            kresource.getItem("waitqcorder"),
            kresource.getItem("waitweightorder"),
            kresource.getItem("waitdeliveryorder"),
            kresource.getItem("waitcompleteorder")
        ];
        //grid column define
        var columnProductOrder = [
            { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
            { id: "OrderCode", name: kresource.getItem("ordercode"), field: "OrderCode", width: 80, cssClass: "bg-gray" },
            { id: "ProductName", name: kresource.getItem("productname"), field: "ProductName", width: 90, cssClass: "bg-gray" },
            { id: "Status", name: kresource.getItem("orderstatus"), field: "Status", width: 90, cssClass: "bg-gray", formatter: Slick.Formatters.EnmuabledText, enumabledTextArray: statusTextArray },
            { id: "Progress", name: kresource.getItem("orderprogress"), field: "Status", width: 60, cssClass: "bg-gray", formatter: Slick.Formatters.PercentStatusBar, maxStatus: statusTextArray.length },
            { id: "UnitPrice", name: kresource.getItem("orderprice"), field: "UnitPrice", width: 60, cssClass: "bg-gray" },
            { id: "Quantity", name: kresource.getItem("quantity"), field: "Quantity", width: 60, cssClass: "bg-gray" },
            { id: "TotalPrice", name: kresource.getItem("totalprice"), field: "TotalPrice", width: 80, cssClass: "bg-gray" },
            { id: "CreatedTime", name: kresource.getItem("ordertime"), field: "CreatedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
            { id: "CustomerName", name: kresource.getItem("customername"), field: "CustomerName", width: 120, cssClass: "bg-gray" },
            { id: "Address", name: kresource.getItem("customeraddress"), field: "Address", width: 120, cssClass: "bg-gray" },
            { id: "Mobile", name: kresource.getItem("mobile"), field: "Mobile", width: 100, cssClass: "bg-gray" },
            { id: "LastUpdatedTime", name: kresource.getItem("lastupdatedtime"), field: "LastUpdatedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
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

                pordermanager.getReadyActivityInstance(row.ID, pordermanager.mProductOrderProcessGUID);
                pordermanager.getAppFlowList(row.ID);

                var status = row.Status;
                if (status == "1") {
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
                if (result.Status == 1) {
                    $.msgBox({
                        title: "MvcDemo / Order",
                        content: kresource.getItem("syncorderokmsg"),
                        type: "info"
                    });

                    //reload data
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
        if (user == undefined) return false;

        var runner = {
            "UserID": user.UserID,
            "UserName": user.UserName,
            "AppName": pordermanager.mcurrentAppName,
            "AppInstanceID": pordermanager.selectedProductOrderID,
            "ProcessGUID": pordermanager.mProductOrderProcessGUID
        };

        var productOrder = {
            "ID": pordermanager.selectedProductOrderID,
            "OrderCode": pordermanager.selectedProductOrderCode,
        };

        if (step == "dispatch") {
            //check dispatch status
            jshelper.ajaxGet("api/productorder/checkdispatched/" + pordermanager.selectedProductOrderID,
                        null,
                        function (result) {
                            if (result.Status == 1) {
                                var condition = {};
                                condition["CanUseStock"] = $("#chkStorage").is(':checked') ? "true" : "false";
                                condition["IsHavingWeight"] = $("#chkWeight").is(':checked') ? "true" : "false";;
                                runner.Conditions = condition;

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

    //#region process list
    pordermanager.getReadyActivityInstance = function (appInstanceID, processGUID) {
        var query = {};
        query.AppInstanceID = appInstanceID;
        query.ProcessGUID = processGUID;

        jshelper.ajaxPost('api/Wf/QueryReadyActivityInstance',
            JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnReadyActivityInstance = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "AppInstanceID", name: kresource.getItem("orderid"), field: "AppInstanceID", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: kresource.getItem("activityname"), field: "ActivityName", width: 80, cssClass: "bg-gray" },
                    { id: "CreatedDateTime", name: kresource.getItem("createdtime"), field: "CreatedDateTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "AssignedToUserNames", name: kresource.getItem("assingtouser"), field: "AssignedToUserNames", width: 320, cssClass: "bg-gray" },
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
            "AppInstanceID": appInstanceID
        };

        jshelper.ajaxPost('api/AppFlow/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnAppFlow = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: kresource.getItem("completeactivityname"), field: "ActivityName", width: 80, cssClass: "bg-gray" },
                    { id: "ChangedTime", name: kresource.getItem("completedtime"), field: "ChangedTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "ChangedUserName", name: kresource.getItem("completeduser"), field: "ChangedUserName", width: 120, cssClass: "bg-gray" },
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