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
            processlist.diagramCreated.subscribe(diagramProcessCreated);
        }

        initializeGlobalVariables();
    }

    kmain.showDiagramReadOnly = function () {
        //the process graph is readonly for business process veiwer,
        //when it needed from applicaiton page.
        kmain.mxGraphEditor = createEditor('Scripts/mxGraph/src/editor/config/workfloweditor-readonly.xml');
    }

    kmain.showDiagramModeling = function () {
        kmain.mxGraphEditor = createEditor('Scripts/mxGraph/src/editor/config/workfloweditor-modeling.xml');
    }

    function initializeGlobalVariables(){
        kmain.mxSelectedProcessEntity = null;
        kmain.mxSelectedProcessEntityList = [];
        kmain.mxSelectedDomElement = {};
        kmain.mxSelectedParticipants = [];
    }

    //intialize workflow editor
    kmain.initializeMxGraphEditor = function () {
        //set mxGraph lang
        setMxGraphLang();

        var keditor = kmain.mxGraphEditor = createEditor('Scripts/mxGraph/src/editor/config/workfloweditor.xml');
        var kgraph = kmain.mxGraphEditor.graph;

        //binding mouse event
        //mxtoolkit.bindingMouseEvent(kgraph);

        keditor.addListener(mxEvent.SAVE, function(){
            kmain.saveProcessFile();
        });
        kgraph.addListener(mxEvent.CELLS_ADDED, function(cell){
            /*
            $.msgBox({
				title: kresource.getItem('info'),
				content: "cell has been added！",
				type: "info"
			});*/
            //mxUtils.error('cell added event！', 200, false);
        });
        kgraph.connectionHandler.addListener(mxEvent.CONNECT, function (sender, evt) {
            var kgraph = kmain.mxGraphEditor.graph;
            var model = kgraph.getModel();
            var edge = evt.getProperty('cell');
            var source = edge.source;
            var target = edge.target;

            if (edge.style === "message") {
                if (source.parent.id === target.parent.id) {
                    model.beginUpdate();
                    model.remove(edge);
                    model.endUpdate();
                    kmsgbox.warn(kresource.getItem('messageflowcrosswarn'));
                }
            } else {
                if (source.parent.id !== target.parent.id) {
                    model.beginUpdate();
                    model.remove(edge);
                    model.endUpdate();
                    kmsgbox.warn(kresource.getItem('normalflowcrosswarn'));
                }
            }
        });
        
        keditor.createProperties = function (cell) {
            if (kmain.mxSelectedDomElement === undefined) {
                kmsgbox.warn(kresource.getItem('activitypropertycreatemsg'));
                return;
            }

            var model = this.graph.getModel();
	        var snode = model.getValue(cell);

            if (mxUtils.isNode(snode)) {
                kmain.mxSelectedDomElement.Cell = cell;

                if (model.isVertex(cell)) {
                    if (snode.nodeName === "Activity"){
                        kmain.mxSelectedDomElement.ElementType = 'Activity';
                        var activity = kmain.mxSelectedDomElement.ElementObject = convert2ActivityObject(cell, model);
                        showActivityPropertyDialog(activity);
                    } else if (snode.nodeName === "Swimlane") {
                        kmain.mxSelectedDomElement.ElementType = 'Swimlane';
                        kmain.mxSelectedDomElement.ElementObject = convert2SwimlaneObject(cell, model);
                        showPoolPropertyDialog(cell, model);
                    }
                } else if (model.isEdge(cell)){ 
                    if (snode.nodeName === "Transition") {
                        //transition page
                        kmain.mxSelectedDomElement.ElementType = 'Transition';
                        var transition = kmain.mxSelectedDomElement.ElementObject = convert2TransitionObject(cell, model);
                        BootstrapDialog.show({
                            title: kresource.getItem('transitionproperty'),
                            message: $('<div></div>').load('transition/edit'),
                            data: { "node": transition },
                            draggable: true
                        });
                    }
                } else {
                    kmsgbox.warn(kresource.getItem('activitypropertyeditwarnmsg'));
                }
            } else {
                kmsgbox.warn(kresource.getItem('activitypropertyeditwarnmsg'));
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

    function showPoolPropertyDialog(cell, model) {
        var pool = model.getValue(cell);
        var isBindingProcess = verifyStartActivityExistInSwimlaneContainer(cell, model);

        //泳道属性窗口
        kmain.mpoolPropertyDialog = BootstrapDialog.show({
            title: kresource.getItem('poolproperty'),
            message: $('<div></div>').load('activity/pool'),
            data: {
                "node": pool,
                "bindingProcess": isBindingProcess
            },
            draggable: true
        });
    }

    function convert2ActivityObject(cell, model){
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

        //services
        var servicesElement = snode.getElementsByTagName("Services")[0];
        if (servicesElement) {
            var services = [];
            Array.prototype.forEach.call(servicesElement.getElementsByTagName("Service"), function (serviceElement) {
                var service = mxfile.getServiceObject(serviceElement);
                services.push(service);
            });
            activity.services = services;
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

    function convert2SwimlaneObject(cell, model) {
        var snode = model.getValue(cell);
        var swimlane = {};
        swimlane.title = snode.getAttribute("label");

        var processElement = snode.getElementsByTagName("Process")[0];
        if (processElement) {
            var process = {};
            process.package = processElement.getAttribute("package");
            process.id = processElement.getAttribute("id");
            process.name = processElement.getAttribute("name");
            process.code = processElement.getAttribute("code");
            process.description = processElement.getAttribute("description");
            swimlane.process = process;
        }
        return swimlane;
    }

    function convert2TransitionObject(cell, model){
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
            if (!mxClient.isBrowserSupported()) {
                kmsgbox.info(kresource.getItem('explorernotsupportmsg'));
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
        } catch (e) {
            kmsgbox.error(kresource.getItem('workfloweditorstartuperrormsg'), e.message);
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

    //xml of slickflow specifictaion
    kmain.previewXml = function () {
        BootstrapDialog.show({
            title: kresource.getItem('content'),
            message: $('<div></div>').load('process/xmlcontent'),
            draggable: true
        });
    }

	kmain.importDiagram = function () {
        BootstrapDialog.show({
            title: kresource.getItem('importxml'),
            message: $('<div></div>').load('process/import'),
            draggable: true
		});
    }

    kmain.validateProcess = function () {
        try {
            var prepareDetail = kvalidator.prepareValidateEntity();
            if (prepareDetail.type !== "OK") {
                kmsgbox.warn(kresource.getItem(prepareDetail.type));
                return false;
            }
        } catch (e) {
            kmsgbox.error(kresource.getItem('processvalidateexceptionmsg'), e.message,)
            return false;
        }

        var entity = prepareDetail.Entity;
        kvalidator.validateProcess(entity, function (result) {
            var validatedDetail = result;
            var list = validatedDetail.activityList;
            var message = '';

            if (validatedDetail.type === "OK") {
                kmsgbox.info(kresource.getItem('processvalidateokmsg'));
            } else if (validatedDetail.type === "EXCEPTION") {
                kmsgbox.error(kresource.getItem('processvalidateexceptionmsg'));
            } else {
                for (var i = 0; i < list.length; i++) {
                    if (message !== '') message = message + ',';
                    var activity = list[i];
                    message = message + activity.ActivityName;
                }
                var msgDetail = kresource.getItem('processvalidatewarningmsg')
                    + kresource.getItem(validatedDetail.type)
                    + message;
                kmsgbox.warn(msgDetail);
            }
        });
    }

    function afterProcessCreated(e, data){
        //intialize process variables
        initializeGlobalVariables();
        //get mxEditor graph xml
        var graphData = kloader.load(data.ProcessEntity);
        kmain.mxSelectedParticipants = graphData.package.participants;
        kmain.mxSelectedProcessEntity = data.ProcessEntity;
    }

    //先创建图形，然后点击保存，需要将流程记录和图形一起保存
    //点击保存按钮，出现流程属性编辑页面，回传流程基本属性
    //然后调用saveProcessFile用于保存流程记录
    function diagramProcessCreated(e, data) {
        initializeGlobalVariables();
        kmain.mxSelectedProcessEntity = data.ProcessEntity;
        var result = kloader.serialize2Xml(kmain.mxSelectedParticipants);
        if (result.status === 1) {
            var xmlContent = result.xmlContent;
            var entity = {
                "ProcessGUID": kmain.mxSelectedProcessEntity.ProcessGUID,
                "ProcessName": kmain.mxSelectedProcessEntity.ProcessName,
                "ProcessCode": kmain.mxSelectedProcessEntity.ProcessCode,
                "PackageType": kmain.mxSelectedProcessEntity.PackageType,
                "Version": kmain.mxSelectedProcessEntity.Version,
                "XmlContent": xmlContent
            };
            processapi.saveProcessFile(entity);
        } else {
            kmsgbox.error(kresource.getItem("processxmlsaveerrormsg"), result.message);
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
                kmain.mxSelectedParticipants = graphData.package.participants;
                kmain.mxSelectedProcessEntity = data.ProcessEntity;
            } else {
                kmsgbox.error(kresource.getItem('processopenerrormsg'), result.Message);
            }
        });
    }

    kmain.saveProcessFile = function () {
        //先判断Swimlane是否存在
        var isSingleProcess = true;
        var countSwimlane = getSwimlaneCountInDiagram();
        if (countSwimlane === 0
            || countSwimlane === 1) {
            isSingleProcess = true;
        } else {
            // 检查泳道流程信息
            var startActivityCountOfSwimlaneCountTotal = getStartActivityInSwimlaneCountTotal();
            if (startActivityCountOfSwimlaneCountTotal < 2) {
                isSingleProcess = true;
            } else {
                isSingleProcess = false;
            }
        }

        if (isSingleProcess === true) {
            //单一流程保存
            if (kmain.mxSelectedProcessEntity === null) {
                //空白流程，直接创建流程记录
                processlist.pselectedProcessEntity = null;
                BootstrapDialog.show({
                    title: kresource.getItem("processcreate"),
                    message: $('<div></div>').load('process/create'),
                    draggable: true
                });
            } else {
                if (kmain.mxSelectedProcessEntity.PackageType === 1) {
                    //由多泳道流程改变为单一流程
                    saveProcessFileBySwimlaneConfirm();
                } else {
                    //先打开流程记录后编辑模式
                    var result = kloader.serialize2Xml(kmain.mxSelectedParticipants);
                    if (result.status === 1) {
                        var xmlContent = result.xmlContent;
                        //保存单一流程
                        saveProcessFileBySingleProcess(kmain.mxSelectedProcessEntity, xmlContent);
                    } else {
                        kmsgbox.warn(kresource.getItem("processxmlsaveerrormsg"), result.message);
                    }
                }
            }
        } else {
            //多泳道流程保存
            var checkType = checkStartActivityExistInSwimlane();
            if (checkType === "SWIMLANE-ACTIVITY-START") {
                //泳道流程，需要先配置属性
                kmsgbox.warn(kresource.getItem("processxmlsavepoolwarnmsg"));
            } else if (checkType === "SWIMLANE-PROCESS-HAVE") {
                //保存多泳道流程
                saveProcessFileBySwimlaneConfirm();
            } else if (checkType === "") {
                kmsgbox.error(kresource.getItem("processxmlothersaveerrormsg"));
            }
        }
    }

    function saveProcessFileBySwimlaneConfirm() {
        kmsgbox.confirm(kresource.getItem('processpoolsaveconfirmmsg'), function (result) {
            if (result === "Yes") {
                //已经绑定流程的泳道流程
                var result = kloader.serialize2Xml(kmain.mxSelectedParticipants);
                if (result.status === 1) {
                    var xmlContent = result.xmlContent;
                    saveProcessFileBySwimlaneProcess(xmlContent);
                } else {
                    kmsgbox.error(kresource.getItem("processxmlpoolsaveerrormsg"), result.message);
                }
                return;
            }
        });
    }

    function getSwimlaneCountInDiagram() {
        var count = 0;
        var model = kmain.mxGraphEditor.graph.getModel();
        var childNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        //列表检查
        for (var i = 0; i < childNodeList.length; i++) {
            var childNode = childNodeList[i];
            var childNodeValue = model.getValue(childNode);
            if (childNodeValue.nodeName === "Swimlane") {
                count = count + 1;
            }
            else {
                ;
            }
        }
        return count;
    }

    function getStartActivityInSwimlaneCountTotal() {
        var count = 0;
        var model = kmain.mxGraphEditor.graph.getModel();
        var childNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        //列表检查
        for (var i = 0; i < childNodeList.length; i++) {
            var childNode = childNodeList[i];
            var childNodeValue = model.getValue(childNode);
            if (childNodeValue.nodeName === "Swimlane") {
                var hasStartNodeInSwimlane = verifyStartActivityExistInSwimlaneContainer(childNode, model);
                if (hasStartNodeInSwimlane === true) {
                    count = count + 1;
                }
            }
            else {
                ;
            }
        }
        return count;
    }

    function verifyStartActivityExistInSwimlaneContainer(parent, model) {
        var isStartNodeExistInSwimlane = false;
        var childNodeList = model.getChildVertices(parent);
        for (var i = 0; i < childNodeList.length; i++) {
            var childNode = childNodeList[i];
            var childNodeValue = model.getValue(childNode);
            if (childNodeValue.nodeName === "Activity") {
                if (mxtoolkit.getActivityType(childNodeValue) === "StartNode") {
                    isStartNodeExistInSwimlane = true;
                    break;
                }
            }
        }
        return isStartNodeExistInSwimlane;
    }

    //保存单一流程
    function saveProcessFileBySingleProcess(entity, xmlContent) {
        var fileEntity = {
            "ProcessGUID": entity.ProcessGUID,
            "Version": entity.Version,
            "XmlContent": xmlContent
        };
        processapi.saveProcessFile(fileEntity);
    }

    //保存多泳道流程
    function saveProcessFileBySwimlaneProcess(xmlContent) {
        var entity = {
            "ProcessEntityList": kmain.mxSelectedProcessEntityList,
            "XmlContent": xmlContent
        };
        processapi.saveProcessFilePool(entity);
    }

    function checkStartActivityExistInSwimlane() {
        kmain.mcheckSwimlaneBindingProcessResultType = "";

        var model = kmain.mxGraphEditor.graph.getModel();
        var childNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        //从泳道检查
        for (var i = 0; i < childNodeList.length; i++) {
            var childNode = childNodeList[i];
            var childNodeValue = model.getValue(childNode);
            if (childNodeValue.nodeName === "Swimlane") {
                var processElement = childNodeValue.getElementsByTagName("Process")[0];
                if (processElement === undefined) {
                    if (verifyStartActivityExistInSwimlaneContainer(childNode, model) === true) {
                        kmain.mcheckSwimlaneBindingProcessResultType = 'SWIMLANE-ACTIVITY-START';
                        break;
                    }
                } else {
                    //如果已经定义ProcessElement，则不用检查
                    kmain.mcheckSwimlaneBindingProcessResultType = 'SWIMLANE-PROCESS-HAVE';
                }
            }
            else {
                //不是Swimlane则不用检查
                ;
            }
        }
        return kmain.mcheckSwimlaneBindingProcessResultType;
    }


    //xml of mxGraphEditor
    //only used to test
    kmain.previewXmlMxGraph = function(){
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
            kmsgbox.warn(kresource.getItem('xmlpreviewwarnmsg'));
            return;
        }
    }
    //#endregion

    //#region set vertex and edit value
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

    //set vertex actions
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

    function removeChildren(node) {
        var last;
        while (last = node.lastChild) node.removeChild(last);
    };

    //set vertex services
    kmain.setVertexServices = function (services) {
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode = model.getValue(kmain.mxSelectedDomElement.Cell);

        if (services) {
            var servicesElement = snode.getElementsByTagName("Services")[0];
            if (!servicesElement) {
                servicesElement = snode.appendChild(snode.ownerDocument.createElement("Services"));
            } else {
                removeChildren(servicesElement);
            }

            var service = null,
                serviceElement = null;
            for (var i = 0; i < services.length; i++) {
                service = services[i];
                serviceElement = mxfile.setServiceElement(servicesElement.ownerDocument, service);
                servicesElement.appendChild(serviceElement);
            }
        }

        model.beginUpdate();
        try {
            model.setValue(kmain.mxSelectedDomElement.Cell, snode);
        } finally {
            model.endUpdate();
        }
    }

    //set edge
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

            if (transition.groupBehaviours.defaultBranch) {
                groupBehavioursElement.setAttribute("defaultBranch", transition.groupBehaviours.defaultBranch);
            }

            if (transition.groupBehaviours.forced) {
                groupBehavioursElement.setAttribute('forced', transition.groupBehaviours.forced);
            }

            if (transition.groupBehaviours.approval) {
                groupBehavioursElement.setAttribute("approval", transition.groupBehaviours.approval);
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

    kmain.setPoolValue = function (pool) {
        var model = kmain.mxGraphEditor.graph.getModel();
        var snode = model.getValue(kmain.mxSelectedDomElement.Cell);
        snode.setAttribute('label', pool.title);
        snode.setAttribute('type', pool.type);

        var process = pool.process;
        if (process !== undefined) {
            var poolProcessElement = snode.getElementsByTagName("Process")[0];
            if (!poolProcessElement) {
                poolProcessElement = snode.appendChild(snode.ownerDocument.createElement("Process"));
            }
            poolProcessElement.setAttribute("package", process.package);
            poolProcessElement.setAttribute("id", process.id);
            poolProcessElement.setAttribute("name", process.name);
            poolProcessElement.setAttribute("code", process.code);
            poolProcessElement.setAttribute("description", process.description);
        }

        model.beginUpdate();
        try {
            model.setValue(kmain.mxSelectedDomElement.Cell, snode);
        } finally {
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

    //#region domain lang and step test
    kmain.codeStudio = function () {
        var win = window.open("model", '_blank');
        win.focus();
    }

    kmain.gotoTutorial = function () {
        processmodel.gotoTutorial();
    }

    kmain.simuTest = function () {
        var win = window.open("/sfw2/", '_blank');
        win.focus();
    }

    kmain.insight = function () {
        var win = window.open("/sfi2/", '_blank');
        win.focus();
    }

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