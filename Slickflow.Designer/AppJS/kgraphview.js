//Summary
//The flowChart is from http://www.codeproject.com/Articles/709340/Implementing-a-Flowchart-with-SVG-and-AngularJS
//License
//This article, along with any associated source code and files, is licensed under The MIT License
//2014.12.31

var kgraph;
if (!kgraph) kgraph = {};

(function () {
    kgraph.nodeWidth = 100;
    kgraph.nodeHeight = 50;
    kgraph.nodeNameHeight = 25;
    kgraph.connectorHeight = 35;
    kgraph.circleWidth = 40;
    kgraph.circleHeight = 20;
    kgraph.diamondWidth = 60;
    kgraph.diamondHeight = 60;

    //#region GraphView
    kgraph.GraphView = function (processGUID, packageData) {
        this.processGUID = processGUID;
        this.packageData = packageData;
        this.data = packageData.process;    //process definition data

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
        this.nodes = createNodes(this.data.snodes);
        
        //#endregion

        //#region node behavious
        this.getMaxNodeId = function () {
            var maxId = 0;
            window.console.log(this.nodes.length);
            for (var i = 0; i < this.nodes.length; i++)
            {
                if (this.nodes[i].data.nodeId > maxId)
                    maxId = this.nodes[i].data.nodeId;
            }
            maxId = maxId + 1;
            return maxId;
        }

        //add a new node
        this.addNode = function (element, x, y) {
            var nodeName = "请输入名称";
            if (element == "StartNode" || element == "EndNode" || element == "GatewayNode") nodeName = "";

            var node = {
                name: nodeName,
                id: jshelper.getUUID(),
                code: '',
                nodeId: this.getMaxNodeId(),
                text: "节点注释",
                type: element,
                x: x,
                y: y,
                width: 120,
                height: 50,
                inputConnectors: [{ type: "input", index: 1, name: "A" }],
                outputConnectors: [{ type: "output", index: 1, name: "X" }]
            };
            this.data.snodes.push(node);

            var knode = new kgraph.Node(node);
            this.nodes.push(knode);
        }

        //find node
        this.findNode = function (nodeId) {
            for (var i = 0; i < this.nodes.length; i++) {
                var node = this.nodes[i];
                if (node.data.nodeId == nodeId) {
                    return node;
                }
            }
            throw new Error("Failed to find node " + nodeId + "nodes count:" + this.nodes.length);
        }

        //get selected nodes
        this.getSelectedNodes = function () {
            var selectedNodes = [];
            for (var i = 0; i < this.nodes.length; i++) {
                var node = this.nodes[i];
                if (node.selected()) {
                    selectedNodes.push(node);
                }
            }
            return selectedNodes;
        }

        //node click
        this.handleNodeClicked = function (node, ctrlKey) {
            if (ctrlKey) {
                node.toggleSelected();
            } else {
                this.deselectAll();
                node.select();
            }

            var nodeIndex = this.nodes.indexOf(node);
            if (nodeIndex == -1) {
                throw new Error("Failed to find node in view model!");
            }

            this.nodes.splice(nodeIndex, 1);
            this.nodes.push(node);
        };
        //#endregion

        //#region line behavious
        //get selected lines
        this.getSelectedLines = function () {
            var selectedLines = [];

            for (var i = 0; i < this.lines.length; i++) {
                var line = this.lines[i];
                if (line.selected()) {
                    selectedLines.push(line);
                }
            }
            return selectedLines;
        };

        //connector
        this.findInputConnector = function (nodeId, connectorIndex) {
            var node = this.findNode(nodeId);

            if (!node.inputConnectors || node.inputConnectors.length < connectorIndex) {
                throw new Error("Node" + nodeId + " 无效的 input connector."
                     + " node inputconnectors length:" + node.inputConnectors.length);
            }
            return node.inputConnectors[connectorIndex - 1];
        }

        this.findOutputConnector = function (nodeId, connectorIndex) {
            var node = this.findNode(nodeId);

            if (!node.outputConnectors || node.outputConnectors.length < connectorIndex) {
                throw new Error("Node:" + nodeId + " 无效的 output connectors."
                    + " node outputconnectors length:" + node.outputConnectors.length);
            }
            return node.outputConnectors[connectorIndex - 1];
        }

        this.createLine = function (sline) {
            var sourceConnector = this.findOutputConnector(sline.source.nodeId,
                sline.source.connectorIndex);
            var destConnector = this.findInputConnector(sline.dest.nodeId,
                sline.dest.connectorIndex);

            return new kgraph.Line(sline, sourceConnector, destConnector);
        }

        //dragging between two connectors
        this.createNewLine = function (sourceConnector, destConnector) {
            var slines = this.data.slines;
            if (!slines) {
                slines = this.data.slines = [];
            }

            var lines = this.lines;
            if (!lines) {
                lines = this.lines = [];
            }

            var sourceNode = sourceConnector.parentNode();
            var sourceConnectorIndex = sourceNode.outputConnectors.indexOf(sourceConnector);
            if (sourceConnectorIndex == -1) {
                sourceConnectorIndex = sourceNode.inputConnectors.indexOf(sourceConnector);
                if (sourceConnectorIndex == -1) {
                    throw new Error("错误的Source Connector数据！");
                }
            }

            var destNode = destConnector.parentNode();
            var destConnectorIndex = destNode.inputConnectors.indexOf(destConnector);
            if (destConnectorIndex == -1) {
                destConnectorIndex = destNode.outputConnectors.indexOf(destConnector);
                if (destConnectorIndex == -1) {
                    throw new Error("错误的Dest Connector数据！");
                }
            }

            var sline = {
                id: jshelper.getUUID(),
                from: sourceNode.data.id,
                to: destNode.data.id,
                description: sourceNode.data.name + "->" + destNode.data.name,
                source: {
                    nodeId: sourceNode.data.nodeId,
                    connectorIndex: sourceConnectorIndex + 1
                },
                dest: {
                    nodeId: destNode.data.nodeId,
                    connectorIndex: destConnectorIndex + 1
                }
            };

            slines.push(sline);

            var line = new kgraph.Line(sline, sourceConnector, destConnector);
            lines.push(line);
        }

        //line mouse down
        this.handleLineMouseDown = function (line, ctrlKey) {
            if (ctrlKey) {
                line.toggleSelected();
            } else {
                this.deselectAll();
                line.select();
            }
        };
        //#endregion

        //#region render graph lines
        //create lines
        this.createLines = function (slines) {
            var lines = [];
            if (slines) {
                for (var i = 0; i < slines.length; i++) {
                    lines.push(this.createLine(slines[i]));
                }
            }
            return lines;
        }
        this.lines = this.createLines(this.data.slines);

        //#endregion

        //#region selection
        //selection
        this.selectAll = function () {
            var ndoes = this.nodes;
            for (var i = 0; i < nodes.length; i++) {
                var ndoe = nodes[i];
                node.select();
            }

            var lines = this.lines;
            for (var i = 0; i < lines.length; i++) {
                var line = lines[i];
                line.select();
            }
        }

        //deselection
        this.deselectAll = function () {
            var nodes = this.nodes;
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                node.deselect();
            }

            var lines = this.lines;
            for (var i = 0; i < lines.length; i++) {
                var line = lines[i];
                line.deselect();
            }
        }

        //delete selection
        this.deleteSelected = function () {
            var newNodeViewModels = [];
            var newNodeDataModels = [];
            var deletedNodeIds = [];

            for (var nodeIndex = 0; nodeIndex < this.nodes.length; nodeIndex++) {
                var node = this.nodes[nodeIndex];

                if (!node.selected()) {
                    newNodeViewModels.push(node);
                    newNodeDataModels.push(node.data);
                } else {
                    deletedNodeIds.push(node.data.nodeId);
                }
            }

            var newLineViewModels = [];
            var newLineDataModels = [];

            for (var lineIndex = 0; lineIndex < this.lines.length; lineIndex++) {
                var line = this.lines[lineIndex];

                if (!line.selected() &&
                    deletedNodeIds.indexOf(line.data.source.nodeId) === -1 &&
                    deletedNodeIds.indexOf(line.data.dest.nodeId) === -1) {

                    newLineViewModels.push(line);
                    newLineDataModels.push(line.data);
                }
            }

            //resave the nodes, nodes data, lines and lines data
            this.nodes = newNodeViewModels;
            this.data.snodes = newNodeDataModels;
            this.lines = newLineViewModels;
            this.data.slines = newLineDataModels;
        }; //end delete selected

        this.applySelectionRect = function (selectionRect) {
            this.deselectAll();

            for (var i = 0; i < this.nodes.length; i++) {
                var node = this.nodes[i];
                if (node.x() >= selectionRect.x &&
                    node.y() >= selectionRect.y &&
                    node.x() + node.width() <= selectionRect.x + selectionRect.width &&
                    node.y() + node.height() <= selectionRect.y + selectionRect.height) {
                    node.select();
                }
            }

            for (var i = 0; i < this.lines.length; i++) {
                var line = this.lines[i];
                if (line.source.parentNode().selected() &&
                    line.dest.parentNode().selected()) {
                    line.select();
                }
            }
        };
        //#endregion

        //#region dragging
        //dragging
        this.updateSelectedNodesLocation = function (deltaX, deltaY) {
            var selectedNodes = this.getSelectedNodes();
            //window.console.log("selected nodes length: " + selectedNodes.length);

            for (var i = 0; i < selectedNodes.length; i++) {
                var node = selectedNodes[i];

                node.data.x += deltaX;
                node.data.y += deltaY;
                //window.console.log("node x: " + node.data.x + "node y: " + node.data.y);
            }
        };
        //#endregion
    };  //end graph view
    //#endregion

    //#region Node
    kgraph.Node = function (snode) {
        this.data = snode;
        this._selected = false;

        this.name = function () {
            return this.data.name || "";
        }

        this.text = function () {
            return this.data.text || "";
        }


        this.type = function () {
            return this.data.type;
        }

        this.x = function () {
            return this.data.x;
        }

        this.y = function () {
            return this.data.y;
        }

        this.width = function () {
            return kgraph.nodeWidth;
        }

        this.height = function () {
            return kgraph.nodeHeight;
        }

        this.select = function () {
            this._selected = true;
        }

        this.deselect = function () {
            this._selected = false;
        }

        this.toggleSelected = function () {
            this._selected = !this._selected;
        }

        this.selected = function () {
            return this._selected;
        }

        this.inputConnectors = createInputConnectors(this.data.inputConnectors, this);
        this.outputConnectors = createOutputConnectors(this.data.outputConnectors, this);
    };
    //#endregion

    var createInputConnectors = function (sconnectors, parentNode) {
        var connectors = [];
        var x = 0, y = 0;

        if (parentNode.type() == "StartNode" || parentNode.type() == "EndNode") {
            x = -(kgraph.circleWidth / 2);
        } else if (parentNode.type() == "TaskNode"){
            y = kgraph.nodeHeight / 2;
        } else if (parentNode.type() == "GatewayNode") {
            x = -(kgraph.diamondWidth / 2);
            y = kgraph.diamondHeight / 2;
        }

        if (sconnectors) {
            for (var i = 0; i < sconnectors.length; i++) {
                var connector = new kgraph.Connector(sconnectors[i],
                    x,
                    y,
                    parentNode);
                connectors.push(connector);
            }
        }
        return connectors;
    };

    var createOutputConnectors = function (sconnectors, parentNode) {
        var connectors = [];
        var x = 0, y = 0;

        if (parentNode.type() == "StartNode" || parentNode.type() == "EndNode") {
            x = kgraph.circleWidth / 2;
        } else if (parentNode.type() == "TaskNode") {
            x = kgraph.nodeWidth;
            y = kgraph.nodeHeight / 2;
        } else if (parentNode.type() == "GatewayNode") {
            x = kgraph.diamondWidth / 2;
            y = kgraph.diamondHeight / 2;
        }

        if (sconnectors) {
            for (var i = 0; i < sconnectors.length; i++) {
                var connector = new kgraph.Connector(sconnectors[i],
                    x,
                    y,
                    parentNode);
                connectors.push(connector);
            }
        }
        return connectors;
    };

    //#region Connector
    kgraph.Connector = function (sconnector, x, y, parentNode) {
        this.data = sconnector;
        this._x = x;
        this._y = y;
        this._parent = parentNode;

        this.name = function () {
            return this.data.name;
        }

        this.x = function () {
            return this._x;
        }

        this.y = function () {
            return this._y;
        }

        this.parentNode = function () {
            return this._parent;
        }
    };
    //#endregion

    //#region Line
    kgraph.Line = function (sline, sourceConnector, destConnector) {
        this.data = sline;
        this.source = sourceConnector;
        this.dest = destConnector;
        this._selected = false;

        this.sourceCoordX = function () {
            return this.source.parentNode().x() + this.source.x();
        }

        this.sourceCoordY = function () {
            return this.source.parentNode().y() + this.source.y();
        }

        this.sourceCoord = function () {
            return {
                x: this.sourceCoordX(),
                y: this.sourceCoordY()
            };
        }

        this.sourceTangentX = function () {
            return kgraph.computeLineSourceTangentX(this.sourceCoord(), this.destCoord());
        }

        this.sourceTangentY = function () {
            return kgraph.computeLineSourceTangentY(this.sourceCoord(), this.destCoord());
        }

        this.destCoordX = function () {
            return this.dest.parentNode().x() + this.dest.x();
        }

        this.destCoordY = function () {
            return this.dest.parentNode().y() + this.dest.y();
        }

        this.destCoord = function () {
            return {
                x: this.destCoordX(),
                y: this.destCoordY()
            };
        }

        this.destTangentX = function () {
            return kgraph.computeLineDestTangentX(this.sourceCoord(), this.destCoord());
        }

        this.destTangentY = function () {
            return kgraph.computeLineDestTangentY(this.sourceCoord(), this.destCoord());
        }

        this.select = function () {
            this._selected = true;
        }

        this.deselect = function () {
            this._selected = false;
        }

        this.toggleSelected = function () {
            this._selected = !this._selected;
        }

        this.selected = function () {
            return this._selected;
        }
    };
    //#endregion

    //#region Tangent Computing
    var computeLineTangentOffset = function (pt1, pt2) {
        return (pt2.x - pt1.x) / 2;
    }

    kgraph.computeLineSourceTangentX = function (pt1, pt2) {
        return pt1.x + computeLineTangentOffset(pt1, pt2);
    }

    kgraph.computeLineSourceTangentY = function (pt1, pt2) {
        return pt1.y;
    }

    kgraph.computeLineSourceTangent = function (pt1, pt2) {
        return {
            x: kgraph.computeLineSourceTangentX(pt1, pt2),
            y: kgraph.computeLineSourceTangentY(pt1, pt2)
        };

    }

    kgraph.computeLineDestTangentX = function (pt1, pt2) {
        return pt2.x - computeLineTangentOffset(pt1, pt2);
    }

    kgraph.computeLineDestTangentY = function (pt1, pt2) {
        return pt2.y;
    }

    kgraph.computeLineDestTangent = function (pt1, pt2) {
        return {
            x: kgraph.computeLineDestTangentX(pt1, pt2),
            y: kgraph.computeLineDestTangentY(pt1, pt2)
        }
    }

    kgraph.computeConnectorY = function (connectorIndex) {
        return kgraph.nodeNameHeight + (connectorIndex * kgraph.connectorHeight);
    }

    kgraph.computeConnectorPos = function (node, connectorIndex, inputConnector) {
        //compare with the x,y position of connector with different node
        var pos = null;
        if (node.type() == "StartNode" || node.type() == "EndNode") {
            pos = {
                x: node.x() + (inputConnector ? (0 - kgraph.circleWidth / 2) : kgraph.circleWidth / 2),
                y: node.y()
            };
        }
        else if (node.type() == "TaskNode") {
            pos = {
                x: node.x() + (inputConnector ? 0 : kgraph.nodeWidth),
                y: node.y() + kgraph.computeConnectorY(connectorIndex)
            };
        }
        else if (node.type() == "GatewayNode") {
            pos = {
                x: node.x() + (inputConnector ? (0 - kgraph.diamondWidth / 2) : kgraph.diamondWidth / 2),
                y: node.y() + kgraph.computeConnectorY(connectorIndex)
            };
        }

        return pos;
    }
    //#endregion

})()
