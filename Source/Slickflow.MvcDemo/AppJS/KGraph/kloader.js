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

var kloader;
if (!kloader) kloader = {};

(function () {
    //#region graph behavious
    //create a new grpah with start and end node
    kloader.createNew = function (processEntity) {
        var packageData = {},
            process = {};

        var snodes = [{
            id: jshelper.getUUID(),
            name: "开始",
            code: "start",
            type: "StartNode",
            left: 370,
            top: 70,
            width: 50,
            height: 50,
            inputConnectors: [{ type: "input", index: 1, name: "A" }],
            outputConnectors: [{ type: "output", index: 1, name: "X" }]
        },
        {
            id: jshelper.getUUID(),
            name: "结束",
            code: "end",
            type: "EndNode",
            left: 370,
            top: 370,
            width: 50,
            height: 50,
            inputConnectors: [{ type: "input", index: 1, name: "A" }],
            outputConnectors: [{ type: "output", index: 1, name: "X" }]
        }];

        packageData.process = process;
        process.id = processEntity.ProcessGUID;
        process.name = processEntity.ProcessName;
        process.description = processEntity.Description;

        process.snodes = snodes;

        var graph = new kgraph.GraphView(processEntity.ProcessGUID, packageData);
        return graph;
    }
    //#endregion

    //#region deserialize xml to javascript object
    kloader.initialize = function (processFileEntity) {
        var packageData = {},
            process = {};
        var participants = [];

        //begin to parse xml document
        var dom = $.parseXML(processFileEntity.XmlContent);
        packageData.participants = participants;
        packageData.process = process;

        //participants
        $(dom).find("Participant").each(function (i) {
            var participant = {};

            participant.type = $(this).attr("type");
            participant.id = $(this).attr("id");
            participant.name = $(this).attr("name");
            participant.code = $(this).attr("code");
            participant.outerId = $(this).attr("outerId");

            participants.push(participant);
        });

        //process
        var processElement = $(dom).find("Process");
        if (processElement) {
            var snodes = [],
                slines = [];
            
            process.name = $(processElement).attr("name");
            process.id = $(processElement).attr("id");

            var pdescElement = $(processElement).children("Description");
            process.description =  jshelper.replaceHTMLTags($(pdescElement).text());

            process.snodes = snodes;
            process.slines = slines;

            //activity nodes
            $(processElement).find("Activity").each(function (i) {
                var activityElement = this;
                var activity = {},
                    activityType = {},
                    performers = [],
                    geography = {},
                    widget = {},
                    connectors = {};

                activity.id = $(activityElement).attr("id");
                activity.name = $(activityElement).attr("name");
                activity.code = $(activityElement).attr("code");
                activity.text = '';

                var actdescElement = $(activityElement).find("Description");
                activity.description = jshelper.replaceHTMLTags($(actdescElement).text());

                activityType = $(activityElement).find("ActivityType");
                activity.type = $(activityType).attr("type");
                if (activity.type == "GatewayNode") {
                    activity.gatewaySplitJoinType = $(activityType).attr("gatewaySplitJoinType");
                    activity.gatewayDirection = $(activityType).attr("gatewayDirection");
                } else if (activity.type == "SubProcessNode") {
                    activity.subid = $(activityType).attr("subid");
                }

                //performers list
                $(activityElement).find("Performer").each(function (i) {
                    var performerElement = this;
                    var performer = {};

                    performer.id = $(performerElement).attr("id");
                    performers.push(performer);
                });
                activity.performers = performers;

                geography = $(activityElement).find("Geography");

                widget = $(geography).find("Widget");

                var inputConnectors = [],
                    outputConnectors = [];
                activity.inputConnectors = inputConnectors;
                activity.outputConnectors = outputConnectors;

                activity.left = parseInt($(widget).attr("left"));
                activity.top = parseInt($(widget).attr("top"));
                activity.width = parseInt($(widget).attr("width"));
                activity.height = parseInt($(widget).attr("height"));

                connectors = $(widget).find("Connectors");

                $(connectors).find("Connector").each(function (i) {
                    var connectorElement = this;

                    var connector = {};
                    connector.type = $(connectorElement).attr("type");
                    connector.index = $(connectorElement).attr("index");
                    connector.name = $(connectorElement).attr("name");

                    var type = $(connectorElement).attr("type");
                    if (type == "input") {
                        activity.inputConnectors.push(connector);
                    } else if (type == "output") {
                        activity.outputConnectors.push(connector);
                    }
                });
                snodes.push(activity);
            });

            //transition
            $(dom).find("Transition").each(function (i) {
                var transition = {};

                transition.id = $(this).attr("id");
                transition.from = $(this).attr("from");
                transition.to = $(this).attr("to");

                var transdescElement = $(this).find("Description");
                transition.description = jshelper.replaceHTMLTags($(transdescElement).text());

                var conditionElement = $(this).find("Condition");
                transition.condition = $(conditionElement).text();

                var geographyElement = $(this).find("Geography");
                var lineElement = $(geographyElement).find("Line");

                transition.fromConnector = $(lineElement).attr("fromConnector");
                transition.toConnector = $(lineElement).attr("toConnector");

                slines.push(transition);
            });
        } //end of processElement

        //render data
        var graphData = {
            "processGUID": processFileEntity.ProcessGUID,
            "version": processFileEntity.Version,
            "packageData": packageData
        };
        var graph = new kgraph.GraphView(graphData);

        return graph;
    }
    //#endregion

    //#region serialize Javscript object to xml
    kloader.serialize2Xml = function (processGUID, packageData) {
        var processFileEntity = {};

        //xml document
        var xw = new XMLWriter("utf-8", "1.0");
        xw.formatting = 'indented';
        xw.indentChar = ' ';
        xw.indentation = 2;

        xw.writeStartDocument(undefined);   //without standalone, otherwise will throw invalid xml error
        xw.writeStartElement("Package");
        //Participants
        var participants = packageData.participants;
        if (participants) {
            var participantCount = participants.length;
            if (participantCount > 0) {
                xw.writeStartElement("Participants");
                for (var i = 0; i < participantCount; i++) {
                    var participant = participants[i];
                    xw.writeStartElement("Participant");
                    xw.writeAttributeString('type', participant.type);
                    xw.writeAttributeString('id', participant.id);
                    xw.writeAttributeString('name', participant.name);
                    xw.writeAttributeString('code', participant.code);
                    xw.writeAttributeString('outerId', participant.outerId);
                    xw.writeEndElement();   //end of single Participant
                }
                xw.writeEndElement();   //end of Participants
            }
        }
        
        //WorkflowProcess
        var process = packageData.process;
        xw.writeStartElement("WorkflowProcess")
        xw.writeStartElement("Process")
        xw.writeAttributeString("name", process.name);
        xw.writeAttributeString("id", process.id);

        if (process.description)
            xw.writeElementString("Description", jshelper.escapeHtml(process.description));

        //Activities
        var snodesCount = process.snodes.length;
        if (snodesCount > 0) {
            xw.writeStartElement("Activities")
            for (var j = 0; j < snodesCount; j++) {
                var snode = process.snodes[j];
                xw.writeStartElement("Activity");
                xw.writeAttributeString("name", snode.name);
                xw.writeAttributeString("id", snode.id);
                xw.writeAttributeString("code", snode.code);

                if (snode.description)
                    xw.writeElementString("Description", jshelper.escapeHtml(snode.description));

                xw.writeStartElement("ActivityType");
                xw.writeAttributeString("type", snode.type);
                if (snode.type == "GatewayNode") {
                    xw.writeAttributeString("gatewaySplitJoinType", snode.gatewaySplitJoinType);
                    xw.writeAttributeString("gatewayDirection", snode.gatewayDirection);
                } else if (snode.type == "SubProcessNode") {
                    xw.writeAttributeString("subid", snode.subid);
                }
                
                xw.writeEndElement()    //end of ActivityType

                if (snode.performers) {
                    var performersCount = snode.performers.length;
                    if (performersCount > 0) {
                        xw.writeStartElement("Performers");
                        for (var p = 0; p < performersCount; p++) {
                            var performer = snode.performers[p];
                            xw.writeStartElement("Performer");
                            xw.writeAttributeString("id", performer.id);
                            xw.writeEndElement();   //end of single Performer
                        }
                        xw.writeEndElement();   //end of Activity Performers
                    }
                }
                
                //activity geograpy
                xw.writeStartElement("Geography");
                xw.writeStartElement("Widget");
                xw.writeAttributeString("left", snode.left);
                xw.writeAttributeString("top", snode.top);
                xw.writeAttributeString("width", snode.width);
                xw.writeAttributeString("height", snode.height);

                //activity connectors
                var inputConnectorCount = snode.inputConnectors.length;
                var outputConnectorCount = snode.outputConnectors.length;
                if (inputConnectorCount > 0
                    || outputConnectorCount > 0)
                {
                    xw.writeStartElement("Connectors");
                    
                    if (inputConnectorCount > 0) {
                        for (var m = 0; m < inputConnectorCount; m++) {
                            var connector = snode.inputConnectors[m];
                            xw.writeStartElement("Connector");
                            xw.writeAttributeString("type", connector.type);
                            xw.writeAttributeString("index", connector.index);
                            xw.writeAttributeString("name", connector.name);
                            xw.writeEndElement();   //end of single Connector
                        }
                    }

                    if (outputConnectorCount > 0) {
                        for (var n = 0; n < outputConnectorCount; n++) {
                            var connector = snode.outputConnectors[n];
                            xw.writeStartElement("Connector");
                            xw.writeAttributeString("type", connector.type);
                            xw.writeAttributeString("index", connector.index);
                            xw.writeAttributeString("name", connector.name);
                            xw.writeEndElement();   //end of single Connector
                        }
                    }
                    xw.writeEndElement();   //end of Connectors
                }
                
                xw.writeEndElement();   //end of Widget
                xw.writeEndElement()    //end of Activity Geography

                xw.writeEndElement();   //end of single Activity
            }
            xw.writeEndElement()    //end of Activities
        }
       

        //Transtions
        if (process.slines) {
            var slinesCount = process.slines.length;
            if (slinesCount > 0) {
                xw.writeStartElement("Transitions")
                for (var t = 0; t < slinesCount; t++) {
                    var sline = process.slines[t];
                    xw.writeStartElement("Transition");
                    xw.writeAttributeString("id", sline.id);
                    xw.writeAttributeString("from", sline.from);
                    xw.writeAttributeString("to", sline.to);

                    if (sline.description)
                        xw.writeElementString("Description", jshelper.escapeHtml(sline.description));

                    if (sline.condition) {
                        xw.writeStartElement("Condition");
                        xw.writeCDATA(sline.condition);
                        xw.writeEndElement();
                    }

                    //Transtion Geography
                    xw.writeStartElement("Geography");
                    xw.writeStartElement("Line");
                    xw.writeAttributeString("fromConnector", sline.fromConnector);
                    xw.writeAttributeString("toConnector", sline.fromConnector);
                    xw.writeEndElement();   //end of Line
                    xw.writeEndElement();   //end of Transition Geography

                    xw.writeEndElement()    //end of Transition
                }
                xw.writeEndElement()    //end of Transitions
            }
        }
       
        
        xw.writeEndElement()    //end of Process
        xw.writeEndElement();   //end of WorkflowProcess
        xw.writeEndElement();    //end of Package

        xw.writeEndDocument();  //end of XmlDocument


        processFileEntity.ProcessGUID = processGUID;
        processFileEntity.XmlContent = xw.flush();  //output xml

        xw.close();

        return processFileEntity;
    }
    //#endregion
})()