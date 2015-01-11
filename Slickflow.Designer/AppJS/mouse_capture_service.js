//Summary
//The flowChart design material is from http://www.codeproject.com/Articles/709340/Implementing-a-Flowchart-with-SVG-and-AngularJS
//License
//This article, along with any associated source code and files, is licensed under The MIT License
//2014.12.31

var mouseCaptureApp = angular.module('mouseCaptureApp', []);

mouseCaptureApp.service('mouseCaptureService', function ($rootScope){
	var $element = document;
	var mouseCaptureConfig = null;

	var mouseMove = function (evt){
		if (mouseCaptureConfig && mouseCaptureConfig.mouseMove){
			mouseCaptureConfig.mouseMove(evt);
			$rootScope.$digest();
		}
	}

	var mouseUp = function (evt){
		if (mouseCaptureConfig && mouseCaptureConfig.mouseUp){
			mouseCaptureConfig.mouseUp(evt);
			$rootScope.$digest();
		}
	}

	return{
		registerElement: function(element){
			$element = element;
		},

		acquire: function(evt, config){
			this.release();
			mouseCaptureConfig = config;
			$element.mousemove(mouseMove);
			$element.mouseup(mouseUp);
		},

		release: function(){
			if (mouseCaptureConfig){
				if (mouseCaptureConfig.released){
					mouseCaptureConfig.released();
				}
				mouseCaptureConfig = null;
			}
			$element.unbind("mousemove", mouseMove);
			$element.unbind("mouseup", mouseUp);
		}
	}
});

mouseCaptureApp.directive('mouseCapture', function(){
	return{
		restrict: 'A',
		controller: function($scope, $element, $attrs, mouseCaptureService){
			mouseCaptureService.registerElement($element);
		}
	};
});

