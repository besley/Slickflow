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
    }

    kmain.showDiagramReadOnly = function () {
        //the process graph is readonly for business process veiwer,
        //when it needed from applicaiton page.
        kmain.mxGraphEditor = createEditor('scripts/mxGraph/src/editor/config/workfloweditor-readonly.xml');
    }

    function initializeGlobalVariables(){
        kmain.mxSelectedProcessEntity = null;
        kmain.mxSelectedDomElement = {};
        kmain.mxSelectedPackageData = null;
        kmain.mxSelectedParticipants = [];
        kmain.mxSelectedProcessStartType = 0;
        kmain.mxSelectedProcessStartExpression = '';
        kmain.mxSelectedProcessEndType = 0;
        kmain.mxSelectedProcessEndExpression = '';
    }

    //intialize workflow editor
    kmain.initializeMxGraphEditor = function () {
        //set mxGraph lang
        setMxGraphLang();

        var keditor = kmain.mxGraphEditor = createEditor('scripts/mxGraph/src/editor/config/workfloweditor.xml');
        var kgraph = kmain.mxGraphEditor.graph;

        keditor.addListener(mxEvent.SAVE, function(){
            kmain.saveProcessFile();
        });
        kgraph.addListener(mxEvent.CELLS_ADDED, function(cell){
            //window.console.log(cell.getValue());
            /*
            $.msgBox({
				title: "Designer / Index",
				content: "图形节点添加事件触发！",
				type: "info"
			});*/
            //mxUtils.error('节点添加事件！', 200, false);
        });
        kgraph.addMouseListener({
            mouseDown: function(sender, evt){
                //window.console.log('mouseDown');
            },
            mouseMove: function(sender, evt){
            },
            mouseUp: function(sender, evt){
            }
        });
        kgraph.connectionHandler.addListener(mxEvent.CONNECT, function (sender, evt) {
            //var edge = evt.getProperty('cell');
            //var style = kgraph.getCellStyle(edge); //style is in object form
            //var newStyle = kgraph.stylesheet.getCellStyle("edgeStyle=orthogonalEdgeStyle;html=1;rounded=1;jettySize=auto;orthogonalLoop=1;strokeColor=#2d8e3d;strokeWidth=2;", style); //Method will merge styles into a new style object.  We must translate to string from here 
            //var array = [];
            //for (var prop in newStyle)
            //    array.push(prop + "=" + newStyle[prop]);
            //edge.style = array.join(';');
        });
        
        keditor.createProperties = function (cell) {
            if (kmain.mxSelectedDomElement === undefined) {
                $.msgBox({
                    title: "Designer / Property",
                    content: kresource.getItem('activitypropertycreatemsg'),
                    type: "alert"
                });
                return;
            }

            var model = this.graph.getModel();
	        var snode = model.getValue(cell);

            if (mxUtils.isNode(snode)) {
                kmain.mxSelectedDomElement.Cell = cell;

                if (model.isVertex(cell)) {
                    if (snode.nodeName === "Activity"){
                        kmain.mxSelectedDomElement.ElementType = 'Activity';
                        var activity = kmain.mxSelectedDomElement.Element = convert2ActivityObject(cell);
                        showActivityPropertyDialog(activity);
                    } else if(snode.nodeName === "Swimlane"){
                        $.msgBox({
                            title: "Designer / Property",
                            content: kresource.getItem('swimlanepropertymsg'),
                            type: "info"
                        });
                    }
                } else if (model.isEdge(cell)){ 
                    if (snode.nodeName === "Transition") {
			            //transition page
                        kmain.mxSelectedDomElement.ElementType = 'Transition';
                        var transition = kmain.mxSelectedDomElement.Element = convert2TransitionObject(cell);
                        BootstrapDialog.show({
                            title: kresource.getItem('transitionproperty'),
				            message: $('<div></div>').load('transition/edit'),
                            data: { "node": transition },
                            draggable: true
			            });
                    }
                } else {
                    $.msgBox({
                        title: "Designer / Property",
                        content: kresource.getItem('activitypropertyeditwarnmsg'),
                        type: "alert"
                    });
                }
            } else {
                $.msgBox({
                    title: "Designer / Property",
                    content: kresource.getItem('activitypropertyeditwarnmsg'),
                    type: "alert"
                });
            }
        }

        keditor.createEvents = function (cell) {
            if (kmain.mxSelectedDomElement === undefined) {
                $.msgBox({
                    title: "Designer / Property",
                    content: kresource.getItem('processselectedwarnmsg'),
                    type: "alert"
                });
                return;
            }

            var model = this.graph.getModel();
            var snode = model.getValue(cell);

            if (mxUtils.isNode(snode)) {
                kmain.mxSelectedDomElement.Cell = cell;

                if (model.isVertex(cell)) {
                    if (snode.nodeName === "Activity") {
                        kmain.mxSelectedDomElement.ElementType = 'Activity';
                        var activity = kmain.mxSelectedDomElement.Element = convert2ActivityObject(cell);
                        showActivityEventDialog(activity);
                    } else if (snode.nodeName === "Swimlane") {
                        $.msgBox({
                            title: "Designer / Property",
                            content: kresource.getItem('swimlanepropertymsg'),
                            type: "info"
                        });
                    }
                } else if (model.isEdge(cell)) {
                    if (snode.nodeName === "Transition") {
                        //transition page
                        kmain.mxSelectedDomElement.ElementType = 'Transition';
                        var transition = kmain.mxSelectedDomElement.Element = convert2TransitionObject(cell);
                        BootstrapDialog.show({
                            title: kresource.getItem('transitionproperty'),
                            message: $('<div></div>').load('transition/edit'),
                            data: { "node": transition },
                            draggable: true
                        });
                    }
                } else {
                    $.msgBox({
                        title: "Designer / Property",
                        content: kresource.getItem('activitypropertyeditwarnmsg'),
                        type: "alert"
                    });
                }
            } else {
                $.msgBox({
                    title: "Designer / Property",
                    content: kresource.getItem('activitypropertyeditwarnmsg'),
                    type: "alert"
                });
            }
        }

        keditor.showAdvanced = function (cell) {
            window.console.log("show advanced...in kmain");
        }
    }

    function showActivityPropertyDialog(activity) {
        BootstrapDialog.show({
            title: kresource.getItem('activityproperty'),
            message: $('<div></div>').load('activity/index'),
            data: { "node": activity },
            draggable: true
        });
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

        //boundaries
        var boundariesElement = snode.getElementsByTagName("Boundaries")[0];
        if (boundariesElement) {
            var boundaries = [];
            Array.prototype.forEach.call(boundariesElement.getElementsByTagName("Boundary"), function (boundaryElement) {
                var boundary = mxfile.getBoundaryObject(boundaryElement);
                boundaries.push(boundary);
            });
            activity.boundaries = boundaries;
        }

        //sections
        var sectionsElement = snode.getElementsByTagName("Sections")[0];
        if (sectionsElement) {
            var sections = [];
            Array.prototype.forEach.call(sectionsElement.getElementsByTagName("Section"), function (sectionElement) {
                var section = mxfile.getSectionObject(sectionElement);
                sections.push(section);
            });
            activity.sections = sections;
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
        if (conditionElement) transition.condition = mxfile.getConditionObject(conditionElement);

        var groupBehavioursElement = sline.getElementsByTagName("GroupBehaviours")[0];
        if (groupBehavioursElement) transition.groupBehaviours = mxfile.getGroupBehavioursObject(groupBehavioursElement);

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
                    content: kresource.getItem('explorernotsupportmsg'),
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
                    document.title = title;
                };
                editor.addListener(mxEvent.OPEN, funct);
                editor.addListener(mxEvent.ROOT, funct);
                funct(editor);
                editor.setStatus('mxGraph ' + mxClient.VERSION);
            }
        }catch (e){
            $.msgBox({
                title: "Designer / Index",
                content: kresource.getItem('workfloweditorstartuperrormsg') + e.message,
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
        processlist.createProcess();
	}

	kmain.openProcess = function () {
        BootstrapDialog.show({
            title: kresource.getItem("processlist"),
            message: $('<div></div>').load('process/list'),
            draggable: true
		});
	}

    kmain.previewXml = function () {
        var xmlContent = kloader.serialize2Xml(kmain.mxSelectedProcessEntity,
            kmain.mxSelectedParticipants); 

        if ($.isEmptyObject(xmlContent) === false){
		    var div = $('<div></div>');
            var text = $('<textarea style="width:540px;min-height:280px;"/>').val(xmlContent).appendTo(div);

            BootstrapDialog.show({
                title: kresource.getItem('xmlcontent'),
			    message: div,
                buttons: [{
                    label: kresource.getItem('close'),
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
                content: kresource.getItem('xmlpreviewwarnmsg'),
				type: "warn"
			});
            return;
        }
	}

	kmain.importDiagram = function () {
        BootstrapDialog.show({
            title: kresource.getItem('importxml'),
            message: $('<div></div>').load('process/import'),
            draggable: true
		});
	}

    function afterProcessCreated(e, data){
        //intialize process variables
        initializeGlobalVariables();
        //get mxEditor graph xml
        var graphData = kloader.load(data.ProcessEntity);
        kmain.mxSelectedPackageData = graphData.package;
        kmain.mxSelectedParticipants = graphData.package.participants;
        kmain.mxSelectedProcessEntity = data.ProcessEntity;
    }

    kmain.saveProcessFile = function () {
        if (kmain.mxSelectedProcessEntity){
            var xmlContent = kloader.serialize2Xml(kmain.mxSelectedProcessEntity,
                kmain.mxSelectedParticipants);
            var entity = {
                "ProcessGUID": kmain.mxSelectedProcessEntity.ProcessGUID,
                "Version": kmain.mxSelectedProcessEntity.Version,
                "StartType": kmain.mxSelectedProcessStartType,
                "StartExpression": kmain.mxSelectedProcessStartExpression,
                "EndType": kmain.mxSelectedProcessEndType,
                "EndExpression": kmain.mxSelectedProcessEndExpression,
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
                    content: kresource.getItem('processopenerrormsg') + result.Message,
					type: "error"
				});
			}
		});
    }

    //xml of mxGraphEditor
    kmain.previewMxGraphXMLContent = function(){
        var model = kmain.mxGraphEditor.graph.getModel();
        var encoder = new mxCodec();
        var mxGraphModelData = encoder.encode(model);
        var xmlContent = mxUtils.getPrettyXml(mxGraphModelData);

        if ($.isEmptyObject(xmlContent) === false) {
            var div = $('<div></div>');
            var text = $('<textarea style="width:540px;min-height:280px;"/>').val(xmlContent).appendTo(div);

            BootstrapDialog.show({
                title: kresource.getItem('xmlcontent'),
                message: div,
                buttons: [{
                    label: kresource.getItem('close'),
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
                content: kresource.getItem('xmlpreviewwarnmsg'),
                type: "warn"
            });
            return;
        }
    }
    //#endregion

    //#region set vertex and edut value
    kmain.setVertexValue = function(activity){
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode =  model.getValue(kmain.mxSelectedDomElement.Cell);

        snode.setAttribute('label', activity.name);   
        snode.setAttribute('code', activity.code);
        snode.setAttribute('url', activity.url);
        
        var descriptionElement = snode.getElementsByTagName("Description")[0]; 
        if (!descriptionElement){
            descriptionElement = snode.appendChild(snode.ownerDocument.createElement('Description'));
        }
        descriptionElement.textContent = activity.description;

        //activity type
        var activityTypeElement = snode.getElementsByTagName("ActivityType")[0];
        activityTypeElement = mxfile.setActivityTypeElement(activityTypeElement, activity);

        //activity boundaries
        var boundariesElement = snode.getElementsByTagName("Boundaries")[0];
        if (!boundariesElement) {
            boundariesElement = snode.appendChild(snode.ownerDocument.createElement("Boundaries"));
        } else {
            removeChildren(boundariesElement);
        }

        if (activity.boundaries) {
            for (var i = 0; i < activity.boundaries.length; i++) {
                var boundary = activity.boundaries[i];
                var boundaryElement = snode.ownerDocument.createElement("Boundary");

                boundaryElement = mxfile.setBoundaryElement(boundaryElement, boundary);
                boundariesElement.appendChild(boundaryElement);
            }
        }

        //activity sections
        var sectionsElement = snode.getElementsByTagName("Sections")[0];
        if (!sectionsElement) {
            sectionsElement = snode.appendChild(snode.ownerDocument.createElement("Sections"));
        } else {
            removeChildren(sectionsElement);
        }

        if (activity.sections) {
            for (var i = 0; i < activity.sections.length; i++) {
                var section = activity.sections[i];
                var sectionElement = snode.ownerDocument.createElement("Section");
                sectionElement = mxfile.setSectionElement(sectionElement, section);
                sectionsElement.appendChild(sectionElement);
            }
        }

        //model update
        model.beginUpdate();
        try{
            model.setValue(kmain.mxSelectedDomElement.Cell, snode);
        }finally{
            model.endUpdate();
        }
    }

    //set vertext performers
    kmain.setVertexPerformers = function(performers){
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode =  model.getValue(kmain.mxSelectedDomElement.Cell);

        if (performers){
            var performersElement = snode.getElementsByTagName("Performers")[0];
            if(!performersElement){
                performersElement = snode.appendChild(snode.ownerDocument.createElement("Performers"));
            } else {
                removeChildren(performersElement);
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

    //set vertext actions
    kmain.setVertexActions = function (actions) {
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode = model.getValue(kmain.mxSelectedDomElement.Cell);

        if (actions) {
            var actionsElement = snode.getElementsByTagName("Actions")[0];
            if (!actionsElement) {
                actionsElement = snode.appendChild(snode.ownerDocument.createElement("Actions"));
            } else {
                removeChildren(actionsElement);
            }

            var action = null,
                actionElement = null;
            for (var i = 0; i < actions.length; i++) {
                action = actions[i];
                actionElement = mxfile.setActionElement(actionsElement.ownerDocument, action);
                actionsElement.appendChild(actionElement);
            }
        }

        model.beginUpdate();
        try {
            model.setValue(kmain.mxSelectedDomElement.Cell, snode);
        } finally {
            model.endUpdate();
        }
    }

    var removeChildren = function (node) {
        var last;
        while (last = node.lastChild) node.removeChild(last);
    };

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

        if (transition.groupBehaviours) {
            var groupBehavioursElement = sline.getElementsByTagName("GroupBehaviours")[0];
            if (!groupBehavioursElement) {
                groupBehavioursElement = sline.appendChild(sline.ownerDocument.createElement("GroupBehaviours"));
            }
            if (jshelper.isNumber(transition.groupBehaviours.priority) === true) {
                groupBehavioursElement.setAttribute('priority', transition.groupBehaviours.priority);
            }

            if (transition.groupBehaviours.forced) {
                groupBehavioursElement.setAttribute('forced', transition.groupBehaviours.forced);
            }
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

    //#region render process diagram for application project
    //render ready activity nodes in kgraph canvas
    kmain.renderReadyTasks = function (activityList) {
        var graph = kmain.mxGraphEditor.graph;
        var model = kmain.mxGraphEditor.graph.getModel();
        model.beginUpdate();
        try {
            $.each(activityList, function (idx, activity) {
                var cell = model.getCell(activity.ActivityGUID);
                cell.setStyle("strokeColor=green;fillColor=green;");    //change ready task color to green
                graph.refresh();
            })
        } finally {
            model.endUpdate();
        }
    }

    //render transition red color for completed transitons
    kmain.renderCompletedTransitions = function (transitionList) {
        var graph = kmain.mxGraphEditor.graph;
        var model = kmain.mxGraphEditor.graph.getModel();
        model.beginUpdate();
        try {
            $.each(transitionList, function (idx, transition) {
                var edge = model.getCell(transition.TransitionGUID);
                if (edge !== undefined) {
                    var style = graph.getCellStyle(edge); //style is in object form
                    var newStyle = graph.stylesheet.getCellStyle("html=1;rounded=1;jettySize=auto;orthogonalLoop=1;strokeColor=red;strokeWidth=2;", style); //Method will merge styles into a new style object.  We must translate to string from here 
                    var array = [];
                    for (var prop in newStyle)
                        array.push(prop + "=" + newStyle[prop]);
                    edge.style = array.join(';');

                    graph.refresh();
                }
            });
        } finally {
            model.endUpdate();
        }
    }
    //#endregion

    //#region step test
    kmain.simuTest = function () {
        var win = window.open("/sfw2c/", '_blank');
        win.focus();
    }
    //#endregion

    //#region resource 
    kmain.changeLang = function (lang) {
        kresource.setLang(lang);
        location.reload();
    }

    function setMxGraphLang(lang) {
        if (lang === undefined || lang === null) lang = kresource.getLang();

        mxLanguage = lang;
        mxClient.language = lang;
    }
    //#endregion
	return kmain;
})()