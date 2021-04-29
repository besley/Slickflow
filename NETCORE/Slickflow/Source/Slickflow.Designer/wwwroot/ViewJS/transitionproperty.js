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
    transitionproperty.mIsGroupBehaviousApprovalStatusEnabled = false;
    transitionproperty.mIsGroupBehaviousDefaultBrachOption = false;

    //div display options
    var myDivOptions = {};
    myDivOptions["default"] = ["divConditions", "divXOrSplitOptions", "divApprovalOrSplitOptions", "divEOrJoinOptions"];
    myDivOptions["OrSplit"] = ["divConditions", "divDefaultBranchOptions"];
    myDivOptions["XOrSplit"] = ["divConditions", "divXOrSplitOptions"];
    myDivOptions["ApprovalOrSplit"] = ["divApprovalOrSplitOptions"];
    myDivOptions["EOrJoin"] = ["divEOrJoinOptions"];
    myDivOptions["else"] = ["divConditions"];


    transitionproperty.load = function () {
        showDivOptionsCotnrolBySplitType("default");
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

                //show divoptions control by dirction type
                showDivOptionsCotnrolBySplitType(direction);
                if (splitJoinType === gatewayproperty.Direction.Split) {
                    if (direction === gatewayproperty.Direction.OrSplit) {
                        //OrSplit
                        transitionproperty.mIsGroupBehaviousDefaultBrachOption = true;
                        if (transition.groupBehaviours && transition.groupBehaviours.defaultBranch) {
                            if (transition.groupBehaviours.defaultBranch === "true") {
                                $("#chkDefaultBranch").prop("checked", true);
                            } else {
                                $("#chkDefaultBranch").prop("checked", false);
                            } 
                        } else {
                            $("#chkDefaultBranch").prop("checked", false);
                        }
                    }
                    else if (direction === gatewayproperty.Direction.XOrSplit) {
                        //XOrSplit
                        transitionproperty.mIsGroupBehaviousPriorityEnabled = true;
                        if (transition.groupBehaviours && transition.groupBehaviours.priority) {
                            $("#txtPriority").val(transition.groupBehaviours.priority);
                        }
                    } else if (direction === gatewayproperty.Direction.ApprovalOrSplit) {
                        //ApprovalOrSplit
                        transitionproperty.mIsGroupBehaviousApprovalStatusEnabled = true;
                        if (transition.groupBehaviours && transition.groupBehaviours.approval) {
                            if (transition.groupBehaviours.approval === "1") {
                                $("#radAgreed").prop('checked', true);
                            } else if (transition.groupBehaviours.approval === "-1") {
                                $("#radRefused").prop('checked', true);
                            }
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
            if (activityType === kmodel.ActivityType.GatewayNode) {
                var splitJoinType = activityTypeElement.getAttribute("gatewaySplitJoinType");
                var direction = activityTypeElement.getAttribute("gatewayDirection");
                var joinPass = activityTypeElement.getAttribute("gatewayJoinPass");
                //show divoptions control by dirction type
                showDivOptionsCotnrolBySplitType(direction);
                if (splitJoinType === gatewayproperty.Direction.Join) {
                    //EOrJoin
                    if (direction === gatewayproperty.Direction.EOrJoin) {
                        if (joinPass === gatewayproperty.JoinPass.ForcedBranchPass) {
                            transitionproperty.mIsGroupBehaviousForcedBranchEnabled = true;
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

    function showDivOptionsCotnrolBySplitType(direction) {
        var divArray = myDivOptions[direction];
        if (divArray !== undefined) {
            $.each(divArray, function (i) {
                var divOptionName = divArray[i];
                if (direction === "default") {
                    $("#" + divOptionName).hide();
                } else {
                    $("#" + divOptionName).show();
                }
            });
        } else {
            var divElse = myDivOptions["else"];
            $("#" + divElse).show();
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
        //OrSplit
        if (transitionproperty.mIsGroupBehaviousDefaultBrachOption === true) {
            var defaultBranch = $("#chkDefaultBranch").is(':checked');
            if (defaultBranch === true) {
                groupBehaviours.defaultBranch = "true";
            } else {
                groupBehaviours.defaultBranch = "false";
            }
        }

        //XOrSplit
        if (transitionproperty.mIsGroupBehaviousPriorityEnabled === true) {
            var priority = $("#txtPriority").val();
            if (priority) {
                if (jshelper.isNumber(priority) === true) {
                    groupBehaviours.priority = parseInt(priority);
                } else {
                    kmsgbox.warn(kresource.getItem("transitionprioritysavewarnmsg"));
                }
            }
        }

        //EOrJoin
        if (transitionproperty.mIsGroupBehaviousForcedBranchEnabled === true) {
            var forced = $("#chkForced").is(':checked');
            if (forced === true) {
                groupBehaviours.forced = "true";
            } else {
                groupBehaviours.forced = "false";
            }
        }

        //ApprovalOrSplit
        if (transitionproperty.mIsGroupBehaviousApprovalStatusEnabled === true) {
            var agreed = $("#radAgreed").is(':checked');
            if (agreed === true) {
                groupBehaviours.approval = 1;           //agreed
            } 

            var refused = $("#radRefused").is(':checked');
            if (refused === true) {
                groupBehaviours.approval = -1;       //refused
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