
var mainPage;
if (!mainPage) mainPage = {};

(function () {
    var mgrdReadyTable = null;
    var mgrdCompletedTable = null;

    mainPage.loadReadyTasks = function (user) {
        var query = {
            "PageIndex": 0,
            "PageSize": 5,
            "UserID": user.UserID
        };

        jshelper.ajaxPost("/SfDemoApi/api/wf/QueryReadyTasks", JSON.stringify(query), function (result) {
            if (mainPage.mgrdCompletedTable !== undefined)
                mainPage.mgrdCompletedTable.destroy();

            var hyperLinkAction = function (dataContext) {
                return dataContext["AppInstanceID"];
            }

            if (result.Status === 1) {
                var columnTaskView = [
                    { id: "ID", name: "ID", field: "TaskID", width: 40, cssClass: "bg-gray" },
                    { id: "AppName", name: "业务票据", field: "AppName", width: 80, cssClass: "bg-gray" },
                    {
                        id: "AppInstanceID", name: "票据ID", field: "AppInstanceID", width: 50, cssClass: "bg-gray",
                        formatter: Slick.Formatters.HyperLinkNewPage, linkUrl: "/MvcDemo/POrder/Index/", linkAction: hyperLinkAction
                    },
                    { id: "ActivityName", name: "当前节点", field: "ActivityName", width: 90, cssClass: "bg-gray" },
                    { id: "CreatedDateTime", name: "创建时间", field: "CreatedDateTime", width: 100, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                    { id: "AssignedToUserName", name: "办理人", field: "AssignedToUserName", width: 120, cssClass: "bg-gray" },
                ];

                var optionsTaskView = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: false,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsReadyTaskView = result.Entity;
                var dvReadyTaskView = new Slick.Data.DataView({ inlineFilters: true });
                var gridReadyTaskView = new Slick.Grid("#myGridReadyTaskView",
                    dvReadyTaskView, columnTaskView, optionsTaskView);

                dvReadyTaskView.onRowsChanged.subscribe(function (e, args){
                    gridReadyTaskView.invalidateRows(args.rows);
                    gridReadyTaskView.render();
                });

                dvReadyTaskView.onRowCountChanged.subscribe(function (e, args){
                    gridReadyTaskView.updateRowCount();
                    gridReadyTaskView.render();
                });

                dvReadyTaskView.beginUpdate();
                dvReadyTaskView.setItems(dsReadyTaskView, "TaskID");
                gridReadyTaskView.setSelectionModel(new Slick.RowSelectionModel());
                gridReadyTaskView.autosizeColumns();
                dvReadyTaskView.endUpdate();

                mainPage.mgrdReadyTalbe = gridReadyTaskView;
            }
        });

    }

    mainPage.loadCompletedTasks = function (user) {
        var query = {
            "PageIndex": 0,
            "PageSize": 5,
            "UserID": user.UserID,
            "EndedByUserID": user.UserID
        };

        var hyperLinkAction = function(dataContext){
            return dataContext["AppInstanceID"];
        }

        jshelper.ajaxPost("/SfDemoApi/api/wf/QueryCompletedTasks", JSON.stringify(query), function (result) {
            if (mainPage.mgrdReadyTable !== undefined)
                mainPage.mgrdReadyTable.destroy();

            if (result.Status === 1) {
                var columnTaskView = [
                    { id: "ID", name: "ID", field: "TaskID", width: 40, cssClass: "bg-gray" },
                    { id: "AppName", name: "业务票据", field: "AppName", width: 80, cssClass: "bg-gray" },
                    {
                        id: "AppInstanceID", name: "票据ID", field: "AppInstanceID", width: 50, cssClass: "bg-gray",
                        formatter: Slick.Formatters.HyperLinkNewPage, linkUrl: "/MvcDemo/POrder/Index/", linkAction: hyperLinkAction
                    },
                    { id: "ActivityName", name: "节点名称", field: "ActivityName", width: 70, cssClass: "bg-gray" },
                    { id: "EndedByUserName", name: "完成人", field: "EndedByUserName", width: 120, cssClass: "bg-gray" },
                    { id: "EndedDateTime", name: "完成时间", field: "EndedDateTime", width: 120, cssClass: "bg-gray", formatter: Slick.Formatters.DataTime },
                ];

                var optionsTaskView = {
                    editable: true,
                    enableCellNavigation: true,
                    enableColumnReorder: false,
                    asyncEditorLoading: true,
                    forceFitColumns: false,
                    topPanelHeight: 25
                };

                var dsCompletedTaskView = result.Entity;
                var dvCompletedTaskView = new Slick.Data.DataView({ inlineFilters: true });
                var gridCompletedTaskView = new Slick.Grid("#myGridCompletedTaskView",
                    dvCompletedTaskView, columnTaskView, optionsTaskView);

                dvCompletedTaskView.onRowsChanged.subscribe(function (e, args) {
                    gridCompletedTaskView.invalidateRows(args.rows);
                    gridCompletedTaskView.render();
                });

                dvCompletedTaskView.onRowCountChanged.subscribe(function (e, args) {
                    gridCompletedTaskView.updateRowCount();
                    gridCompletedTaskView.render();
                });

                dvCompletedTaskView.beginUpdate();
                dvCompletedTaskView.setItems(dsCompletedTaskView, "TaskID");
                gridCompletedTaskView.setSelectionModel(new Slick.RowSelectionModel());
                gridCompletedTaskView.autosizeColumns();
                dvCompletedTaskView.endUpdate();

                mainPage.mgrdCompletedTable = gridCompletedTaskView;
            }
        });
    }
})()