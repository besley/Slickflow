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

Notes:
JSPlumb library is referenced from: https://github.com/sporritt/jsPlumb
License
JsPlumb project is licensed under The MIT License
2015.05.12

*/


var kgraph;
if (!kgraph) kgraph = {};

(function () {

    //#region kgraph Configuration part
    kgraph.Config = {
        NODE_PREFIX: "ACT",
        endpoint: ["Dot", { radius: 3 }],
        connector: ["Bezier", { curviness: 100 }],
        connectorOverlays : [["Arrow", { width: 10, length: 10, location: 1 }]],
        connectorPaintStyle : {
                                    strokeStyle: "rgb(0,32,80)",
                                    fillStyle: "rgb(0,32,80)",
                                    opacity: 0.5,
                                    radius: 2,
                                    lineWidth: 2
                                },
        connectorStyle: {
                            lineWidth: 4,
                            strokeStyle: "rgb(0,32,80)",
                            joinstyle: "round",
                            outlineColor: "rgb(251,251,251)",
                            outlineWidth: 2
                        },
        connectorHoverStyle: {
                                lineWidth: 4,
                                strokeStyle: "#216477",
                                outlineWidth: 0,
                                outlineColor: "rgb(251,251,251)"
                            }
    };
    //#endregion

    //#region GraphView
    kgraph.GraphView = function (graphData) {
        var self = this;

        this.processGUID = graphData.processGUID;
        this.packageData = graphData.packageData;
        this.processData = graphData.packageData.process;    //process definition data

        //#region init jsplumb 
        var initJsPlumb = function () {
            //clear graph container
            //$("#kgraphContainer").remove();

            //set graph container
            jsPlumb.setContainer("kgraphContainer");

            //bind connection event
            jsPlumb.bind("connection", function (info, orgEvent) {
                //packaged line object
                //find the transiton object in sline collection
                var source = info.source.id.substr(3, info.source.id.length - 3);
                var target = info.target.id.substr(3, info.target.id.length - 3);     //div id format: ACT+UUID

                var sline = findTransitonObject(source, target);        
                if (sline === null) {
                    //this is a new sline object
                    sline = {
                        id: jshelper.getUUID(),
                        from: source,
                        to: target,
                        description: "请输入转移描述信息",
                        fromConnector: 1,
                        toConnector: 1
                        
                    };

                    //push sline into the slines
                    var slines = graphData.packageData.process.slines;
                    slines.push(sline);

                    //push line into the lines
                    var line = new kgraph.Line(sline);
                    self.lines.push(line);
                }

                //bind connection click and dblclick event
                info.connection
                    .bind("click", function (conn, orgEvent) {
                        var source = info.source.id.substr(3, info.source.id.length - 3);
                        var target = info.target.id.substr(3, info.target.id.length - 3);     //div id format: ACT+UUID
                        var line = findTransitionObjectWrapper(source,
                                        target);   

                        toggleConnection(conn, line);
                    })
                    .bind("dblclick", function (conn, orgEvent) {
                        var transitionDialog = $("#divTransitionDialog")
                            .load("/sfd/views/transitionproperty.html",
                                function () {
                                    var dialogOptions = {
                                        title: "转移定义数据",
                                        width: 500,
                                        height: 400,
                                        modal: true,
                                        autoOpen: false,
                                        beforeClose: function (evt, ui) {
                                            ;
                                        },
                                        close: function (event, ui) {
                                            //$(this).dialog("destroy");
                                        }
                                    };

                                    var line = kmain.currentSelectedDomElement.line;

                                    transitionDialog
                                        .data("line", line)
                                        .dialog(dialogOptions)
                                        .dialog('open');
                                }
                        );  //end of load function
                    }); //end of bind function
                //window.console.log("connection is created...");
            })


            jsPlumb.bind("connectionDetached", function (info, orgEvent) {
                //remove the connection


                //window.console.log("connection detached...");
            })

            jsPlumb.registerConnectionTypes({
                "selected": {
                    paintStyle: { strokeStyle: "red", lineWidth: 5 },
                    hoverPaintStyle: { lineWidth: 7 },
                    cssClass: "connector-selected"
                }
            });
        }

        //toggle connection style
        var toggleConnection = function (conn, line) {
            if (kmain.currentSelectedDomElement != null) {
                if (kmain.currentSelectedDomElement.connection != null) {
                    kmain.currentSelectedDomElement.connection.toggleType("selected");
                } else if (kmain.currentSelectedDomElement.node != null) {
                    $(kmain.currentSelectedDomElement.element).toggleClass("highlight");
                }
            }
            kmain.currentSelectedDomElement = { type: "CONNECTION", connection: conn, line: line };
            conn.toggleType("selected");
        }

        var queryLine = function (sline, lines) {
            var line = null;
            for (var i = 0; i < lines.length; i++) {
                if (sline.id === lines[i].sdata.id) {
                    line = lines[i];
                    break;
                }
            }
            return line;
        }

        //var getTransitionDescription = function (source, target) {
        //    var sourceText = $("#" + source.id).text();
        //    var targetText = $("#" + target.id).text();

        //    var description = sourceText + "->" + targetText;
        //    return description;
        //}

        var findTransitonObject = function (from, to) {
            var sline = null;
            var slines = graphData.packageData.process.slines;

            for (var i = 0; i < slines.length; i++) {
                if (slines[i].from === from 
                    && slines[i].to === to) {
                    sline = slines[i];
                    break;
                }
            }
            return sline;
        }

        var findTransitionObjectWrapper = function (source, target) {
            var line = null;
            var sline = findTransitonObject(source, target);

            for (var i = 0; i < self.lines.length; i++) {
                if (sline.id == self.lines[i].id()) {
                    line = self.lines[i];
                    break;
                }
            }
            return line;
        }
        
        //initialize jsplumb common properties
        initJsPlumb();

        //#endregion
        

        //#region render graph nodes
        //create nodes
        var createNodes = function (snodes) {
            var node = null,
                nodes = [];

            if (snodes) {
                for (var i = 0; i < snodes.length; i++) {
                    node = new kgraph.Node(snodes[i]);
                    nodes.push(node);
                }
            }
            return nodes;
        }

        this.nodes = createNodes(this.processData.snodes);

        //create lines
        var createLines = function (slines) {
            var line = null, lines = [];
            if (slines) {
                for (var i = 0; i < slines.length; i++) {
                    line = new kgraph.Line(slines[i]);
                    line.render();

                    lines.push(line);
                }
            }
            return lines;
        }

        //create lines
        this.lines = createLines(this.processData.slines);

        //drop single bpmn noation part on graph canvas
        this.createNode = function (target) {
            var snode = {
                id: jshelper.getUUID(),     //get new uuid for node id
                name: "新节点",
                code: "",
                type: target.type,
                left: target.left,
                top: target.top,
                inputConnectors: [],
                outputConnectors: []
            };
             
            var node = new kgraph.Node(snode);
            this.processData.snodes.push(snode);
            this.nodes.push(node);
        }
        //#endregion

    };  //end graph view
    //#endregion

    //#region Node
    kgraph.Node = function (snode) {
        this.sdata = snode;
        var self = this;

        var commonStyle = {
            endpoint: kgraph.Config.endpoint,
            paintStyle: kgraph.Config.connectorPaintStyle,
            hoverPaintStyle: { fillStyle: "lightblue" },
            connectorStyle: kgraph.Config.connectStyle,
            connectorHoverStyle: kgraph.Config.connectorHoverStyle,
            isSource: true,
            isTarget: true,
            //connector: ["Flowchart", { stub: [40, 60], gap: 10, cornerRadius: 5, alwaysRespectStubs: true }],  //连接线的样式种类有[Bezier],[Flowchart],[StateMachine ],[Straight ]
            connector: kgraph.Config.connector,
            maxConnections: -1,
            connectorOverlays: kgraph.Config.connectorOverlays
        };


        this.id = function () {
            return this.sdata.id;
        }

        this.name = function () {
            return this.sdata.name || "";
        }

        this.code = function () {
            return this.sdata.code || "";
        }

        this.text = function () {
            return this.sdata.text || "";
        }

        this.type = function () {
            return this.sdata.type;
        }

        this.left = function () {
            return this.sdata.left;
        }

        this.top = function () {
            return this.sdata.top;
        }

        this.width = function () {
            return this.sdata.width;
        }

        this.height = function () {
            return this.sdata.height;
        }

        this.setNodeName = function (name) {
            var nodeId = kgraph.Config.NODE_PREFIX + this.id();
            $("#" + nodeId).text(name);
        }

        this.render = function () {
            var element = null;
            var nodeId = "ACT" + this.id();

            //render different type node
            if (this.type() === "StartNode") {
                element = this.renderStartNode(nodeId);
            }
            else if (this.type() === "EndNode") {
                element = this.renderEndNode(nodeId);
            }
            else if (this.type() === "TaskNode") {
                element = this.renderTaskNode(nodeId, this);
            }
            else if (this.type() === "GatewayNode") {
                element = this.renderGatewayNode(nodeId, this);
            }

            //update node widget width and height
            this.sdata.width = element.width();
            this.sdata.height = element.height();

            //binding click and dblclick event for node
            element
                .bind("click", function () {
                    toggleDomElement(self, this);
                })
                .bind("dblclick", function () {
                    openActivityPropertyDialog(self)
                });

            //update node position when drag or move node on canvas
            jsPlumb.draggable(nodeId, {
                stop: function (event, ui) {
                    //update div node position
                    var left = ui.position.left;
                    var top = ui.position.top;

                    snode.left = left;
                    snode.top = top;
                }
            });
        }

        //update selected css style for dom element
        var toggleDomElement = function (node, element){
            if (kmain.currentSelectedDomElement != null){
                if(kmain.currentSelectedDomElement.connection != null) {
                    kmain.currentSelectedDomElement.connection.toggleType("selected");
                } else if (kmain.currentSelectedDomElement.node != null){
                    $(kmain.currentSelectedDomElement.element).toggleClass("highlight");
                }
            }
            kmain.currentSelectedDomElement = { type: "NODE", node: node, element: element };
            $(kmain.currentSelectedDomElement.element).toggleClass("highlight");
        }

        //render task node
        this.renderTaskNode = function (nodeId, node) {
            var element = $("<div>").attr("id", nodeId).addClass('item');

            element.css({
                left: this.sdata.left,
                top: this.sdata.top,
                position: 'absolute',
            });

            element.text(node.name());

            $('#kgraphContainer').append(element);
            //$("#" + chartID).draggable({ containment: "parent" });//保证拖动不跨界

            jsPlumb.addEndpoint(nodeId, {
                anchors: ["Left"]
            }, commonStyle)

            jsPlumb.addEndpoint(nodeId, {
                anchors: ["Right"]
            }, commonStyle)

            return element;
        }

        this.renderGatewayNode = function (nodeId, node) {
            var element = $("<div>").attr("id", nodeId).addClass('gateway');

            element.css({
                left: this.sdata.left,
                top: this.sdata.top,
                position: 'absolute',
            });

            //element.text(node.name());

            $('#kgraphContainer').append(element);

            jsPlumb.addEndpoint(nodeId, {
                anchors: ["Left"]
            }, commonStyle)

            jsPlumb.addEndpoint(nodeId, {
                anchors: ["Right"]
            }, commonStyle)

            return element;
        }

        //render start node
        this.renderStartNode = function (nodeId) {
            var element = $("<div>").attr("id", nodeId).addClass('item').addClass('circle-start');
            element.css({
                left: this.sdata.left,
                top: this.sdata.top,
                position: 'absolute'
            });
            $('#kgraphContainer').append(element);

            jsPlumb.addEndpoint(nodeId, {
                anchors: ["Right"]
            }, commonStyle)

            return element;
        }

        //render end node
        this.renderEndNode = function (nodeId) {
            var element = $("<div>").attr("id", nodeId).addClass('item').addClass('circle-end');
            element.css({
                left: this.sdata.left,
                top: this.sdata.top,
                position: 'absolute'
            });
            $('#kgraphContainer').append(element);


            jsPlumb.addEndpoint(nodeId, {
                anchors: ["Left"]
            }, commonStyle)

            return element;
        }

        this.dropNode = function () {
            alert("node was dropped.");
        }

        //finally render a new node...
        this.render();

        //activity property dialog page
        var openActivityPropertyDialog = function (node) {
            $("#divActivityDialog").children().remove();
            var pageTitle = "";
            var pageUrl = "";

            if (node.type() == "TaskNode") {
                pageTitle = "活动定义数据";
                pageUrl = "/sfd/views/activityproperty.html";
            } else if (node.type() == "GatewayNode") {
                pageTitle = "Gateway定义数据";
                pageUrl = "/sfd/views/gatewayproperty.html";
            } else {
                return;
            }

            var dialogOptions = {
                title: pageTitle,
                width: 530,
                height: 500,
                modal: true,
                autoOpen: false,
                beforeClose: function (evt, ui) {
                    ;
                },
                close: function (event, ui) {
                    $(this).children().remove();
                    $(this).dialog("destroy");
                }
            };

            var activityDialog = $("#divActivityDialog")
                .load(pageUrl,
                    function () {
                        activityDialog
                            .data("node", node)
                            .dialog(dialogOptions)
                            .dialog('open');
                    }
            );
        }
    };
    //#endregion

    //#region Line
    kgraph.Line = function (sline) {
        this.sdata = sline;
        this.connection = null;
        var self = this;

        this.id = function (){
            return this.sdata.id;
        }

        this.from = function () {
            return this.sdata.from;
        }

        this.to = function () {
            return this.sdata.to;
        }

        this.render = function () {
            var sourceDiv = "ACT" + sline.from;
            var targetDiv = "ACT" + sline.to;
            
            //create a new connection
            var conn = jsPlumb.connect({
                source: sourceDiv,
                target: targetDiv,
                anchors: ["Right", "Left"],
                endpoint: kgraph.Config.endpoint,
                connector: kgraph.Config.connector,
                connectorStyle: kgraph.Config.connectorStyle,
                connectorHoverStyle: kgraph.Config.connectorHoverStyle,
                connectorOverlays: kgraph.Config.connectorOverlays,
                overlays: kgraph.Config.connectorOverlays,
                detachable: false
            });

            this.connection = conn;
        }
    }
    //#endregion
})()
