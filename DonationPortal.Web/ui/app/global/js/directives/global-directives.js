/**
* 3000 Miles to a Cure: Global Directives
* 
* Any global specific directives should be defined here..
* 
* A directive is something that introduces new syntax. Directives are markers 
* on a DOM element which attach a special behavior to it. For example, static HTML does not 
* know how to create and display a date picker widget. 
*
* More : http://www.sitepoint.com/practical-guide-angularjs-directives/
* Good tutorial: http://ng-learn.org/2014/01/Dom-Manipulations/
*/
define(['angular'], function (angular) {

"use strict";

var globalDirectives = angular.module('globalDirectives', []);

/**
* Loading indicator directive.
* Usage: <div ui-loading-indicator class="well" title="Please wait...loading data."> </div>
*/
globalDirectives.directive("uiLoadingIndicator", function() {
    window.app_log('directive', 'uiLoadingIndicator: setting up the \'ui-loading-indicator\' directive');
    return {
        restrict : "A",
        template: " ",
        link : function(scope, element, attrs) {
            scope.$on("loading-started", function(e) {
                element.css({"display" : ""});
                //the actual message is passed through the "title" attribute
                element.html("<img src='/ui/images/ajax-loader.gif' />&nbsp;Please wait...loading data.");
            });

            scope.$on("saving-started", function(e) {
                element.css({"display" : ""});
                //the actual message is passed through the "title" attribute
                element.html("<img src='/ui/images/ajax-loader.gif' />&nbsp;Please wait...saving data.");
            });

            scope.$on("loading-complete", function(e) {
                element.css({"display" : "none"});
            });
        }
    };
});

});