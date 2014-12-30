/**
* 3K Miles to a Cure App: controllers
*/

define(['angular'], function (angular) {

"use strict";

var homepageControllers = angular.module('homepageControllers', []);

homepageControllers.controller('HomepageController', ['$scope', 'StaffListService',
	function($scope, StaffListService)
	{
		window.log('homepage controller init..');
		$scope.isLoading = true;
		$scope.staff_container = StaffListService.query();
		window.log('Retrieved homepage staff..');
		window.log($scope.staff_container);
	}
]);

});