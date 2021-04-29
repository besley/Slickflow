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

var kloader = (function () {
    function kloader() {
    }

    //#region load xml to javascript object
    kloader.load = function (processFileEntity) {
        var xmlContent = processFileEntity.XmlContent;
        var doc = mxUtils.parseXml(xmlContent);

        var package = {};

        //participants
        package.participants = loadParticipants(doc);

        //layout
        var layout = loadLayout(doc);
        package.swimlanes = layout.swimlanes;
        package.groups = layout.groups;
        package.messages = layout.messages;

        //workflow
        package.processes = loadProcesses(doc);

        //render data
        var graphData = {
            "processGUID": processFileEntity.ProcessGUID,
            "version": processFileEntity.Version,
            "package": package
        };

        //load vertex, edges into canvas
        var kGraphData = new kmodel.GraphData(graphData);

        return kGraphData;
    }

    function loadParticipants(doc) {
        var participants = [];
        Array.prototype.forEach.call(doc.getElementsByTagName("Participant"), function (participantElement) {
            var participant = mxfile.getParticipantObject(participantElement);

            participants.push(participant);
        });
        return participants;
    }

    function loadLayout(doc) {
        //layout
        var layout = {};
        var swimlanes = [], groups = [], messages = [];
        var layoutElement = doc.documentElement.getElementsByTagName("Layout")[0];    //get from package element
        if (layoutElement) {
            //swimlanes
            var swimlanesElement = layoutElement.getElementsByTagName("Swimlanes")[0];
            if (swimlanesElement) {
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
            layout.swimlanes = swimlanes;

            //groups
            var groupsElement = layoutElement.getElementsByTagName("Groups")[0];
            if (groupsElement) {
                Array.prototype.forEach.call(groupsElement.getElementsByTagName("Group"), function (groupElement) {
                    //group
                    var group = mxfile.getGroupObject(groupElement);

                    //geography
                    var geographyElementGroup = groupElement.getElementsByTagName("Geography")[0];

                    if (geographyElementGroup) {
                        group.geography = mxfile.getGeographyGroupObject(geographyElementGroup);
                    }
                    groups.push(group);
                });
            }
            layout.groups = groups;

            //messages
            Array.prototype.forEach.call(doc.getElementsByTagName("Message"), function (messageElement) {
                var message = mxfile.getMessageObject(messageElement);

                var messageDescElement = messageElement.getElementsByTagName("Description")[0];
                if (messageDescElement) message.description = jshelper.replaceHTMLTags(messageDescElement.textContent);

                //geography
                var geographyElement = messageElement.getElementsByTagName("Geography")[0];
                if (geographyElement) {
                    message.geography = mxfile.getGeographyEdgeObject(geographyElement);
                }
                messages.push(message);
            });
            layout.messages = messages;
        }
        return layout;
    }

    //workflow
    function loadProcesses(doc) {
        //workflow
        var processes = [];
        var workflowElement = doc.getElementsByTagName("WorkflowProcesses")[0];
        Array.prototype.forEach.call(workflowElement.getElementsByTagName("Process"), function (processElement) {
            //process
            var process = {};
            process.name = processElement.getAttribute("name");
            process.id = processElement.getAttribute("id");

            var pdescElement = processElement.getElementsByTagName("Description")[0];
            if (pdescElement) process.description = jshelper.replaceHTMLTags(pdescElement.textContent);

            //activities, transitions and messages
            process.activities = loadActivities(processElement);
            process.transitions = loadTransitions(processElement);

            processes.push(process);
        });
        return processes;
    }

    function loadActivities(processElement) {
        var activities = [];
        //activities
        Array.prototype.forEach.call(processElement.getElementsByTagName("Activity"), function (activityElement) {
            var activity = {},
                performers = [],
                actions = [],
                services = [],
                boundaries = [],
                sections = [],
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

            //actions list
            Array.prototype.forEach.call(activityElement.getElementsByTagName("Action"), function (actionElement) {
                var action = mxfile.getActionObject(actionElement);
                actions.push(action);
            });
            activity.actions = actions;

            //services list
            Array.prototype.forEach.call(activityElement.getElementsByTagName("Service"), function (serviceElement) {
                var service = mxfile.getServiceObject(serviceElement);
                services.push(service);
            });
            activity.services = services;

            //boudaries list
            Array.prototype.forEach.call(activityElement.getElementsByTagName("Boundary"), function (boundaryElement) {
                var boundary = mxfile.getBoundaryObject(boundaryElement);
                boundaries.push(boundary);
            });
            activity.boundaries = boundaries;

            //sections list
            Array.prototype.forEach.call(activityElement.getElementsByTagName("Section"), function (sectionElement) {
                var section = mxfile.getSectionObject(sectionElement);
                sections.push(section);
            });
            activity.sections = sections;

            //geography
            geographyElement = activityElement.getElementsByTagName("Geography")[0];
            if (geographyElement) {
                activity.geography = mxfile.getGeographyVertexObject(geographyElement);
            }
            activities.push(activity);
        });
        return activities;
    }

    function loadTransitions(processElement) {
        var transitions = [];
        //transition
        Array.prototype.forEach.call(processElement.getElementsByTagName("Transition"), function (transitionElement) {
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

            //group behavious
            var groupBehavioursElement = transitionElement.getElementsByTagName("GroupBehaviours")[0];
            if (groupBehavioursElement) {
                transition.groupBehaviours = mxfile.getGroupBehavioursObject(groupBehavioursElement);
            }

            //geography
            var geographyElement = transitionElement.getElementsByTagName("Geography")[0];
            if (geographyElement) {
                transition.geography = mxfile.getGeographyEdgeObject(geographyElement);
            }
            transitions.push(transition);
        });
        return transitions;
    }
    //#endregion

    //#region serialize Javscript object to xml
    kloader.serialize2Xml = function (participants) {
        var result = {};
        result["status"] = 0;
        result["message"] = '';
        result["xmlContent"] = '';

        try {
            //xml document
            var doc = mxUtils.parseXml('<?xml version="1.0" encoding="utf-8"?><Package></Package>');
            var packageElement = doc.documentElement;

            //sync global perfromers into participants array
            var model = kmain.mxGraphEditor.graph.getModel();
            var newParticipants = syncGlobalParticipants(participants, model);

            //Participants
            writeParticipants(doc, packageElement, newParticipants);

            //layout
            var layoutElement = writeLayout(doc, packageElement, model);

            //WorkflowProcesses
            var workflowElement = writeWorkflowProcesses(doc, packageElement, model);
            packageElement.insertBefore(workflowElement, layoutElement);

            //get pretty xml content
            var xmlContent = vkbeautify.xml(mxUtils.getXml(doc, ' '));

            result.status = 1;
            result.xmlContent = xmlContent;
        } catch (e) {
            result.message = e.message;
        }
        return result;
    }

    function writeWorkflowProcesses(doc, parent, model) {
        kmain.mxSelectedProcessEntityList.length = 0;
        var workflowElement = doc.createElement("WorkflowProcesses");
        
        //根据泳道创建流程记录
        var isPoolProcessNotExist = true;
        var layoutElement = doc.documentElement.getElementsByTagName("Layout")[0];
        var swimlanesElement = layoutElement.getElementsByTagName("Swimlanes")[0];
        if (swimlanesElement) {
            var swimlaneNodeList = swimlanesElement.getElementsByTagName("Swimlane");
            for (var i = 0; i < swimlaneNodeList.length; i++) {
                var swimlaneElement = swimlaneNodeList[i];
                var processElement = swimlaneElement.getElementsByTagName("Process")[0];
                if (processElement) {
                    isPoolProcessNotExist = false;
                    var swimlaneCell = model.getCell(swimlaneElement.getAttribute("id"));
                    writeProcessPool(doc, workflowElement, swimlaneCell, processElement, model);
                }
            }
        } else {
            isPoolProcessNotExist = true;
        }

        if (isPoolProcessNotExist === true) {
            //没有泳道，或泳道没有流程配置，直接保存
            var processEntity = kmain.mxSelectedProcessEntity;
            var diagramParent = kmain.mxGraphEditor.graph.getDefaultParent();

            writeProcess(doc, workflowElement, diagramParent, processEntity, model);
        }
        return workflowElement;
    }

    //process
    function writeProcess(doc, xmlParent, diagramParent, processEntity, model) {
        //流程记录列表
        kmain.mxSelectedProcessEntityList.push(processEntity);

        //流程XML内容生成
        var processElement = doc.createElement("Process");
        xmlParent.appendChild(processElement);

        processElement.setAttribute("id", processEntity.ProcessGUID);
        processElement.setAttribute("name", processEntity.ProcessName);
        processElement.setAttribute("code", processEntity.ProcessCode);
        processElement.setAttribute("package", processEntity.PackageType);

        var descriptionElement = doc.createElement("Description");
        processElement.appendChild(descriptionElement);

        var description = doc.createTextNode(jshelper.escapeHtml(processEntity.Description));
        descriptionElement.appendChild(description);

        //Activities
        writeActivities(doc, processElement, diagramParent, model);

        //Transtions
        writeTransitions(doc, processElement, diagramParent, model);

        return processElement;
    }

    function writeProcessPool(doc, xmlParent, diagramParent, diagramProcess, model) {
        var processElement = doc.createElement("Process");
        xmlParent.appendChild(processElement);

        var processEntity = {};
        processEntity.ProcessGUID = diagramProcess.getAttribute("id");
        processElement.setAttribute("id", processEntity.ProcessGUID);

        processEntity.ProcessName = diagramProcess.getAttribute("name");
        processElement.setAttribute("name", processEntity.ProcessName);

        processEntity.ProcessCode = diagramProcess.getAttribute("code");
        processElement.setAttribute("code", processEntity.ProcessCode);

        processEntity.Version = kmain.mxSelectedProcessEntity.Version;      //主流程版本
        processElement.setAttribute("version", processEntity.Version);

        var packageType = diagramProcess.getAttribute("package");
        if (packageType === "MainProcess")
            processEntity.PackageType = 1;
        else
            processEntity.PackageType = 2;
        processElement.setAttribute("package", packageType);

        var descriptionElement = doc.createElement("Description");
        processElement.appendChild(descriptionElement);

        var descriptionText = jshelper.escapeHtml(diagramProcess.getAttribute("description"));
        processEntity.Description = descriptionText;
        var description = doc.createTextNode(descriptionText);
        descriptionElement.appendChild(description);

        //Activities
        writeActivitiesPool(doc, processElement, diagramParent, model);

        //Transtions
        writeTransitionsPool(doc, processElement, diagramParent, model);

        //追加泳道流程列表，用于webapi提交
        kmain.mxSelectedProcessEntityList.push(processEntity);

        return processElement;
    }

    //sync activity perfomers into new participants
    function syncGlobalParticipants(oldParticipants, model) {
        var newParticipants = [];
        var vertexNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        syncEachVertexPerformers(newParticipants, oldParticipants, vertexNodeList, model);

        return newParticipants;
    }

    //sync each vertex performers into new participants recurivly
    function syncEachVertexPerformers(newParticipants, oldParticipants, vertexNodeList, model) {
        var vertex = null,
            snode = null,
            newParticipant = null,
            performersNode = null,
            performerList = null,
            performer = null,
            performerGUID = "";

        if (vertexNodeList.length > 0) {
            for (var i = 0; i < vertexNodeList.length; i++) {
                vertex = vertexNodeList[i];
                var childNodeList = model.getChildVertices(vertex);     //group control contains child vertexs.
                if (childNodeList.length > 0) {
                    //recursivly sync performers
                    syncEachVertexPerformers(newParticipants, oldParticipants, childNodeList, model);
                }
                else {
                    snode = model.getValue(vertex);
                    performersNode = snode.getElementsByTagName("Performers")[0];

                    if (performersNode) {
                        var performerList = performersNode.getElementsByTagName("Performer");

                        if (performerList.length > 0) {
                            for (var p = 0; p < performerList.length; p++) {
                                performer = performerList[p];
                                performerGUID = performer.getAttribute("id");

                                if (isNotExistInNewParticipants(newParticipants, performerGUID) === true) {
                                    newParticipant = getParticipantFromPerformer(oldParticipants, performerGUID);
                                    newParticipants.push(newParticipant);
                                }
                            }
                        }
                    }
                }
            }
        }
        return newParticipants;
    }

    //check participant exist
    function isNotExistInNewParticipants(newParticipants, performerGUID) {
        var isNotExist = true;
        for (var i = 0; i < newParticipants.length; i++) {
            var p = newParticipants[i];
            if (p !== null && p.id === performerGUID) {
                isNotExist = false;
                break;
            }
        }
        return isNotExist;
    }

    //get participant from perfomer
    function getParticipantFromPerformer(oldParticipants, performerGUID) {
        var newParticipant = null;

        for (var i = 0; i < oldParticipants.length; i++) {
            var p = oldParticipants[i];
            if (p !== null && p.id === performerGUID) {
                newParticipant = {};
                newParticipant.type = p.type;
                newParticipant.id = p.id;
                newParticipant.name = p.name;
                newParticipant.code = p.code;
                newParticipant.outerId = p.outerId;
                break;
            }
        }
        return newParticipant;
    }

    //participants
    function writeParticipants(doc, parent, participants) {
        var participantsElement = doc.createElement("Participants");
        parent.appendChild(participantsElement);

        if (participants) {
            for (var i = 0; i < participants.length; i++) {
                var participant = participants[i];
                if (participant !== null) {
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

        //write swimlanes
        writeSwimlanes(doc, layoutElement, model);

        //write group
        writeGroups(doc, layoutElement, model);

        //write message
        writeMessages(doc, layoutElement, model);
    }

    function writeSwimlanes(doc, layout, model) {
        var isSwimlanesExist = false;
        var swimlanesElement = null;
        var childNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());

        for (var i = 0; i < childNodeList.length; i++) {
            var swimlaneNode = childNodeList[i];
            var swimlaneNodeValue = model.getValue(swimlaneNode);

            if (swimlaneNodeValue.nodeName === "Swimlane") {
                if (isSwimlanesExist === false) {
                    swimlanesElement = doc.createElement("Swimlanes");
                    layout.appendChild(swimlanesElement);
                    isSwimlanesExist = true;
                }
                var swimlaneElement = mxfile.setSwimlaneElement(doc, swimlaneNodeValue);

                swimlaneElement.setAttribute("id", swimlaneNode.id);
                swimlanesElement.appendChild(swimlaneElement);
               
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

    function writeGroups(doc, layout, model) {
        var groupsElement = doc.createElement("Groups");
        layout.appendChild(groupsElement);

        var groupNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        for (var i = 0; i < groupNodeList.length; i++) {
            var groupNode = groupNodeList[i];
            var groupNodeValue = model.getValue(groupNode);

            if (groupNodeValue.nodeName === "Group") {
                var groupElement = doc.createElement("Group");
                groupsElement.appendChild(groupElement);

                groupElement.setAttribute("id", groupNode.id);
                groupElement.setAttribute("name", groupNodeValue.getAttribute("label"));

                //group geography
                var geographyElement = doc.createElement("Geography");
                groupElement.appendChild(geographyElement);

                geographyElement.setAttribute("parent", groupNode.parent.id);
                if (groupNode.style)
                    geographyElement.setAttribute("style", groupNode.style);
                else
                    geographyElement.setAttribute("style", "verticalAlign=top;");

                var widgetElement = doc.createElement("Widget");
                geographyElement.appendChild(widgetElement);

                widgetElement.setAttribute("left", groupNode.geometry.x);
                widgetElement.setAttribute("top", groupNode.geometry.y);
                widgetElement.setAttribute("width", groupNode.geometry.width);
                widgetElement.setAttribute("height", groupNode.geometry.height);
            }
        }
    }    

    //activities
    function writeActivities(doc, xmlParent, diagramParent, model) {
        //queried activityies from the graph activities in the graph view
        var vertexNodeList = model.getChildVertices(diagramParent);
        var vertex = null,
            snode = null,
            childNode = null,
            childNodeValue = null;

        if (vertexNodeList.length > 0) {
            var activitiesElement = doc.createElement("Activities");
            xmlParent.appendChild(activitiesElement);

            for (var i = 0; i < vertexNodeList.length; i++) {
                vertex = vertexNodeList[i];
                snode = model.getValue(vertex);

                if (snode.nodeName === "Activity") {
                    //activities in the default parent
                    writeActivity(doc, activitiesElement, vertex, snode)
                } else if (snode.nodeName === "Swimlane") {
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
                } else if (snode.nodeName === "Group") {
                    //activities in the group
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

    //activities
    function writeActivitiesPool(doc, xmlParent, diagramParent, model) {
        //queried activityies from the graph activities in the graph view
        var vertexNodeList = model.getChildVertices(diagramParent);
        var vertex = null,
            snode = null;

        if (vertexNodeList.length > 0) {
            var activitiesElement = doc.createElement("Activities");
            xmlParent.appendChild(activitiesElement);

            for (var i = 0; i < vertexNodeList.length; i++) {
                vertex = vertexNodeList[i];
                snode = model.getValue(vertex);

                if (snode.nodeName === "Activity") {
                    //activities in the default parent
                    writeActivity(doc, activitiesElement, vertex, snode)
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
            actionsNode = null,
            servicesNode = null,
            boundariesNode = null,
            sectionsNode = null;

        //Activity
        var activityElement = doc.createElement("Activity");
        parent.appendChild(activityElement);

        activityElement.setAttribute("id", vertex.id);
        activityElement.setAttribute("name", snode.getAttribute("label"));
        var activityCode = snode.getAttribute("code");
        //get random 6 digits characters if activity code is null
        if (activityCode === null || activityCode.trim() === "") activityCode = jshelper.getRandomString(6);
        activityElement.setAttribute("code", activityCode);
        activityElement.setAttribute("url", snode.getAttribute("url"));

        //Description
        descriptionNode = snode.getElementsByTagName("Description")[0];
        if (descriptionNode) {
            var descriptionElement = doc.createElement("Description");
            activityElement.appendChild(descriptionElement);

            if (descriptionNode.textContent !== 'undefined') {
                var description = doc.createTextNode(jshelper.escapeHtml(descriptionNode.textContent));
                descriptionElement.appendChild(description);
            }
        }

        //Activity Type
        activityTypeNode = snode.getElementsByTagName("ActivityType")[0];
        activityType = activityTypeNode.getAttribute("type");

        var activityTypeElement = doc.createElement("ActivityType");
        activityElement.appendChild(activityTypeElement);

        activityTypeElement.setAttribute("type", activityType);
        if (activityType === kmodel.Config.NODE_TYPE_START      //"StartNode"
            || activityType === kmodel.Config.NODE_TYPE_END     // "EndNode"
            || activityType === kmodel.Config.NODE_TYPE_INTERMEDIATE) {         // "IntermediateNode"
            var trigger = activityTypeNode.getAttribute("trigger");
            activityTypeElement.setAttribute("trigger", trigger);

            var expression = activityTypeNode.getAttribute("expression");
            activityTypeElement.setAttribute("expression", expression);

            //message node
            var msgDirection = activityTypeNode.getAttribute("messageDirection");
            activityTypeElement.setAttribute("messageDirection", msgDirection);
        } else if (activityType === "GatewayNode") {
            activityTypeElement.setAttribute("gatewaySplitJoinType", activityTypeNode.getAttribute("gatewaySplitJoinType"));
            activityTypeElement.setAttribute("gatewayDirection", activityTypeNode.getAttribute("gatewayDirection"));
            activityTypeElement.setAttribute("gatewayJoinPass", activityTypeNode.getAttribute("gatewayJoinPass"));
        } else if (activityType === "SubProcessNode") {
            activityTypeElement.setAttribute("subId", activityTypeNode.getAttribute("subId"));
            activityTypeElement.setAttribute("subType", activityTypeNode.getAttribute('subType'));
            activityTypeElement.setAttribute("subVar", activityTypeNode.getAttribute('subVar'));
        } else if (activityType === "MultipleInstanceNode") {
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

        //Actions
        actionsNode = snode.getElementsByTagName("Actions")[0];
        if (actionsNode) {
            var actionList = actionsNode.getElementsByTagName("Action");
            if (actionList.length > 0) {
                var actionsElement = doc.createElement("Actions");
                activityElement.appendChild(actionsElement);

                for (var a = 0; a < actionList.length; a++) {
                    var action = actionList[a];
                    if (action && action.hasAttribute("type")) {
                        var actionElement = doc.createElement("Action");
                        actionsElement.appendChild(actionElement);
                        mxfile.setActionElementByHTML(doc, actionElement, action);
                    }
                }
            }
        }

        //Services
        servicesNode = snode.getElementsByTagName("Services")[0];
        if (servicesNode) {
            var serviceList = servicesNode.getElementsByTagName("Service");
            if (serviceList.length > 0) {
                var servicesElement = doc.createElement("Services");
                activityElement.appendChild(servicesElement);

                for (var a = 0; a < serviceList.length; a++) {
                    var service = serviceList[a];
                    var serviceElement = doc.createElement("Service");
                    servicesElement.appendChild(serviceElement);
                    mxfile.setServiceElementByHTML(doc, serviceElement, service);
                }
            }
        }

        //Boundaries
        boundariesNode = snode.getElementsByTagName("Boundaries")[0];
        if (boundariesNode) {
            var boundaryList = boundariesNode.getElementsByTagName("Boundary");
            if (boundaryList.length > 0) {
                var boundariesElement = doc.createElement("Boundaries");
                activityElement.appendChild(boundariesElement);

                for (var b = 0; b < boundaryList.length; b++) {
                    var boundary = boundaryList[b];
                    if (boundary) {
                        var boundaryElement = doc.createElement("Boundary");
                        boundariesElement.appendChild(boundaryElement);

                        boundaryElement.setAttribute("event", boundary.getAttribute("event"));
                        boundaryElement.setAttribute("expression", boundary.getAttribute("expression"));
                    }
                }
            }
        }

        //Sections
        sectionsNode = snode.getElementsByTagName("Sections")[0];
        if (sectionsNode) {
            var sectionList = sectionsNode.getElementsByTagName("Section");
            if (sectionList.length > 0) {
                var sectionsElement = doc.createElement("Sections");
                activityElement.appendChild(sectionsElement);

                for (var s = 0; s < sectionList.length; s++) {
                    var section = sectionList[s];
                    if (section) {
                        var sectionElement = doc.createElement("Section");
                        sectionsElement.appendChild(sectionElement);

                        sectionElement.setAttribute("name", section.getAttribute("name"));
                        var cDataSection = doc.createCDATASection(section.textContent);
                        sectionElement.appendChild(cDataSection);
                    }
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
    function writeTransitions(doc, xmlParent, diagramParent, model) {
        var transitionsElement = doc.createElement("Transitions");
        xmlParent.appendChild(transitionsElement);

        //transitions in default parent
        var edgeList = model.getChildEdges(diagramParent);
        lookupEdgeList(doc, transitionsElement, model, edgeList);

        //transitions in swimlanes
        var childList = null;
        var swimlanes = kloader.getSwimlanes(model);
        for (var i = 0; i < swimlanes.length; i++) {
            childList = model.getChildEdges(model.getCell(swimlanes[i]));
            lookupEdgeList(doc, transitionsElement, model, childList);
        }

        //transitions in groups
        var groupList = null;
        var groups = kloader.getGroups(model);
        for (var i = 0; i < groups.length; i++) {
            groupList = model.getChildEdges(model.getCell(groups[i]));
            lookupEdgeList(doc, transitionsElement, model, groupList);
        }
    }

    function writeTransitionsPool(doc, xmlParent, diagramParent, model) {
        var transitionsElement = doc.createElement("Transitions");
        xmlParent.appendChild(transitionsElement);

        //transitions in specific parent
        var edgeList = model.getChildEdges(diagramParent);
        lookupEdgeList(doc, transitionsElement, model, edgeList);
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
    kloader.getSwimlanes = function(model) {
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

    //group in default parent
    kloader.getGroups = function(model) {
        var groups = [];
        var vertexNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        var vertex = null,
            snode = null;

        for (var i = 0; i < vertexNodeList.length; i++) {
            vertex = vertexNodeList[i];
            snode = model.getValue(vertex);
            if (snode.nodeName === "Group") {
                groups.push(vertex.id);
            }
        }
        return groups;
    }

    //transition
    function writeTransition(doc, parent, edge, sline) {
        var receiverNode = null,
            receiverType = '',
            conditionNode = null,
            conditionType = '',
            conditionTextNode = null,
            groupBehavioursNode = null,
            defaultBranch = '',
            priority = '',
            forced = '',
            approval = '';

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

            if (descriptionNode.textContent !== 'undefined') {
                var description = doc.createTextNode(jshelper.escapeHtml(descriptionNode.textContent));
                descriptionElement.appendChild(description);
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

        //group behaviours 
        groupBehavioursNode = sline.getElementsByTagName("GroupBehaviours")[0];
        if (groupBehavioursNode) {
            var groupBehavioursElement = doc.createElement("GroupBehaviours");
            transitionElement.appendChild(groupBehavioursElement);

            //OrSplit
            defaultBranch = groupBehavioursNode.getAttribute("defaultBranch");
            if ($.isEmptyObject(defaultBranch) === false && defaultBranch !== "undefined" && defaultBranch !== "null") {
                groupBehavioursElement.setAttribute("defaultBranch", defaultBranch);
            }

            //XOrSplit
            priority = groupBehavioursNode.getAttribute("priority");
            if ($.isEmptyObject(priority) === false && priority !== "undefined" && priority !== "null") {
                groupBehavioursElement.setAttribute("priority", priority);
            }

            //EOrJoin
            forced = groupBehavioursNode.getAttribute("forced");
            if ($.isEmptyObject(forced) === false && forced !== "undefined" && forced !== "null") {
                groupBehavioursElement.setAttribute("forced", forced);
            }

            //ApprovalOrSplit
            approval = groupBehavioursNode.getAttribute("approval");
            if ($.isEmptyObject(approval) === false && approval !== "undefined" && approval !== "null") {
                groupBehavioursElement.setAttribute("approval", approval);
            }
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

        //transition geograpy
        var geographyElement = doc.createElement("Geography");
        transitionElement.appendChild(geographyElement);

        geographyElement.setAttribute("parent", edge.parent.id);
        geographyElement.setAttribute("style", edge.style);

        //points array
        var points = edge.geometry.points;
        if (points) {
            var pointsElement = doc.createElement("Points");
            geographyElement.appendChild(pointsElement);

            $.each(points, function (i, p) {
                var pointElement = doc.createElement("Point");
                pointsElement.appendChild(pointElement);

                pointElement.setAttribute("x", p.x);
                pointElement.setAttribute("y", p.y)
            });
        }        
    }

    //messages
    function writeMessages(doc, parent, model) {
        var messagesElement = doc.createElement("Messages");
        parent.appendChild(messagesElement);

        var messageList = model.getChildEdges(kmain.mxGraphEditor.graph.getDefaultParent());
        lookupMessageList(doc, messagesElement, model, messageList);
    }

    function lookupMessageList(doc, parent, model, messageList) {
        var message = null,
            sline = null;

        if (messageList.length > 0) {
            for (var i = 0; i < messageList.length; i++) {
                message = messageList[i];
                sline = model.getValue(message);

                if (sline.nodeName === "Message") {
                    writeMessage(doc, parent, message, sline);
                }
            }
        }
    }

    function writeMessage(doc, parent, message, sline) {
        var messageElement = doc.createElement("Message");
        parent.appendChild(messageElement);

        messageElement.setAttribute("id", message.id);
        messageElement.setAttribute("from", message.source.id);
        messageElement.setAttribute("to", message.target.id);

        //description
        descriptionNode = sline.getElementsByTagName("Description")[0];
        if (descriptionNode) {
            var descriptionElement = doc.createElement("Description");
            messageElement.appendChild(descriptionElement);

            if (descriptionNode.textContent !== 'undefined') {
                var description = doc.createTextNode(jshelper.escapeHtml(descriptionNode.textContent));
                descriptionElement.appendChild(description);
            }
        } 

        //message geography
        var geographyElement = doc.createElement("Geography");
        messageElement.appendChild(geographyElement);

        geographyElement.setAttribute("parent", message.parent.id);
        geographyElement.setAttribute("style", message.style);

        //points array
        var points = message.geometry.points;
        if (points) {
            var pointsElement = doc.createElement("Points");
            geographyElement.appendChild(pointsElement);

            $.each(points, function (i, p) {
                var pointElement = doc.createElement("Point");
                pointsElement.appendChild(pointElement);

                pointElement.setAttribute("x", p.x);
                pointElement.setAttribute("y", p.y)
            });
        }    
    }

    //#endregion

    return kloader;
})()