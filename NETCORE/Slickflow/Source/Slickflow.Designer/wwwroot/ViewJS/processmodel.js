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

var processmodel = (function () {
    function processmodel() {
    }

    processmodel.mprocessTemplate = {};
    processmodel.mprocessTemplate["Sequence"] = "";
    processmodel.mprocessTemplate["Parallel"] = "";
    processmodel.mprocessTemplate["BatchUpdate"] = "";
    processmodel.mcodemirrorEditor = null;
    processmodel.mneedRefreshTaskList = false;

    processmodel.initLang = function () {
        $("#ddlTemplateType").change(function (i, o) {
            var option = $(this).val();
            if (option !== "default") {
                processapi.loadTemplate(option);
                if (option === "RunProcess") {
                    processmodel.mneedRefreshTaskList = true;
                    $("#divTaskList").show();
                } else {
                    processmodel.mneedRefreshTaskList = false;
                    $("#divTaskList").hide();
                }
            } else
                processmodel.setDefaultCodeText();
        });
    }

    processmodel.setDefaultCodeText = function () {
        var defaultText = "//1)you can create a process diagram through process graph language.\n//2)three language templates have been loaded. please try it."
            + "\n//3)you can also learn it from codeproject article: https://www.codeproject.com/Articles/5252483/Slickflow-Coding-Graphic-Model-User-Manual";
        processmodel.mcodemirrorEditor.setCode(defaultText);
    }

    processmodel.executeGraph = function () {
        var text = processmodel.mcodemirrorEditor.getCode();
        if (text !== "") {
            var entity = { "Body": text };
            processapi.executeProcessGraph(entity, function (entity) {
                kmain.mxGraphEditor.graph.getModel().clear();
                var graphData = kloader.load(entity);
                if (processmodel.mneedRefreshTaskList === true) {
                    processmodel.refreshTask();
                }
            });
        }
        else {
            kmsgbox.warn(kresource.getItem('domainlangwwarnmsg'));
        }
    }

    processmodel.refreshTask = function () {
        getTaskList();
        getDoneList();
    }

    function getTaskList() {
        $('#loading-indicator').show();

        jshelper.ajaxGet('api/Wf2Xml/GetTaskToDoListTop', null, function (result) {
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

        jshelper.ajaxGet('api/Wf2Xml/GetTaskDoneListTop', null, function (result) {
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

    processmodel.gotoTutorial = function () {
        var lang = kresource.getLang();
        var url = (lang === "zh") ? "https://www.cnblogs.com/slickflow/p/11936786.html"
            : "https://www.codeproject.com/Articles/5252483/Slickflow-Coding-Graphic-Model-User-Manual";
        window.open(url, "_blank");
    }
    return processmodel;
})();