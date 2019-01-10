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

var transitionproperty = (function () {
    function transitionproperty() {
    }

    transitionproperty.load = function () {
        var transition = kmain.mxSelectedDomElement.Element;

        if (transition) {
            $("#txtDescription").val(transition.description);
            if (transition.receiver) {
                if (transition.receiver.type)
                    $("#ddlReceiverType").val(transition.receiver.type);
            }

            if (transition.condition)
                $("#txtCondition").val($.trim(transition.condition.text));
        }
    }

    transitionproperty.save = function () {
        var description = $("#txtDescription").val();
        var receiver = {};
        var receiverType = $("#ddlReceiverType").val();
        if (receiverType !== "default") receiver.type = receiverType;

        var condition = {};
        condition.type = "Expression";
        condition.text = $.trim($("#txtCondition").val());

        var transition = kmain.mxSelectedDomElement.Element;
        if (transition !== null){
            transition.description = description;
            transition.receiver = receiver;
            transition.condition = condition;

		    //update line label text
            kmain.setEdgeValue(transition);
        }
    }

    return transitionproperty;
})()