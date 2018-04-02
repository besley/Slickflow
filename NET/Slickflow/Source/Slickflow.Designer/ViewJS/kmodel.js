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
		NODE_TYPE_TASK: "TaskNode",
		NODE_TYPE_END: "EndNode",
		NODE_TYPE_GATEWAY: "GatewayNode",
		NODE_TYPE_SUBPROCESS: "SubProcessNode",
		NODE_TYPE_MULTIPLEINSTANCE: "MultipleInstanceNode",
		NODE_TYPE_COMPLEX_SIGNTOGETHER: "SignTogether",
		NODE_TYPE_COMPLEX_SIGHFORWARD: "SignForward",
		ELEMENT_TYPE_NODE: "NODE",
		ELEMENT_TYPE_CONNECTION: "CONNECTION"
	};

    kmodel.GraphData = function(graphData){
        this.package = graphData.package;
        this.process = graphData.package.process;

        //Swimlanes
        function createSwimlanes(swimlanes){
            var yongdao = null,
                yongdaos = [];
            if (swimlanes && swimlanes.length > 0){
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try{
				    for (var i = 0; i < swimlanes.length; i++) {
                        yongdao = mxtoolkit.insertSwimlane(graph, swimlanes[i]);
					    yongdaos.push(yongdao);
				    }
                }finally{
                    model.endUpdate();
                }
            }
            return yongdaos;
        }
        this.yongdaos = createSwimlanes(this.process.swimlanes);

        //Activities
        function createNodes(activities){
            var node = null,
                nodes = [];

			if (activities && activities.length > 0) {
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try{
				    for (var i = 0; i < activities.length; i++) {
                        node = mxtoolkit.insertVertex(graph, activities[i]);
					    nodes.push(node);
				    }
                }finally{
                    model.endUpdate();
                }
			}
			return nodes;
        }
        this.nodes = createNodes(this.process.activities);

        //Transitions
        function createLines(transitions){
            var line = null, 
                lines = [];

			if (transitions && transitions.length > 0) {
                var graph = kmain.mxGraphEditor.graph;
                var model = graph.getModel();
                model.beginUpdate();
                try{
				    for (var i = 0; i < transitions.length; i++) {
                        mxtoolkit.insertEdge(graph, transitions[i]);
					    lines.push(line);
				    }
                }finally{
                    model.endUpdate();
                }
			}
			return lines;
        }
        this.lines = createLines(this.process.transitions);
    }
    return kmodel;
})()


