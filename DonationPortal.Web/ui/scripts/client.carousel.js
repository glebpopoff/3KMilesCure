define(function (require) {

	"use strict";

	var $ = require("jquery");

	require("jquery");
	require("carousel");
	require("imagesLoaded");
	require("transit");
	require("touchSwipe");

	(function() {

		function initCarousel() {
			log('init carousel');

			var $carousel = $('.carousel'); // generic carousel class
			var $carouselAutoRotate = $('.carousel.auto-rotate'); // auto rotation carousel class
			var $carouselResponsive = $('.carousel.responsive'); // responsive carousel class

			// Configuration options for the global/generic carousel - http://docs.dev7studios.com/jquery-plugins/caroufredsel-advanced
			// An element with with class "carousel" will get the following options
			var carouselOptions = {
				width: '100%',
				height: 'variable',
				responsive: false,
				circular: true,
				infinite: false,
				align: "center",
				items: {
					width: 'variable',
					height: 'variable',
					visible: {
						min: 1,
						max: 1
					}
				},
				auto: {
					play: false,
					timeoutDuration: 8000
				},
				scroll: {
					duration: 400,
					pauseOnHover: 'resume',
					fx: 'slide',
					easing: 'easeInOutExpo'
					//onBefore: function (data) {},
					//onAfter: function (data) {}
				},
				prev: {
					button: function () {
						return $(this).parent().siblings('.prev');
					},
					key: "left"
				},
				next: {
					button: function () {
						return $(this).parent().siblings('.next');
					},
					key: "right"
				},
				pagination: {
					container: function () {
						return $(this).parent().siblings('.carousel-nav');
					},
					keys: true
				},
				swipe: {
					onTouch: true
				}
			};

			// Configuration options for auto rotating carousel
			// Carousels with the class "auto-rotate" will get the following options along with the generic carousel options
			var autoRotate = {
				auto: {
					play: true,
					timeoutDuration: 8000
				}
			};

			// Configuration options for responsive carousel
			// Carousels with the class "responsive" will get the following options along with the generic carousel
			var responsive = {
				responsive: true,
				items: {
					width: '980',
					height: 'variable',
					visible: {
						min: 1,
						max: 1
					}
				}
			};

			// Building the carousels
			if ($carousel.length > 0) {

				// Once all images are loaded...
				$carousel.imagesLoaded( function() {

					// Build generic carousel
					$carousel.each(function(){
						$(this).carouFredSel(carouselOptions, {wrapper: {classname: "carousel-wrapper"},transition: true}).removeClass('no-js');
					});

					// Build generic carousel with auto rotation options
					$carouselAutoRotate.each(function(){
						$(this).carouFredSel($.extend(carouselOptions, autoRotate), {wrapper: {classname: "carousel-wrapper"},transition: true}).removeClass('no-js');
					});

					// Build generic carousel with responsive options
					$carouselResponsive.each(function(){
						$(this).carouFredSel($.extend(carouselOptions, responsive), {wrapper: {classname: "carousel-wrapper"},transition: true}).removeClass('no-js');
					});

				});

			}

		}

		initCarousel();

	}());

});
