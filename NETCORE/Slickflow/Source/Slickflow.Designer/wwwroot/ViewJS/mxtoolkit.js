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

    //#region insert element
    mxtoolkit.insertSwimlane = function(graph, swimlane){
        var doc = mxUtils.createXmlDocument();
        var swimlaneElement = doc.createElement('Swimlane');
        swimlaneElement.setAttribute("id", swimlane.id);
        swimlaneElement.setAttribute("label", swimlane.title);
        swimlaneElement.setAttribute("type", swimlane.type);

        var process = swimlane.process;
        if (process) {
            var processElement = doc.createElement('Process');
            swimlaneElement.appendChild(processElement);

            processElement.setAttribute("package", process.package);
            processElement.setAttribute("id", process.id);
            processElement.setAttribute("name", process.name);
            processElement.setAttribute("code", process.code);
            processElement.setAttribute("description", process.description);
        }

        var model = graph.getModel();
        var parent = graph.getDefaultParent();
        var geography = swimlane.geography;
        var widget = geography.widget;
        var swimlaneVertex = graph.insertVertex(parent, swimlane.id, swimlaneElement, 
            widget.left, widget.top, widget.width, widget.height,
            geography.style);

        return swimlaneVertex;
    }

    mxtoolkit.insertGroup = function (graph, group) {
        var doc = mxUtils.createXmlDocument();
        var groupElement = doc.createElement("Group");
        groupElement.setAttribute("label", group.name);

        var model = graph.getModel();
        var parent = graph.getDefaultParent();
        var geography = group.geography;
        var widget = geography.widget;
        var groupVertex = graph.insertVertex(parent, group.id, groupElement,
            widget.left, widget.top, widget.width, widget.height,
            geography.style);

        return groupVertex;
    }

    mxtoolkit.insertMessage = function (graph, message) {
        var doc = mxUtils.createXmlDocument();
        var messageElement = mxfile.setMessageElement(doc, message);

        //var geography
        var geography = message.geography;
        var model = graph.getModel();

        var parent = null;
        if (geography) parent = model.getCell(geography.parent);
        if (!parent) parent = graph.getDefaultParent();

        var edge = graph.insertEdge(parent, message.id, messageElement,
            graph.getModel().getCell(message.from), graph.getModel().getCell(message.to), geography.style);

        //points
        if (geography) {
            if (geography.points) {
                edge.geometry.points = geography.points;
            }
        }
        return edge;
    }

    mxtoolkit.insertSubSwimlane = function (graph, parent) {
        var lane = graph.insertVertex(parent, null, 'Lane A', 0, 0, 640, 110);
        lane.setConnectable(false);
    }

	mxtoolkit.insertVertex = function(graph, activity){
        var geography = {},
            widget = {};
        var parent = null;


        if (activity.geography) {
            geography = activity.geography;
            widget = geography.widget;

            var model = graph.getModel();
            parent = model.getCell(geography.parent);
        } else {
            parent = graph.getDefaultParent();
            widget.left = 100;
            widget.top = 100;
            widget.width = 72;
            widget.height = 32;
        }

        //geography
        if (geography === null || geography.style === null || geography.style === "null" || geography.style === undefined || geography.style === "undefined") {
            geography.style = mxconfig.getVertexStyle(activity);
        }

        var vertex = graph.insertVertex(parent, activity.id, createXmlElement(activity), 
            widget.left, widget.top, widget.width, widget.height, 
            geography.style);
        return vertex;
    }

    function createXmlElement(activity) {
        var doc = mxUtils.createXmlDocument();
        var activityElement = doc.createElement('Activity');

        activityElement.setAttribute("id", activity.id);
        activityElement.setAttribute('label', activity.name);
        activityElement.setAttribute('code', activity.code);
        activityElement.setAttribute('url', activity.url);

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
        if (activity.performers && activity.performers.length > 0) {
            var performersElement = mxfile.setPerformersElement(doc, activity.performers);
            activityElement.appendChild(performersElement);
        }

        //actions
        if (activity.actions && activity.actions.length > 0) {
            var actionsElement = mxfile.setActionsElement(doc, activity.actions);
            activityElement.appendChild(actionsElement);
        }

        //services
        if (activity.services && activity.services.length > 0) {
            var servicesElement = mxfile.setServicesElement(doc, activity.services);
            activityElement.appendChild(servicesElement);
        }
        
        //boudaries
        if (activity.boundaries && activity.boundaries.length > 0) {
            var boundariesElement = mxfile.setBoundariesElement(doc, activity.boundaries);
            activityElement.appendChild(boundariesElement);
        }

        //sections
        if (activity.sections && activity.sections.length > 0) {
            var sectionsElement = mxfile.setSectionsElement(doc, activity.sections);
            activityElement.appendChild(sectionsElement);
        }
        return activityElement;
    }

    mxtoolkit.insertEdge = function(graph, transition){       
        var doc = mxUtils.createXmlDocument();
        var transitionElement = mxfile.setTransitionElement(doc, transition);
        
        //condition
        if ($.isEmptyObject(transition.condition) === false) {
            var conditionElement = mxfile.setConditionElement(doc, transition.condition);
            transitionElement.appendChild(conditionElement);                  
        }

        //group behaviours
        if ($.isEmptyObject(transition.groupBehaviours) === false) {
            var groupBehavioursElement = mxfile.setGroupBehavioursElement(doc, transition.groupBehaviours);
            transitionElement.appendChild(groupBehavioursElement);
        }
        
        //receiver
        if ($.isEmptyObject(transition.receiver) === false){
            var receiverElement = mxfile.setReceiverElement(doc, transition.receiver);
            transitionElement.appendChild(receiverElement);    
        }

        //geography
        var geography = transition.geography;
        var model = graph.getModel();
        var parent = null;
        if (geography) parent = model.getCell(geography.parent);
        if (!parent) parent = graph.getDefaultParent();

        var edge = graph.insertEdge(parent, transition.id, transitionElement,
            graph.getModel().getCell(transition.from), graph.getModel().getCell(transition.to), geography.style);

        //points
        if (geography) {
            if (geography.points) {
                edge.geometry.points = geography.points;
            }
        }
        return edge;
    }
    //#endregion

    //#region binding mouse event
    mxtoolkit.bindingMouseEvent = function (graph) {
        graph.addMouseListener(
            {
                currentState: null,
                currentIconSet: null,
                mouseDown: function (sender, me) {
                    // Hides icons on mouse down
                    if (this.currentState != null) {
                        this.dragLeave(me.getEvent(), this.currentState);
                        this.currentState = null;
                    }
                },
                mouseMove: function (sender, me) {
                    if (this.currentState !== null && (me.getState() === this.currentState ||
                        me.getState() === null)) {
                        var iconTolerance = 20;
                        var tmp = new mxRectangle(me.getGraphX() - iconTolerance,
                            me.getGraphY() - iconTolerance, 2 * iconTolerance, 2 * iconTolerance);

                        if (mxUtils.intersects(tmp, this.currentState)) {
                            return;
                        }
                    }

                    var tmp = graph.view.getState(me.getCell());

                    // Ignores everything but vertices
                    if (graph.isMouseDown || (tmp != null && !graph.getModel().isVertex(tmp.cell))) {
                        tmp = null;
                    }

                    if (tmp != this.currentState) {
                        if (this.currentState != null) {
                            this.dragLeave(me.getEvent(), this.currentState);
                        }

                        this.currentState = tmp;

                        if (this.currentState != null) {
                            this.dragEnter(me.getEvent(), this.currentState);
                        }
                    }
                },
                mouseUp: function (sender, me) { },
                dragEnter: function (evt, state) {
                    if (this.currentIconSet === null) {
                        var currentCell = state.cell;
                        if (currentCell.value.nodeName === "Swimlane") {
                            this.currentIconSet = new mxIconSet(state);
                        }
                    }
                },
                dragLeave: function (evt, state) {
                    if (this.currentIconSet != null) {
                        this.currentIconSet.destroy();
                        this.currentIconSet = null;
                    }
                }
            });
    };
    //#endregion

    //#region common node method
    mxtoolkit.getActivityType = function (snode) {
        var activityTypeNode = snode.getElementsByTagName("ActivityType")[0];
        var activityType = activityTypeNode.getAttribute("type");
        return activityType;
    }
    //#endregion

    //#region mxIconSet class
    // Defines a new class for all icons
    function mxIconSet(state) {
        this.images = [];
        var graph = state.view.graph;
        var currentCell = state.cell;

        // Icon1
        var img = mxUtils.createImage('scripts/mxgraph/src/images/insert.gif');
        img.setAttribute('title', 'Insert');
        img.style.position = 'absolute';
        img.style.cursor = 'pointer';
        img.style.width = '16px';
        img.style.height = '16px';
        img.style.left = (state.x + state.width) + 'px';
        img.style.top = (state.y - 16) + 'px';

        mxEvent.addGestureListeners(img,
            mxUtils.bind(this, function (evt) {
                var s = graph.gridSize;
                mxtoolkit.insertSubSwimlane(graph, currentCell);
                mxEvent.consume(evt);
                this.destroy();
            })
        );

        state.view.graph.container.appendChild(img);
        this.images.push(img);
    };

    mxIconSet.prototype.destroy = function () {
        if (this.images != null) {
            for (var i = 0; i < this.images.length; i++) {
                var img = this.images[i];
                img.parentNode.removeChild(img);
            }
        }

        this.images = null;
    };
    //#endregion

	return mxtoolkit;
})()