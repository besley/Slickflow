//Summary
//The flowChart design material is from http://www.codeproject.com/Articles/709340/Implementing-a-Flowchart-with-SVG-and-AngularJS
//License
//This article, along with any associated source code and files, is licensed under The MIT License
//2014.12.31

var app = angular.module('kGraphApp', ['draggingApp']);

app.directive('kGraph', function () {
	return {
		restrict: 'E',
		templateUrl: 'AppJS/template.html',
		replace: true,
		scope: {
			graph: '=graph'
		},
		controller: 'kGraphCtrl'
	}
});

app.controller('kGraphCtrl', ['$scope', 'draggingService', '$element',
    function kGraphCtrl($scope, draggingService, $element) {
        var controller = this;
        this.document = document;

        $scope.draggingLine = false;
        $scope.connectorSize = 10;
        $scope.dragSelecting = false;

        $scope.dragSelectionRect = {
            x: 0, y: 0, width:0, height: 0
        };

        $scope.mouseOverConnector = null;
        $scope.mouseOverLine = null;
        $scope.mouseOverNode = null;

        $scope.trueCoords = null;
        $scope.currentSelectedLine = null;
        $scope.currentSelectedNode = null;


        this.lineClass = "line";
        this.connectorClass = "connector";
        this.nodeClass = "node";

        this.jQuery = function (element) {
            return $(element);
        }

        this.searchUp = function (element, parentClass) {
            if (element == null || element.length == 0) {
                return null;
            }

            if (hasClassSVG(element, parentClass)) {
                return element;
            }

            return this.searchUp(element.parent(), parentClass);
        }

        this.hitTest = function (clientX, clientY) {
            return this.document.elementFromPoint(clientX, clientY);
        }

        this.checkForHit = function (mouseOverElement, whichClass) {
            var hoverElement = this.searchUp(this.jQuery(mouseOverElement), whichClass);
            if (!hoverElement) {
                return null;
            }
            return hoverElement.scope();
        }

        this.translateCoordinates = function (x, y) {
            var svg_elem = $element.get(0);
            var matrix = svg_elem.getScreenCTM();
            var point = svg_elem.createSVGPoint();

            point.x = x;
            point.y = y;

            return point.matrixTransform(matrix.inverse());
        }

        //#region activity definition page
        $scope.nodeClick = function (evt, node) {
            $("#divActivityDialog").children().remove();
            $scope.currentSelectedNode = node;
            var pageTitle = "";
            var pageUrl = "";

            if (node.type() == "TaskNode") {
                pageTitle = "活动定义数据";
                pageUrl = "/sfd/views/activityproperty.html";
            } else if (node.type() == "GatewayNode") {
                pageTitle = "Gateway定义数据";
                pageUrl = "/sfd/views/gatewayproperty.html";
            }

            var dialogOptions = {
                title: pageTitle,
                width: 530,
                height: 450,
                modal: true,
                autoOpen: false,
                beforeClose: function (evt, ui) {
                    ;
                },
                close: function (event, ui) {
                    $(this).children().remove();
                    $(this).dialog("destroy");
                }
            };

            var activityDialog = $("#divActivityDialog")
                .load(pageUrl,
                    function () {
                        activityDialog
                            .data("node", node)
                            .dialog(dialogOptions)
                            .dialog('open');
                    }
            );
        }
        //#endregion

        //#region transition definition page
        $scope.lineClick = function (evt, line) {
            $scope.currentSelectedLine = line;
            var transitionDialog = $("#divTransitionDialog")
                .load("/sfd/views/transitionproperty.html",
                    function () {
                        var dialogOptions = {
                            title: "转移定义数据",
                            width: 500,
                            height: 300,
                            modal: true,
                            autoOpen: false,
                            beforeClose: function (evt, ui) {
                                ;
                            },
                            close: function (event, ui) {
                                //$(this).dialog("destroy");
                            }
                        };

                        transitionDialog
                            .data("line", line)
                            .dialog(dialogOptions)
                            .dialog('open');
                    }
            );
        }
        //#endregion

        //#region Graph UI event
        $scope.mouseDown = function (evt) {
            if ($scope.graph == undefined)
                return;

            $scope.graph.deselectAll();

            draggingService.startDrag(evt, {
                dragStarted: function (x, y) {
                    $scope.dragSelecting = true;
                    var startPoint = controller.translateCoordinates(x, y);
                    $scope.dragSelectionStartPoint = startPoint;
                    $scope.dragSelectionRect = {
                        x: startPoint.x,
                        y: startPoint.y,
                        width: 0,
                        height: 0
                    }
                },
                dragging: function (x, y) {
                    var startPoint = $scope.dragSelectionStartPoint;
                    var curPoint = controller.translateCoordinates(x, y);
                    $scope.dragSelectionRect = {
                        x: curPoint.x > startPoint.x ? startPoint.x : curPoint.x,
                        y: curPoint.y > startPoint.y ? startPoint.y : curPoint.y,
                        width: curPoint.x > startPoint.x ? curPoint.x - startPoint.x : startPoint.x - curPoint.x,
                        height: curPoint.y > startPoint.y ? curPoint.y - startPoint.y : startPoint.y - curPoint.y
                    };
                },
                dragEnd: function () {
                    $scope.dragSelecting = false;
                    $scope.graph.applySelectionRect($scope.dragSelectionRect);
                    delete $scope.dragSelectionStartPoint;
                    delete $scope.dragSelectionRect
                }
            })
        }   //end mousedown

        $scope.mouseMove = function (evt) {
            $scope.mouseOverLine = null;
            $scope.mouseOverConnector = null;
            $scope.mouseOverNode = null;

            var mouseOverElement = controller.hitTest(evt.clientX, evt.clientY);
            if (mouseOverElement == null) {
                return;
            }

            if (!$scope.draggingLine) {
                var scope = controller.checkForHit(mouseOverElement, controller.lineClass);
                $scope.mouseOverLine = (scope && scope.line) ? scope.line : null;
                if ($scope.mouseOverLine) {
                    return;
                }
            }

            var scope = controller.checkForHit(mouseOverElement, controller.connectorClass);
            $scope.mouseOverConnector = (scope && scope.connector) ? scope.connector : null;

            if ($scope.mouseOverConnector) {
                return;
            }

            var scope = controller.checkForHit(mouseOverElement, controller.nodeClass);
            $scope.mouseOverNode = (scope && scope.node) ? scope.node : null;
        }   //end mouse move


        $scope.mouseEnter = function (evt) {
            var relatedTarget = evt.relatedTarget;

            if (relatedTarget) {
                var element = $(relatedTarget).attr("element");

                if (element && element != "") {
                    var point = controller.translateCoordinates(evt.clientX, evt.clientY);
                    //create new node
                    $scope.graph.addNode(element, point.x, point.y);
                }
            }
        }

        $scope.mouseUp = function (evt) {
            //window.console.log("mouse up in the graph view...");
        }

        $scope.nodeMouseDown = function (evt, node) {
            var graph = $scope.graph;
            var lastMouseCoords;

            draggingService.startDrag(evt, {
                dragStarted: function (x, y) {
                    lastMouseCoords = controller.translateCoordinates(x, y);

                    if (!node.selected()) {
                        graph.deselectAll();
                        node.select();
                    }
                },
                dragging: function (x, y) {
                    var curCoords = controller.translateCoordinates(x, y);
                    var deltaX = curCoords.x - lastMouseCoords.x;
                    var deltaY = curCoords.y - lastMouseCoords.y;

                    //window.console.log("move deltaX: " + deltaX + " move deltaY: " + deltaY);
                    graph.updateSelectedNodesLocation(deltaX, deltaY);
                    lastMouseCoords = curCoords;
                },
                clicked: function () {
                    graph.handleNodeClicked(node, evt.ctrlKey);
                }
            });
        };   //  end node mouse down

        $scope.lineMouseDown = function (evt, line) {
            var graph = $scope.graph;
            graph.handleLineMouseDown(line, evt.ctrlKey);

            evt.stopPropagation();
            evt.preventDefault();
        };

        $scope.connectorMouseDown = function (evt, node, connector, connectorIndex, isInputConnector) {
            draggingService.startDrag(evt, {
                dragStarted: function (x, y) {
                    var curCoords = controller.translateCoordinates(x, y);
                    $scope.draggingLine = true;
                    $scope.dragPoint1 = kgraph.computeConnectorPos(node, connectorIndex, isInputConnector);
                    $scope.dragPoint2 = {
                        x: curCoords.x,
                        y: curCoords.y
                    };
                    $scope.dragTangent1 = kgraph.computeLineSourceTangent($scope.dragPoint1, $scope.dragPoint2);
                    $scope.dragTangent2 = kgraph.computeLineDestTangent($scope.dragPoint1, $scope.dragPoint2);
                },
                dragging: function (x, y, evt) {
                    var startCoords = controller.translateCoordinates(x, y);
                    $scope.dragPoint1 = kgraph.computeConnectorPos(node, connectorIndex, isInputConnector);
                    $scope.dragPoint2 = {
                        x: startCoords.x,
                        y: startCoords.y
                    };
                    $scope.dragTangent1 = kgraph.computeLineSourceTangent($scope.dragPoint1, $scope.dragPoint2);
                    $scope.dragTangent2 = kgraph.computeLineDestTangent($scope.dragPoint1, $scope.dragPoint2);
                },
                dragEnded: function () {
                    if ($scope.mouseOverConnector &&
                        $scope.mouseOverConnector !== connector) {
                        $scope.graph.createNewLine(connector, $scope.mouseOverConnector);
                    }
                    $scope.draggingLine = false;
                    delete $scope.dragPoint1;
                    delete $scope.dragPoint2;
                    delete $scope.dragTangent1;
                    delete $scope.dragTangent2;
                }
            });
        }   //end connector mouse down
        //#endregion
}]);

