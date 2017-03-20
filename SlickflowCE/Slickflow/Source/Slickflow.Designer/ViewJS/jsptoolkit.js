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

var jsptoolkit = (function () {
	function jsptoolkit() {
	}

	jsptoolkit.jspinstance = null;

	//intialize jsplumb instance
	jsptoolkit.init = function (container) {
		var instance = jsptoolkit.jspinstance = jsPlumb.getInstance({
			DragOptions: {
				cursor: 'pointer',
				zIndex: 2000
			},
			ConnectionOverlays: [
				["Arrow", {
					location: 1,
					visible: true,
					width: 11,
					length: 11,
					id: "ARROW",
					events: {
						click: function () { alert("you clicked on the arrow overlay") }
					}
				}],
				["Label", {
					location: 0.4,
					id: "label",
					cssClass: "aLabel",
					events: {
						tap: function () {
							//alert("hey");
						}
					}
				}]
			],
			Container: container
		});

		var basicType = {
			connector: "StateMachine",
			paintStyle: { stroke: "red", strokeWidth: 4 },
			hoverPaintStyle: { stroke: "blue" },
			//overlays: ["Arrow"]
		};

		instance.registerConnectionType("basic", basicType);

		//suspend drawing and initialise
		instance.batch(function () {
			instance.bind("click", function (conn, orgEvent) {
				//when connection clicked, its style will be reset to basic
				conn.toggleType("basic");
			});
		});
	}
	//#endregion

	//#region flowchart configuration, initialize
	var connectorType =	["Flowchart", { stub: [2, 2], gap: 1, cornerRadius: 5, alwaysRespectStubs: true }]
		,
		connectorPaintStyle = {
			strokeWidth: 2,
			stroke: "#61B7CF",
			joinstyle: "round",
			outlineStroke: "white",
			outlineWidth: 2
		},
		connectorHoverStyle = {
			strokeWidth: 3,
			stroke: "#216477",
			outlineWidth: 5,
			outlineStroke: "white"
		},
		endpointHoverStyle = {
			fill: "#216477",
			stroke: "#216477"
		},
		sourceEndpoint = {
			endpoint: "Dot",
			paintStyle: {
				stroke: "#7AB02C",
				fill: "transparent",
				radius: 4,
				strokeWidth: 1
			},
			isSource: true,
			connector: connectorType,
			connectorStyle: connectorPaintStyle,
			hoverPaintStyle: endpointHoverStyle,
			connectorHoverStyle: connectorHoverStyle,
			maxConnections: 10,
			dragOptions: {},
			overlays: [
				["Label", {
			   		location: [0.5, 1.5],
			   		label: "Drag",
			   		cssClass: "endpointSourceLabel",
			   		visible: false
				   }
				]
			]
		},
		targetEndpoint = {
			endpoint: "Dot",
			paintStyle: { fill: "#7AB02C", radius: 4 },
			hoverPaintStyle: endpointHoverStyle,
			maxConnections: -1,
			dragOptions: { hoverClass: "hover", activeClass: "active" },
			isTarget: true,
			overlays: [["Label", { location: [0.5, -0.5], label: "Drop", cssClass: "endpointTargetLabel", visible: false }]]
		};

	//#region endpoint manager
	function _addEndpoints(toId, sourceAnchors, targetAnchors) {
		for (var i = 0; i < sourceAnchors.length; i++) {
			var sourceUUID = toId + sourceAnchors[i];
			jsptoolkit.jspinstance.addEndpoint(toId, sourceEndpoint, {
				anchor: sourceAnchors[i], uuid: sourceUUID
			});
		}
		for (var j = 0; j < targetAnchors.length; j++) {
			var targetUUID = toId + targetAnchors[j];
			jsptoolkit.jspinstance.addEndpoint(toId, targetEndpoint, {
				anchor: targetAnchors[j], uuid: targetUUID
			});
		}
	};

	jsptoolkit.addEndpointsTask = function (nodeId) {
		_addEndpoints(nodeId, ["RightMiddle"], ["LeftMiddle"]);
	}

	jsptoolkit.addEndpointsStart = function (nodeId) {
		_addEndpoints(nodeId, ["RightMiddle"], []);
	}

	jsptoolkit.addEndpointsEnd = function (nodeId) {
		_addEndpoints(nodeId, [], ["LeftMiddle"]);
	}

	jsptoolkit.addEndpointsGateway = function (nodeId) {
		_addEndpoints(nodeId, ["RightMiddle"], ["LeftMiddle"]);
	}
	//#endregion

	//#region transition/line
	jsptoolkit.bindConnectionEvents = function (graph) {
		function findTransitonObject(from, to) {
			var sline = null;
			var slines = graph.packageData.process.slines;

			for (var i = 0; i < slines.length; i++) {
				if (slines[i].from === from
                    && slines[i].to === to) {
					sline = slines[i];
					break;
				}
			}
			return sline;
		}

		function findLineObject(source, target) {
			var line = null;
			var sline = findTransitonObject(source, target);

			for (var i = 0; i < graph.lines.length; i++) {
				if (sline.id === graph.lines[i].id()) {
					line = graph.lines[i];
					break;
				}
			}
			return line;
		}

		//find graph element and to fire related events
		jsptoolkit.jspinstance.bind("connection", function (info, orgEvent) {
			//packaged line object
			//find the transiton object in sline collection
			var source = info.source.id.substr(3, info.source.id.length - 3);
			var target = info.target.id.substr(3, info.target.id.length - 3);     //div id format: ACT+UUID

			var sline = findTransitonObject(source, target);
			if (sline === null) {
				var anchors = jsptoolkit.getConnectionAnchors(info.connection);
				//this is a new sline object
				sline = {
					id: jshelper.getUUID(),
					from: source,
					to: target,
					description: "",
					anchors: anchors,
					sourceId: info.connection.sourceId,             //example: ACTa3fc831f-2d94-418a-d243-c014d0692aa4
					targetId: info.connection.targetId,
					fromConnector: 1,
					toConnector: 1
				};

				//push sline into the slines
				if (kmain.mgraphView) {
					var slines = kmain.mgraphView.packageData.process.slines;
					slines.push(sline);

					//push line into the lines
					var line = new kgraph.Line(sline);
					kmain.mgraphView.lines.push(line);
				}
			}

			//bind connection click and dblclick event
			info.connection
				.bind("click", function (conn, orgEvent) {
					var source = info.source.id.substr(3, info.source.id.length - 3);
					var target = info.target.id.substr(3, info.target.id.length - 3);     //div id format: ACT+UUID
					var line = findLineObject(source, target);

					//set the current selected connection
					kgraph.mcurrentSelectedDomElement = {
						type: kgraph.Config.ELEMENT_TYPE_CONNECTION,
						connection: conn,
						line: line
					};

					//show pop menu...
					$(".popmenu").hide();

					var left = orgEvent.clientX;
					var top = orgEvent.clientY;

					$("#divPopMenuCommand")
						.css({
							left: left,
							top: top
						})
						.show();

					orgEvent.stopPropagation();
				})
				.bind("dblclick", function (conn, orgEvent) {
					kmain.showTransitionPropertyDialog();
				})
				.bind("mouseover", function (conn, orgEvent) {
					//alert("some words..");
				}); //end of bind function
			//window.console.log("connection is created...");
		});
	}

	jsptoolkit.connect = function (line) {
		var conn = jsptoolkit.jspinstance.connect({
			uuids: [line.sdata.sourceId + "RightMiddle", line.sdata.targetId + "LeftMiddle"],
			anchors: line.sdata.anchors,
		});

		jsptoolkit.setConnectionText(conn, line.description());

		return conn;
	}

	jsptoolkit.getConnectionAnchors = function (connection) {
		var anchors = $.map(connection.endpoints, function (endpoint) {
			return [[endpoint.anchor.x,
			endpoint.anchor.y,
			endpoint.anchor.orientation[0],
			endpoint.anchor.orientation[1],
			endpoint.anchor.offsets[0],
			endpoint.anchor.offsets[1]]];
		});
		return anchors;
	}

	jsptoolkit.setConnectionText = function(conn, text) {
		conn.getOverlay("label").setLabel(text);
	};
	//#endregion

	//#region jsplumb draggable, remove
	jsptoolkit.draggableSingleNode = function (nodeId, snode) {
		//update node position when drag or move node on canvas
		jsptoolkit.jspinstance.draggable(nodeId, {
			grid: [1, 1],
			stop: function (event, ui) {
				//update div node position
				var left = parseInt($("#" + nodeId).position().left, 10);
				var top = parseInt($("#" + nodeId).position().top, 10);

				snode.left = left;
				snode.top = top;

				if (snode.type === kgraph.Config.NODE_TYPE_GATEWAY) {
					snode.top += 9;		//to fix gateway node position top value
				}
			}
		});
	}

	jsptoolkit.remove = function (element) {
		jsptoolkit.jspinstance.remove(element);
	}

	jsptoolkit.detach = function (connection) {
		jsptoolkit.jspinstance.detach(connection);
	}

	jsptoolkit.detachAll = function (element) {
		jsptoolkit.jspinstance.detachAllConnections(element);
	}

	jsptoolkit.empty = function () {
		jsptoolkit.jspinstance.empty();
	}

	jsptoolkit.clearCanvas = function () {
		if (jsptoolkit.jspinstance) {
			jsptoolkit.jspinstance.deleteEveryEndpoint();
			jsptoolkit.jspinstance.detachEveryConnection();
			jsptoolkit.jspinstance.empty("#kgraphCanvas");
			$("#kgraphCanvas").empty();
		}
	}
	//#endregion

	return jsptoolkit;
})()


jsPlumb.ready(function () {
	//intialize jsplumb instance
	jsptoolkit.init('kgraphCanvas');
});