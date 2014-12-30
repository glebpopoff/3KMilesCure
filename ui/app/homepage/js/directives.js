/**
* 3K Miles to a Cure App: directives
*/

define([
    "jquery",
    'angular',
    "jqlauncher",
    "angular-scroll",
    'angular-bootstrap-ui'
], function ($, angular, jqlauncher) {
    "use strict";

	myApp.directive('ngIf', function() {
    return {
        link: function(scope, element, attrs) {
            if(scope.$eval(attrs.ngIf)) {
                // remove '<div ng-if...></div>'
                element.replaceWith(element.children())
            } else {
                element.replaceWith(' ')
            }
        }
    }
});
	
});

