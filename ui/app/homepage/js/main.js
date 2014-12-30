/**
* 3000 Miles to a Cure: RequireJS configuration
*/

//global variables
var APP_TIMESTAMP = (new Date()).getTime();
var APP_CACHE_SID = APP_TIMESTAMP; //enabling caching by setting APP_CACHE_SID to a static value (e.g. APP_CACHE_SID = 1)

// hey Angular, we're bootstrapping manually!
window.name = "NG_DEFER_BOOTSTRAP!";

(function (require) {

    'use strict';

    requirejs.config({
        urlArgs: "t=" +  APP_CACHE_SID,
        waitSeconds: 200,
        paths:{
            "angular": ["//ajax.googleapis.com/ajax/libs/angularjs/1.3.0/angular.min", "/ui/vendor/angular/angular"],
            "angular-resource": "/ui/vendor/angular/angular-resource.min",
            "angular-route": "/ui/vendor/angular/angular-route.min",
            "global-library": "/ui/app/global/js/lib/global-library",
            "jquery": "/ui/vendor/jquery-legacy/jquery.min",
            "bootstrap": "/ui/vendor/bootstrap/dist/js/bootstrap.min"
        },
        shim:{
            angular:{
                deps: ['jquery'],
                exports:'angular' 
                },
            "angular-resource": ['angular'],
            "angular-route": ['angular'],
            "bootstrap": ['jquery']
        }
    });

    require(["global-library", "jquery", "angular", "app", "bootstrap"], function(globalLibrary, jq, angular, app, bootstrap) {
        angular.element(document).ready(function() {
            window.app_init('homepageApp')
            angular.bootstrap(document, [APP_ID]);
        });
    });

    

}(require));