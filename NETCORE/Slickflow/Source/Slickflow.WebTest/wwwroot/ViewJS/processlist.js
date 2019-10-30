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

var processlist = (function () {
    function processlist() {
    }

    processlist.pselectedProcessEntity = null;
    processlist.pselectedTaskEntity = null;

    //#region Process DataGrid
    processlist.getProcessList = function () {
        $('#loading-indicator').show();

        jshelper.ajaxPost('api/Wf2Xml/GetProcessListSimple', null, function (result) {
            if (result.Status === 1) {
                var divProcessGrid = document.querySelector('#myProcessGrid');
                $(divProcessGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 50 },
                        { headerName: kresource.getItem('processguid'), field: 'ProcessGUID', width: 120 },
                        { headerName: kresource.getItem('processname'), field: 'ProcessName', width: 200 },
                        { headerName: kresource.getItem('version'), field: 'Version', width: 40 },
                        { headerName: kresource.getItem('status'), field: 'IsUsing', width: 40, cellRenderer: onIsUsingCellRenderer },
                    ],
                    rowSelection: 'single',
                    onSelectionChanged: onSelectionChanged,
                    onRowDoubleClicked: onRowDoubleClicked
                };

                function onIsUsingCellRenderer(params) {
                    return params.value == 1 ? kresource.getItem('active') : kresource.getItem('unactive');
                }

                function onStartTypeCellRenderer(params) {
                    var startType = '';
                    if (params.value == 1)
                        startType = kresource.getItem('timer');
                    else if (params.value == 2)
                        startType = kresource.getItem('email');
                    return startType;
                }

                new agGrid.Grid(divProcessGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridOptions.api.getSelectedRows();
                    var selectedProcessID = 0;
                    selectedRows.forEach(function (selectedRow, index) {
                        processlist.pselectedProcessEntity = selectedRow;
                        processlist.getTaskList();
                        processlist.getDoneList();
                    });
                }

                function onRowDoubleClicked(e, args) {
                    processlist.editProcess();
                }
            }
            else {
                $.msgBox({
                    title: "Designer / Process",
                    content: kresource.getItem("processlistloaderrormsg") + result.Message,
                    type: "error"
                });
            }
        });
    }

    processlist.getTaskList = function () {
        $('#loading-indicator').show();

        var taskQuery = {};
        taskQuery.ProcessGUID = processlist.pselectedProcessEntity.ProcessGUID;
        taskQuery.Version = processlist.pselectedProcessEntity.Version;
        taskQuery.AppName = sfconfig.Runner.AppName;
        taskQuery.AppInstanceID = sfconfig.Runner.AppInstanceID;

        jshelper.ajaxPost('api/Wf2Xml/GetTaskList', JSON.stringify(taskQuery), function (result) {
            if (result.Status === 1) {
                var divTaskGrid = document.querySelector('#myTaskGrid');
                $(divTaskGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'TaskID', width: 50 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 160 },
                        { headerName: kresource.getItem('assigneduserid'), field: 'AssignedToUserID', width: 100 },
                        { headerName: kresource.getItem('assignedusername'), field: 'AssignedToUserName', width: 100 },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 200 },
                    ],
                    rowSelection: 'single',
                    onSelectionChanged: onSelectionChanged,
                };

                new agGrid.Grid(divTaskGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridOptions.api.getSelectedRows();
                    selectedRows.forEach(function (selectedRow, index) {
                        processlist.pselectedTaskEntity = selectedRow;
                    });
                }
            } else {
                $.msgBox({
                    title: "Test / Task",
                    content: kresource.getItem("tasklistloaderrormsg") + result.Message,
                    type: "error"
                });
            }

        });
    };

    processlist.getDoneList = function () {
        $('#loading-indicator').show();

        var taskQuery = {};
        taskQuery.ProcessGUID = processlist.pselectedProcessEntity.ProcessGUID;
        taskQuery.Version = processlist.pselectedProcessEntity.Version;
        taskQuery.AppName = sfconfig.Runner.AppName;
        taskQuery.AppInstanceID = sfconfig.Runner.AppInstanceID;

        jshelper.ajaxPost('api/Wf2Xml/GetDoneList', JSON.stringify(taskQuery), function (result) {
            if (result.Status === 1) {
                var divTaskGrid = document.querySelector('#myDoneGrid');
                $(divTaskGrid).empty();

                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'TaskID', width: 50 },
                        { headerName: kresource.getItem('activityname'), field: 'ActivityName', width: 160 },
                        { headerName: kresource.getItem('endedbyuserid'), field: 'EndedByUserID', width: 100 },
                        { headerName: kresource.getItem('endedbyusername'), field: 'EndedByUserName', width: 100 },
                        { headerName: kresource.getItem('endeddatetime'), field: 'EndedDateTime', width: 200 },
                    ],
                    rowSelection: 'single',
                    onSelectionChanged: onSelectionChanged,
                };

                new agGrid.Grid(divTaskGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridOptions.api.getSelectedRows();
                    selectedRows.forEach(function (selectedRow, index) {
                        processlist.pselectedTaskDoneEntity = selectedRow;
                    });
                }
            } else {
                $.msgBox({
                    title: "Test / Task",
                    content: kresource.getItem("tasklistloaderrormsg") + result.Message,
                    type: "error"
                });
            }
        });
    }

    processlist.start = function () {
        var processEntity = processlist.pselectedProcessEntity;
        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            processapi.start(sfconfig.Runner, function (result) {
                if (result.Status === 1) {
                    $.msgBox({
                        title: "Process / Start",
                        content: kresource.getItem('processstartedokmsg'),
                        type: "info"
                    });
                    processlist.getTaskList();
                    processlist.getDoneList();
                } else {
                    $.msgBox({
                        title: "Process / Start",
                        content: kresource.getItem('processstartederrormsg') + result.Message,
                        type: "warn"
                    });
                }
            })
        } else {
            $.msgBox({
                title: "Process / Start",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.run = function () {
        var processEntity = processlist.pselectedProcessEntity;

        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.AssignedToUserID;
                sfconfig.Runner["UserName"] = taskEntity.AssignedToUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                $('#modelNextStepForm').modal('show');
                nextactivitytree.showNextActivityTree(sfconfig.Runner, sfconfig.Command.RUN);
            } else {
                $.msgBox({
                    title: "Process / Run",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Process / Run",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.revise = function () {
        var processEntity = processlist.pselectedProcessEntity;

        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.AssignedToUserID;
                sfconfig.Runner["UserName"] = taskEntity.AssignedToUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                $('#modelNextStepForm').modal('show');
                nextactivitytree.showNextActivityTree(sfconfig.Runner, sfconfig.Command.REVISE);
            } else {
                $.msgBox({
                    title: "Process / Revise",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Process / Revise",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.withdraw = function () {
        var processEntity = processlist.pselectedProcessEntity;
        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskDoneEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.EndedByUserID;
                sfconfig.Runner["UserName"] = taskEntity.EndedByUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                $.msgBox({
                    title: "Are You Sure",
                    content: kresource.getItem('processwithdrawconfirmmsg'),
                    type: "confirm",
                    buttons: [{ value: "Yes" }, { value: "Cancel" }],
                    success: function (result) {
                        processapi.withdraw(sfconfig.Runner, function (result) {
                            if (result.Status === 1) {
                                $.msgBox({
                                    title: "Process / Reject",
                                    content: kresource.getItem('processwithdrawnokmsg'),
                                    type: "info"
                                });
                                processlist.getTaskList();
                                processlist.getDoneList();
                            } else {
                                $.msgBox({
                                    title: "Process / Reject",
                                    content: kresource.getItem("processwithdrawnerrormsg") + result.Message,
                                    type: "warn"
                                });
                            }
                        })
                    }
                });
            } else {
                $.msgBox({
                    title: "Process / Widthdraw",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Process / Widthdraw",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.sendback = function () {
        var processEntity = processlist.pselectedProcessEntity;
        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.AssignedToUserID;
                sfconfig.Runner["UserName"] = taskEntity.AssignedToUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                $('#modelPrevStepForm').modal('show');
                prevactivitytree.showPrevActivityTree(sfconfig.Runner);
            } else {
                $.msgBox({
                    title: "Process / Sendback",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }            
        } else {
            $.msgBox({
                title: "Process / Sendback",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.resend = function () {
        var processEntity = processlist.pselectedProcessEntity;
        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.AssignedToUserID;
                sfconfig.Runner["UserName"] = taskEntity.AssignedToUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                processapi.resend(sfconfig.Runner, function (result) {
                    if (result.Status === 1) {
                        $.msgBox({
                            title: "Process / Resend",
                            content: kresource.getItem('processresentokmsg'),
                            type: "info"
                        });
                        processlist.getTaskList();
                        processlist.getDoneList();
                    } else {
                        $.msgBox({
                            title: "Process / Resend",
                            content: kresource.getItem("processresenterrormsg") + result.Message,
                            type: "warn"
                        });
                    }
                })
            } else {
                $.msgBox({
                    title: "Process / Resend",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Process / Resend",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.reject = function () {
        var processEntity = processlist.pselectedProcessEntity;
        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.AssignedToUserID;
                sfconfig.Runner["UserName"] = taskEntity.AssignedToUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                $.msgBox({
                    title: "Are You Sure",
                    content: kresource.getItem('processrejectwarnmsg'),
                    type: "confirm",
                    buttons: [{ value: "Yes" }, { value: "Cancel" }],
                    success: function (result) {
                        processapi.reject(sfconfig.Runner, function (result) {
                            if (result.Status === 1) {
                                $.msgBox({
                                    title: "Process / Reject",
                                    content: kresource.getItem('processrejectedokmsg'),
                                    type: "info"
                                });
                                processlist.getTaskList();
                                processlist.getDoneList();
                            } else {
                                $.msgBox({
                                    title: "Process / Reject",
                                    content: kresource.getItem("processrejectederrormsg") + result.Message,
                                    type: "warn"
                                });
                            }
                        })
                    }
                });
            } else {
                $.msgBox({
                    title: "Process / Reject",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Process / Reject",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.close = function () {
        var processEntity = processlist.pselectedProcessEntity;
        if (processEntity != null) {
            sfconfig.initRunner();
            sfconfig.Runner["ProcessGUID"] = processEntity.ProcessGUID;
            sfconfig.Runner["Version"] = processEntity.Version;

            var taskEntity = processlist.pselectedTaskEntity;
            if (taskEntity != null) {
                sfconfig.Runner["UserID"] = taskEntity.AssignedToUserID;
                sfconfig.Runner["UserName"] = taskEntity.AssignedToUserName;
                sfconfig.Runner["TaskID"] = taskEntity.TaskID;

                $.msgBox({
                    title: "Are You Sure",
                    content: kresource.getItem('processclosewarnmsg'),
                    type: "confirm",
                    buttons: [{ value: "Yes" }, { value: "Cancel" }],
                    success: function (result) {
                        processapi.close(sfconfig.Runner, function (result) {
                            if (result.Status === 1) {
                                $.msgBox({
                                    title: "Process / Close",
                                    content: kresource.getItem('processclosedokmsg'),
                                    type: "info"
                                });
                                processlist.getTaskList();
                                processlist.getDoneList();
                            } else {
                                $.msgBox({
                                    title: "Process / Close",
                                    content: kresource.getItem('processclosederrormsg') + result.Message,
                                    type: "warn"
                                });
                            }
                        })
                    }
                });
            } else {
                $.msgBox({
                    title: "Process / Close",
                    content: kresource.getItem('tasklectedwarnmsg'),
                    type: "warn"
                });
            }
        } else {
            $.msgBox({
                title: "Process / Close",
                content: kresource.getItem('processselectedwarnmsg'),
                type: "warn"
            });
        }
    }

    processlist.deleteInstance = function () {
        var instanceQuery = {};
        instanceQuery.ProcessGUID = processlist.pselectedProcessEntity.ProcessGUID;
        instanceQuery.Version = processlist.pselectedProcessEntity.Version;
        instanceQuery.AppName = sfconfig.Runner.AppName;
        instanceQuery.AppInstanceID = sfconfig.Runner.AppInstanceID;

        $.msgBox({
            title: "Are You Sure",
            content: kresource.getItem('processinstanceclearwarnmsg'),
            type: "confirm",
            buttons: [{ value: "Yes" }, { value: "Cancel" }],
            success: function (result) {
                if (result == "Yes") {
                    processapi.deleteInstance(instanceQuery, function (result) {
                        if (result.Status === 1) {
                            $.msgBox({
                                title: "ProcessInstance / Delete",
                                content: kresource.getItem("processinstanceclearokmsg"),
                                type: "info"
                            });
                            processlist.getTaskList();
                            processlist.getDoneList();
                        } else {
                            $.msgBox({
                                title: "ProcessInstance / Delete",
                                content: kresource.getItem("processinstanceclearerrormsg") + result.Message, 
                                type: "error"
                            });
                        }
                    });
                }
            }
        });
    };

    return processlist;
})()

//process api
var processapi = (function () {
    function processapi() {
    }

    processapi.queryProcessFile = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/QueryProcessFile',
            JSON.stringify(query),
            function (result) {
                callback(result);
            }
        );
    }

    processapi.start = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/StartProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    //processapi.next = function (query, callback) {
    //    jshelper.ajaxPost('api/Wf2Xml/GextNextActivityUserTree',
    //        JSON.stringify(query),
    //        function (result) {
    //            callback(result);
    //        });
    //}

    processapi.next = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/GetNextStepInfo',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.run = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/RunProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.revise = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/ReviseProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    //processapi.prev = function (query, callback) {
    //    jshelper.ajaxPost('api/Wf2Xml/GetPrevActivityUserTree',
    //        JSON.stringify(query),
    //        function (result) {
    //            callback(result);
    //        });
    //}

    processapi.prev = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/GetPreviousStepInfo',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.sendback = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/SendBackProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.withdraw = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/WithdrawProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.resend = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/ResendProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.reject = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/RejectProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.close = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/CloseProcess',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });
    }

    processapi.deleteInstance = function (query, callback) {
        jshelper.ajaxPost('api/Wf2Xml/DeleteInstance',
            JSON.stringify(query),
            function (result) {
                callback(result);
            });   
    }

    return processapi;
})()