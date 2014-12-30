/**
* 3K Miles to a Cure App: RequireJS configuration
*/

//set to true to enable console logging
var APP_DEBUG = true;

//console.log wrapper
window.log=function(){ if (!APP_DEBUG) return; log.history=log.history||[];log.history.push(arguments);if(this.console){console.log(Array.prototype.slice.call(arguments))}};

// hey Angular, we're bootstrapping manually!
window.name = "NG_DEFER_BOOTSTRAP!";

(function (require) {

    'use strict';

    requirejs.config({
        urlArgs: "t=" +  (new Date()).getTime(),
        paths:{
            "angular":  [
                "//ajax.googleapis.com/ajax/libs/angularjs/1.2.13/angular.min",
                "/ui/vendor/angular/angular"],
                "angular-bootstrap-ui" : "/ui/vendor/angular-bootstrap/ui-bootstrap-tpls",
                "angular-scroll" :  "/ui/vendor/angular-scroll/angular-scroll.min",
                "angular-resource" :  "/ui/vendor/angular-resource/angular-resource.min"
            , jquery :          "/ui/vendor/jquery-legacy/jquery.min"
           , bootstrap:       "/ui/vendor/bootstrap/dist/js/bootstrap.min"
        }

        , shim:{
            angular:{
                deps: ['jquery'],
                exports:'angular' 
                },
            "angular-bootstrap-ui" :    ['angular'],
            "angular-scroll" :          ['angular'],
            "angular-resource" :          ['angular'],
            bootstrap:                  ['jquery']
        }
    });

    require(["jquery", "angular", "app", "bootstrap"], function(jq, angular, app, bootstrap) {
        angular.element(document).ready(function() {
            angular.bootstrap(document, ['3000milesCure']);
        });
    });

}(require));