/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
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