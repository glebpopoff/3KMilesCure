// Safe Console Polyfill
window.log = function () {
    'use strict';
    log.history = log.history || [];
    log.history.push(arguments);
    if (window.console) {
        console.log(Array.prototype.slice.call(arguments));
    }
};
require.config({
    //To get timely, correct error triggers in IE, force a define/shim exports check.
    //enforceDefine: true,

    paths: {
        jquery: [
            'http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min',
            //If the CDN location fails, load from this location
            'vendor/jquery/jquery-1.9.1.min'
        ],
        //define js libraries
        jqueryUI:       'vendor/jqueryUI/jquery-ui-1.10.4.custom.min', // JqueryUI script library
        bootstrap:      'vendor/bootstrap/bootstrap-3.2.0.min', // Bootstrap framework library
        carousel:      	'vendor/carouFredSel/jquery.carouFredSel-6.2.1-packed', // Plugin to develop custom interactive carousels
        imagesLoaded:   'vendor/imagesLoaded/imagesloaded.pkgd.min', // Plugin to detect when images have been loaded
        transit:      	'vendor/carouFredSel/helper-plugins/jquery.transit.min', // Plugin to utilize css3 transitions with jquery fallback
        touchSwipe:     'vendor/carouFredSel/helper-plugins/jquery.touchSwipe.min', // A jquery based plugin for touch based device support
		circiful:     	'vendor/circiful/jquery.circliful.min', // A jquery based plugin for circle charts
		isotope:   		'vendor/isotope/isotope.pkgd.min', // A jquery based plugin masonry positioning elements
        bridget:    	'vendor/jquery-bridget/jquery.bridget', // Plugin for isotope compatibility
        placeholder:    'vendor/jquery.placeholder/jquery.placeholder.min', // Adds placeholder support for non-modern browsers
        shadowBox:      'vendor/shadowbox/shadowbox.min' // Adds overlay menu functionality
    },
    //library dependencies
    shim: {
        'jqueryUI':     ['jquery'],
        'bootstrap':    ['jquery'],
        'imagesLoaded': ['jquery'],
        'carousel':    	['jquery'],
        'transit':    	['jquery'],
        'touchSwipe':   ['jquery'],
        'circiful':   	['jquery'],
        'isotope':   	['bridget','jquery'],
        'bridget':  	['jquery'],
        'placeholder':	['jquery'],
        'shadowBox':    ['jquery']
    }

});
require(['client.app'], function(App){
    'use strict';
    App.init();
});



