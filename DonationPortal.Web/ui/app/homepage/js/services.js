/**
* 3000 Miles to a Cure: Services
*/

define(['angular'], function (angular) {

"use strict";

var homepageServices = angular.module('homepageServices', ['ngResource']);

/**
* Donation Service: submit form data to the server-side controller
*/	
homepageServices.factory('DonationService', ['$http', function($http) {

	//web service call
	var doRequest = function(data) {
		var url = '/api/donation';
		window.app_log('service', 'DonationService: making a call to \'' + url + '\'');
		var promise = $http({
		        method  : 'POST',
		        url     : url,
		        data    : data,  // pass in data as strings
		        headers : { 'Content-Type': 'application/json' }  // set the headers so angular passing info as form data (not request payload)
		    })
	        .success(function(data) {
	        	if (data && data.Status)
	        	{
	        		//successful submission
	        		if (data.Status == APP_WS_SUCCESS)
	        		{
	        			window.app_log('service', 'DonationService: successfully submitted data.');
	        		} else
	        		{
	        			window.app_log('service', 'DonationService: unable to submit data. Refer to server side log.');
	        		}
	        	} else
	        	{
	        		window.app_log('service', 'DonationService: unable to submit data. Refer to server side log.');
	        	}

	        });
	    return promise;
	};

	return {
		submitData: function(data) { return doRequest(data); },
	};

}]);	  

});