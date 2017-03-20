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

var activityproperty = (function () {
	function activityproperty() {
	}

	//load activity property
	activityproperty.loadActivity = function () {
		$("#ddlComplexType").prop("selectedIndex", -1);
		$("#ddlMergeType").prop("selectedIndex", -1);
		$("#ddlCompareType").prop("selectedIndex", -1);

		var node = kgraph.mcurrentSelectedDomElement.node;
		if (node) {
			//fill activity basic information
			$("#txtActivityName").val(node.sdata.name);
			$("#txtActivityCode").val(node.sdata.code);
			$("#txtDescription").val(node.sdata.description);
			//load performer list of current activity
			activityproperty.getActivityPerformer(node);
		}
	}

	//load performer datagrid
	activityproperty.getActivityPerformer = function (node) {
		var divPerformerGrid = document.querySelector('#myPerformerGrid');
		$(divPerformerGrid).empty();

		var gridOptions = {
			columnDefs: [
				{ headerName: 'ID', field: 'id', width: 160 },			//performerguid
				{ headerName: '角色名称', field: 'name', width: 160 },
				{ headerName: '角色代码', field: 'code', width: 160 }
			],
			rowSelection: 'single',
			onSelectionChanged: onSelectionChanged
		};

		new agGrid.Grid(divPerformerGrid, gridOptions);
		var dsActivityPerformer = loadActivityPerformers(node);

		gridOptions.api.setRowData(dsActivityPerformer);

		function onSelectionChanged() {
			var selectedRows = gridOptions.api.getSelectedRows();
			selectedRows.forEach(function (selectedRow, index) {
				kmain.mselectedActivityPerformerGUID = selectedRow.id;
			});
		}
	}

	function loadActivityPerformers(node) {
		var performer = null;
		var activityPerformers = [];

		if (node.sdata.performers && node.sdata.performers.length > 0) {
			for (var i = 0; i < node.sdata.performers.length; i++) {
				performer = getPerformerByGUID(node.sdata.performers[i].id);
				if (performer) activityPerformers.push(performer);
			}
		}
		return activityPerformers;
	}

	function getPerformerByGUID(id) {
		var performer = null;
		var participants = null;
		var packageData = null;

		var packageData = kmain.mcurrentPackageData;
		if (packageData && packageData.participants) {
			for (var i = 0; i < packageData.participants.length; i++) {
				var p = packageData.participants[i];
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

	//save activity basic information
	activityproperty.saveActivity = function () {
		var activityName = $("#txtActivityName").val();
		var activityCode = $("#txtActivityCode").val();
		var description = $("#txtDescription").val();
		var node = kgraph.mcurrentSelectedDomElement.node;


		if (node) {
			node.sdata.name = activityName;
			node.sdata.code = activityCode;
			node.sdata.description = description;

			//update jsplumb node name
			node.setNodeName(activityName);
		}
	};

	//#region performers add / delete
	//add performer to activity performers list
	activityproperty.addPerformer = function () {
		kmain.mselectedParticipantType = null;
		kmain.mselectedParticipantItem = null;

		BootstrapDialog.show({
			title: '角色列表',
			message: $('<div></div>').load('role/list')
		});
	}

	//sync activity perfromer datasource
	activityproperty.syncActivityPerformers = function (participantType, participantItem) {
		var node = kgraph.mcurrentSelectedDomElement.node;      //kgraph directive
		var performers = node.sdata.performers;
		if (!performers) {
			performers = node.sdata.performers = [];
		}


		//check participants exists the newly added role item
		var packageData = kmain.mcurrentPackageData;
		var participants = packageData.participants;

		var participantAdded = null;
		for (var i = 0; i < participants.length; i++) {
			if (participantType === "role"
                && participants[i].type === "Role"
                && participantItem.ID === participants[i].outerId) {
				participantAdded = participants[i];
				break;
			}
		}

		if (participantAdded) {
			//check this participant wether exists in the activity performers gridview
			var isExisted = false;
			for (var i = 0; i < performers.length; i++) {
				if (participantAdded.id === performers[i].id) {
					isExisted = true;
					break;
				}
			}

			if (isExisted) {
				$.msgBox({
					title: "Desinger / ActivityProperty",
					content: "要添加的角色或用户数据已经存在！",
					type: "info"
				});
			} else {
				var performer = {
					"id": participantAdded.id,
					"name": participantAdded.name,
					"code": participantAdded.code,
					"outerId": participantAdded.outerId
				}
				node.sdata.performers.push(performer);
				//refresh the activity performer gridview
				activityproperty.getActivityPerformer(node);
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
				kmain.mgraphView.packageData.participants.push(participant);

				//add new performer to node activity performers gridview
				var performer = {
					"id": participant.id,
					"name": participant.name,
					"code": participant.code,
					"outerId": participant.outerId
				}
				node.sdata.performers.push(performer);
				//refresh the activity performer gridview
				activityproperty.getActivityPerformer(node);
			}
		}
	}

	//del performer from activity performers list
	activityproperty.delPerformer = function () {
		var performerGUID = kmain.mselectedActivityPerformerGUID;
		var node = kgraph.mcurrentSelectedDomElement.node;
		var performers = node.sdata.performers;

		//remove the selected performer record 
		node.sdata.performers = jQuery.grep(performers, function (item) {
			return item.id !== performerGUID;
		});
		//refresh the gridview
		activityproperty.getActivityPerformer(node);
	}
	//#endregion

	return activityproperty;
})()