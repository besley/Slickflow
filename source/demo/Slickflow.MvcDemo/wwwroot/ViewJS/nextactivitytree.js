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
                    //Pop up steps for personnel to handle pop ups
                    var nextStep = result.Entity[0];        //Single Step Demo
                    var zNodes = [
                         { id: 0, pId: -1, name: "Next Step Info", type: "root", open: true },
                         {
                             id: 1,
                             pId: 0,
                             name: nextStep.ActivityName,
                             activityId: nextStep.ActivityId,
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
                                uid: o.UserId,
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
        //Get next step info
        var selectedNodes = nextactivitytree.mzTree.getCheckedNodes();
        if (selectedNodes.length <= 0) {
            $.msgBox({
                title: "DynFlow / GoNext",
                content: "Please select the next performer list to run！",
                type: "alert"
            });
            return false;
        }

        //单步可选示例
        //Single step optional example
        var nextStep = {};
        nextStep.Users = [];
        var activityId = "", activityName="";
        var user = null;
        var userlist = [];
        $.each(selectedNodes, function (i, o) {
            if (o.type === "activity") {
                activityId = o.activityId;
                activityName = o.activityName;
            } else if (o.type === "user") {
                user = { UserId: o.uid, UserName: o.name };
                userlist.push(user);
            }
        });

        var WfAppRunner = nextactivitytree.mEntity.WfAppRunner;
        WfAppRunner.NextActivityPerformers = {};
        WfAppRunner.NextActivityPerformers[activityId] = userlist;

        //console.log(nextactivitytree.mstepName);
        //console.log(nextactivitytree.mEntity);
        
        $.msgBox({
            title: "Are You Sure",
            content: "Please confirm next step：" + activityName,
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
                                    content: "The process has moved on to the next step！",
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
