define(function (require) {
	"use strict";

	var $ = require("jquery");
	require("jquery");
	require("isotope");

	(function() {
	    function initSocialStream(){
			if ($('#social-stream').length > 0) {
				var $container = $('#social-stream .stream');
				// initialize Isotope
				$container.isotope({
					itemSelector: '.social-block',
					masonry: {
						columnWidth: '.grid-sizer'
					}
				});

				// layout Isotope again after all images have loaded
				$container.imagesLoaded( function() {
					$container.isotope('layout');
					log('loaded');
				});

				// filtering Isotope
				$('.social-nav li a').click(function(e){
					var selector = $(this).attr('data-filter');
					var $filter = $('.social-nav li');
					$container.isotope({ filter: selector });
					$(this).parent().addClass('active');
					if($(this).parent('li').is('active')){
						// filter is active
						log('is active');
					} else {
						$filter.removeClass('active');
						$(this).parent('li').addClass('active');
						//var $getFilter = $(this).text();
					}
					e.preventDefault();
				});

			}
		}

		initSocialStream();

	}());


});


