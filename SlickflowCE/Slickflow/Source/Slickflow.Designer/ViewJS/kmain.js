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
  
    kmain.init = function (mode) {
		//waiting...
		showProgressBar();

		$("#kgraphCanvas").on("click", function (e) {
			$(this).focus();
		})

		//fix two modal dialog in IE9 to IE11
		$.fn.modal.Constructor.prototype.enforceFocus = function () { };

		//register process opened event
        if ("undefined" !== typeof processlist) {
            processlist.afterCreated.subscribe(afterProcessCreated);
            processlist.afterOpened.subscribe(afterProcessOpened);
        }

        //initilize a workflow editor
        initializeMxGraphEditor(mode);
    }

    function initializeGlobalVariables(){
        kmain.mxSelectedProcessEntity = null;
        kmain.mxSelectedDomElement = {};
        kmain.mxSelectedPackageData = null;
        kmain.mxSelectedParticipants = [];
    }

    //intialize workflow editor
    function initializeMxGraphEditor(mode) {
        if (mode === 'READONLY') {
            //the process graph is readonly for business process veiwer,
            //when it needed from applicaiton page.
            $("#barTopMenu").hide();
            kmain.mxGraphEditor = createEditor('scripts/mxGraph/src/editor/config/workfloweditor-readonly.xml');
        } else {
            $("#barTopMenu").show();
            kmain.mxGraphEditor = createEditor('scripts/mxGraph/src/editor/config/workfloweditor.xml');
        }
        
        kmain.mxGraphEditor.addListener(mxEvent.SAVE, function(){
            kmain.saveProcessFile();
        });
        kmain.mxGraphEditor.graph.addListener(mxEvent.CELLS_ADDED, function(cell){
            //window.console.log(cell.getValue());
            /*
            $.msgBox({
				title: "Designer / Index",
				content: "图形节点添加事件触发！",
				type: "info"
			});*/
            //mxUtils.error('节点添加事件！', 200, false);
        });
        kmain.mxGraphEditor.graph.addMouseListener({
            mouseDown: function(sender, evt){
                //window.console.log('mouseDown');
            },
            mouseMove: function(sender, evt){
            },
            mouseUp: function(sender, evt){
            }
        });

        kmain.mxGraphEditor.createProperties = function(cell){
            var model = this.graph.getModel();
	        var snode = model.getValue(cell);

            if (mxUtils.isNode(snode)){
                kmain.mxSelectedDomElement.Cell = cell;

                if (model.isVertex(cell)) {
                    if (snode.nodeName === "Activity"){
                        kmain.mxSelectedDomElement.ElementType = 'Activity';
                        var activity = kmain.mxSelectedDomElement.Element = convert2ActivityObject(cell);
                        showActivityPropertyDialog(activity);
                    } else if(snode.nodeName === "Swimlane"){
                        ;
                    }
                } else if (model.isEdge(cell)){ 
                    if (snode.nodeName === "Transition") {
			            //transition page
                        kmain.mxSelectedDomElement.ElementType = 'Transition';
                        var transition = kmain.mxSelectedDomElement.Element = convert2TransitionObject(cell);
			            BootstrapDialog.show({
				            title: '转移属性',
				            message: $('<div></div>').load('transition/edit'),
                            data: { "node": transition },
                            draggable: true
			            });
                    }
                }
            }
        }

        kmain.mxGraphEditor.showAdvanced = function (cell) {
            window.console.log("show advanced...in kmain");
        }
    }

    function showActivityPropertyDialog(activity){
        if (activity.type === kmodel.Config.NODE_TYPE_TASK
	        || activity.type === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
	        BootstrapDialog.show({
		        title: '活动属性',
		        message: $('<div></div>').load('activity/edit'),
                data: { "node": activity },
                draggable: true
	        });
        } else if (activity.type === kmodel.Config.NODE_TYPE_GATEWAY) {
	        BootstrapDialog.show({
		        title: '网关决策属性',
		        message: $('<div></div>').load('activity/gateway'),
                data: { "node": activity },
                draggable: true
	        });
        } else if (activity.type === kmodel.Config.NODE_TYPE_SUBPROCESS) {
	        BootstrapDialog.show({
		        title: '子流程属性',
		        message: $('<div></div>').load('activity/subprocess'),
                data: { "node": activity },
                draggable: true
	        });
        } else if (activity.type === kmodel.Config.NODE_TYPE_START
	        || activity.type === kmodel.Config.NODE_TYPE_END){
		        ;
        }
        else {
	        $.msgBox({
		        title: "Designer / Property",
		        content: "未知节点类型！" + activity.type,
		        type: "alert"
	        });
	        return false;
        }
    }

    function convert2ActivityObject(cell){
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode = model.getValue(cell);

        //activity
        var activity = mxfile.getActivityObject(snode, "label");

        //description
        var descriptionElement = snode.getElementsByTagName("Description")[0];
        if (descriptionElement) activity.description = descriptionElement.textContent;

        //activity type
        var activityTypeElement = snode.getElementsByTagName("ActivityType")[0];
        activity = mxfile.getActivityTypeObject(activity, activityTypeElement);

        //performers
        var performersElement = snode.getElementsByTagName("Performers")[0];
        if (performersElement){
            var performers = [];
            Array.prototype.forEach.call(performersElement.getElementsByTagName("Performer"), function (performerElement) {
                var performer = mxfile.getPerformerObject(performerElement);
                performers.push(performer);
            });
            activity.performers = performers;
        }

        //actions
        var actionsElement = snode.getElementsByTagName("Actions")[0];
        if (actionsElement) {
            var actions = [];
            Array.prototype.forEach.call(actionsElement.getElementsByTagName("Action"), function (actionElement) {
                var action = mxfile.getActionObject(actionElement);
                actions.push(action);
            });
            activity.actions = actions;
        }
        return activity;
    }

    function convert2TransitionObject(cell){
        var model = kmain.mxGraphEditor.graph.getModel();
        var sline = model.getValue(cell);

        var transition = mxfile.getTransitionObject(sline);

        var descriptionElement = sline.getElementsByTagName("Description")[0];
        if (descriptionElement) transition.description = descriptionElement.textContent;

        var conditionElement = sline.getElementsByTagName("Condition")[0];
        transition.condition = mxfile.getConditionObject(conditionElement);
        
        var receiverElement = sline.getElementsByTagName("Receiver")[0];
        transition.receiver = mxfile.getReceiverObject(receiverElement);

        return transition;
    }

    function createEditor(config){
        var editor = null;
        try{
            if (!mxClient.isBrowserSupported()){
                $.msgBox({
				    title: "Designer / Index",
				    content: "不支持当前版本的浏览器，请使用更新版本的浏览器！",
				    type: "info"
			    });
            } else {
                mxObjectCodec.allowEval = true;
                var node = mxUtils.load(config).getDocumentElement();

                editor = new mxEditor(node);
                mxObjectCodec.allowEval = false;
                editor.graph.createPanningManager = function(){
                    var pm = new mxPanningManager(this);
                    pm.border = 30;
                    return pm;
                };
                editor.graph.allowAutoPanning = true;
                editor.graph.timerAutoScroll = true;

                var title = document.title;
                var funct = function(sender){
                    document.title = title + ' - ' + sender.getTitle();
                };
                editor.addListener(mxEvent.OPEN, funct);
                editor.addListener(mxEvent.ROOT, funct);
                funct(editor);
                editor.setStatus('mxGraph ' + mxClient.VERSION);
            }
        }catch (e){
            $.msgBox({
				title: "Designer / Index",
				content: "图形设计器启动异常：" + e.message,
				type: "info"
			});
            throw e;
        }
        return editor;
    }
	//#endregion

	//#region toolbar action in home page
	kmain.createProcess = function () {
        //clear canvas
        kmain.mxGraphEditor.graph.getModel().clear();
        //create process
		processlist.createProcess();
	}

	kmain.openProcess = function () {
		BootstrapDialog.show({
			title: '流程列表',
            message: $('<div></div>').load('process/list'),
            draggable: true
		});
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

	kmain.previewXml = function () {
        var xmlContent = kloader.serialize2Xml(kmain.mxSelectedProcessEntity, 
            kmain.mxSelectedPackageData); 

        if ($.isEmptyObject(xmlContent) === false){
		    var div = $('<div></div>');
            var text = $('<textarea style="width:540px;min-height:280px;"/>').val(xmlContent).appendTo(div);

		    BootstrapDialog.show({
			    title: 'XML文件内容',
			    message: div,
			    buttons: [{
				    label: '关闭',
				    cssClass: 'btn-primary',
				    action: function (dialogItself) {
					    dialogItself.close();
				    }
                }],
                draggable: true
		    });
        } else {
			$.msgBox({
				title: "Designer / Index",
				content: "图形内容为空，请确认是否打开流程记录！",
				type: "error"
			});
            return;
        }
	}

    function afterProcessCreated(e, data){
        //intialize process variables
        initializeGlobalVariables();
        //get mxEditor graph xml
        kmain.mxSelectedProcessEntity = data.ProcessEntity;
        //save process file
        kmain.saveProcessFile();
    }

    kmain.saveProcessFile = function(){
        if (kmain.mxSelectedProcessEntity){
            var xmlContent = kloader.serialize2Xml(kmain.mxSelectedProcessEntity, 
                kmain.mxSelectedPackageData);
            var entity = {
                "ProcessGUID": kmain.mxSelectedProcessEntity.ProcessGUID,
                "Version": kmain.mxSelectedProcessEntity.Version,
                "XmlContent": xmlContent
            };
            processapi.saveProcessFile(entity);
        } else {
            processlist.createProcess();
        }
    }

    function afterProcessOpened(e, data) {
        //intialize process variables
        initializeGlobalVariables();

        var query = {
            "ProcessGUID": data.ProcessEntity.ProcessGUID,
            "Version": data.ProcessEntity.Version
        };

        processapi.queryProcessFile(query, function (result) {
			if (result.Status === 1) {
                //clear graph canvas
                kmain.mxGraphEditor.graph.getModel().clear();
				//inialize graph canvas
                var graphData = kloader.load(result.Entity);
                kmain.mxSelectedPackageData = graphData.package;
                kmain.mxSelectedParticipants = graphData.package.participants;
                kmain.mxSelectedProcessEntity = data.ProcessEntity;
			} else {
				$.msgBox({
					title: "Designer / Process",
					content: "流程定义记录读取失败！错误：" + result.Message,
					type: "error"
				});
			}
		});
	}

    //xml of mxGraphEditor
    function previewMxGraphXMLContent(){
        var model = kmain.mxGraphEditor.graph.getModel();
        var encoder = new mxCodec();
        var mxGraphModelData = encoder.encode(model);
        var graphXmlContent = mxUtils.getPrettyXml(mxGraphModelData);

        return graphXmlContent;
    }
    //#endregion

    //#region set vertex and edut value
    kmain.setVertexValue = function(activity){
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode =  model.getValue(kmain.mxSelectedDomElement.Cell);

        snode.setAttribute('label', activity.name);   
        snode.setAttribute('code', activity.code);

        var descriptionElement = snode.getElementsByTagName("Description")[0]; 
        if (!descriptionElement){
            descriptionElement = snode.appendChild(snode.ownerDocument.createElement('Description'));
        }
        descriptionElement.textContent = activity.description;

        //activity type
        var activityTypeElement = snode.getElementsByTagName("ActivityType")[0];
        activityTypeElement = mxfile.setActivityTypeElement(activityTypeElement, activity);

        //activity action
        var actionsElement = snode.getElementsByTagName("Actions")[0];
        if (!actionsElement) {
            actionsElement = snode.appendChild(snode.ownerDocument.createElement("Actions"));
        }

        var actionElement = actionsElement.getElementsByTagName("Action")[0];
        if (!actionElement) {
            actionElement = actionsElement.appendChild(actionsElement.ownerDocument.createElement("Action"));
        } 

        if (activity.actions) {
            var action = activity.actions[0];
            actionElement = mxfile.setActionElement(actionElement, action);
        }

        model.beginUpdate();
        try{
            model.setValue(kmain.mxSelectedDomElement.Cell, snode);
        }finally{
            model.endUpdate();
        }
    }

    kmain.setVertexPerformers = function(performers){
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode =  model.getValue(kmain.mxSelectedDomElement.Cell);

        if (performers){
            var performersElement = snode.getElementsByTagName("Performers")[0];
            if(!performersElement){
                performersElement = snode.appendChild(snode.ownerDocument.createElement("Performers"));
            } else {
                var performerList = performersElement.getElementsByTagName("Performer");
                for (var i = 0; i < performerList.length; i++){
                    performersElement.removeChild(performerList[i]);
                }
            }

            var performer = null, 
                performerElement = null;
            for (var i = 0; i < performers.length; i++){
                performer = performers[i];
                performerElement = mxfile.setPerformerElement(performersElement.ownerDocument, performer);
                performersElement.appendChild(performerElement);
            }
        }

        model.beginUpdate();
        try{
            model.setValue(kmain.mxSelectedDomElement.Cell, snode);
        }finally{
            model.endUpdate();
        }
    }

    kmain.setEdgeValue = function(transition){
        var model = kmain.mxGraphEditor.graph.getModel();
        var sline = model.getValue(kmain.mxSelectedDomElement.Cell);

        var descriptionElement = sline.getElementsByTagName("Description")[0];
        if (!descriptionElement) {
            descriptionElement = sline.appendChild(sline.ownerDocument.createElement('Description'));
        }
        descriptionElement.textContent = transition.description;
        
        sline.setAttribute('label', transition.description);
        sline.setAttribute('from', transition.from);
        sline.setAttribute('to', transition.to);

        if (transition.condition){
            var conditionElement = sline.getElementsByTagName("Condition")[0];
            if (!conditionElement) {
                conditionElement = sline.appendChild(sline.ownerDocument.createElement('Condition'));
            }
            conditionElement.setAttribute('type', transition.condition.type);
            var conditionTextElement = conditionElement.getElementsByTagName("ConditionText")[0];
            if (!conditionTextElement){
                conditionTextElement = conditionElement.appendChild(sline.ownerDocument.createElement('ConditionText'));
            }
            conditionTextElement.textContent = transition.condition.text;
        }

        var receiverElement = sline.getElementsByTagName("Receiver")[0];
        if (!receiverElement){
            receiverElement = sline.appendChild(sline.ownerDocument.createElement('Receiver'));
        }
        receiverElement.setAttribute("type", transition.receiver.type);
        
        model.beginUpdate();
        try{
            model.setValue(kmain.mxSelectedDomElement.Cell, sline);
        }finally{
            model.endUpdate();
        }
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
	//#endregion

    //render ready activity nodes in kgraph canvas
    kmain.renderReadyTasks = function (activityList) {
        var graph = kmain.mxGraphEditor.graph;
        var model = kmain.mxGraphEditor.graph.getModel();
        model.beginUpdate();
        try {
            $.each(activityList, function (idx, activity) {
                window.console.log(activity.ActivityGUID);
                var cell = model.getCell(activity.ActivityGUID)
                window.console.log(cell);
                cell.setStyle("strokeColor=green;fillColor=green;");    //change ready task color to green
                graph.refresh();
            })
        } finally {
            model.endUpdate();
        }
    }
	return kmain;
})()