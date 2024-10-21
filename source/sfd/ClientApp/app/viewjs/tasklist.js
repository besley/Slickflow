import processapi from './processapi.js';

const tasklist = (function () {
    function tasklist() {
    }

    tasklist.refreshTask = function () {
        getTaskList();
        getDoneList();
    }

    function getTaskList() {
        $('#loading-indicator').show();

        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetTaskToDoListTop', null, function (result) {
            if (result.Status === 1) {
                var divTaskGrid = document.querySelector('#myToDoTaskGrid');
                $(divTaskGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'TaskID', width: 50 },
                        { headerName: kresource.getItem('appname'), field: 'AppName', width: 120 },
                        { headerName: kresource.getItem('appinstanceid'), field: 'AppInstanceID', width: 120 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 160 },
                        { headerName: kresource.getItem('assigneduserid'), field: 'AssignedToUserID', width: 100 },
                        { headerName: kresource.getItem('assignedusername'), field: 'AssignedToUserName', width: 100 },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 200 },
                    ],
                    rowSelection: 'single'
                };

                new agGrid.Grid(divTaskGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();
            } else {
                kmsgbox.error(kresource.getItem("tasklistloaderrormsg"), result.Message);
            }
        });
    };

    function getDoneList() {
        $('#loading-indicator').show();

        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetTaskDoneListTop', null, function (result) {
            if (result.Status === 1) {
                var divTaskGrid = document.querySelector('#myDoneTaskGrid');
                $(divTaskGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'TaskID', width: 50 },
                        { headerName: kresource.getItem('appname'), field: 'AppName', width: 120 },
                        { headerName: kresource.getItem('appinstanceid'), field: 'AppInstanceID', width: 120 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 160 },
                        { headerName: kresource.getItem('endedbyuserid'), field: 'EndedByUserID', width: 100 },
                        { headerName: kresource.getItem('endedbyusername'), field: 'EndedByUserName', width: 100 },
                        { headerName: kresource.getItem('endeddatetime'), field: 'EndedDateTime', width: 200 },
                    ],
                    rowSelection: 'single'
                };

                new agGrid.Grid(divTaskGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();
            } else {
                kmsgbox.error(kresource.getItem("tasklistloaderrormsg"), result.Message);
            }
        });
    }

    return tasklist;
})();

export default processmodel