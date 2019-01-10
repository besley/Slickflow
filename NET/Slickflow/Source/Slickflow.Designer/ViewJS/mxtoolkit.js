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