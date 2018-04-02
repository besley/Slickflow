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

var gatewayproperty = (function () {
    function gatewayproperty() {
    }

    gatewayproperty.splitOptions = [
        {"value": "AndSplit", "text": "与分支"},
        {"value": "OrSplit", "text": "或分支"}
    ];

    gatewayproperty.joinOptions = [
        { "value": "AndJoin", "text": "与合并" },
        { "value": "OrJoin", "text": "或合并" }
    ];

    gatewayproperty.loadGatewayInformation = function () {
        var activity = kmain.mxSelectedDomElement.Element;
        if (activity) {
            //fill activity basic information
            $("#txtDescription").val(activity.description);
            if (activity.gatewaySplitJoinType) {
                var splitJoinType = activity.gatewaySplitJoinType;
                $("#ddlGatewayType").val(splitJoinType);
                $("#ddlGatewayType").attr("disabled", true);
                //fill direction type
                gatewayproperty.appendDirectionType(splitJoinType);
                if (activity.gatewayDirection){
                    $("#ddlDirectionType").val(activity.gatewayDirection);
                }
            }
        }

        $("#ddlGatewayType").change(function () {
            var splitJoinType = $("#ddlGatewayType").val();
            gatewayproperty.appendDirectionType(splitJoinType);
        });
    }

    gatewayproperty.appendDirectionType = function (splitJoinType) {
        //initialize default options
        $("#ddlDirectionType")
            .empty()
            .append('<option value="default" selected>--请选择--</option>');

        //load details
        var item = null;
        if (splitJoinType == "Split") {
            for (var i = 0; i < gatewayproperty.splitOptions.length; i++) {
                item = gatewayproperty.splitOptions[i];
                $('#ddlDirectionType')
                    .append($("<option></option>")
                    .attr("value", item.value)
                    .text(item.text));
            }
        } else if (splitJoinType == "Join") {
            for (var i = 0; i < gatewayproperty.joinOptions.length; i++) {
                item = gatewayproperty.joinOptions[i];
                $('#ddlDirectionType')
                    .append($("<option></option>")
                    .attr("value", item.value)
                    .text(item.text));
            }
        }
    }

    gatewayproperty.saveGatewayInformation = function () {
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

        var activity = kmain.mxSelectedDomElement.Element;
        if (activity) {
            activity.description = description;
            activity.gatewaySplitJoinType = splitJoinType;
            activity.gatewayDirection = directionType;

            //update node user object
            kmain.setVertexValue(activity);
        }
    }

    return gatewayproperty;
})()