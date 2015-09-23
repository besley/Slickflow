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

var gatewayManager;
if (!gatewayManager) gatewayManager = {};

(function () {
    gatewayManager.splitOptions = [
        {"value": "AndSplit", "text": "与分支"},
        {"value": "OrSplit", "text": "或分支"}
    ];

    gatewayManager.joinOptions = [
        { "value": "AndJoin", "text": "与合并" },
        { "value": "OrJoin", "text": "或合并" }
    ];

    gatewayManager.appendDirectionType = function (splitJoinType) {
        //initialize default options
        $("#ddlDirectionType")
            .empty()
            .append('<option selected="selected" value="default">--请选择--</option>');

        //load details
        var item = null;
        if (splitJoinType == "Split") {
            for (var i = 0; i < gatewayManager.splitOptions.length; i++) {
                item = gatewayManager.splitOptions[i];
                $('#ddlDirectionType')
                    .append($("<option></option>")
                    .attr("value", item.value)
                    .text(item.text));
            }
        } else if (splitJoinType == "Join") {
            for (var i = 0; i < gatewayManager.joinOptions.length; i++) {
                item = gatewayManager.joinOptions[i];
                $('#ddlDirectionType')
                    .append($("<option></option>")
                    .attr("value", item.value)
                    .text(item.text));
            }
        }
    }

    gatewayManager.loadGatewayInformation = function () {
        var node = kmain.currentSelectedDomElement.node;

        if (node) {
            //fill activity basic information
            $("#txtDescription").val(node.sdata.description);
            if (node.sdata.gatewaySplitJoinType) {
                var splitJoinType = node.sdata.gatewaySplitJoinType;
                $("#ddlGatewayType").val(splitJoinType);
                if (node.sdata.gatewayDirection) {
                    gatewayManager.appendDirectionType(splitJoinType);
                    $("#ddlDirectionType").val(node.sdata.gatewayDirection);
                }
            }
        }

        $("#ddlGatewayType").change(function () {
            var splitJoinType = $("#ddlGatewayType").val();
            gatewayManager.appendDirectionType(splitJoinType);
        });
    }

    gatewayManager.saveGatewayInformation = function () {
        var description = $("#txtDescription").val();
        var splitJoinType = $("#ddlGatewayType").val();
        var directionType = $("#ddlDirectionType").val();

        if (splitJoinType == "default") {
            $.msgBox({
                title: "Designer / GatewayProperty",
                content: "请重新选择分支合并类型！",
                type: "info"
            });
            return;
        } else if (directionType == "default") {
            $.msgBox({
                title: "Designer / GatewayProperty",
                content: "请重新选择分支合并的子类型！",
                type: "info"
            });
            return;
        }

        var node = window.parent.$("#divActivityDialog").data("node");

        if (node) {
            node.sdata.description = description;
            node.sdata.gatewaySplitJoinType = splitJoinType;
            node.sdata.gatewayDirection = directionType;
        }
        window.parent.$('#divActivityDialog').dialog('close');
    }
})()