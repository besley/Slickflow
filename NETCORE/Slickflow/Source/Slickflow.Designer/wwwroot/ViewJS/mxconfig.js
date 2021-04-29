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

    //style configuration
    mxconfig.style = {};
    mxconfig.style["start"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/event.png';
    mxconfig.style["end"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/event_end.png';
    mxconfig.style["start-timer"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/start_event_timer.png';
    mxconfig.style["end-timer"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/end_event_timer.png';
    mxconfig.style["start-message"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/message.png';
    mxconfig.style["end-message"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/end_event_message.png';
    mxconfig.style["intermediate"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/intermediate.png';
    mxconfig.style["intermediate-timer"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/intermediate_event_timer.png';
    mxconfig.style["subprocess"] = 'rectangle';
    mxconfig.style["gateway-split"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/fork.png';
    mxconfig.style["gateway-join"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/merge.png';
    mxconfig.style["subprocess"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/subprocess.png';;
    mxconfig.style["multipleinstance"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/multiple_instance_task.png';
    mxconfig.style["service"] = 'symbol;image=Scripts/mxGraph/src/editor/images/symbols/service_task.png';

    mxconfig.getVertexStyle = function(activity){
        var style = null;
        var nodeType = activity.type;
        var trigger = activity.trigger;

		//render different type node
        if (nodeType === kmodel.Config.NODE_TYPE_START) {
            if (trigger === "Timer") {
                style = mxconfig.style["start-timer"];
            } else if (trigger === "Message") {
                style = mxconfig.style["start-message"];
            } else {
                style = mxconfig.style["start"];
            }
        } else if (nodeType === kmodel.Config.NODE_TYPE_INTERMEDIATE) {
            if (trigger === "Timer") {
                style = mxconfig.style["intermediate-timer"];
            } else {
                style = mxconfig.style["intermediate"];
            }
        } else if (nodeType === kmodel.Config.NODE_TYPE_END) {
            if (trigger === "Timer") {
                style = mxconfig.style["end-timer"];
            } else if (trigger === "Message") {
                style = mxconfig.style["end-message"];
            } else {
                style = mxconfig.style["end"];
            }
		}else if (nodeType === kmodel.Config.NODE_TYPE_TASK) {
			style = mxconfig.style["task"];
		}
		else if (nodeType === kmodel.Config.NODE_TYPE_MULTIPLEINSTANCE) {
			style = mxconfig.style["multipleinstance"];
        }
        else if (nodeType === kmodel.Config.NODE_TYPE_SERVICE) {
            style = mxconfig.style["service"];
        }
		else if (nodeType === kmodel.Config.NODE_TYPE_GATEWAY) {
			style = mxconfig.style["gateway-split"];
		}
		else if (nodeType === kmodel.Config.NODE_TYPE_SUBPROCESS) {
			style = mxconfig.style["subprocess"];
		}
		else {
			throw new Error("Unknown Activity Type！");
		}
        return style;
    }
    return mxconfig;
})()




