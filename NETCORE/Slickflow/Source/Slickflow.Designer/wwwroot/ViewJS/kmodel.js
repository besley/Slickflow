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

var kmodel = (function () {
	function kmodel() {
	}

	kmodel.Config = {
		NODE_PREFIX: "ACT",
        NODE_TYPE_START: "StartNode",
        NODE_TYPE_INTERMEDIATE: "IntermediateNode",
        NODE_TYPE_END: "EndNode",
		NODE_TYPE_TASK: "TaskNode",
        NODE_TYPE_SERVICE: "ServiceNode",
        NODE_TYPE_GATEWAY: "GatewayNode",
		NODE_TYPE_SUBPROCESS: "SubProcessNode",
        NODE_TYPE_MULTIPLEINSTANCE: "MultipleInstanceNode",
		NODE_TYPE_COMPLEX_SIGNTOGETHER: "SignTogether",
		NODE_TYPE_COMPLEX_SIGHFORWARD: "SignForward",
		ELEMENT_TYPE_NODE: "NODE",
        ELEMENT_TYPE_CONNECTION: "CONNECTION",
        ACTION_TYPE_EVENT: "Event",
        ACTION_METHOD_TYPE_LOCALSERVICE: "LocalService",
        ACTION_METHOD_TYPE_CSHARPLIBRARY: "CSharpLibrary",
        ACTION_METHOD_TYPE_WEBAPI: "WebApi",
        ACTION_METHOD_TYPE_SQL: "SQL",
        ACTION_METHOD_TYPE_STOREPROCEDURE: "StoreProcedure",
        ACTION_METHOD_TYPE_PYTHON: "Python",
        ACTION_FIRE_BEFORE: "Before",
        ACTION_FIRE_AFTER: "After",
        BOUNDARY_EVENT_TIMER: "Timer",
        BOUNDARY_EVENT_METHOD: "Method",
    };

    kmodel.ActivityType = {
        StartNode: "StartNode",
        EndNode: "EndNode",
        TaskNode: "TaskNode",
        ServiceNode: "ServiceNode",
        GatewayNode: "GatewayNode",
        SubProcessNode: "SubProcessNode",
        MultipleInstanceNode: "MultipleInstanceNode",
        IntermediateNode: "IntermediateNode",
    }

    kmodel.GraphData = function (graphData) {
        this.package = graphData.package;

        //Swimlanes
        function createSwimlanes(swimlanes){
            if (swimlanes && swimlanes.length > 0){
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try{
                    for (var i = 0; i < swimlanes.length; i++) {
                        mxtoolkit.insertSwimlane(graph, swimlanes[i]);
				    }
                }finally{
                    model.endUpdate();
                }
            }
        }
        createSwimlanes(this.package.swimlanes);

        //Groups
        function createGroups(groups) {
            if (groups && groups.length > 0) {
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try {
                    for (var i = 0; i < groups.length; i++) {
                        mxtoolkit.insertGroup(graph, groups[i]);
                    }
                } finally {
                    model.endUpdate();
                }
            }
        }
        createGroups(this.package.groups);

        function createProcesses(processes) {
            for (var i = 0; i < processes.length; i++) {
                var process = processes[i];
                createNodes(process.activities);
                createLines(process.transitions);
            }
        }
        createProcesses(this.package.processes);

        //Activities
        function createNodes(activities){
			if (activities && activities.length > 0) {
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try{
				    for (var i = 0; i < activities.length; i++) {
                        mxtoolkit.insertVertex(graph, activities[i]);
				    }
                }finally{
                    model.endUpdate();
                }
			}
        }

        //Transitions
        function createLines(transitions){
			if (transitions && transitions.length > 0) {
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try{
				    for (var i = 0; i < transitions.length; i++) {
                        mxtoolkit.insertEdge(graph, transitions[i]);
				    }
                }finally{
                    model.endUpdate();
                }
			}
        }

        //Messages
        function createMessages(messages) {
            if (messages && messages.length > 0) {
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try {
                    for (var i = 0; i < messages.length; i++) {
                        mxtoolkit.insertMessage(graph, messages[i]);
                    }
                } finally {
                    model.endUpdate();
                }
            }
        }
        createMessages(this.package.messages);
    }
    return kmodel;
})()


