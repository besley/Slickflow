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

var mxconfig = (function () {
	function mxconfig() {
	}

    mxconfig.style = {};
    mxconfig.style["start"] = 'symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png';
    mxconfig.style["end"] = 'symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png';
    mxconfig.style["subprocess"] = 'rectangle';
    mxconfig.style["gateway-split"] = 'symbol;image=scripts/mxGraph/src/editor/images/symbols/fork.png';
    mxconfig.style["gateway-join"] = 'symbol;image=scripts/mxGraph/src/editor/images/symbols/merge.png';
    mxconfig.style["subprocess"] = 'rounded';
    mxconfig.style["multipleinstance"] = 'symbol;image=scripts/mxGraph/src/editor/images/symbols/samll_multiple_instance_task.png';

    mxconfig.getVertexStyle = function(activity){
        var style = null;
        var nodeType = activity.type;

		//render different type node
		if (nodeType === kmodel.Config.NODE_TYPE_START) {
            style = mxconfig.style["start"];
		}
		else if (nodeType === kmodel.Config.NODE_TYPE_END) {
			style = mxconfig.style["end"];
		}else if (nodeType === kmodel.Config.NODE_TYPE_TASK) {
			style = mxconfig.style["task"];
		}
		else if (nodeType === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
			style = mxconfig.style["multipleinstance"];
		}
		else if (nodeType === kmodel.Config.NODE_TYPE_GATEWAY) {
			style = mxconfig.style["gateway-split"];
		}
		else if (nodeType === kmodel.Config.NODE_TYPE_SUBPROCESS) {
			style = mxconfig.style["subprocess"];
		}
		else {
			throw new Error("未知节点类型！");
		}
        return style;
    }
    return mxconfig;
})()




