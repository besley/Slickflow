/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。

The Slickflow Designer project.
Copyright (C) 2016  .NET Workflow Engine Library

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

//#region activity property
var activityproperty = (function () {
	function activityproperty() {
    }

    //tab configuration
    var myPageTabs = {};
    myPageTabs["task"] = ["tabRole", "tabPage", "tabMyProperties", "tabAction", "tabTaskDelay"];
    myPageTabs["mi"] = ["tabMI", "tabRole", "tabPage", "tabMyProperties", "tabAction"];
    myPageTabs["sub"] = ["tabSub", "tabRole", "tabMyProperties", "tabAction"];
    myPageTabs["service"] = ["tabService"];
    myPageTabs["gateway"] = ["tabGateway"];
    myPageTabs["start"] = ["tabAction"];
    myPageTabs["startCron"] = ["tabStartCron", "tabAction"];
    myPageTabs["end"] = ["tabAction"];
    myPageTabs["endDelay"] = ["tabEndDelay", "tabAction"];
    myPageTabs["intermediate"] = ["tabAction"];
    myPageTabs["interTimerDelay"] = ["tabTaskDelay", "tabAction"];
    myPageTabs["startMessage"] = ["tabMessage"];
    myPageTabs["interMessage"] = ["tabMessage"];
    myPageTabs["endMessage"] = ["tabMessage"];
    myPageTabs["conditional"] = ["tabCondition"];

    activityproperty.mselectedActivityPerformerGUID = "";
    //activityproperty.mjsoneditor = null;

    //action configuration
    var myActionPanel = {};
    myActionPanel["LocalService"] = ["argument", "expression"];
    myActionPanel["ProcessService"] = ["process"];
    myActionPanel["CSharpLibrary"] = ["argument", "assembly"];
    myActionPanel["WebApi"] = ["webapi", "argument", "expression", "submethod"];
    myActionPanel["SQL"] = ["argument", "codetext"];
    myActionPanel["StoreProcedure"] = ["argument", "expression"];
    myActionPanel["Python"] = ["argument", "codetext"];

    function initControls() {
        $("#ddlComplexType").prop("selectedIndex", -1);
        $("#ddlMergeType").prop("selectedIndex", -1);
        $("#ddlCompareType").prop("selectedIndex", -1);

        //action related control
        $("#ddlActionMethodTypeBefore").on('change', function (e) {
            showMethodPanel(this.value, 'Before');
        });

        $("#ddlActionMethodTypeAfter").on('change', function (e) {
            showMethodPanel(this.value, 'After');
        });

        //service related control
        $("#ddlServiceMethodType").on('change', function (e) {
            $(".methoditemhideme").hide();
            var fields = myActionPanel[this.value];

            if (fields !== undefined) {
                fields.forEach(function (item, i) {
                    $("." + item).show();
                })
            }
        });
    }

    function showMethodPanel(method, fire) {
        $(".methoditemhideme").hide();
        var fields = myActionPanel[method];
        if (fields !== undefined) {
            fields.forEach(function (item, i) {
                var field = item + "-" + fire;
                $("." + field).show();
            });
        }
    }

    activityproperty.loadActivity = function () {
        initControls();

        var activity = kmain.mxSelectedDomElement.ElementObject;
        showTabsByActivityType(activity);
        dispalyActivityProperty(activity);
    }

    function showTabsByActivityType(activity) {
        $(".hideme").hide();

        var tabs = null;
        var type = activity.type;
        if (type === kmodel.Config.NODE_TYPE_TASK) {
            tabs = myPageTabs["task"];
        } else if (type === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
            tabs = myPageTabs["mi"];
        } else if (type === kmodel.Config.NODE_TYPE_SERVICE) {
            tabs = myPageTabs["service"];
        } else if (type === kmodel.Config.NODE_TYPE_GATEWAY) {
            tabs = myPageTabs["gateway"];
        } else if (type === kmodel.Config.NODE_TYPE_SUBPROCESS) {
            tabs = myPageTabs["sub"];
        } else if (type === kmodel.Config.NODE_TYPE_START) {
            if (activity.trigger === "Timer") {
                tabs = myPageTabs["startCron"];
            } else if (activity.trigger === "Message") {
                tabs = myPageTabs["startMessage"];
            } else if (activity.trigger === "Conditional") {
                tabs = myPageTabs["conditional"];
            } else {
                tabs = myPageTabs["start"];
            }             
        } else if (type === kmodel.Config.NODE_TYPE_INTERMEDIATE) {
            if (activity.trigger === "Timer") {
                tabs = myPageTabs["interTimerDelay"];
            } else if (activity.trigger === "Message") {
                tabs = myPageTabs["interMessage"];
            } else if (activity.trigger === "Conditional") {
                tabs = myPageTabs["conditional"];
            } else {
                tabs = myPageTabs["intermediate"];
            }
        } else if (type === kmodel.Config.NODE_TYPE_END) {
            if (activity.trigger === "Timer") {
                tabs = myPageTabs["endDelay"];
            } else if (activity.trigger === "Message") {
                tabs = myPageTabs["endMessage"];
            } else {
                tabs = myPageTabs["end"];
            }            
        } else {
	        return false;
        }

        if (tabs) {
            for (var i = tabs.length; i > 0; i--) {
                var tab = tabs[i - 1];
                $("#" + tab).parent().show();
                $("#" + tab).show();
                $("#" + tab).tab('show');
            }
        }
    }

    function dispalyActivityProperty(activity) {
        displayInfoCommon(activity);

        var type = activity.type;
        if (type === kmodel.Config.NODE_TYPE_TASK) {
            displayInfoTask(activity);
        } else if (type === kmodel.Config.NODE_TYPE_SERVICE) {
            activityproperty.loadServices(activity);
        } else if (type === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
            displayInfoMI(activity);
        } else if (type === kmodel.Config.NODE_TYPE_GATEWAY) {
            gatewayproperty.load(activity);
        } else if (type === kmodel.Config.NODE_TYPE_SUBPROCESS) {
            displayInfoSub(activity);
        } else if (type === kmodel.Config.NODE_TYPE_START
            || type === kmodel.Config.NODE_TYPE_INTERMEDIATE
            || type === kmodel.Config.NODE_TYPE_END) {
            eventproperty.load(activity);
        } else {
            return false;
        }
    }

    function displayInfoCommon(activity) {
        if (activity) {
            //fill activity basic information
            $("#txtActivityName").val(activity.name);
            $("#txtActivityCode").val(activity.code);
            $("#txtActivityUrl").val(activity.url);
            $("#txtDescription").val(activity.description);
        }
    }

    function displayInfoTask(activity) {
        //load performer list of current activity
        loadActivityPerformer(activity);

        //load boundary of current activity
        loadActivityBoundary(activity);

        //load sections of current activity
        loadActivitySections(activity);

        //load activityactions
        activityproperty.loadActions(activity);
    }

    function displayInfoMI(activity) {
		//render multiple instance node property
		$("#ddlComplexType").val(activity.complexType);
		$("#ddlMergeType").val(activity.mergeType);
		$("#ddlCompareType").val(activity.compareType);
        $("#txtCompleteOrder").val(activity.completeOrder);

        displayInfoTask(activity);
    }

    function displayInfoSub(activity) {
        displayInfoTask(activity);
        subprocessmanager.load(activity);
    }

	//load performer datagrid
	function loadActivityPerformer(activity) {
		var divPerformerGrid = document.querySelector('#myPerformerGrid');
		$(divPerformerGrid).empty();

		var gridOptions = {
			columnDefs: [
                { headerName: 'ID', field: 'id', width: 160 },			//performerguid
                { headerName: kresource.getItem('rolename'), field: 'name', width: 160 },
                { headerName: kresource.getItem('rolecode'), field: 'code', width: 160 }
			],
			rowSelection: 'single',
			onSelectionChanged: onSelectionChanged
		};

		new agGrid.Grid(divPerformerGrid, gridOptions);
		var dsActivityPerformer = loadActivityPerformersDataSource(activity);

		gridOptions.api.setRowData(dsActivityPerformer);

		function onSelectionChanged() {
			var selectedRows = gridOptions.api.getSelectedRows();
			selectedRows.forEach(function (selectedRow, index) {
				activityproperty.mselectedActivityPerformerGUID = selectedRow.id;
			});
		}
    }

	function loadActivityPerformersDataSource(activity) {
		var performer = null;
		var activityPerformers = [];

		if (activity.performers && activity.performers.length > 0) {
			for (var i = 0; i < activity.performers.length; i++) {
				performer = getPerformerByGUID(activity.performers[i].id);
                if (performer === null) {
                    performer = {
                        "id": activity.performers[i].id
                    };
                }
                activityPerformers.push(performer);
			}
		}
		return activityPerformers;
	}

    function getPerformerByGUID(id) {
		var performer = null;

        //it's better to get role information from database
		var participants = kmain.mxSelectedParticipants
		if (participants && participants.length > 0) {
			for (var i = 0; i < participants.length; i++) {
				var p = participants[i];
                if (id === p.id) {
                    performer = {
                        "id": p.id,
                        "name": p.name,
                        "code": p.code,
                        "outerId": p.outerId
                    };
                    break;
                }
			}
        }
		return performer;
    }

    //load activity boundary
    function loadActivityBoundary(activity) {
        var boundaries = activity.boundaries;

        if (boundaries && boundaries.length > 0) {
            var boundary = boundaries[0];

            //fill boundary info
            $("#txtOverdue").val(boundary.expression);
        }
    }

    //load activity sections
    function loadActivitySections(activity) {
        var sections = activity.sections;

        if (sections && sections.length > 0) {
            for (var i = 0; i < sections.length; i++) {
                var section = sections[i];
                if (section.name === "myProperties") {
                    //fill myProperties info
                    $("#txtMyProperties").val(section.text);
                    //activityproperty.mjsoneditor.set(section.text);
                }
            }
        }
    }

	
	//#region performers add / delete
	//add performer to activity performers list
	activityproperty.addPerformer = function () {
        BootstrapDialog.show({
            title: kresource.getItem('rolelist'),
            message: $('<div></div>').load('role/list'),
            draggable: true
		});
	}

	//sync activity perfromer datasource
	activityproperty.syncActivityPerformers = function (participantType, participantItem) {
		var activity =  kmain.mxSelectedDomElement.ElementObject;
		var performers = activity.performers;
		if (!performers) {
			performers = activity.performers = [];
		}

		//check participants exists the newly added role item
        var participantAdded = null;
        if (kmain.mxSelectedParticipants !== null) {
            var participants = kmain.mxSelectedParticipants;
            for (var i = 0; i < participants.length; i++) {
                if (participantType === "role"
                    && participants[i].type === "Role"
                    && participantItem.ID === participants[i].outerId) {
                    participantAdded = participants[i];
                    break;
                }
            }
        }

		if (participantAdded) {
			//check this participant wether exists in the activity performers gridview
			var isExisted = false;
			for (var i = 0; i < performers.length; i++) {
				if (participantAdded.id === performers[i].id) {             //guid
					isExisted = true;
					break;
				}
			}

            if (isExisted) {
                kmsgbox.info(kresource.getItem('syncactivityperformerswarnmsg'));
			} else {
				var performer = {
					"id": participantAdded.id,
					"name": participantAdded.name,
					"code": participantAdded.code,
					"outerId": participantAdded.outerId
				}
				activity.performers.push(performer);
				//refresh the activity performer gridview
				loadActivityPerformer(activity);
                //update vertex user object property
                kmain.setVertexPerformers(activity.performers);
			}
		} else {
			//added this new participant item to participants collection
			if (participantType === "role") {
				var participant = {
					"id": jshelper.getUUID(),
					"type": "Role",
					"name": participantItem.RoleName,
					"code": participantItem.RoleCode,
					"outerId": participantItem.ID
				};

				kmain.mxSelectedParticipants.push(participant);

				//add new performer to node activity performers gridview
				var performer = {
					"id": participant.id,
					"name": participant.name,
					"code": participant.code,
					"outerId": participant.outerId
				}
				activity.performers.push(performer);
				//refresh the activity performer gridview
				loadActivityPerformer(activity);
                //update vertex user object property
                kmain.setVertexPerformers(activity.performers);
			}
		}
	}

	//del performer from activity performers list
	activityproperty.delPerformer = function () {
		var performerGUID = activityproperty.mselectedActivityPerformerGUID;
        var activity =  kmain.mxSelectedDomElement.ElementObject
		var performers = activity.performers;

		//remove the selected performer record 
		activity.performers = jQuery.grep(performers, function (item) {
			return item.id !== performerGUID;
		});
		//refresh the gridview
		loadActivityPerformer(activity);
        //update vertex user object property
        kmain.setVertexPerformers(activity.performers);
	}
	//#endregion

    //#region Action操作
    //load actions
    activityproperty.loadActions = function(activity) {
        if (activity) {
            var actions = activity.actions;
            if (actions && actions.length > 0) {
                for (var i = 0; i < actions.length; i++) {
                    var action = actions[i];
                    //fill action info
                    if (action && action.type === "Event") {
                        loadActionDetail(action);
                    }
                }
            }
        }
    }

    function loadActionDetail(action) {
        var fire = action.fire;
        if (action.method !== "None") {
            $("#ddlActionMethodType" + fire).val(action.method).trigger('change');
        }
        
        //input fields
        $("#txtActionArguments" + fire).val(action.arguments);
        $("#txtActionExpression" + fire).val(action.expression);

        if (action.method === "WebApi") {
            $("#ddlWebApiMethod" + fire).val(action.subMethod);
        } else if (action.method === "SQL" || action.method === "Python") {
            var codeInfo = action.codeInfo;
            if (codeInfo !== undefined) $("#txtCodeText" + fire).val(codeInfo.codeText.trim());
        } else if (action.method === "CSharpLibrary") {
            var methodInfo = action.methodInfo;
            if (methodInfo !== undefined) {
                $("#txtAssemblyFullName" + fire).val(methodInfo.assemblyFullName);
                $("#txtTypeFullName" + fire).val(methodInfo.typeFullName);
                $("#txtMethodName" + fire).val(methodInfo.methodName);
            }
        }
    }

    //save actions
    activityproperty.saveActions = function(activity) {
        if (activity) {
            activity.actions = [];

            var methodBefore = $("#ddlActionMethodTypeBefore").val();
            if (methodBefore !== "None") {
                var actionBefore = setActionDetail("Before", methodBefore);
                activity.actions.push(actionBefore);
            }

            var methodAfter = $("#ddlActionMethodTypeAfter").val();
            if (methodAfter !== "None") {
                var actionAfter = setActionDetail("After", methodAfter);
                activity.actions.push(actionAfter);
            }
            //update node user object
            kmain.setVertexActions(activity.actions);
        }
    }

    function setActionDetail(fire, method) {
        var action = {};
        action.type = kmodel.Config.ACTION_TYPE_EVENT;             //"Event";
        action.fire = fire;                                       //"Before"/"After";
        action.arguments = $("#txtActionArguments" + fire).val();
        action.expression = $("#txtActionExpression" + fire).val();
        action.method = method;

        //sub method after
        if (method === kmodel.Config.ACTION_METHOD_TYPE_WEBAPI) {
            action.subMethod = $("#ddlWebApiMethod" + fire).val();
        } else if (method === kmodel.Config.ACTION_METHOD_TYPE_SQL
            || method === kmodel.Config.ACTION_METHOD_TYPE_PYTHON) {
            var codeInfo = {};
            codeInfo.codeText = $("#txtCodeText" + fire).val();
            action.codeInfo = codeInfo;
        } else if (method === kmodel.Config.ACTION_METHOD_TYPE_CSHARPLIBRARY) {
            var methodInfo = {};
            methodInfo.assemblyFullName = $("#txtAssemblyFullName" + fire).val();
            methodInfo.typeFullName = $("#txtTypeFullName" + fire).val();
            methodInfo.methodName = $("#txtMethodName" + fire).val();
            action.methodInfo = methodInfo;
        }
        return action;
    }
    //#endregion

    //#region services操作
    //load service
    activityproperty.loadServices = function (activity) {
        if (activity) {
            var services = activity.services;
            if (services && services.length > 0) {
                for (var i = 0; i < services.length; i++) {
                    var service = services[i];
                    //fill action info
                    loadServiceDetail(service);
                }
            }
        }
    }

    function loadServiceDetail(service) {
        if (service.method !== "None") {
            $("#ddlServiceMethodType").val(service.method).trigger('change');
        }

        //input fields
        $("#txtArgumentsService").val(service.arguments);
        $("#txtExpressionService").val(service.expression);

        if (service.method === "WebApi") {
            $("#ddlWebApiMethodService").val(service.subMethod);
        } else if (service.method === "SQL" || service.method === "Python") {
            var codeInfo = service.codeInfo;
            if (codeInfo !== undefined) $("#txtCodeTextService").val(codeInfo.codeText.trim());
        } else if (service.method === "CSharpLibrary") {
            var methodInfo = service.methodInfo;
            if (methodInfo !== undefined) {
                $("#txtAssemblyFullNameService").val(methodInfo.assemblyFullName);
                $("#txtTypeFullNameService").val(methodInfo.typeFullName);
                $("#txtMethodNameService").val(methodInfo.methodName);
            }
        }
    }

    //save services
    activityproperty.saveServices = function (activity) {
        if (activity) {
            activity.services = [];

            var methodService = $("#ddlServiceMethodType").val();
            if (methodService !== "None") {
                var service = setServiceDetail(methodService);
                activity.services.push(service);
            }
            //update node user object
            kmain.setVertexServices(activity.services);
        }
    }

    function setServiceDetail(method) {
        var service = {};
        service.arguments = $("#txtArgumentsService").val();
        service.expression = $("#txtExpressionService").val();
        service.method = method;

        //sub method after
        if (method === "WebApi") {
            service.subMethod = $("#ddlWebApiMethodService").val();
        } else if (method === "SQL"
            || method === "Python") {
            var codeInfo = {};
            codeInfo.codeText = $("#txtCodeTextService").val();
            service.codeInfo = codeInfo;
        } else if (method === "CSharpLibrary") {
            var methodInfo = {};
            methodInfo.assemblyFullName = $("#txtAssemblyFullNameService").val();
            methodInfo.typeFullName = $("#txtTypeFullNameService").val();
            methodInfo.methodName = $("#txtMethodNameService").val();
            service.methodInfo = methodInfo;
        }
        return service;
    }
    //#endregion

    //save activity basic information
    activityproperty.saveActivity = function () {
        var activity = kmain.mxSelectedDomElement.ElementObject;
        saveActivityCommon(activity);

        var type = activity.type;
        if (type === kmodel.Config.NODE_TYPE_TASK) {
            saveTask(activity);
        } else if (type === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
            saveMI(activity);
        } else if (type === kmodel.Config.NODE_TYPE_SERVICE) {
            saveService(activity);
        } else if (type === kmodel.Config.NODE_TYPE_GATEWAY) {
            gatewayproperty.save(activity);
        } else if (type === kmodel.Config.NODE_TYPE_SUBPROCESS) {
            subprocessmanager.save(activity);
        } else if (type === kmodel.Config.NODE_TYPE_START
            || type === kmodel.Config.NODE_TYPE_INTERMEDIATE
            || type === kmodel.Config.NODE_TYPE_END) {
            eventproperty.save(activity);
        } else {
            return false;
        }
    }

    function saveActivityCommon(activity) {
        var activityName = $("#txtActivityName").val();
        var activityCode = $("#txtActivityCode").val();
        var activityUrl = $("#txtActivityUrl").val();
        var description = $("#txtDescription").val();

        activity.name = activityName;
        activity.code = activityCode;
        activity.url = activityUrl;
        activity.description = description;
    }

    function saveTask(activity) {
        //boundaries
        activity.boundaries = [];
        var boundary = {};
        boundary.event = kmodel.Config.BOUNDARY_EVENT_TIMER;
        boundary.expression = $("#txtOverdue").val();
        activity.boundaries.push(boundary);

        //sections
        activity.sections = [];
        var section = {};
        section.name = "myProperties";           //customer use the name to store json data 2019/03/20 Besley
        section.text = $("#txtMyProperties").val();
        //section.text = activityproperty.mjsoneditor.get();
        activity.sections.push(section);

        //actions
        activityproperty.saveActions(activity);

        //update node user object
        kmain.setVertexValue(activity);
    }

    function saveMI(activity) {
        activity.complexType = $("#ddlComplexType").val();
        activity.mergeType = $("#ddlMergeType").val();
        activity.compareType = $("#ddlCompareType").val();
        activity.completeOrder = $("#txtCompleteOrder").val();

        saveTask(activity);
    }

    function saveService(activity) {
        //services
        activityproperty.saveServices(activity);


        kmain.setVertexValue(activity);
    }

	return activityproperty;
})()
//#endregion

//#region gateway property
var gatewayproperty = (function () {
    function gatewayproperty() {
    }

    gatewayproperty.Direction = {
        Split: "Split",
        Join: "Join",
        AndSplit: "AndSplit",
        AndJoin: "AndJoin",
        OrSplit: "OrSplit",
        OrJoin: "OrJoin",
        XOrSplit: "XOrSplit",
        ApprovalOrSplit: "ApprovalOrSplit",
        XOrJoin: "XOrJoin",
        EOrJoin: "EOrJoin",
        AndSplitMI: "AndSplitMI",
        AndJoinMI: "AndJoinMI",
    };

    gatewayproperty.JoinPass = {
        TokenCountPass: "TokenCountPass",
        ForcedBranchPass: "ForcedBranchPass",
    };

    gatewayproperty.load = function (activity) {
        if (activity) {
            //fill activity basic information
            if (activity.gatewaySplitJoinType) {
                var splitJoinType = activity.gatewaySplitJoinType;
                $("#ddlGatewayType").val(splitJoinType);
                $("#ddlGatewayType").attr("disabled", true);
                //fill direction type
                gatewayproperty.appendDirectionType(splitJoinType);
                if (activity.gatewayDirection) {
                    var direction = activity.gatewayDirection;
                    $("#ddlDirectionType").val(direction);

                    if (direction === gatewayproperty.Direction.EOrJoin) {
                        $("#divJoinPassType").show();
                        $("#ddlJoinPassType").val(activity.gatewayJoinPass);
                    } else {
                        $("#divJoinPassType").hide();
                    }
                }
            }
        }

        $("#ddlGatewayType").change(function () {
            var splitJoinType = $("#ddlGatewayType").val();
            gatewayproperty.appendDirectionType(splitJoinType);
        });

        $("#ddlDirectionType").change(function () {
            var direction = $("#ddlDirectionType").val();
            if (direction === gatewayproperty.Direction.EOrJoin) {
                $("#divJoinPassType").show();
            } else {
                $("#divJoinPassType").hide();
            }
        })
    }

    gatewayproperty.appendDirectionType = function (splitJoinType) {
        var splitOptions = [
            { "value": "AndSplit", "text": kresource.getItem('andsplit') },
            { "value": "OrSplit", "text": kresource.getItem('orsplit') },
            { "value": "XOrSplit", "text": kresource.getItem('xorsplit') },
            { "value": "ApprovalOrSplit", "text": kresource.getItem('approvalorsplit') },
            { "value": "AndSplitMI", "text": kresource.getItem('andsplitmi') },
        ];

        var joinOptions = [
            { "value": "AndJoin", "text": kresource.getItem("andjoin") },
            { "value": "OrJoin", "text": kresource.getItem("orjoin") },
            { "value": "XOrJoin", "text": kresource.getItem("xorjoin") },
            { "value": "EOrJoin", "text": kresource.getItem("eorjoin") },
            { "value": "AndJoinMI", "text": kresource.getItem("andjoinmi") }
        ];

        //initialize default options
        var optiondefault = kresource.getItem('optiondefault');
        $("#ddlDirectionType")
            .empty()
            .append('<option value="default" selected>' + optiondefault + '</option > ');

        //load details
        var item = null;
        if (splitJoinType === gatewayproperty.Direction.Split) {
            for (var i = 0; i < splitOptions.length; i++) {
                item = splitOptions[i];
                $('#ddlDirectionType')
                    .append($("<option></option>")
                        .attr("value", item.value)
                        //.text(item.text));
                        .text(kresource.getItem(item.value.toLowerCase())));
            }
        } else if (splitJoinType === gatewayproperty.Direction.Join) {
            for (var i = 0; i < joinOptions.length; i++) {
                item = joinOptions[i];
                $('#ddlDirectionType')
                    .append($("<option></option>")
                        .attr("value", item.value)
                        //.text(item.text));
                        .text(kresource.getItem(item.value.toLowerCase())));
            }
        }
    }

    gatewayproperty.save = function (activity) {
        var splitJoinType = $("#ddlGatewayType").val();
        var directionType = $("#ddlDirectionType").val();
        var joinPasssType = $("#ddlJoinPassType").val();

        if (splitJoinType === "default") {
            kmsgbox.info(kresource.getItem("gatewaysavewarnmsg"));
            return;
        } else if (directionType === "default") {
            kmsgbox.info(kresource.getItem("gatewaydirectionsavewarnmsg"));
            return;
        }

        if (activity) {
            activity.gatewaySplitJoinType = splitJoinType;
            activity.gatewayDirection = directionType;
            if (directionType === gatewayproperty.Direction.EOrJoin) {
                activity.gatewayJoinPass = joinPasssType;
            }
            //update node user object
            kmain.setVertexValue(activity);
        }
    }

    return gatewayproperty;
})()
//#endregion

//#region event property
var eventproperty = (function () {
    function eventproperty() {
    }

    //popup cron editor dialog
    eventproperty.editCron = function () {
        var expression = $("#txtCronExpression").val();
        BootstrapDialog.show({
            title: kresource.getItem("cronexpression"),
            message: $('<div></div>').load('cron/edit'),
            data: { "expression": expression },
            draggable: true,
            onshown: function (e) {
                var expr = this.data.expression;
                setTimeout(function () {
                    if (expr && expr !== "null") {
                        $("#cronExpressionValue").html(expr);
                        $.syncCronExpression(expr);

                        $.updateCronExpression();
                        $.updateCronGui();
                    }
                }, 200);
            }
        });
    }

    //load activity
    eventproperty.load = function (activity) {
        if (activity) {
            if (activity.type === kmodel.Config.NODE_TYPE_START) {
                if (activity.trigger === "Timer") {
                    $("#txtCronExpression").val(activity.expression);
                } else if (activity.trigger === "Message") {
                    $("#txtMessageTopic").val(activity.expression);
                } else if (activity.trigger === "Conditional") {
                    $("#txtCondition").val(activity.expression);
                }
            }
            else if (activity.type === kmodel.Config.NODE_TYPE_END) {
                if (activity.trigger === "Timer") {
                    $("#txtDeadline").val(activity.expression);
                } else if (activity.trigger === "Message") {
                    $("#txtMessageTopic").val(activity.expression);
                } else if (activity.trigger === "Conditional") {
                    $("#txtCondition").val(activity.expression);
                }
            }
            else if (activity.type === kmodel.Config.NODE_TYPE_INTERMEDIATE) {
                if (activity.trigger === "Timer") {
                    $("#txtOverdue").val(activity.expression);
                } else if (activity.trigger === "Message") {
                    $("#txtMessageTopic").val(activity.expression);
                } else if (activity.trigger === "Conditional") {
                    $("#txtCondition").val(activity.expression);
                }
            }
            activityproperty.loadActions(activity);
        }
    }

    //save activity basic information
    eventproperty.save = function (activity) {
        if (activity) {
            if (activity.type === kmodel.Config.NODE_TYPE_START) {
                if (activity.trigger === "Timer") {
                    activity.expression = $("#txtCronExpression").val();
                } else if (activity.trigger === "Message") {
                    activity.expression = $("#txtMessageTopic").val();
                } else if (activity.trigger === "Conditional") {
                    activity.expression = $("#txtCondition").val();
                }
            }
            else if (activity.type === kmodel.Config.NODE_TYPE_END) {
                if (activity.trigger === "Timer") {
                    activity.expression = $("#txtDeadline").val();
                } else if (activity.trigger === "Message") {
                    activity.expression = $("#txtMessageTopic").val();
                } else if (activity.trigger === "Conditional") {
                    activity.expression = $("#txtCondition").val();
                }
            }
            else if (activity.type === kmodel.Config.NODE_TYPE_INTERMEDIATE) {
                if (activity.trigger === "Timer") {
                    activity.expression = $("#txtOverdue").val();
                } else if (activity.trigger === "Message") {
                    activity.expression = $("#txtMessageTopic").val();
                } else if (activity.trigger === "Conditional") {
                    activity.expression = $("#txtCondition").val();
                }
            }

            //save actions
            activityproperty.saveActions(activity);

            //update node user object
            kmain.setVertexValue(activity);
        }
    }
    return eventproperty;
})()
//#endregion

//#region sub process manager
var subprocessmanager = (function () {
    function subprocessmanager() {
    }

    var msubprocessguid = null;
    var msubprocessname = null;

    subprocessmanager.load = function (activity) {
        if (activity !== null) {
            if (activity.subId !== "") {
                $("#txtProcessGUID").val(activity.subId);
                subprocessmanager.getProcess(activity.subId);
            } else {
                if (activity.subVar !== "") {
                    $("#txtSubVar").val(activity.subVar);
                }
            }
        }
        subprocessmanager.getProcessList();
    }

    //get process records
    subprocessmanager.getProcessList = function () {
        $("#spinner").show();
        jshelper.ajaxPost('api/Wf2Xml/GetProcessListSimple', null, function (result) {
            if (result.Status === 1) {
                var divProcessGrid = document.querySelector('#mySubProcessGrid');
                var gridOptions = {
                    columnDefs: [
                        { headerName: 'ID', field: 'ID', width: 50 },
                        { headerName: kresource.getItem('processguid'), field: 'ProcessGUID', width: 120 },
                        { headerName: kresource.getItem('processname'), field: 'ProcessName', width: 160 },
                        { headerName: kresource.getItem('version'), field: 'Version', width: 40 },
                        { headerName: kresource.getItem('status'), field: 'IsUsing', width: 60 },
                        { headerName: kresource.getItem('createddatetime'), field: 'CreatedDateTime', width: 120 }
                    ],
                    rowSelection: 'single',
                    onSelectionChanged: onSelectionChanged,
                    onRowDoubleClicked: onRowDoubleClicked
                };

                new agGrid.Grid(divProcessGrid, gridOptions);
                gridOptions.api.setRowData(result.Entity);

                $('#loading-indicator').hide();

                function onSelectionChanged() {
                    var selectedRows = gridOptions.api.getSelectedRows();
                    var selectedProcessID = 0;
                    selectedRows.forEach(function (selectedRow, index) {
                        selectedProcessID = selectedRow.ID;
                        msubprocessguid = selectedRow.ProcessGUID;      //marked and returned selected row info
                        msubprocessname = selectedRow.ProcessName;
                    });
                }

                function onRowDoubleClicked(e, args) {
                    var currentProcessGUID = $("#txtProcessGUID").val();
                    if (currentProcessGUID !== msubprocessguid) {
                        kmsgbox.confirm(kresource.getItem("subprocessconfirmmsg"), function (result) {
                            if (result === "Yes") {
                                $("#txtProcessGUID").val(msubprocessguid);
                                $("#txtProcessName").val(msubprocessname);
                                return;
                            }
                        });
                    }
                }
            }
            else {
                kmsgbox.error(kresource.getItem("processlisterrormsg"));
            }
        });

        function datetimeFormatter(row, cell, value, columnDef, dataContext) {
            if (value != null && value != "") {
                return value.substring(0, 10);
            }
        }
    }

    subprocessmanager.getProcess = function (processGUID) {
        if (processGUID !== null
            && processGUID !== undefined) {
            var query = { "ProcessGUID": processGUID };
            jshelper.ajaxPost('api/Wf2Xml/GetProcess', JSON.stringify(query), function (result) {
                if (result.Status === 1) {
                    var entity = result.Entity;

                    $("#txtProcessName").val(entity.ProcessName);
                }
            });
        }
    }

    subprocessmanager.save = function (activity) {
        if (activity) {
            var subProcessId = $("#txtProcessGUID").val();
            activity.subId = subProcessId;
            if (subProcessId !== '') {
                activity.subType = 'Fixed';
            } else {
                var subVarName = $("#txtSubVar").val();
                if (subVarName !== '') {
                    activity.subType = 'Dynamic';
                    activity.subVar = subVarName;
                }
            }
            
            //update node user object
            kmain.setVertexValue(activity);
        }
    }

    return subprocessmanager;
})()

var poolproperty = (function () {
    function poolproperty() {
    }

    poolproperty.loadPool = function () {
        initControls();
        var pool = kmain.mxSelectedDomElement.ElementObject;
        $("#txtPoolTitle").val(pool.title);

        // 获取泳道绑定的流程信息
        var process = pool.process;        
        if (process !== undefined) {
            $("#ddlPackageType").val(process.package);
            $("#txtProcessGUID").val(process.id);
            $("#txtProcessName").val(process.name);
            $("#txtProcessCode").val(process.code);
            $("#txtProcessDescription").val(process.description);
        }
    }

    function initControls() {
        var bindingProcess = kmain.mpoolPropertyDialog.getData('bindingProcess');
        if (bindingProcess === true) {
            $("#divBindingProcess").show();
        } else {
            $("#divBindingProcess").hide();
        }

        var process = {};
        $("#ddlPackageType").change(function () {
            var pool = kmain.mxSelectedDomElement.ElementObject;
            var packageType = $("#ddlPackageType").val();
            if (packageType === "MainProcess") {   
                //唯一性检查，只能有一个主流程
                if (isNotUniqueMainProcessOfCurrentPool() === true) {
                    kmsgbox.warn(kresource.getItem("processpooluniquewarnmsg"));
                    $("#ddlPackageType").val("PoolProcess");
                    return false;
                } else {
                    //主流程
                    process = kmain.mxSelectedProcessEntity;
                    if (process !== null) {
                        $("#txtProcessGUID").val(process.ProcessGUID);
                        $("#txtProcessName").val(process.ProcessName);
                        $("#txtProcessCode").val(process.ProcessCode);
                        $("#txtProcessDescription").val(process.Description);
                    } else {
                        var newProcessGUID = jshelper.getUUID();
                        $("#txtProcessGUID").val(newProcessGUID);
                    }
                }
            } else if (packageType === "PoolProcess") {
                //泳道流程
                process = pool.process;
                if (process !== undefined) {
                    $("#txtProcessGUID").val(process.id);
                    $("#txtProcessName").val(process.name);
                    $("#txtProcessCode").val(process.code);
                    $("#txtProcessDescription").val(process.description);
                } else {
                    var newProcessGUID = jshelper.getUUID();
                    $("#txtProcessGUID").val(newProcessGUID);
                    $("#txtProcessName").val("");
                    $("#txtProcessCode").val("");
                    $("#txtProcessDescription").val();
                }
            }            
        });
    }

    function isNotUniqueMainProcessOfCurrentPool() {
        var isNotUnique = false;
        var cell = kmain.mxSelectedDomElement.Cell;
        var pool = kmain.mxSelectedDomElement.ElementObject;
        var model = kmain.mxGraphEditor.graph.getModel();
        var childNodeList = model.getChildVertices(kmain.mxGraphEditor.graph.getDefaultParent());
        //列表检查
        for (var i = 0; i < childNodeList.length; i++) {
            var childNode = childNodeList[i];
            var childNodeValue = model.getValue(childNode);
            if (childNodeValue.nodeName === "Swimlane") {
                if (childNode.id !== cell.id) {
                    if (childNodeValue.getAttribute("type") === "MainProcess") {
                        isNotUnique = true;
                        break;
                    }
                } 
            }
        }
        return isNotUnique;
    }

    poolproperty.savePool = function () {
        var pool = kmain.mxSelectedDomElement.ElementObject;
        pool.title = $("#txtPoolTitle").val();
        pool.type = $("#ddlPackageType").val();
        
        var bindingProcess = kmain.mpoolPropertyDialog.getData('bindingProcess');
        if (bindingProcess === true) {
            var process = pool.process;
            if (process === undefined) {
                process = {};
            }

            process.id = $("#txtProcessGUID").val();
            process.package = $("#ddlPackageType").val();
            process.name = $("#txtProcessName").val();
            process.code = $("#txtProcessCode").val();
            process.description = $("#txtProcessDescription").val();

            if (process.package.trim() === ""
                || process.name.trim() === ""
                || process.code.trim() === "") {
                kmsgbox.warn(kresource.getItem("processxmlsavepoolwarnmsg"));
                return false;
            }
            pool.process = process;
        }
        kmain.setPoolValue(pool);
        kmain.mpoolPropertyDialog.close();
    }

    return poolproperty;
})()
//#endregion