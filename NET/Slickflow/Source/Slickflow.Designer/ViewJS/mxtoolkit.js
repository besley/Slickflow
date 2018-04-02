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

var mxtoolkit = (function () {
	function mxtoolkit() {
	}

    mxtoolkit.insertSwimlane = function(graph, swimlane){
        var doc = mxUtils.createXmlDocument();
        var swimlaneElement = doc.createElement('Swimlane');
        swimlaneElement.setAttribute("label", swimlane.name);

        var model = graph.getModel();
        var parent = graph.getDefaultParent();
        var geography = swimlane.geography;
        var widget = geography.widget;
        var swimlaneVertex = graph.insertVertex(parent, swimlane.id, swimlaneElement, 
            widget.left, widget.top, widget.width, widget.height,
            geography.style);

        return swimlaneVertex;
    }

	mxtoolkit.insertVertex = function(graph, activity){
        var geography = activity.geography;
        var model = graph.getModel();

        //geography
        var parent =  model.getCell(geography.parent);
        if (!parent) parent = graph.getDefaultParent();

        if (geography.style === null || geography.style === undefined || geography.style === "undefined") {
            geography.style = mxconfig.getVertexStyle(activity)
        }

        var widget = geography.widget;
        var vertex = graph.insertVertex(parent, activity.id, createXmlElement(activity), 
            widget.left, widget.top, widget.width, widget.height, 
            geography.style);

        return vertex;
    }

    function createXmlElement(activity){
        var doc = mxUtils.createXmlDocument();
        var activityElement = doc.createElement('Activity');

        activityElement.setAttribute('label', activity.name);
        activityElement.setAttribute('code', activity.code);

        var descElement = doc.createElement('Description');
        activityElement.appendChild(descElement);

        var description = doc.createTextNode(activity.description);
        descElement.appendChild(description);
        
        //activity type
        var activityTypeElement = doc.createElement('ActivityType');
        activityTypeElement.setAttribute('type', activity.type);
        activityTypeElement = mxfile.setActivityTypeElement(activityTypeElement, activity);
        activityElement.appendChild(activityTypeElement);

        //performers
        if (activity.performers && activity.performers.length > 0){
            var performersElement = mxfile.setPerformersElement(doc, activity.performers);
            activityElement.appendChild(performersElement);
        }

        return activityElement;
    }

    mxtoolkit.insertEdge = function(graph, transition){       
        var doc = mxUtils.createXmlDocument();
        var transitonElement = mxfile.setTransitionElement(doc, transition);
        
        //condition
        if ($.isEmptyObject(transition.condition) === false) {
            var conditionElement = mxfile.setConditionElement(doc, transition.condition);
            transitonElement.appendChild(conditionElement);                  
        }
        
        //receiver
        if ($.isEmptyObject(transition.receiver) === false){
            var receiverElement = mxfile.setReceiverElement(doc, transition.receiver);
            transitonElement.appendChild(receiverElement);    
        }

        //geography
        var geography = transition.geography;
        var model = graph.getModel();
        var parent = null;
        if (geography) parent = model.getCell(geography.parent);
        if (!parent) parent = graph.getDefaultParent();
        
        var edge = graph.insertEdge(parent, transition.id, transitonElement, 
            graph.getModel().getCell(transition.from), graph.getModel().getCell(transition.to));
        
        return edge;
    }

	return mxtoolkit;
})()