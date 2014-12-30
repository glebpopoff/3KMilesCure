/**
* 3K Miles to a Cure App: services
*/

define(['angular'], function (angular) {
    "use strict";

var homepageServices = angular.module('homepageServices', ['ngResource']);
	
    /*homepageServices.factory('StaffListService', function() {
        return {
            getAllUsers: function(callback) {
                $.get('/app-staff/api/1.0/list').success(function(data) {
                    if (data && data.length > 0)
                    {
                        window.log('StaffList: retrieved \'' + data.length + '\' records');
                        return callback(data);
                    } else
                    {
                        window.log('StaffList: unable to retrieve records')
                    }
            });
            }
        };
    });*/

    homepageServices.factory('StaffListService', ['$resource',
  function($resource){
    return $resource('/app-staff/api/1.0/list', {}, {
      query: {method:'GET', params:{}, isArray:true}
    });
  }]);

});