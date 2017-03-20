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

var kgraph = (function () {
	function kgraph() {
	}

	//#region kgraph definitioin
	kgraph.mcurrentSelectedDomElement = null;

	kgraph.Config = {
		NODE_PREFIX: "ACT",
		NODE_TYPE_START: "StartNode",
		NODE_TYPE_TASK: "TaskNode",
		NODE_TYPE_END: "EndNode",
		NODE_TYPE_GATEWAY: "GatewayNode",
		NODE_TYPE_SUBPROCESS: "SubProcessNode",
		ELEMENT_TYPE_NODE: "NODE",
		ELEMENT_TYPE_CONNECTION: "CONNECTION"
	};
	//#endregion

	//drop single bpmn noation part on graph canvas
	kgraph.drawSingleNode = function (target) {
		var snode = {
			id: jshelper.getUUID(),     //get new uuid for node id
			code: "",
			type: target.type,
			complexType: target.complexType,
			left: target.left,
			top: target.top,
			inputConnectors: [],
			outputConnectors: []
		};

		//set node name accord by node type
		if (target.type === kgraph.Config.NODE_TYPE_START)
			snode.name = "开始"
		else if (target.type === kgraph.Config.NODE_TYPE_END)
			snode.name = "结束"
		else if (target.type === kgraph.Config.NODE_TYPE_GATEWAY)
			snode.name = ""
		else if (target.type === kgraph.Config.NODE_TYPE_SUBPROCESS)
			snode.name = "子流程";
		else if (target.type === kgraph.Config.NODE_TYPE_TASK)
			snode.name = "任务";
		else
			throw new Error("未知节点类型！");

		var node = new kgraph.Node(snode);

		kmain.mgraphView.processData.snodes.push(snode);
		kmain.mgraphView.nodes.push(node);

		return snode;
	}
	//#endregion


	//#region Node
	kgraph.Node = function (snode) {
		this.sdata = snode;
		var self = this;

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

		this.complexType = function () {
			return this.sdata.complexType;
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
			var nodeId = kgraph.Config.NODE_PREFIX + this.id();
			var nodeType = this.type();

			//render different type node
			if (nodeType === kgraph.Config.NODE_TYPE_START) {
				element = this.renderStartNode(nodeId, this);
			}
			else if (nodeType === kgraph.Config.NODE_TYPE_END) {
				element = this.renderEndNode(nodeId, this);
			}
			else if (nodeType === kgraph.Config.NODE_TYPE_TASK) {
				element = this.renderTaskNode(nodeId, this);
			}
			else if (nodeType === kgraph.Config.NODE_TYPE_GATEWAY) {
				element = this.renderGatewayNode(nodeId, this);
			}
			else if (nodeType === kgraph.Config.NODE_TYPE_SUBPROCESS) {
				element = this.renderSubProcessNode(nodeId, this);
			}
			else {
				throw new Error("未知节点类型！");
			}

			//update node widget width and height
			this.sdata.width = element.width();
			this.sdata.height = element.height();

			//binding click and dblclick event for node
			element
				.bind("mousedown", function (e) {
					e.stopPropagation();
				})
                .bind("click", function (e) {
                	$(".popmenu").hide();
                	kgraph.mcurrentSelectedDomElement = {
                		type: kgraph.Config.ELEMENT_TYPE_NODE,
                		node: self,
                		element: this
                	};

                	//show pop menu...
                	var left = $("#kgraphCanvas").position().left + element.position().left + element.width() + 30;
                	var top = $("#kgraphCanvas").position().top + element.position().top;
                	var currentNode = kgraph.mcurrentSelectedDomElement.node.sdata;
                	var $elem = $("#divPopMenu");

                	if (currentNode.type === kgraph.Config.NODE_TYPE_END) {
                		$elem = $("#divPopMenuCommand");
                	}

                	$elem
						.css({
							left: left,
							top: top
						})
						.show();

                	e.stopPropagation();
                })
                .bind("dblclick", function () {
                	kmain.showActivityProperty(self);
                });

			//make node draggable
			jsptoolkit.draggableSingleNode(nodeId, snode);
		}//end of render function

		//render task node
		this.renderTaskNode = function (nodeId, node) {
			var element = $("<div>").attr("id", nodeId).addClass('task').addClass('gnode');
			element
				.text(node.name())
				.css({
					left: this.sdata.left,
					top: this.sdata.top,
					position: 'absolute',
				})
				.appendTo($('#kgraphCanvas'));

			jsptoolkit.addEndpointsTask(nodeId);
			return element;
		}

		//create gateway node
		this.renderGatewayNode = function (nodeId, node) {
			var icon = $("<div>").addClass("ctrl_container");	//.text("X");
			var element = $("<div>").addClass("decision").addClass("gnode").attr("id", nodeId).css({
				left: this.sdata.left,
				top: this.sdata.top,
				position: "absolute"
			});

			element
				.append(icon)
        		.appendTo($('#kgraphCanvas'));

			jsptoolkit.addEndpointsGateway(nodeId);
			return element;
		}

		//create sub process node
		this.renderSubProcessNode = function (nodeId, node) {
			var element = $("<div>").attr("id", nodeId).addClass('task').addClass('gnode');
			element
				.css({
					left: this.sdata.left,
					top: this.sdata.top,
					position: 'absolute',
				})
        		.text(node.name())
				.appendTo($('#kgraphCanvas'));

			jsptoolkit.addEndpointsTask(nodeId);
			return element;
		}

		//render start node
		this.renderStartNode = function (nodeId, node) {
			var element = $("<div>").attr("id", nodeId).addClass('circle-start').addClass('gnode');
			element
				.css({
					left: this.sdata.left,
					top: this.sdata.top,
					position: 'absolute'
				})
        		.appendTo($('#kgraphCanvas'));

			jsptoolkit.addEndpointsStart(nodeId);
			return element;
		}

		//render end node
		this.renderEndNode = function (nodeId, node) {
			var element = $("<div>").attr("id", nodeId).addClass('circle-end').addClass('gnode');
			element
				.css({
					left: this.sdata.left,
					top: this.sdata.top,
					position: 'absolute'
				})
        		.appendTo($('#kgraphCanvas'));

			jsptoolkit.addEndpointsEnd(nodeId);
			return element;
		}

		//finally render a new node...
		this.render();
	};
	//#endregion

	//#region Line
	kgraph.Line = function (sline) {
		this.sdata = sline;
		this.connection = null;

		var self = this;

		this.id = function () {
			return this.sdata.id;
		}

		this.from = function () {
			return this.sdata.from;
		}

		this.to = function () {
			return this.sdata.to;
		}

		this.description = function () {
			return this.sdata.description;
		}

		this.render = function () {
			this.connection = jsptoolkit.connect(this);
		}
	}
	//#endregion

	//#region GraphView
	kgraph.GraphView = function (graphData) {
		var self = this;

		this.processGUID = graphData.processGUID;
		this.packageData = graphData.packageData;
		this.processData = graphData.packageData.process;    //process definition data

		//bind connection event
		jsptoolkit.bindConnectionEvents(this);

		//#region render graph nodes
		//create nodes
		function createNodes (snodes) {
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
		function createLines (slines) {
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
		//#endregion
	};  //end graph view
	//#endregion

	return kgraph;
})()