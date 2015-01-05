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

    gatewayManager.saveGatewayInformation = function () {
        var description = $("#txtDescription").val();
        var splitJoinType = $("#ddlGatewayType").val();
        var directionType = $("#ddlDirectionType").val();

        if (splitJoinType == "default") {
            alert("请重新选择分支合并类型！");
            return;
        } else if (directionType == "default") {
            alert("请重新选择分支合并的子类型！");
            return;
        }

        var node = window.parent.$("#divActivityDialog").data("node");

        if (node) {
            node.data.description = description;
            node.data.gatewaySplitJoinType = splitJoinType;
            node.data.gatewayDirection = directionType;
        }
        window.parent.$('#divActivityDialog').dialog('close');
    }
})()