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

var nextactivitytree = (function () {
    function nextactivitytree() {
    }

    nextactivitytree.mzTree = null;
    nextactivitytree.mstepName = '';
    nextactivitytree.mEntity = null;
    nextactivitytree.mCommand = '';

    function getZTreeSetting() {
        var setting = {
            check: {
                enable: true
            },
            view: {
                //addHoverDom: addHoverDom,
                //removeHoverDom: removeHoverDom,
                dblClickExpand: false,
                showLine: true,
                selectedMulti: false
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: ""
                }
            },
            callback: {
                beforeClick: function (treeId, treeNode) {
                    var zTree = $.fn.zTree.getZTreeObj("nextStepTree");
                    if (treeNode.isParent) {
                        zTree.expandNode(treeNode);
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        };
        return setting;
    }

    nextactivitytree.reload = function () {
        var runner = sfconfig.Runner;
        runner.Conditions = {};
        
        var jsonString = $("#txtConditionVariables").val();
        var kvpair = jsonString.split(',');

        for (var i = 0; i < kvpair.length; i++) {
            var pair = kvpair[i].split(':');
            var key = pair[0].trim();
            var value = pair[1].trim();
            runner.Conditions[key] = value;
        }
        //加载步骤列表
        //$("#nextStepTree").empty();
        nextactivitytree.showNextActivityTree(runner, nextactivitytree.mCommand);
    }

    nextactivitytree.showNextActivityTree = function (runner, command) {
        $("#nextStepTree").empty();
        nextactivitytree.mCommand = command;

        //获取下一步
        processapi.next(runner, function (result) {
            if (result.Status === 1) {
                //弹窗步骤人员办理弹窗
                var zNodes = [{ id: 0, pId: -1, name: kresource.getItem("nextstepinfo"), type: "root", open: true }];
                var index = 0;
                var parent = 0;
                var steps = result.Entity.NextActivityRoleUserTree;

                //如果下一步为空，则给出提示信息
                if (steps.length === 0) {
                    var alertMessage = result.Entity.Message;
                    kmsgbox.warn(kresource.getItem("nextsteptreeshowalertmsg"), alertMessage);
                    return;
                }

                //添加步骤节点
                for (var i = 0; i < steps.length; i++) {
                    var nextStep = steps[i];
                    index = index + 1;
                    var stepNode = {
                        id: index,
                        pId: 0,
                        name: nextStep.ActivityName,
                        activityGUID: nextStep.ActivityGUID,
                        activityName: nextStep.ActivityName,
                        type: "activity",
                        open: false
                    }
                    zNodes.push(stepNode);

                    //添加用户节点
                    if (nextStep.Users != null) {
                        parent = index;
                        var userNode = null;
                        $.each(nextStep.Users, function (i, o) {
                            index = index + 1;
                            userNode = {
                                id: index,
                                pId: parent,
                                name: o.UserName,
                                uid: o.UserID,
                                type: "user"
                            };
                            zNodes.push(userNode);
                        });
                    }
                }              

                var t = $("#nextStepTree");
                nextactivitytree.mzTree = $.fn.zTree.init(t, getZTreeSetting(), zNodes);
            } else {
                kmsgbox.warn(kresource.getItem("nextsteptreeshowerrormsg"), result.Message);
            }
         });
    }

    nextactivitytree.sure = function () {
        //取得下一步节点信息
        var selectedNodes = nextactivitytree.mzTree.getCheckedNodes();
        if (selectedNodes.length <= 0) {
            kmsgbox.warn(kresource.getItem("nextstepinfowarnmsg"));
            return false;
        }

        var wfAppRunner = sfconfig.Runner;
        wfAppRunner.NextActivityPerformers = {};

        //多步可选
        var activityGUID = "";
        $.each(selectedNodes, function (i, o) {
            if (o.type === "activity" && o.pId === 0) {
                activityGUID = o.activityGUID;
                wfAppRunner.NextActivityPerformers[activityGUID] = getPerformerList(o);
            } 
        });

        //加载控制参数
        var includeControlParameters = $("#chkControlParameters").is(':checked');
        if (includeControlParameters === true) {
            wfAppRunner.ControlParameterSheet = {};

            var jsonControlString = $("#txtControlParameters").val();
            var kvpairControl = jsonControlString.split(',');

            for (var i = 0; i < kvpairControl.length; i++) {
                var pairControl = kvpairControl[i].split(':');
                var keyControl = pairControl[0].trim();
                var valueControl = pairControl[1].trim();
                wfAppRunner.ControlParameterSheet[keyControl] = valueControl;
            }
        }

        //加载动态变量
        var includeDynamicVariables = $("#chkVariableParameters").is(':checked');
        if (includeDynamicVariables === true) {
            wfAppRunner.DynamicVariables = {};

            var jsonVariableString = $("#txtVariableParameters").val();
            var kvpairVariable = jsonVariableString.split(',');

            for (var i = 0; i < kvpairVariable.length; i++) {
                var pv = kvpairVariable[i].split(':');
                var keyVariable = pv[0].trim();
                var valueVariable = pv[1].trim();
                wfAppRunner.DynamicVariables[keyVariable] = valueVariable;
            }
        }

        if (nextactivitytree.mCommand === sfconfig.Command.RUN) {
            //流程流转到下一步
            processapi.run(wfAppRunner, function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(kresource.getItem("processrunokmsg"));
                    $("#modelNextStepForm").modal("hide");

                    //刷新任务列表
                    refreshTaskList();
                } else {
                    kmsgbox.warn(kresource.getItem("processrunerrormsg"), result.Message);
                }
            });
        } else if (nextactivitytree.mCommand === sfconfig.Command.REVISE) {
            //流程流转到下一步
            processapi.revise(wfAppRunner, function (result) {
                if (result.Status === 1) {
                    kmsgbox.info(kresource.getItem("processreviseokmsg"));
                    $("#modelNextStepForm").modal("hide");

                    //刷新任务列表
                    refreshTaskList();
                } else {
                    kmsgbox.error(kresource.getItem("processreviseerrormsg"), result.Message);
                }
            });
        }
    }

    function refreshTaskList() {
        processlist.getTaskList();
        processlist.getDoneList();
    }

    var getPerformerList = function (node) {
        var user = null;
        var userlist = [];
        var parent = node.id;

        if (node.children) {
            $.each(node.children, function (i, o) {
                if (o.type === "user" && o.pId === parent && o.checked === true) {
                    user = { UserID: o.uid, UserName: o.name };
                    userlist.push(user);
                }
            })
        }
        return userlist;
    }
    return nextactivitytree;
})();
