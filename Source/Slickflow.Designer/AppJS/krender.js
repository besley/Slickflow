var krender;
if (!krender) krender = {};

(function () {
    //#region graph behavious
    //create a new grpah with start and end node
    krender.createNew = function (processEntity) {
        var packageData = {},
            process = {};

        var snodes = [{
            id: jshelper.getUUID(),
            name: "开始",
            code: "start",
            nodeId: 1,
            type: "StartNode",
            x: 370,
            y: 70,
            width: 50,
            height: 50,
            inputConnectors: [{ type: "input", index: 1, name: "A" }],
            outputConnectors: [{ type: "output", index: 1, name: "X" }]
        },
        {
            id: jshelper.getUUID(),
            name: "结束",
            code: "end",
            nodeId: 2,
            type: "EndNode",
            x: 370,
            y: 370,
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

    //加载XML内容，序列化为Javascript对象
    krender.load = function (processFileEntity) {
        var packageData = {},
            process = {};
        var participants = [];

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

                var nodeId = parseInt($(widget).attr("nodeId"));
                activity.nodeId = nodeId;

                activity.x = parseInt($(widget).attr("left"));
                activity.y = parseInt($(widget).attr("top"));
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

                transition.source = {};
                transition.dest = {};

                var geographyElement = $(this).find("Geography");
                var lineElement = $(geographyElement).find("Line");

                transition.source.nodeId = $(lineElement).attr("fromNode");
                transition.source.connectorIndex = $(lineElement).attr("fromConnector");
                transition.dest.nodeId = $(lineElement).attr("toNode");
                transition.dest.connectorIndex = $(lineElement).attr("toConnector");

                slines.push(transition);
            });
        } //end of processElement

        //render data
        var processGUID = processFileEntity.ProcessGUID;
        var graph = new kgraph.GraphView(processGUID, packageData);

        return graph;
    }

    //序列化Javscript实体为XML
    krender.serialize = function (processGUID, packageData) {
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
                xw.writeAttributeString("nodeId", snode.nodeId);
                xw.writeAttributeString("left", snode.x);
                xw.writeAttributeString("top", snode.y);
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
                    xw.writeAttributeString("fromNode", sline.source.nodeId);
                    xw.writeAttributeString("fromConnector", sline.source.connectorIndex);
                    xw.writeAttributeString("toNode", sline.dest.nodeId);
                    xw.writeAttributeString("toConnector", sline.dest.connectorIndex);
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