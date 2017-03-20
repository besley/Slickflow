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

var kmain = (function () {
	function kmain() {
	}

	//#region initialize
	kmain.mselectedActivityPerformerGUID = "";
	kmain.mselectedParticipantType = null;
	kmain.mselectedParticipantItem = null;
	kmain.mcurrentXmlContent = "";

	kmain.init = function () {
		//waiting...
		showProgressBar();

		//attach events
		attachEvents();
		setImagePartDraggable();
		setCanvasDroppable();

		$("#kgraphCanvas").on("click", function (e) {
			$(this).focus();
		})

		//fix two modal dialog in IE9 to IE11
		$.fn.modal.Constructor.prototype.enforceFocus = function () { };

		//register some event
		processlist.beforeRender.subscribe(renderProcess);
	}
	//#endregion

	//#region preparation
	function showProgressBar() {
		$('.progress .progress-bar').progressbar({
			transition_delay: 200
		});

		var $modal = $('.js-loading-bar'),
            $bar = $modal.find('.bar');

		$modal.modal('show');

		setTimeout(function () {
			$modal.modal('hide');
		}, 500);
	}

	function attachEvents() {
		//registered keyup event for document
		$("body")
			.click(function (e) {
				$(".popmenu").hide();
			})
			.keyup(function (e) {
				if (e.target.id != "kgraphCanvas") return false;
				if (e.keyCode === 46) {
					if (kgraph.mcurrentSelectedDomElement !== null) {
						//remove node or connection
						kmain.removeCanvasElement();
					}
				}
			});
	}
	//#endregion

	//#region panel drag and drop event
	function setImagePartDraggable () {
		$(".imagepart").draggable({
			helper: "clone",//复制自身
			scope: "dragflag"//标识
		});
	}

	function setCanvasDroppable () {
		$("#kgraphCanvas").droppable({
			accept: ".imagepart",
			activeClass: "drop-active",
			scope: "dragflag",
			cursor: "cross",
			drop: function (event, ui) {
				var left = parseInt(ui.offset.left - $(this).offset().left, 10);
				var top = parseInt(ui.offset.top - $(this).offset().top, 10);
				var nodeType = ui.draggable[0].id;
				var target = {
					type: nodeType,
					left: left,
					top: top
				}

				//drag bpmn image part and drop to the graph container
				//will create a new node and update the node collection
				if (kmain.mgraphView === undefined) {
					$.msgBox({
						title: "Designer / Index",
						content: "请先打开流程记录！",
						type: "info"
					});
					return;
				}

				kgraph.drawSingleNode(target);
			}
		});
	}
	//#endregion

	//#region add/remove node or connection element
	kmain.addNewNodeWithConnection = function (event, type) {
		var left = parseInt(event.clientX - event.offsetX, 10);
		var top = parseInt(event.clientY - event.offsetY, 10);
		var target = {
			type: type,
			left: left,
			top: top
		}

		//call dropNode function
		var targetNode = kgraph.drawSingleNode(target);

		//add connection between these two nodes
		var sourceNode = kgraph.mcurrentSelectedDomElement.node.sdata;
		var sline = {
			sourceId: "ACT" + sourceNode.id,
			targetId: "ACT" + targetNode.id,
			anchors: [[1, 0.5, 0, 0, 5, 0], [0, 0.5, 0, 0, 0, 0]],
			description: ''
		};
		var line = new kgraph.Line(sline);

		line.render();
	}

	kmain.removeCanvasElement = function () {
		if (kgraph.mcurrentSelectedDomElement.type === kgraph.Config.ELEMENT_TYPE_NODE) {
			$.msgBox({
				title: "Are You Sure",
				content: "确认要删除节点吗? 将会删除节点属性等数据!!!",
				type: "confirm",
				buttons: [{ value: "Yes" }, { value: "Cancel" }],
				success: function (result) {
					if (result === "Yes") {
						//jsPlumb.remove(kgraph.mcurrentSelectedDomElement.element);
						jsptoolkit.remove(kgraph.mcurrentSelectedDomElement.element);

						//remove node or releated lines from collection
						onNodeRemoved(kgraph.mcurrentSelectedDomElement.node);
						kgraph.mcurrentSelectedDomElement = null;
						return;
					}
				}
			});
		} else if (kgraph.mcurrentSelectedDomElement.type === kgraph.Config.ELEMENT_TYPE_CONNECTION) {
			$.msgBox({
				title: "Are You Sure",
				content: "确认要删除连线吗? 将会删除连线上的条件等数据!!!",
				type: "confirm",
				buttons: [{ value: "Yes" }, { value: "Cancel" }],
				success: function (result) {
					if (result === "Yes") {
						jsPlumb.detach(kgraph.mcurrentSelectedDomElement.connection);

						//remove line from collection
						onLineRemoved(kgraph.mcurrentSelectedDomElement.line);
						kgraph.mcurrentSelectedDomElement = null;
						return;
					}
				}
			});
		}
	}

	//remove node event
	var onNodeRemoved = function (node) {
		var id = node.id();
		var removingIndexOfSLines = [], removingIndexOfLines = [];
		var lines = kmain.mgraphView.lines;
		var slines = kmain.mgraphView.processData.slines;

		//first step, remove transtion of this node
		//remove line view from lines of graph canvas
		for (var i = 0; i < lines.length; i++) {
			if (id === lines[i].from()
                || id === lines[i].to()) {
				removingIndexOfLines.push(i);
			}
		}

		for (var i = removingIndexOfLines.length - 1; i >= 0; i--) {
			lines.splice(removingIndexOfLines[i], 1);
		}

		//remove line data source from transition collection
		for (var i = 0; i < slines.length; i++) {
			if (id === slines[i].from
                || id === slines[i].to) {
				removingIndexOfSLines.push(i);
			}
		}

		for (var i = removingIndexOfSLines.length - 1; i >= 0; i--) {
			slines.splice(removingIndexOfSLines[i], 1);
		}

		//remove node from the nodeview collection
		var nodes = kmain.mgraphView.nodes;
		for (var i = 0; i < nodes.length; i++) {
			if (id === nodes[i].id()) {
				nodes.splice(i, 1);
				break;
			}
		}

		//remove the node from activity collection
		var snodes = kmain.mgraphView.processData.snodes;
		for (var i = 0; i < snodes.length; i++) {
			if (id === snodes[i].id) {
				snodes.splice(i, 1);
				break;
			}
		}
	}

	var onLineRemoved = function (line) {
		var sline = null;
		var id = line.id();
		var lines = kmain.mgraphView.lines;
		var slines = kmain.mgraphView.processData.slines;

		//remove line data view from collection
		for (var i = 0; i < lines.length; i++) {
			if (id === lines[i].id()) {
				lines.splice(i, 1);
				break;
			}
		}

		//remove line data source from transition collection
		for (var i = 0; i < slines.length; i++) {
			if (id === slines[i].id) {
				slines.splice(i, 1);
				break;
			}
		}
	}
	//#endregion

	//#region diamgram xml
	kmain.createProcess = function () {
		processlist.createProcess();
	}

	kmain.openProcess = function () {
		BootstrapDialog.show({
			title: '流程列表',
			message: $('<div></div>').load('process/list'),
		});
	}

	kmain.setSelectedProcessGUIDCurrent = function (processGUID) {
		if (kmain.mselectedProcessGUIDCurrent !== processGUID) {
			kmain.mselectedProcessGUIDPrevious = kmain.mselectedProcessGUIDCurrent;
			kmain.mselectedProcessGUIDCurrent = processGUID;
			kmain.misSelectedNew = true;
		} else {
			kmain.misSelectedNew = false;
		}
	}

	function renderProcess(e, data) {
		//set current selected processguid
		var processGUID = data.ProcessGUID;
		kmain.setSelectedProcessGUIDCurrent(processGUID);

		//retrieve process file data
		var processVersion = kmain.mselectedProcessVersion;
		var query = {
			processGUID: processGUID,
			processVersion: processVersion
		};

		if (kmain.misSelectedNew && processGUID) {
			processapi.queryProcessFile(query, function (result) {
				if (result.Status === 1) {
					jsptoolkit.clearCanvas();

					//inialize graph canvas
					var processFileEntity = result.Entity;
					kmain.mgraphView = kloader.initialize(processFileEntity);
					kmain.mcurrentPackageData = kmain.mgraphView.packageData;
					kmain.mcurrentXmlContent = processFileEntity.XmlContent;
				} else {
					$.msgBox({
						title: "Designer / Process",
						content: "流程定义记录读取失败！错误：" + result.Message,
						type: "error"
					});
				}
			});
		}
	}

	kmain.saveProcessFile = function () {
		if (kmain.mgraphView !== undefined) {
			var processGUID = kmain.mgraphView.processGUID;
			var packageData = kmain.mgraphView.packageData;     //include participants and process 
			var processFileEntity = kloader.serialize2Xml(processGUID, packageData);
			kmain.mcurrentXmlContent = processFileEntity.XmlContent;

			processapi.saveProcessFile(processFileEntity);
		} else {
			$.msgBox({
				title: "Designer / Index",
				content: "请确认图形是否处于编辑状态！",
				buttons: [{ value: "Ok" }],
			});
		}
	}

	kmain.setting = function () {
		BootstrapDialog.show({
			title: '系统参数设置',
			message: $("<div>你可以在这个页面设置一些参数。。。</div>"),
			buttons: [{
				label: '确定',
				cssClass: 'btn-primary',
				action: function (dialogItself) {
					dialogItself.close();
				}
			}]
		});
	}

	kmain.exportXML = function () {
		var div = $('<div></div>');
		var text = $('<textarea style="width:540px;min-height:280px;"/>').val(kmain.mcurrentXmlContent).appendTo(div);

		BootstrapDialog.show({
			title: 'XML文件内容',
			message: div,
			buttons: [{
				label: '关闭',
				cssClass: 'btn-primary',
				action: function (dialogItself) {
					dialogItself.close();
				}
			}]
		});
	}
	//#endregion

	//#region activity and transition property
	kmain.showActivityProperty = function () {
		if (kgraph.mcurrentSelectedDomElement.type === kgraph.Config.ELEMENT_TYPE_NODE) {
			var curSelectedNode = kgraph.mcurrentSelectedDomElement.node;
			if (curSelectedNode.sdata.type) {
				kmain.showPropertyDialogCase("activity", curSelectedNode);
			}
		} else if (kgraph.mcurrentSelectedDomElement.type === kgraph.Config.ELEMENT_TYPE_CONNECTION) {
			var curSelectedLine = kgraph.mcurrentSelectedDomElement.line;
			kmain.showPropertyDialogCase("transition", curSelectedLine);
		}
	}

	kmain.showPropertyDialogCase = function (elmType, node) {
		if (elmType === "activity") {
			//activity page
			if (node.type() === kgraph.Config.NODE_TYPE_TASK) {
				BootstrapDialog.show({
					title: '活动属性',
					message: $('<div></div>').load('activity/edit'),
					data: { "node": node }
				});
			} else if (node.type() === kgraph.Config.NODE_TYPE_GATEWAY) {
				BootstrapDialog.show({
					title: '网关决策属性',
					message: $('<div></div>').load('activity/gateway'),
					data: { "node": node }
				});
			} else if (node.type() === kgraph.Config.NODE_TYPE_SUBPROCESS) {
				BootstrapDialog.show({
					title: '子流程属性',
					message: $('<div></div>').load('activity/subprocess'),
					data: { "node": node }
				});
			} else if (node.type() === kgraph.Config.NODE_TYPE_START
				|| node.type() === kgraph.Config.NODE_TYPE_END){
				 ;
			}
			else {
				$.msgBox({
					title: "Designer / Property",
					content: "未知节点类型！" + node.type(),
					type: "alert"
				});
				return false;
			}
		} else {
			//transition page
			BootstrapDialog.show({
				title: '转移属性',
				message: $('<div></div>').load('transition/edit'),
				data: { "node": node }
			});
		}
		
	}
	//#endregion

	return kmain;
})()