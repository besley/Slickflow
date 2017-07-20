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

var mxfile = (function () {
    function mxfile() {
    }

    mxfile.getActivityObject = function (activityElement, nameType) {
        var activity = {};

        activity.id = activityElement.getAttribute("id");
        if (nameType === "name") {
            activity.name = activityElement.getAttribute("name");
        } else if (nameType === "label") {
            activity.name = activityElement.getAttribute("label");
        }
        activity.code = activityElement.getAttribute("code");

        return activity;
    }

    mxfile.setActivityTypeElement = function (activityTypeElement, activity) {
        if (activity.type === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
            activityTypeElement.setAttribute("complexType", activity.complexType);
            activityTypeElement.setAttribute("mergeType", activity.mergeType);
            activityTypeElement.setAttribute("compareType", activity.compareType);
            activityTypeElement.setAttribute("completeOrder", activity.completeOrder);
        } else if (activity.type === kmodel.Config.NODE_TYPE_GATEWAY) {
            activityTypeElement.setAttribute("gatewaySplitJoinType", activity.gatewaySplitJoinType);
            activityTypeElement.setAttribute("gatewayDirection", activity.gatewayDirection);
        } else if (activity.type === kmodel.Config.NODE_TYPE_SUBPROCESS) {
            activityTypeElement.setAttribute("subId", activity.subId);
        } 
        return activityTypeElement;
    }

    mxfile.getActivityTypeObject = function (activity, activityTypeElement) {
        activity.type = activityTypeElement.getAttribute("type");
        if (activity.type === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
            activity.complexType = activityTypeElement.getAttribute("complexType");
            activity.mergeType = activityTypeElement.getAttribute("mergeType");
            activity.compareType = activityTypeElement.getAttribute("compareType");
            activity.completeOrder = activityTypeElement.getAttribute("completeOrder");
        } else if (activity.type === kmodel.Config.NODE_TYPE_GATEWAY) {
            activity.gatewaySplitJoinType = activityTypeElement.getAttribute("gatewaySplitJoinType");
            activity.gatewayDirection = activityTypeElement.getAttribute("gatewayDirection");
        } else if (activity.type === kmodel.Config.NODE_TYPE_SUBPROCESS) {
            activity.subId = activityTypeElement.getAttribute("subId");
        } 
        return activity;
    }

    mxfile.setPerformersElement = function (doc, performers) {
        var performersElement = doc.createElement("Performers");
        var performerElement = null;

        for (var i = 0; i < performers.length; i++) {
            //convert performer to performerNode
            performerElement = mxfile.setPerformerElement(doc, performers[i]);
            performersElement.appendChild(performerElement);
        }
        return performersElement;
    }

    mxfile.setPerformerElement = function (doc, performer) {
        var performerElement = doc.createElement("Performer");
        performerElement.setAttribute("id", performer.id);

        return performerElement;
    }

    mxfile.getPerformerObject = function (performerElement) {
        var performer = {};
        performer.id = performerElement.getAttribute("id");

        return performer;
    }

    mxfile.setActionsElement = function (doc, actions) {
        var actionsElement = doc.createElement("Actions");
        var actionElement = null;

        for (var i = 0; i < actions.length; i++) {
            //convert action to actionElement
            var actionElement = doc.createElement("Action");
            actionElement = mxfile.setActionElement(actionElement, actions[i]);
            actionsElement.appendChild(actionElement);
        }
        return actionsElement;
    }

    mxfile.setActionElement = function (actionElement, action) {
        actionElement.setAttribute("type", action.type);
        actionElement.setAttribute("name", action.name);
        actionElement.setAttribute("assembly", action.assembly);
        actionElement.setAttribute("interface", action.interface);
        actionElement.setAttribute("method", action.method);

        return actionElement;
    }

    mxfile.getActionObject = function (actionElement) {
        var action = {};
        action.type = actionElement.getAttribute("type");
        action.name = actionElement.getAttribute("name");
        action.assembly = actionElement.getAttribute("assembly");
        action.interface = actionElement.getAttribute("interface");
        action.method = actionElement.getAttribute("method");

        return action;
    }

    mxfile.setTransitionElement = function (doc, transition) {
        var transitonElement = doc.createElement('Transition');
        transitonElement.setAttribute('from', transition.from);
        transitonElement.setAttribute('to', transition.to);

        //description
        var descElement = doc.createElement('Description');
        transitonElement.appendChild(descElement);

        var description = doc.createTextNode(transition.description);
        descElement.appendChild(description);

        //set edge text
        transitonElement.setAttribute("label", transition.description);

        return transitonElement;
    }

    mxfile.getTransitionObject = function (transitionElement) {
        var transition = {};
        transition.id = transitionElement.getAttribute("id");
        transition.from = transitionElement.getAttribute("from");
        transition.to = transitionElement.getAttribute("to");

        return transition;
    }

    mxfile.setConditionElement = function (doc, condition) {
        var conditionElement = doc.createElement('Condition');
        conditionElement.setAttribute('type', condition.type);

        var conditionTextElement = doc.createElement('ConditionText');
        conditionElement.appendChild(conditionTextElement);

        var expression = doc.createTextNode($.trim(condition.text));
        conditionTextElement.appendChild(expression);    

        return conditionElement;
    }

    mxfile.getConditionObject = function (conditionElement) {
        var condition = {};
        condition.type = conditionElement.getAttribute("type");

        var conditionTextElement = conditionElement.getElementsByTagName("ConditionText")[0];
        if (conditionTextElement) condition.text = jshelper.replaceHTMLTags(conditionElement.textContent);

        return condition;
    }

    mxfile.setReceiverElement = function (doc, receiver) {
        var receiverElement = doc.createElement("Receiver");
        receiverElement.setAttribute('type', receiver.type);

        return receiverElement;
    }

    mxfile.getReceiverObject = function (receiverElement) {
        var receiver = {};
        var receiverType = receiverElement.getAttribute("type");
        if (receiverType !== undefined) {
            receiver.type = receiverType;
        }
        return receiver;
    }

    mxfile.getGeographyEdgeObject = function (geographyElement) {
        var geography = {};
        geography.parent = geographyElement.getAttribute("parent");
        geography.style = geographyElement.getAttribute("style");

        return geography;
    }

    mxfile.getGeographyVertexObject = function (geographyElement) {
        var geography = {};
        geography.parent = geographyElement.getAttribute("parent");
        geography.style = geographyElement.getAttribute("style");

        var widgetElement = geographyElement.getElementsByTagName("Widget")[0];
        if (widgetElement) {
            geography.widget = mxfile.getWidgetVertexObject(widgetElement);
        }
        return geography;
    }

    mxfile.getWidgetVertexObject = function (widgetElement) {
        var widget = {};
        widget.left = parseInt(widgetElement.getAttribute("left"));
        widget.top = parseInt(widgetElement.getAttribute("top"));
        widget.width = parseInt(widgetElement.getAttribute("width"));
        widget.height = parseInt(widgetElement.getAttribute("height"));

        return widget;
    }

    mxfile.getParticipantObject = function (participantElement) {
        var participant = {};

        participant.type = participantElement.getAttribute("type");
        participant.id = participantElement.getAttribute("id");
        participant.name = participantElement.getAttribute("name");
        participant.code = participantElement.getAttribute("code");
        participant.outerId = participantElement.getAttribute("outerId");

        return participant;
    }

    mxfile.setParticipantElement = function (doc, participant) {
        var participantElement = doc.createElement("Participant");
        participantElement.setAttribute('type', participant.type);
        participantElement.setAttribute('id', participant.id);
        participantElement.setAttribute('name', participant.name);
        participantElement.setAttribute('code', participant.code);
        participantElement.setAttribute('outerId', participant.outerId);

        return participantElement;
    }

    mxfile.getSwimlaneObject = function (swimlaneElement) {
        var swimlane = {};
        swimlane.id = swimlaneElement.getAttribute("id");
        swimlane.name = swimlaneElement.getAttribute("name");

        return swimlane;
    }

    mxfile.getGeographySwimlaneObject = function (geographyElement) {
        var geography = {};
        geography.parent = geographyElement.getAttribute("parent");
        geography.style = geographyElement.getAttribute("style");

        var widgetElement = geographyElement.getElementsByTagName("Widget")[0];
        if (widgetElement) {
            var widget = {};
            widget.left = parseInt(widgetElement.getAttribute("left"));
            widget.top = parseInt(widgetElement.getAttribute("top"));
            widget.width = parseInt(widgetElement.getAttribute("width"));
            widget.height = parseInt(widgetElement.getAttribute("height"));
            geography.widget = widget;
        }
        return geography;
    }

    return mxfile;
})()