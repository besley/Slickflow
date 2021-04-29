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

var kvalidator = (function () {
    function kvalidator() {
    }

    kvalidator.prepareValidateEntity = function() {
        var prepareDetail = {};
        prepareDetail["type"] = "OK";

        var vertexList = [];
        var activityList = [];
        var transitionList = [];
        var model = kmain.mxGraphEditor.graph.getModel();

        //find start vertex
        var snode = null,
            childNode = null,
            childNodeValue = null,
            activityTypeNode = null,
            startActivityGUID = '',
            activityType = '';

        var vertexList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        $.each(vertexList, function (i, vertex) {
            snode = model.getValue(vertex);
            if (snode.nodeName === "Activity") {
                //Activity Type
                activityTypeNode = snode.getElementsByTagName("ActivityType")[0];
                activityType = activityTypeNode.getAttribute("type");
                if (activityType === kmodel.Config.NODE_TYPE_START) {
                    startActivityGUID = vertex.id;
                }
                var activity = {};
                activity["ActivityGUID"] = vertex.id;
                activity["ActivityName"] = snode.getAttribute('label');
                activityList.push(activity);

            } else if (snode.nodeName === "Swimlane") {
                //activities in the swimlane
                var childNodeList = model.getChildVertices(model.getCell(vertex.id));
                for (var j = 0; j < childNodeList.length; j++) {
                    childNode = childNodeList[j];
                    childNodeValue = model.getValue(childNode);
                    if (childNodeValue.nodeName === "Activity") {
                        //Activity Type
                        activityTypeNode = childNodeValue.getElementsByTagName("ActivityType")[0];
                        activityType = activityTypeNode.getAttribute("type");
                        if (activityType === kmodel.Config.NODE_TYPE_START) {
                            startActivityGUID = childNode.id;
                        }
                        var activity = {};
                        activity["ActivityGUID"] = childNode.id;
                        activity["ActivityName"] = childNodeValue.getAttribute('label');
                        activityList.push(activity);
                    }
                }
            } else if (snode.nodeName === "Group") {
                //activities in the group
                var childNodeList = model.getChildVertices(model.getCell(vertex.id));
                for (var j = 0; j < childNodeList.length; j++) {
                    childNode = childNodeList[j];
                    childNodeValue = model.getValue(childNode);
                    if (childNodeValue.nodeName === "Activity") {
                        //Activity Type
                        activityTypeNode = childNodeValue.getElementsByTagName("ActivityType")[0];
                        activityType = activityTypeNode.getAttribute("type");
                        if (activityType === kmodel.Config.NODE_TYPE_START) {
                            startActivityGUID = childNode.id;
                        }
                        var activity = {};
                        activity["ActivityGUID"] = childNode.id;
                        activity["ActivityName"] = childNodeValue.getAttribute('label');
                        activityList.push(activity);
                    }
                }
            }
        });

        if (startActivityGUID === '') {
            prepareDetail["type"] = 'processvalidateresult_type_nostartactivity';
            return prepareDetail;
        }

        //transitions in default parent
        var edgeList = model.getChildEdges(kmain.mxGraphEditor.graph.getDefaultParent());
        lookupEdgeList(model, edgeList, transitionList);

        //transitions in swimlanes
        var childList = null;
        var swimlanes = kloader.getSwimlanes(model);
        for (var i = 0; i < swimlanes.length; i++) {
            childList = model.getChildEdges(model.getCell(swimlanes[i]));
            lookupEdgeList(model, childList, transitionList);
        }

        //transitions in groups
        var groupList = null;
        var groups = kloader.getGroups(model);
        for (var i = 0; i < groups.length; i++) {
            groupList = model.getChildEdges(model.getCell(groups[i]));
            lookupEdgeList(model, groupList, transitionList);
        }

        var entity = {};
        entity.StartActivityGUID = startActivityGUID;
        entity.ActivityList = activityList;
        entity.TransitionList = transitionList;

        prepareDetail["Entity"] = entity;
        return prepareDetail;
    }

    function lookupEdgeList(model, edgeList, transitionList) {
        $.each(edgeList, function (i, edge) { 
            var sline = model.getValue(edge);
            if (sline.nodeName === "Transition") {
                var transition = {};
                transition["FromActivityGUID"] = edge.source.id;
                transition["ToActivityGUID"] = edge.target.id;
                transitionList.push(transition);
            }
        });
    }

    kvalidator.validateProcess = function (entity, callback) {
        var validatedDetail = {};
        validatedDetail["type"] = "OK";

        jshelper.ajaxPost('api/Wf2Xml/ValidateProcess', JSON.stringify(entity), function (result) {
            if (result.Status === 1) {
                var validateResult = result.Entity;
                var isolatedActivityList = validateResult.ActivityList;
                if (isolatedActivityList !== null
                    && isolatedActivityList.length > 0) {
                    validatedDetail["type"] = 'processvalidateresult_type_isolatedactivity';
                    validatedDetail["activityList"] = isolatedActivityList;
                } else {
                    ;//OK
                }
            } else {
                validatedDetail["type"] = 'EXCEPTION';
                validatedDetail["activityList"] = null;
            }
            if (callback) callback(validatedDetail);
        });
    }
    return kvalidator;
})()