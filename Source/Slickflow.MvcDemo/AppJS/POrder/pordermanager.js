
var porderManager;
if (!porderManager) porderManager = {};

(function () {
    var porderStatus = {};
    porderStatus.Ready = 1;
    porderStatus.Dispatched = 2;
    porderStatus.Sampled = 3;
    porderStatus.Manufactured = 4;
    porderStatus.QCChecked = 5,
    porderStatus.Weighted = 6,
    porderStatus.Deliveried = 7;

    porderManager.checkUserPermission = function () {
        var buttonResource = $("button[rescode]");

        for (var i = 0; i < buttonResource.length; i++) {
            var rescode = $(buttonResource[i]).attr("rescode");
            var isPermitted = lsm.checkUserPermission(rescode);
            if (isPermitted == false) {
                $(buttonResource[i]).prop('disabled', true);
            }
        }
    }

    var mgridProductOrder = null;
    var mdvProductOrder = null;

    var selectedProductOrderID = 0;
    var statusTextArray = ["准备", "已派单", "已打样", "已生产", "已质检", "已称重", "已发货"];
    var selectedRow = null;
    var selectedRowIndex = 0;

    //#region current logon user
    porderManager.getCurrentLogonUser = function () {
        var user = lsm.getUserIdentity();

        var runner = {
            "UserID": user.UserID,
            "UserName": user.UserName,
            "AppName": "生产订单",
            "AppInstanceID": porderManager.selectedProductOrderID,
            "ProcessGUID": gProductOrderProcessGUID
        };
        return runner;
    }
    //#endregion

    //#region Product Order DataGrid
    porderManager.getProductOrderList = function () {
        var query = {
            "PageIndex": 0,
            "PageSize": 50
        };

        jshelper.ajaxPost('/SfDemoApi/api/ProductOrder/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
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
                    { id: "Address", name: "客户地址", field: "Address", width: 200, cssClass: "bg-gray" },
                    { id: "Mobile", name: "手机", field: "Mobile", width: 100, cssClass: "bg-gray" }
                ];

                var optionsProductOrder = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: false,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsProductOrder = result.Entity;

                var dvProductOrder = porderManager.mdvProductOrder = new Slick.Data.DataView({ inlineFilters: true });
                var gridProductOrder = porderManager.mgridProductOrder = new Slick.Grid("#myGridProductOrder",
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
                        porderManager.selectedRow = row;
                        porderManager.selectedRowIndex = selectedRowIndex;
                        porderManager.selectedProductOrderID = row.ID;
                        porderManager.selectedProductOrderCode = row.OrderCode;

                        porderManager.getAppFlowList(row.ID);
                    }
                });
            }
        });
    }
    //#endregion

    //#region product order lifetime
    porderManager.updateRowCell = function (status){
        var row = porderManager.selectedRow;
        var rowIndex = porderManager.selectedRowIndex;

        row.Status = status;
        porderManager.mgridProductOrder.invalidateRow(row);
        porderManager.mgridProductOrder.invalidateRow(rowIndex);
        row[porderManager.mgridProductOrder.getColumns()[3].id] = status;
        row[porderManager.mgridProductOrder.getColumns()[4].id] = status;
        porderManager.mdvProductOrder.updateItem(row.ID, row);
        porderManager.mgridProductOrder.render();
    }

    porderManager.syncorder = function () {
        jshelper.ajaxPost('/sfdemoapi/api/productorder/syncorder',
            null,
            function (result) {
                if (result.Status == 1) {
                    alert("订单数据同步成功！")

                    //重新加载数据
                    porderManager.getProductOrderList();
                } else {
                    alert(result.Message);
                }
            });
    }

    //派单(流程第一个节点)
    porderManager.dispatch = function () {
        var productOrder = { 
            "ID": porderManager.selectedProductOrderID, 
            "OrderCode": porderManager.selectedProductOrderCode,
        };
        var runner = porderManager.getCurrentLogonUser();
        var condition = {};
        condition["CanUseStock"] = "false";
        condition["IsHavingWeight"] = "false";

        runner.Conditions = condition;

        var entity = {
            "ProductOrderEntity": productOrder,
            "WfAppRunner": runner
        };

        jshelper.ajaxPost('/sfdemoapi/api/productorder/dispatch',
            JSON.stringify(entity),
            function (result) {
                if (result.Status == 1) {
                    //修改行单元格数值
                    porderManager.updateRowCell(porderStatus.Dispatched);
                    alert("派单成功!")
                } else {
                    alert(result.Message);
                }
            });
    }

    //打样
    porderManager.sample = function () {
        var productOrder = {
            "ID": porderManager.selectedProductOrderID,
            "OrderCode": porderManager.selectedProductOrderCode,
        };
        var runner = porderManager.getCurrentLogonUser();
        var entity = {
            "ProductOrderEntity": productOrder,
            "WfAppRunner": runner
        };

        //get next step information
        jshelper.ajaxPost('/sfdemoapi/api/wf/querynextstep',
            JSON.stringify(runner),
            function (result) {
                if (result.Status === 1) {
                    var nextStep = result.Entity[0];
                    var r = confirm("要流转到下一步" + nextStep.ActivityName + "吗?");

                    if (r == true) {
                        jshelper.ajaxPost('/sfdemoapi/api/productorder/sample',
                            JSON.stringify(entity),
                            function (result) {
                                if (result.Status == 1) {
                                    //修改行单元格数值
                                    porderManager.updateRowCell(porderStatus.Sampled);
                                    alert("打样成功！");
                                } else {
                                    alert(result.Message);
                                }
                            });
                    }
                }
                else {
                    alert(result.Message);
                }
            });
    }

    //生产
    porderManager.manufacture = function () {
        var productOrder = {
            "ID": porderManager.selectedProductOrderID,
            "OrderCode": porderManager.selectedProductOrderCode,
        };
        var runner = porderManager.getCurrentLogonUser();
        var entity = {
            "ProductOrderEntity": productOrder,
            "WfAppRunner": runner
        };

        //get next step information
        jshelper.ajaxPost('/sfdemoapi/api/wf/querynextstep',
            JSON.stringify(runner),
            function (result) {
                if (result.Status === 1) {
                    var nextStep = result.Entity[0];
                    var r = confirm("要流转到下一步" + nextStep.ActivityName + "吗?");

                    if (r == true) {
                        jshelper.ajaxPost('/sfdemoapi/api/productorder/manufacture',
                            JSON.stringify(entity),
                            function (result) {
                                if (result.Status == 1) {
                                    //修改行单元格数值
                                    porderManager.updateRowCell(porderStatus.Manufactured);
                                    alert("生产成功!");
                                } else {
                                    alert(result.Message);
                                }
                            });
                    }
                }
                else {
                    alert(result.Message);
                }
            });
    }

    //质检
    porderManager.qccheck = function () {
        var productOrder = {
            "ID": porderManager.selectedProductOrderID,
            "OrderCode": porderManager.selectedProductOrderCode,
        };
        var runner = porderManager.getCurrentLogonUser();
        var entity = {
            "ProductOrderEntity": productOrder,
            "WfAppRunner": runner
        };

        //get next step information
        jshelper.ajaxPost('/sfdemoapi/api/wf/querynextstep',
            JSON.stringify(runner),
            function (result) {
                if (result.Status === 1) {
                    var nextStep = result.Entity[0];
                    var r = confirm("要流转到下一步" + nextStep.ActivityName + "吗?");

                    if (r == true) {
                        jshelper.ajaxPost('/sfdemoapi/api/productorder/qccheck',
                            JSON.stringify(entity),
                            function (result) {
                                if (result.Status == 1) {
                                    //修改行单元格数值
                                    porderManager.updateRowCell(porderStatus.QCChecked);
                                    alert("质检成功!");
                                } else {
                                    alert(result.Message);
                                }
                            });
                    }
                }
                else {
                    alert(result.Message);
                }
            });
    }

    //称重
    porderManager.weight = function () {
        var productOrder = {
            "ID": porderManager.selectedProductOrderID,
            "OrderCode": porderManager.selectedProductOrderCode,
        };
        var runner = porderManager.getCurrentLogonUser();
        var entity = {
            "ProductOrderEntity": productOrder,
            "WfAppRunner": runner
        };

        //get next step information
        jshelper.ajaxPost('/sfdemoapi/api/wf/querynextstep',
            JSON.stringify(runner),
            function (result) {
                if (result.Status === 1) {
                    var nextStep = result.Entity[0];
                    var r = confirm("要流转到下一步" + nextStep.ActivityName + "吗?");

                    if (r == true) {
                        jshelper.ajaxPost('/sfdemoapi/api/productorder/weight',
                            JSON.stringify(entity),
                            function (result) {
                                if (result.Status == 1) {
                                    //修改行单元格数值
                                    porderManager.updateRowCell(porderStatus.Weighted);
                                    alert("称重成功!");
                                } else {
                                    alert(result.Message);
                                }
                            });
                    }
                }
                else {
                    alert(result.Message);
                }
            });
    }

    //发货
    porderManager.delivery = function () {
        var productOrder = {
            "ID": porderManager.selectedProductOrderID,
            "OrderCode": porderManager.selectedProductOrderCode,
        };
        var runner = porderManager.getCurrentLogonUser();
        var entity = {
            "ProductOrderEntity": productOrder,
            "WfAppRunner": runner
        };

        //get next step information
        jshelper.ajaxPost('/sfdemoapi/api/wf/querynextstep',
            JSON.stringify(runner),
            function (result) {
                if (result.Status === 1) {
                    var nextStep = result.Entity[0];
                    var r = confirm("要流转到下一步" + nextStep.ActivityName + "吗?");

                    if (r == true) {
                        jshelper.ajaxPost('/sfdemoapi/api/productorder/delivery',
                            JSON.stringify(entity),
                            function (result) {
                                if (result.Status == 1) {
                                    //修改行单元格数值
                                    porderManager.updateRowCell(porderStatus.Deliveried);
                                    alert("发货成功, 流程结束!");
                                } else {
                                    alert(result.Message);
                                }
                            });
                    }
                }
                else {
                    alert(result.Message);
                }
            });
    }
    //#endregion

    //#region 业务流程数据记录
    porderManager.getAppFlowList = function (appInstanceID) {
        var query = {
            "PageIndex": 0,
            "PageSize": 20,
            "AppInstanceID": appInstanceID
        };

        jshelper.ajaxPost('/SfDemoApi/api/AppFlow/QueryPaged', JSON.stringify(query), function (result) {
            if (result.Status === 1) {
                var columnAppFlow = [
                    { id: "ID", name: "ID", field: "ID", width: 40, cssClass: "bg-gray" },
                    { id: "ActivityName", name: "节点名称", field: "ActivityName", width: 80, cssClass: "bg-gray" },
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
            }
        });
    }
    //#endregion
})()