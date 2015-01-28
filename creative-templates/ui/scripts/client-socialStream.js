define(function (require) {
	"use strict";

	var $ = require("jquery");
	require("jquery");
	require("bridget");
	var Isotope = require("isotope");

	(function() {
	    function initSocialStream(){
			if ($('#social-stream').length > 0) {

				$.bridget( 'isotope', Isotope );
				var $container = $('.social-dashboard > .container');
				var $socialStream = $('#social-stream .stream');

				// initialize Isotope after all images have loaded
				$socialStream.imagesLoaded( function() {
					$socialStream.isotope({
						itemSelector: '.social-block',
						masonry: {
							gutter: 20
						}
					});
					$('#social-stream').removeClass('loading');
				});

				$(window).resize(function(){
					// initialize Isotope layout on resize
					$socialStream.isotope('layout');
				});

				// filtering Isotope
				$('.social-nav li a').click(function(e){
					var selector = $(this).attr('data-filter');
					var $filter = $('.social-nav li');
					$socialStream.isotope({ filter: selector });
					$(this).parent().addClass('active');
					if($(this).parent('li').is('active')){
						// filter is active
						log('is active');
					} else {
						$filter.removeClass('active');
						$(this).parent('li').addClass('active');
					}
					e.preventDefault();
				});

			}
		}

		initSocialStream();

	}());


});


