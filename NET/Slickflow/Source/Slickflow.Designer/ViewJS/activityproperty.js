/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。

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

    activityproperty.mselectedActivityPerformerGUID = "";

	//load activity property
	activityproperty.loadActivity = function () {
        var activity = kmain.mxSelectedDomElement.Element;

		if (activity) {
			//fill activity basic information
			$("#txtActivityName").val(activity.name);
			$("#txtActivityCode").val(activity.code);
			$("#txtDescription").val(activity.description);

			//load performer list of current activity
            loadActivityPerformer(activity);
		}
	}

	//load performer datagrid
	function loadActivityPerformer(activity) {
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
				if (performer) activityPerformers.push(performer);
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

	//save activity basic information
	activityproperty.saveActivity = function () {
		var activityName = $("#txtActivityName").val();
		var activityCode = $("#txtActivityCode").val();
		var description = $("#txtDescription").val();
		var activity =  kmain.mxSelectedDomElement.Element;


		if (activity) {
			activity.name = activityName;
			activity.code = activityCode;
            activity.description = description;

            //update node user object
            kmain.setVertexValue(activity);
		}
	};

	//#region performers add / delete
	//add performer to activity performers list
	activityproperty.addPerformer = function () {
		BootstrapDialog.show({
			title: '角色列表',
            message: $('<div></div>').load('role/list'),
            draggable: true
		});
	}

	//sync activity perfromer datasource
	activityproperty.syncActivityPerformers = function (participantType, participantItem) {
		var activity =  kmain.mxSelectedDomElement.Element;
		var performers = activity.performers;
		if (!performers) {
			performers = activity.performers = [];
		}

		//check participants exists the newly added role item
        var participantAdded = null;
        if (kmain.mxSelectedPackageData != null) {
            var participants = kmain.mxSelectedPackageData.participants;
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
        var activity =  kmain.mxSelectedDomElement.Element
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

	return activityproperty;
})()