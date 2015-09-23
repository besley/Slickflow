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

var kmain;
if (!kmain) kmain = {};

(function () {
    
    misSelectedNew = false;
    mselectedProcessGUID = null;
    mselectedProcessRecord = null;
    mselectedProcessVersion = null;
    mgraphView = null;
    mcurrentPackageData = null;

    //activity property page variables
    selectedActivityPerformerGUID = null;
    selectedParticipantType = null;
    selectedParticipantItem = null;

    //current select element of graph
    currentSelectedDomElement = null;
    
    //transition property page variables
    currentSelectedLine = null;

    //#region kgraph mainp age init
    kmain.init = function () {
        //clear 
        //make bpmn image part draggable
        makeProcessNotationDraggalbe();

        //make graph container droppable
        makeGraphContainerDroppable();

        //keyup for delete event detected
        attachDeleteEvent();
    }

    var attachDeleteEvent = function () {
        //registered keyup event for document
        $(document).keyup(function (e) {
            if (e.keyCode == 46) {
                if (kmain.currentSelectedDomElement != null) {
                    if (kmain.currentSelectedDomElement.type === "NODE") {
                        if (confirm("确认要删除节点吗? 将会删除节点属性及用户角色等数据。") === true) {
                            jsPlumb.remove(kmain.currentSelectedDomElement.element);

                            //remove node or releated lines from collection
                            onNodeRemoved(kmain.currentSelectedDomElement.node);
                            kmain.currentSelectedDomElement = null;
                            return;
                        }
                    }
                    else if (kmain.currentSelectedDomElement.type === "CONNECTION") {
                        if (confirm("确认要删除连线吗? 将会删除连线上的条件等数据。") === true) {
                            jsPlumb.detach(kmain.currentSelectedDomElement.connection);

                            //remove line from collection
                            onLineRemoved(kmain.currentSelectedDomElement.line);
                            kmain.currentSelectedDomElement = null;
                            return;
                        }
                    }
                }
            }
        });
    }

    var makeProcessNotationDraggalbe = function () {
        $(".imagepart").draggable({
            //revert: "valid",//拖动之后原路返回
            helper: "clone",//复制自身
            scope: "dragflag"//标识
        });
    }

    var makeGraphContainerDroppable = function () {
        $("#kgraphContainer").droppable({
            accept: ".imagepart",
            activeClass: "drop-active",
            scope: "dragflag",
            drop: function (event, ui) {
                var left = parseInt(ui.offset.left - $(this).offset().left);
                var top = parseInt(ui.offset.top - $(this).offset().top) + 4;
                var nodeType = ui.draggable[0].id;
                var target = {
                    type: nodeType,
                    left: left,
                    top: top
                }

                //drag bpmn image part and drop to the graph container
                //will create a new node and update the node collection
                if (kmain.mgraphView === undefined) {
                    alert("请先打开流程记录！");
                    return;
                }
                kmain.mgraphView.createNode(target);
            }
        });
    }
    //#endregion

    //#region process dialog
    kmain.loadProcess = function () {
        var processDialog = $("#divProcessDialog")
            .load("/sfd/views/processlist.html",
                function () {
                    var dialogOptions = {
                        title: "流程定义数据",
                        width: 700,
                        height: 500,
                        modal: true,
                        autoOpen: false,
                        beforeClose: function (evt, ui) {
                            var processGUID = kmain.mselectedProcessGUID;
                            var processVersion = kmain.mselectedProcessVersion;
                            var query = {
                                processGUID: processGUID,
                                processVersion: processVersion
                            };
                            if (kmain.misSelectedNew && processGUID) {
                                //load process xml file
                                processFileManager.queryProcessFile(query, function (result) {
                                    if (result.Status == 1) {
                                        $("#kgraphContainer").html("");
                                        jsPlumb.deleteEveryEndpoint();
                                        jsPlumb.detachEveryConnection();

                                        var processFileEntity = result.Entity;
                                        kmain.mgraphView = kloader.initialize(processFileEntity);
                                        kmain.mcurrentPackageData = kmain.mgraphView.packageData;
                                    } else {
                                        alert(result.Message);
                                    }
                                });
                            }
                        },
                        close: function (event, ui) {
                            $(this).children().remove();
                            $(this).dialog("destroy");
                        }
                    };

                    //open process dialog
                    processDialog
                        .dialog(dialogOptions)
                        .dialog('open');
                }   //end of load function
            );
    }

    kmain.saveProcessFile = function () {
        var processGUID = kmain.mgraphView.processGUID;
        var packageData = kmain.mgraphView.packageData;     //include participants and process 
        var processFileEntity = kloader.serialize2Xml(processGUID, packageData);

        console.info("%o", processFileEntity);

      //  processFileManager.saveProcessFile(processFileEntity);
    }
    //#endregion

    //#region remove node and lines
    var onNodeRemoved = function (node) {
        var id = node.id();
        var removingIndexOfSLines = [], removingIndexOfLines = [];
        var lines = kmain.mgraphView.lines;
        var slines = kmain.mgraphView.processData.slines;

        //first step, remove transtion of this node
        //remove line view from lines of graph canvas
        for (var i = 0; i < lines.length; i++) {
            if (id == lines[i].from()
                || id == lines[i].to()) {
                removingIndexOfLines.push(i);
            }
        }

        for (var i = 0; i < removingIndexOfLines.length; i++) {
            lines.splice(removingIndexOfLines[i], 1);
        }


        //remove line data source from transition collection
        for (var i = 0; i < slines.length; i++) {
            if (id == slines[i].from 
                || id == slines[i].to) {
                removingIndexOfSLines.push(i);
            }
        }

        for (var i = 0; i < removingIndexOfSLines.length; i++) {
            slines.splice(removingIndexOfSLines[i], 1);
        }

        //remove node from the nodeview collection
        var nodes = kmain.mgraphView.nodes;
        for (var i = 0; i < nodes.length; i++){
            if (id == nodes[i].id()) {
                nodes.splice(i, 1);
                break;
            }
        }

        //remove the node from activity collection
        var snodes = kmain.mgraphView.processData.snodes;
        for (var i = 0; i < snodes.length; i++) {
            if (id == snodes[i].id) {
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
            if (id == lines[i].id()) {
                lines.splice(i, 1);
                break;
            }
        }

        //remove line data source from transition collection
        for (var i = 0; i < slines.length; i++) {
            if (id == slines[i].id) {
                slines.splice(i, 1);
                break;
            }
        }
    }
    //#endregion
    
})()