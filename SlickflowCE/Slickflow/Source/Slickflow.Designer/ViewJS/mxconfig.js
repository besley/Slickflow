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




