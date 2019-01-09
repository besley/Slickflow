/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。

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

var kloader = (function () {
    function kloader() {
    }

    //#region load xml to javascript object
    kloader.load = function (processFileEntity) {
        var package = {},
            process = {},
            participants = [];

        package.participants = participants;
        package.process = process;

        var xmlContent = processFileEntity.XmlContent;
        var doc = mxUtils.parseXml(xmlContent);

        //participants
        Array.prototype.forEach.call(doc.getElementsByTagName("Participant"), function (participantElement) {
            var participant = mxfile.getParticipantObject(participantElement);

            participants.push(participant);
        });

        //process
        var processElement = doc.getElementsByTagName("Process")[0];
        if (processElement) {
            var activities = [],
                transitions = [],
                swimlanes = [];

            //basic information
            process.name = processElement.getAttribute("name");
            process.id = processElement.getAttribute("id");

            var pdescElement = processElement.getElementsByTagName("Description")[0];
            if (pdescElement) process.description = jshelper.replaceHTMLTags(pdescElement.textContent);

            //swimlanes
            var layoutElement = doc.documentElement.getElementsByTagName("Layout")[0];    //get from package element
            if (layoutElement) {
                var swimlanesElement = layoutElement.getElementsByTagName("Swimlanes")[0];
                Array.prototype.forEach.call(swimlanesElement.getElementsByTagName("Swimlane"), function (swimlaneElement) {
                    //swimlane
                    var swimlane = mxfile.getSwimlaneObject(swimlaneElement);
                    
                    //geography
                    var geographyElement = swimlaneElement.getElementsByTagName("Geography")[0];
                    if (geographyElement) {
                        swimlane.geography = mxfile.getGeographySwimlaneObject(geographyElement);
                    }
                    swimlanes.push(swimlane);
                });
            }
            process.swimlanes = swimlanes;

            //activities and transitions
            process.activities = activities;
            process.transitions = transitions;

            //activities
            Array.prototype.forEach.call(processElement.getElementsByTagName("Activity"), function (activityElement) {
                var activity = {},
                    performers = [],
                    actions = [],
                    geographyElement = {};

                //activity
                activity = mxfile.getActivityObject(activityElement, "name");

                var actdescElement = activityElement.getElementsByTagName("Description")[0];
                if (actdescElement) activity.description = jshelper.replaceHTMLTags(actdescElement.textContent);

                //set activity type info
                var activityTypeElement = activityElement.getElementsByTagName("ActivityType")[0];
                activity = mxfile.getActivityTypeObject(activity, activityTypeElement);

                //performers list
                Array.prototype.forEach.call(activityElement.getElementsByTagName("Performer"), function (performerElement) {
                    var performer = mxfile.getPerformerObject(performerElement);

                    performers.push(performer);
                });
                activity.performers = performers;

                //geography
                geographyElement = activityElement.getElementsByTagName("Geography")[0];
                if (geographyElement) {
                    activity.geography = mxfile.getGeographyVertexObject(geographyElement);
                }
                activities.push(activity);
            });


            //transition
            Array.prototype.forEach.call(doc.getElementsByTagName("Transition"), function (transitionElement) {
                var transition = mxfile.getTransitionObject(transitionElement);

                //description
                var transdescElement = transitionElement.getElementsByTagName("Description")[0];
                if (transdescElement) transition.description = jshelper.replaceHTMLTags(transdescElement.textContent);

                //receiver
                var receiverElement = transitionElement.getElementsByTagName("Receiver")[0];
                if (receiverElement) {
                    transition.receiver = mxfile.getReceiverObject(receiverElement);
                }

                //condition
                var conditionElement = transitionElement.getElementsByTagName("Condition")[0];
                if (conditionElement) {
                    transition.condition = mxfile.getConditionObject(conditionElement);
                }
                
                //geography
                var geographyElement = transitionElement.getElementsByTagName("Geography")[0];
                if (geographyElement) {
                    transition.geography = mxfile.getGeographyEdgeObject(geographyElement);
                }
                transitions.push(transition);
            });
        } //end of processElement

        //render data
        var graphData = {
            "processGUID": processFileEntity.ProcessGUID,
            "version": processFileEntity.Version,
            "package": package
        };

        var kGraphData = new kmodel.GraphData(graphData);
        return kGraphData;
    }
    //#endregion

    //#region serialize Javscript object to xml
    kloader.serialize2Xml = function (processEntity, package) {
        if (!processEntity) return null;

        //xml document
        var doc = mxUtils.parseXml('<?xml version="1.0" encoding="utf-8"?><Package></Package>');       
        var packageElement = doc.documentElement;
        
        //Participants
        writeParticipants(doc, packageElement, package);

        //WorkflowProcess
        var processElement = writeProcess(doc, packageElement, processEntity);

        //layout
        var model = kmain.mxGraphEditor.graph.getModel();
        writeLayout(doc, packageElement, model);

        //Activities
        writeActivities(doc, processElement, model);

        //Transtions
        writeTransitions(doc, processElement, model);

        //get pretty xml content
        var xmlContent = vkbeautify.xml(mxUtils.getXml(doc, ' '));

        return xmlContent;
    }

    //participants
    function writeParticipants(doc, parent, package) {
        var participantsElement = doc.createElement("Participants");
        parent.appendChild(participantsElement);

        if (package && package.participants) {
            var participantsList = package.participants;
            if (participantsList && participantsList.length > 0) {
                for (var i = 0; i < participantsList.length; i++) {
                    var participant = participantsList[i];
                    var participantElement = mxfile.setParticipantElement(doc, participant);

                    participantsElement.appendChild(participantElement);
                }
            }
        }
    }

    //layout information
    function writeLayout(doc, parent, model) {
        var layoutElement = doc.createElement("Layout");
        parent.appendChild(layoutElement);

        var swimlanesElement = doc.createElement("Swimlanes");
        layoutElement.appendChild(swimlanesElement);

        var swimlaneNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        for (var i = 0; i < swimlaneNodeList.length; i++) {
            var swimlaneNode = swimlaneNodeList[i];
            var swimlaneNodeValue = model.getValue(swimlaneNode);

            if (swimlaneNodeValue.nodeName === "Swimlane") {
                var swimlaneElement = doc.createElement("Swimlane");
                swimlanesElement.appendChild(swimlaneElement);

                swimlaneElement.setAttribute("id", swimlaneNode.id);
                swimlaneElement.setAttribute("name", swimlaneNodeValue.getAttribute("label"));

                //swimlane geography
                var geographyElement = doc.createElement("Geography");
                swimlaneElement.appendChild(geographyElement);

                geographyElement.setAttribute("parent", swimlaneNode.parent.id);
                geographyElement.setAttribute("style", swimlaneNode.style);

                var widgetElement = doc.createElement("Widget");
                geographyElement.appendChild(widgetElement);

                widgetElement.setAttribute("left", swimlaneNode.geometry.x);
                widgetElement.setAttribute("top", swimlaneNode.geometry.y);
                widgetElement.setAttribute("width", swimlaneNode.geometry.width);
                widgetElement.setAttribute("height", swimlaneNode.geometry.height);
            }
        }
    }

    //process
    function writeProcess(doc, parent, processEntity) {
        var workflowElement = doc.createElement("WorkflowProcesses");
        parent.appendChild(workflowElement);

        var processElement = doc.createElement("Process");
        workflowElement.appendChild(processElement);

        processElement.setAttribute("name", processEntity.ProcessName);
        processElement.setAttribute("id", processEntity.ProcessGUID);
        
        var descriptionElement = doc.createElement("Description");
        processElement.appendChild(descriptionElement);

        var description = doc.createTextNode(jshelper.escapeHtml(processEntity.Description));
        descriptionElement.appendChild(description);

        return processElement;
    }

    //activities
    function writeActivities(doc, parent, model) {
        //queried activityies from the graph activities in the graph view
        var vertexNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        var vertex = null,
            snode = null,
            childNode = null,
            childNodeValue = null;

        if (vertexNodeList.length > 0) {
            var activitiesElement = doc.createElement("Activities");
            parent.appendChild(activitiesElement);

            for (var i = 0; i < vertexNodeList.length; i++) {
                vertex = vertexNodeList[i];
                snode = model.getValue(vertex);

                if (snode.nodeName === "Activity") {
                    //activities in the default parent
                    writeActivity(doc, activitiesElement, vertex, snode)
                } if (snode.nodeName === "Swimlane") {
                    //activities in the swimlane
                    var childNodeList = model.getChildVertices(model.getCell(vertex.id));
                    for (var j = 0; j < childNodeList.length; j++) {
                        childNode = childNodeList[j];
                        childNodeValue = model.getValue(childNode);
                        if (childNodeValue.nodeName === "Activity") {
                            writeActivity(doc, activitiesElement, childNode, childNodeValue);
                        } else {
                            window.console.log("invalid node type:" + childNodeValue.nodeName);
                        }
                    }
                }
            }
        }
    }

    //Activity
    function writeActivity(doc, parent, vertex, snode) {
        var activityTypeNode = null,
            activityType = '',
            descriptionNode = null,
            performersNode = null,
            actionsNode = null;

        //Activity
        var activityElement = doc.createElement("Activity");
        parent.appendChild(activityElement);

        activityElement.setAttribute("id", vertex.id);
        activityElement.setAttribute("name", snode.getAttribute("label"));
        activityElement.setAttribute("code", snode.getAttribute("code"));

        //Description
        descriptionNode = snode.getElementsByTagName("Description")[0];
        if (descriptionNode) {
            var descriptionElement = doc.createElement("Description");
            activityElement.appendChild(descriptionElement);

            var description = doc.createTextNode(jshelper.escapeHtml(descriptionNode.textContent));
            descriptionElement.appendChild(description);
        } 

        //Activity Type
        activityTypeNode = snode.getElementsByTagName("ActivityType")[0];
        activityType = activityTypeNode.getAttribute("type");

        var activityTypeElement = doc.createElement("ActivityType");
        activityElement.appendChild(activityTypeElement);

        activityTypeElement.setAttribute("type", activityType);
        if (activityType == "GatewayNode") {
            activityTypeElement.setAttribute("gatewaySplitJoinType", activityTypeNode.getAttribute("gatewaySplitJoinType"));
            activityTypeElement.setAttribute("gatewayDirection", activityTypeNode.getAttribute("gatewayDirection"));
        } else if (activityType == "SubProcessNode") {
            activityTypeElement.setAttribute("subId", activityTypeNode.getAttribute("subId"));
        } else if (activityType == "MultipleInstanceNode") {
            activityTypeElement.setAttribute("complexType", activityTypeNode.getAttribute("complexType"));
            activityTypeElement.setAttribute("mergeType", activityTypeNode.getAttribute("mergeType"));
            activityTypeElement.setAttribute("compareType", activityTypeNode.getAttribute("compareType"));
            activityTypeElement.setAttribute("completeOrder", activityTypeNode.getAttribute("completeOrder"));
        }

        //Performers
        performersNode = snode.getElementsByTagName("Performers")[0];
        if (performersNode) {
            var performerList = performersNode.getElementsByTagName("Performer");
            if (performerList.length > 0) {
                var performersElement = doc.createElement("Performers");
                activityElement.appendChild(performersElement);

                for (var p = 0; p < performerList.length; p++) {
                    var performer = performerList[p];
                    var performerElement = doc.createElement("Performer");
                    performersElement.appendChild(performerElement);

                    performerElement.setAttribute("id", performer.getAttribute("id"));
                }
            }
        }

        //Activity Geograpy
        var geographyElement = doc.createElement("Geography");
        activityElement.appendChild(geographyElement);

        geographyElement.setAttribute("parent", vertex.parent.id);
        geographyElement.setAttribute("style", vertex.style);

        var widgetElement = doc.createElement("Widget");
        geographyElement.appendChild(widgetElement);

        widgetElement.setAttribute("left", vertex.geometry.x);
        widgetElement.setAttribute("top", vertex.geometry.y);
        widgetElement.setAttribute("width", vertex.geometry.width);
        widgetElement.setAttribute("height", vertex.geometry.height);
    }

    //transitions
    function writeTransitions(doc, parent, model) {
        var transitionsElement = doc.createElement("Transitions");
        parent.appendChild(transitionsElement);

        //transitions in default parent
        var edgeList = model.getChildEdges(kmain.mxGraphEditor.graph.getDefaultParent());
        lookupEdgeList(doc, transitionsElement, model, edgeList);

        //transitions in swimlanes
        var childList = null;
        var swimlanes = getSwimlanes(model);
        for (var i = 0; i < swimlanes.length; i++) {
            childList = model.getChildEdges(model.getCell(swimlanes[i]));
            lookupEdgeList(doc, transitionsElement, model, childList);
        }
    }

    function lookupEdgeList(doc, parent, model, edgeList) {
        var edge = null,
            sline = null;

        if (edgeList.length > 0) {
            for (var j = 0; j < edgeList.length; j++) {
                edge = edgeList[j];
                sline = model.getValue(edge);

                if (sline.nodeName === "Transition") {
                    writeTransition(doc, parent, edge, sline);
                }
            }
        }
    }


    //swimlanes in default parent
    function getSwimlanes(model) {
        var swimlanes = [];
        var vertexNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        var vertex = null,
            snode = null;

        for (var i = 0; i < vertexNodeList.length; i++) {
            vertex = vertexNodeList[i];
            snode = model.getValue(vertex);
            if (snode.nodeName === "Swimlane") {
                swimlanes.push(vertex.id);
            }
        }
        return swimlanes;
    }

    //transition
    function writeTransition(doc, parent, edge, sline) {
        var receiverNode = null,
            receiverType = '',
            conditionNode = null,
            conditionType = '',
            conditionTextNode = null;

        //transition
        var transitionElement = doc.createElement("Transition");
        parent.appendChild(transitionElement);

        transitionElement.setAttribute("id", edge.id);
        transitionElement.setAttribute("from", edge.source.id);
        transitionElement.setAttribute("to", edge.target.id);

        //description
        descriptionNode = sline.getElementsByTagName("Description")[0];
        if (descriptionNode) {
            var descriptionElement = doc.createElement("Description");
            transitionElement.appendChild(descriptionElement);

            var description = doc.createTextNode(jshelper.escapeHtml(descriptionNode.textContent));
            descriptionElement.appendChild(description);
        } 

        //receiver
        receiverNode = sline.getElementsByTagName("Receiver")[0];
        if (receiverNode) {
            var receiverElement = doc.createElement("Receiver");
            transitionElement.appendChild(receiverElement);
            
            receiverType = receiverNode.getAttribute('type');
            if ($.isEmptyObject(receiverType) === false && receiverType !== "undefined" && receiverType !== "null") {
                receiverElement.setAttribute("type", receiverType);
            }
        }

        //condition
        conditionNode = sline.getElementsByTagName("Condition")[0];
        if (conditionNode) {
            var conditionElement = doc.createElement("Condition");
            transitionElement.appendChild(conditionElement);

            conditionType = conditionNode.getAttribute('type');
            if ($.isEmptyObject(conditionType) === false && conditionType !== "undefined") {
                conditionElement.setAttribute("type", conditionType);
                conditionTextNode = conditionNode.getElementsByTagName("ConditionText")[0];
                if ($.isEmptyObject(conditionTextNode) === false) {
                    var conditionTextElement = doc.createElement("ConditionText");
                    conditionElement.appendChild(conditionTextElement);

                    var conditionContent = $.trim(conditionTextNode.textContent);
                    if (conditionContent !== "") {
                        var expression = doc.createCDATASection(conditionContent);
                        conditionTextElement.appendChild(expression); 
                    }
                }
            }
        }

        //transition geograpy
        var geographyElement = doc.createElement("Geography");
        transitionElement.appendChild(geographyElement);

        geographyElement.setAttribute("parent", edge.parent.id);
        geographyElement.setAttribute("style", edge.style);
    }
    //#endregion

    return kloader;
})()