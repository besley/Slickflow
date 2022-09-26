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

var prevactivitytree = (function () {
    function prevactivitytree() {
    }

    prevactivitytree.mzTree = null;
    prevactivitytree.mstepName = '';
    prevactivitytree.mEntity = null;

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
                    var zTree = $.fn.zTree.getZTreeObj("prevStepTree");
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

    prevactivitytree.reload = function () {
        var runner = sfconfig.Runner;

        //加载步骤列表
        $("#prevStepTree").empty();
        prevactivitytree.showPrevActivityTree(runner);
    }

    prevactivitytree.showPrevActivityTree = function (runner) {
        //获取下一步
        processapi.prev(runner, function (result) {
            if (result.Status === 1) {
                //弹窗步骤人员办理弹窗
                var zNodes = [{ id: 0, pId: -1, name: kresource.getItem("previousstepinfo"), type: "root", open: true }];
                var index = 0;
                var parent = 0;
                var stepInfo = result.Entity;
                var steps = stepInfo.PreviousActivityRoleUserTree;
                var hasGatewayPassed = stepInfo.HasGatewayPassed;

                if (hasGatewayPassed === true) {
                    $("#divSendBackOptions").show();
                } else {
                    $("#divSendBackOptions").hide();
                }

                //添加步骤节点
                for (var i = 0; i < steps.length; i++) {
                    var prevStep = steps[i];
                    index = index + 1;
                    var stepNode = {
                        id: index,
                        pId: 0,
                        name: prevStep.ActivityName,
                        activityGUID: prevStep.ActivityGUID,
                        activityName: prevStep.ActivityName,
                        type: "activity",
                        open: false
                    }
                    zNodes.push(stepNode);

                    //添加用户节点
                    if (prevStep.Users != null) {
                        parent = index;
                        var userNode = null;
                        $.each(prevStep.Users, function (i, o) {
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

                var t = $("#prevStepTree");
                prevactivitytree.mzTree = $.fn.zTree.init(t, getZTreeSetting(), zNodes);
            } else {
                kmsgbox.warn(kresource.getItem("prevsteptreeshowerrormsg"), result.Message);
            }
        });
    }

    prevactivitytree.sure = function () {
        //取得下一步节点信息
        var selectedNodes = prevactivitytree.mzTree.getCheckedNodes();
        if (selectedNodes.length <= 0) {
            kmsgbox.warn(kresource.getItem("previousstepinfowarnmsg"));
            return false;
        }

        var wfAppRunner = sfconfig.Runner;
        wfAppRunner.NextActivityPerformers = {};

        //多步可选
        var activityGUID = "";
        $.each(selectedNodes, function (i, o) {
            if (o.type === "activity" && o.pId === 0) {
                activityGUID = o.activityGUID;
                if (wfAppRunner.NextActivityPerformers[activityGUID]) {
                    //重复的步骤，需要整合在一个节点下面
                    //并行会签会追加在同一ActivityGUID下
                    //会签加签节点GUID和名称相同，但是有不同的两条节点活动实例记录，不能使用Dictionary数据结构，需要合并在同一个节点记录下。
                    appendPerformer(wfAppRunner.NextActivityPerformers[activityGUID], getPerformerList(o));
                } else {
                    wfAppRunner.NextActivityPerformers[activityGUID] = getPerformerList(o);
                }
            }
        });

        //退回参数
        var isCancellingBrotherNodes = $("#chkCancelBrothersNode").prop('checked');
        wfAppRunner.ControlParameterSheet = {};
        wfAppRunner.ControlParameterSheet["IsCancellingBrothersNode"] = isCancellingBrotherNodes === true? 1: 0;

        //流程退回到上一步
        processapi.sendback(wfAppRunner, function (result) {
            if (result.Status === 1) {
                kmsgbox.info(kresource.getItem("processsendbackokmsg"));
                $("#modelPrevStepForm").modal("hide");

                //刷新任务列表
                processlist.getTaskList();
                processlist.getDoneList();
            } else {
                kmsgbox.error(kresource.getItem("processsendbackerrormsg"), result.Message);
            }
        });
    }

    var appendPerformer = function (originalUserList, userList) {
        $.each(userList, function (i, u) {
            originalUserList.push(u);
        });
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
    return prevactivitytree;
})();
