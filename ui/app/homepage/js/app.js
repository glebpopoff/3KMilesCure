/**
* 3000 Miles to a Cure: Angular Application File
*/

define([
    "jquery",
    "angular",
    "angular-resource",
    "angular-route",
    "controllers",
    "services",
    "directives",
    "google-maps-api"
], function ($, angular, jqlauncher) {

"use strict";

var homepageApp = angular.module('homepageApp', ['ngRoute', 'homepageControllers', 'homepageServices', "globalDirectives"]);

//application routing
homepageApp.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $routeProvider.
      when('/home', {
         title: '3000 Miles to a Cure | Homepage',
        templateUrl: '/ui/app/homepage/partials/homepage.html?' + APP_CACHE_SID,
        controller: 'HomepageCtrl'
      }).
      when('/about', {
        title: '3000 Miles to a Cure | About',
        templateUrl: '/ui/app/homepage/partials/about.html?' + APP_CACHE_SID,
        controller: 'AboutCtrl'
      }).
      when('/events', {
        title: '3000 Miles to a Cure | Events',
        templateUrl: '/ui/app/homepage/partials/events.html?' + APP_CACHE_SID,
        controller: 'EventsCtrl'
      }).
      when('/events/:id', {
        title: '3000 Miles to a Cure | Events',
        templateUrl: '/ui/app/homepage/partials/events.html?' + APP_CACHE_SID,
        controller: 'EventsCtrl'
      }).
      when('/donate', {
        title: '3000 Miles to a Cure | Donate',
        templateUrl: '/ui/app/homepage/partials/donation.html?' + APP_CACHE_SID,
        controller: 'DonationCtrl'
      }).
      when('/donate/:id', {
        title: '3000 Miles to a Cure | Donate',
        templateUrl: '/ui/app/homepage/partials/donation.html?' + APP_CACHE_SID,
        controller: 'DonationCtrl'
      }).
      //this is important to avoid infinite loop redirect (other NG apps redirect back here on NG 404)
      otherwise( { redirectTo: '/home' });

      // use the HTML5 History API
      $locationProvider.html5Mode(true);
}]);

//update global template
homepageApp.run(['$location', '$rootScope', function($location, $rootScope) {
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        $rootScope.title = (current.$$route && current.$$route.title) ? current.$$route.title : '3000 Miles to a Cure';
        var v = APP_CACHE_SID;
        $rootScope.ng_app_version = v;
    });
}]);
    
});