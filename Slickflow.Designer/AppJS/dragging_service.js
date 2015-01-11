//Summary
//The flowChart design material is from http://www.codeproject.com/Articles/709340/Implementing-a-Flowchart-with-SVG-and-AngularJS
//License
//This article, along with any associated source code and files, is licensed under The MIT License
//2014.12.31

var draggingApp = angular.module('draggingApp', ['mouseCaptureApp']);

draggingApp.service('draggingService', function($rootScope, mouseCaptureService){
	var threshold = 5;

	return {
	    startDrag: function (evt, config) {
	        var dragging = false;
	        var x = evt.pageX;
	        var y = evt.pageY;

	        var mouseMove = function (evt) {
	            if (!dragging) {
	                if (evt.pageX - x > threshold ||
                        evt.pageY - y > threshold) {
	                    dragging = true;

	                    if (config.dragStarted) {
	                        config.dragStarted(x, y, evt);
	                    }

	                    if (config.dragging) {
	                        config.dragging(evt.pageX, evt.pageY, evt);
	                    }
	                }
	            } else {
	                if (config.dragging) {
	                    config.dragging(evt.pageX, evt.pageY, evt);
	                }

	                x = evt.pageX;
	                y = evt.pageY;
	            }
	        };

	        var released = function () {
	            if (dragging) {
	                if (config.dragEnded) {
	                    config.dragEnded();
	                }
	            }
	            else {
	                if (config.clicked) {
	                    config.clicked();
	                }
	            }
	        };

	        var mouseUp = function (evt) {
	            mouseCaptureService.release();

	            evt.stopPropagation();
	            evt.preventDefault();
	        };

	        mouseCaptureService.acquire(evt, {
	            mouseMove: mouseMove,
	            mouseUp: mouseUp,
	            released: released
	        });

	        evt.stopPropagation();
	        evt.preventDefault();
	    } //end start drag
	}; //end return
});
