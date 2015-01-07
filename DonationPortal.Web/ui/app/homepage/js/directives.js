/**
* 3000 Miles to a Cure: Directives
*
* More : http://www.sitepoint.com/practical-guide-angularjs-directives/
* Good tutorial: http://ng-learn.org/2014/01/Dom-Manipulations/
*/
define(['angular'], function (angular) {

"use strict";

var globalDirectives = angular.module('globalDirectives', []);

/**
* Google Maps Route Distance Directive
* Usage: <div google-maps-route-distance class="well"> </div>
*/
globalDirectives.directive("googleMapsRouteDistance", function() {
    window.app_log('directive', 'googleMapsRouteDistance: setting up the \'google-maps-route-distance\' directive');
    return {
        restrict : "A",
        template: " ",
        link : function(scope, element, attrs) {
        	var directionsDisplay;
			var directionsService = new google.maps.DirectionsService();
			var map;
            element.html("HELLO");
        }
    };
});


    
});