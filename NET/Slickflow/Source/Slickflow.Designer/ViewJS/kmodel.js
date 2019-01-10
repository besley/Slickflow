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


