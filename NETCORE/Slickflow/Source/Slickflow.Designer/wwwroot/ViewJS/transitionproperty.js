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

var transitionproperty = (function () {
    function transitionproperty() {
    }

    transitionproperty.mIsGroupBehaviousPriorityEnabled = false;
    transitionproperty.mIsGroupBehaviousForcedBranchEnabled = false;

    transitionproperty.load = function () {
        var transition = kmain.mxSelectedDomElement.ElementObject;

        if (transition) {
            $("#txtDescription").val(transition.description);
            if (transition.receiver) {
                if (transition.receiver.type)
                    $("#ddlReceiverType").val(transition.receiver.type);
            }

            if (transition.condition)
                $("#txtCondition").val($.trim(transition.condition.text));

            showTranstionGroupBehaviours(transition);
        }
    }

    function showTranstionGroupBehaviours(transition) {
        $("#divSplitOptions").hide();
        $("#divJoinOptions").hide();

        var graph = kmain.mxGraphEditor.graph;
        var cell = graph.getSelectionCell();

        //source node connected with the transition
        var sourceNode = graph.getModel().getValue(cell.source);         //source node
        if (sourceNode) {
            var activityTypeElement = sourceNode.getElementsByTagName("ActivityType")[0];
            var activityType = activityTypeElement.getAttribute("type");
            if (activityType === kmodel.ActivityType.GatewayNode) {
                var splitJoinType = activityTypeElement.getAttribute("gatewaySplitJoinType");
                var direction = activityTypeElement.getAttribute("gatewayDirection");
                if (splitJoinType === gatewayproperty.Direction.Split) {
                    if (direction === gatewayproperty.Direction.XOrSplit) {
                        transitionproperty.mIsGroupBehaviousPriorityEnabled = true;
                        $("#divSplitOptions").show();
                        if (transition.groupBehaviours && transition.groupBehaviours.priority) {
                            $("#txtPriority").val(transition.groupBehaviours.priority);
                        }
                    }
                }
            }
        }

        //target node connected with the transition
        var targetNode = graph.getModel().getValue(cell.target);
        if (targetNode) {
            var activityTypeElement = targetNode.getElementsByTagName("ActivityType")[0];
            var activityType = activityTypeElement.getAttribute("type");
            if (activityType == kmodel.ActivityType.GatewayNode) {
                var splitJoinType = activityTypeElement.getAttribute("gatewaySplitJoinType");
                var direction = activityTypeElement.getAttribute("gatewayDirection");
                var joinPass = activityTypeElement.getAttribute("gatewayJoinPass");
                if (splitJoinType === gatewayproperty.Direction.Join) {
                    if (direction === gatewayproperty.Direction.EOrJoin) {
                        if (joinPass === gatewayproperty.JoinPass.ForcedBranchPass) {
                            transitionproperty.mIsGroupBehaviousForcedBranchEnabled = true;
                            $("#divJoinOptions").show();
                            if (transition.groupBehaviours && transition.groupBehaviours.forced) {
                                if (transition.groupBehaviours.forced === "true") {
                                    $("#chkForced").prop("checked", true);
                                }
                                else {
                                    $("#chkForced").prop("checked", false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    transitionproperty.save = function () {
        var description = $("#txtDescription").val();
        var receiver = {};
        var receiverType = $("#ddlReceiverType").val();
        if (receiverType !== "default") receiver.type = receiverType;

        var condition = {};
        condition.type = "Expression";
        condition.text = $.trim($("#txtCondition").val());

        var groupBehaviours = {};
        if (transitionproperty.mIsGroupBehaviousPriorityEnabled === true) {
            var priority = $("#txtPriority").val();
            if (priority) {
                if (jshelper.isNumber(priority) === true) {
                    groupBehaviours.priority = parseInt(priority);
                } else {
                    $.msgBox({
                        title: "Transition / Property",
                        content: kresource.getItem("transitionprioritysavewarnmsg"),
                        type: "alert"
                    });
                }
            }
        }

        if (transitionproperty.mIsGroupBehaviousForcedBranchEnabled === true) {
            var forced = $("#chkForced").is(':checked');
            if (forced === true) {
                groupBehaviours.forced = "true";
            } else {
                groupBehaviours.forced = "false";
            }
        }

        var transition = kmain.mxSelectedDomElement.ElementObject;
        if (transition !== null){
            transition.description = description;
            transition.receiver = receiver;
            transition.condition = condition;
            transition.groupBehaviours = groupBehaviours;

		    //update line label text
            kmain.setEdgeValue(transition);
        }
    }

    return transitionproperty;
})()