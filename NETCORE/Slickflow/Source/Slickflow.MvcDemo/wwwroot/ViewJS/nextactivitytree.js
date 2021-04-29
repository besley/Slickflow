var nextactivitytree = (function () {
    function nextactivitytree() {
    }
    nextactivitytree.mzTree = null;
    nextactivitytree.mstepName = '';
    nextactivitytree.mEntity = null;

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

    nextactivitytree.showNextActivityTree = function (entity, step) {
        nextactivitytree.mstepName = step;
        nextactivitytree.mEntity = entity;

        var url = '';
        if (step === "dispatch")
            url = 'api/wf/GetFirstStepRoleUserTree';
        else
            url = 'api/wf/GetNextStepRoleUserTree';

        //get next step information
        jshelper.ajaxPost(url,
            JSON.stringify(entity.WfAppRunner),
            function (result) {
                if (result.Status === 1) {
                    //弹窗步骤人员办理弹窗
                    var nextStep = result.Entity[0];        //单步演示
                    var zNodes = [
                         { id: 0, pId: -1, name: "下一步流转", type: "root", open: true },
                         {
                             id: 1,
                             pId: 0,
                             name: nextStep.ActivityName,
                             activityGUID: nextStep.ActivityGUID,
                             activityName: nextStep.ActivityName,
                             type: "activity",
                             open: false
                         }
                    ];

                    if (nextStep.Users != null) {
                        var id = 2;
                        var userNode = null;
                        $.each(nextStep.Users, function (i, o) {
                            userNode = {
                                id: id,
                                pId: 1,
                                name: o.UserName,
                                uid: o.UserID,
                                type: "user"
                            };
                            zNodes.push(userNode);
                            id = id + 1;
                        });
                    }

                    var t = $("#nextStepTree");
                    nextactivitytree.mzTree = $.fn.zTree.init(t, getZTreeSetting(), zNodes);
                } else {
                    $.msgBox({
                        title: "DynFlow / GoNext",
                        content: result.Message,
                        type: "info"
                    });
                }
            }
        );
    }

    nextactivitytree.sure = function () {
        //取得下一步节点信息
        var selectedNodes = nextactivitytree.mzTree.getCheckedNodes();
        if (selectedNodes.length <= 0) {
            $.msgBox({
                title: "DynFlow / GoNext",
                content: "请单击选中下一步人员列表再进行流转！",
                type: "alert"
            });
            return false;
        }

        //单步可选示例
        var nextStep = {};
        nextStep.Users = [];
        var activityGUID = "", activityName="";
        var user = null;
        var userlist = [];
        $.each(selectedNodes, function (i, o) {
            if (o.type === "activity") {
                activityGUID = o.activityGUID;
                activityName = o.activityName;
            } else if (o.type === "user") {
                user = { UserID: o.uid, UserName: o.name };
                userlist.push(user);
            }
        });

        var WfAppRunner = nextactivitytree.mEntity.WfAppRunner;
        WfAppRunner.NextActivityPerformers = {};
        WfAppRunner.NextActivityPerformers[activityGUID] = userlist;
        
        $.msgBox({
            title: "Are You Sure",
            content: "请确认下一步是否要流转到步骤：" + activityName,
            type: "confirm",
            buttons: [{ value: "Yes" }, { value: "Cancel" }],
            success: function (result) {
                if (result === "Yes") {
                    jshelper.ajaxPost("api/productorder/" + nextactivitytree.mstepName,
                        JSON.stringify(nextactivitytree.mEntity),
                        function (result) {
                            if (result.Status === 1) {
                                $.msgBox({
                                    title: "DynFlow / GoNext",
                                    content: "流程已经流转到下一步！",
                                    type: "info"
                                });
                                $("#modelNextStepForm").modal("hide");
                            } else {
                                $.msgBox({
                                    title: "DynFlow / GoNext",
                                    content: result.Message,
                                    type: "info"
                                });
                            }
                        });
                }
            }
        });
    }
    return nextactivitytree;
})();
