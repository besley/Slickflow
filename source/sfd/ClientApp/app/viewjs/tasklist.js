import kmsgbox from '../script/kmsgbox.js';
import processapi from './processapi.js';

const tasklist = (function () {
    function tasklist() {
    }

    tasklist.refreshTask = function () {
        getTaskList();
        getDoneList();
    }

    function getTaskList() {
        kmsgbox.showProgressBar();

        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetTaskToDoListTop', null, function (result) {
            if (result.Status === 1) {
                var divTaskGrid = document.querySelector('#myToDoTaskGrid');
                $(divTaskGrid).empty();

                var gridOptions = {
                    theme: themeBalham,
                    columnDefs: [
                        { headerName: 'Id', field: 'TaskId', width: 50 },
                        { headerName: kresource.getItem('appname'), field: 'AppName', width: 120 },
                        { headerName: kresource.getItem('appinstanceid'), field: 'AppInstanceId', width: 120 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 160 },
                        { headerName: kresource.getItem('assigneduserid'), field: 'AssignedUserId', width: 100 },
                        { headerName: kresource.getItem('assignedusername'), field: 'AssignedUserName', width: 100 },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 200 },
                    ],
                    rowSelection: {
                        mode: 'singleRow',
                        checkboxes: false,
                        enableClickSelection: true
                    },
                };

                gridOptions.rowData = result.Entity;
                const gridApi = createGrid(divTaskGrid, gridOptions);
            } else {
                kmsgbox.error(kresource.getItem("tasklistloaderrormsg"), result.Message);
            }
            kmsgbox.hideProgressBar();
        });
    };

    function getDoneList() {
        kmsgbox.showProgressBar();

        jshelper.ajaxGet(kconfig.webApiUrl + 'api/Wf2Xml/GetTaskDoneListTop', null, function (result) {
            if (result.Status === 1) {
                var divTaskGrid = document.querySelector('#myDoneTaskGrid');
                $(divTaskGrid).empty();

                var gridOptions = {
                    theme: themeBalham,
                    columnDefs: [
                        { headerName: 'Id', field: 'TaskId', width: 50 },
                        { headerName: kresource.getItem('appname'), field: 'AppName', width: 120 },
                        { headerName: kresource.getItem('appinstanceid'), field: 'AppInstanceId', width: 120 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 160 },
                        { headerName: kresource.getItem('endeduserid'), field: 'EndedUserId', width: 100 },
                        { headerName: kresource.getItem('endedusername'), field: 'EndedUserName', width: 100 },
                        { headerName: kresource.getItem('endeddatetime'), field: 'EndedDateTime', width: 200 },
                    ],
                    rowSelection: {
                        mode: 'singleRow',
                        checkboxes: false,
                        enableClickSelection: true
                    },
                };

                gridOptions.rowData = result.Entity;
                const gridApi = createGrid(divTaskGrid, gridOptions);
            } else {
                kmsgbox.error(kresource.getItem("tasklistloaderrormsg"), result.Message);
            }
            kmsgbox.hideProgressBar();
        });
    }

    return tasklist;
})();

export default processmodel