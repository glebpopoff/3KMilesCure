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
homepageControllers.controller('DonationCtrl', ['$scope', 'DonationService',
	function($scope, DonationService) {
		window.log('DonationCtrl: init..');

		//init page model
		$scope.donationModel = {}; 
		
		//triggered on the 'Donation Now' button click
		$scope.proceedToDonate = function()
		{ 
			window.app_log('controller', 'DonationCtrl: submitting data..');

			DonationService.submitData($scope.donationModel).then(function(response) {
				if (response && response.status == 200 && response.data.Payload &&
					response.data && response.data.Status == 'Success'  && response.data.Payload.token
					)
		    	{
		    		//set quick status for the next controller
					SharedDataService.setData('Successfully submitted data.');

					//redirect back to the list page
					$location.path('/donation/thank-you');
				 	return;

		    	} else
		    	{
		    		$scope.error_message = 'Unable to update the record. Please contact support.';

		    	}
		    });
		}
	}
]);

});