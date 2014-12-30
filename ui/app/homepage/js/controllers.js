/**
* 3000 Miles to a Cure: Controllers
*/

define(['angular'], function (angular) {

"use strict";

var homepageControllers = angular.module('homepageControllers', []);

/**
* Homepage Controller
*/
homepageControllers.controller('HomepageCtrl', ['$scope', 
	function($scope) {
		window.log('HomepageCtrl: init..');
		
	}
]);

/**
* About Controller
*/
homepageControllers.controller('AboutCtrl', ['$scope', 
	function($scope) {
		window.log('AboutCtrl: init..');
		
	}
]);

/**
* Events Controller
*/
homepageControllers.controller('EventsCtrl', ['$scope', 
	function($scope) {
		window.log('EventsCtrl: init..');
		
	}
]);

/**
* Donation Controller
*/
homepageControllers.controller('DonationCtrl', ['$scope', 
	function($scope) {
		window.log('DonationCtrl: init..');
		
	}
]);

});