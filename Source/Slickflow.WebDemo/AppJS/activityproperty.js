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

var activityManager;
if (!activityManager) activityManager = {};

(function () {
    //load performer datagrid
    activityManager.getActivityPerformer = function(node) {
        var columnActivityPerformer = [
           { id: "id", name: "id", field: "outerId", width: 60, cssClass: "bg-gray" },
           { id: "RoleName", name: "角色名称", field: "name", width: 200, cssClass: "bg-gray" },
           { id: "RoleCode", name: "角色代码", field: "code", width: 200, cssClass: "bg-gray" },
           { id: "guid", name: "guid", field: "id", width: 40, visible: false }
        ];

        var optionsActivityPerformer = {
            editable: true,
            enableCellNavigation: true,
            enableColumnReorder: true,
            asyncEditorLoading: true,
            forceFitColumns: false,
            topPanelHeight: 25
        };

        var dsActivityPerformer = loadActivityPerformers(node);
        var dvActivityPerformer = new Slick.Data.DataView({ inlineFilters: true });
        var gridActivityPerformer = new Slick.Grid("#myGridPerformer",
            dvActivityPerformer,
            columnActivityPerformer,
            optionsActivityPerformer);

        dvActivityPerformer.onRowsChanged.subscribe(function (e, args) {
            gridActivityPerformer.invalidateRows(args.rows);
            gridActivityPerformer.render();
        });

        dvActivityPerformer.onRowCountChanged.subscribe(function (e, args) {
            gridActivityPerformer.updateRowCount();
            gridActivityPerformer.render();
        });

        dvActivityPerformer.beginUpdate();
        dvActivityPerformer.setItems(dsActivityPerformer, "id");
        gridActivityPerformer.setSelectionModel(new Slick.RowSelectionModel());
        dvActivityPerformer.endUpdate();

        //rows change event
        gridActivityPerformer.onSelectedRowsChanged.subscribe(function (e, args) {
            var selectedRowIndex = args.rows[0];
            var row = dvActivityPerformer.getItemByIdx(selectedRowIndex);
            if (row) {
                kmain.selectedActivityPerformerGUID = row.id;
            }
        });
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
                if (id == p.id) {
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
    activityManager.saveActivityInformation = function () {
        var activityName = $("#txtActivityName").val();
        var activityCode = $("#txtActivityCode").val();
        var description = $("#txtDescription").val();
        var node = window.parent.$("#divActivityDialog").data("node");

        if (node) {
            node.sdata.name = activityName;
            node.sdata.code = activityCode;
            node.sdata.description = description;

            //update jsplumb node name
            node.setNodeName(activityName);
        }

        window.parent.$('#divActivityDialog').dialog('close');
    };

    //add performer to activity performers list
    activityManager.addPerformer = function () {
        kmain.selectedParticipantType = null;
        kmain.selectedParticipantItem = null;

        var roleDialog = $("#divRoleDialog")
            .load("/sfd/views/rolelist.html",
            function () {
                var dialogOptions = {
                    title: "角色数据",
                    width: 500,
                    height: 400,
                    modal: true,
                    autoOpen: false,
                    beforeClose: function (evt, ui) {
                        var participantType = kmain.selectedParticipantType;
                        var participantItem = kmain.selectedParticipantItem;
                        //sync activity perfromer datasource
                        if (participantType && participantItem) {
                            syncActivityPerformers(participantType, participantItem);
                        }
                    },
                    close: function (event, ui) {
                        $(this).children().remove();
                        $(this).dialog("destroy");
                    }
                };

                roleDialog
                    .dialog(dialogOptions)
                    .dialog('open');
            }
        );
    }

    //sync activity perfromer datasource
    function syncActivityPerformers(participantType, participantItem) {
        var node = kmain.currentSelectedDomElement.node;      //kgraph directive
        var performers = node.sdata.performers;
        if (!performers) {
            performers = node.sdata.performers = [];
        }


        //check participants exists the newly added role item
        var packageData = kmain.mcurrentPackageData;
        var participants = packageData.participants;

        var participantAdded = null;
        for (var i = 0; i < participants.length; i++) {
            if (participantType == "role" 
                && participants[i].type == "Role"
                && participantItem.ID == participants[i].outerId) {
                participantAdded = participants[i];
                break;
            }
        }

        if (participantAdded) {
            //check this participant wether exists in the activity performers gridview
            var isExisted = false;
            for (var i = 0; i < performers.length; i++) {
                if (participantAdded.id == performers[i].id) {
                    isExisted = true;
                    break;
                }
            }

            if (isExisted) {
                alert("要添加的角色或用户数据已经存在！");
            } else {
                var performer = {
                    "id": participantAdded.id,
                    "name": participantAdded.name,
                    "code": participantAdded.code,
                    "outerId": participantAdded.outerId
                }
                node.sdata.performers.push(performer);
                //refresh the activity performer gridview
                activityManager.getActivityPerformer(node);
            }
        } else {
            //added this new participant item to participants collection
            if (participantType == "role") {
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
                activityManager.getActivityPerformer(node);
            }
        }
    }

    //del performer from activity performers list
    activityManager.delPerformer = function () {
        var performerGUID = kmain.selectedActivityPerformerGUID;
        var node = kmain.currentSelectedDomElement.node;
        var performers = node.sdata.performers;

        //remove the selected performer record 
        node.sdata.performers = jQuery.grep(performers, function (item) {
            return item.id != performerGUID;
        });

        //refresh the gridview
        activityManager.getActivityPerformer(node);
    }
})()