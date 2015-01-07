/**
* 3000 Miles to a Cure: Global Library
*/

//set to true for each flag to enable console logging
var APP_DEBUG= {'general':true, 'directive':true, 'controller':true, 'service':true};

//make sure this is blank in production 
var SERVICE_DOMAIN = '';//http://terraform.resultsbuilder.com';

//global app identifier
var APP_ID = '3kMilesCure';

//global WS success response code 
var APP_WS_SUCCESS = 'ok';

//local storage identifier
var LOCAL_STORAGE_ID = '3kmilescure-app-storage';

//global controller name
var APP_CONTROLLER_ID = '3kMilesCureController';

//console.log wrapper
window.log = function(){ if (!APP_DEBUG['general']) return; log.history=log.history||[];log.history.push(arguments);if(this.console){ console.log(Array.prototype.slice.call(arguments))}};

//console.log wrapper 2.0 or poor man's log4JS 
window.app_log = function(flag) { if (APP_DEBUG[flag]) { window.log(arguments[1]); }  };

//outputs some information about the app, caching, timestamp of initialization
window.app_init = function(appId) { APP_ID = appId; window.log(APP_ID + ' instantiated', ((APP_CACHE_SID == APP_TIMESTAMP) ? 'caching disabled' : 'caching enabled'),'' + new Date()); };
 