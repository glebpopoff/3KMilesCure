define(function (require) {

	"use strict";

	var $ = require("jquery");
	require("jquery");
	require("jqueryUI");
	require("bootstrap");
	//require("selectBoxIt");
	require("placeholder");
	require("shadowBox");

	(function() {

		// jQuery SelectBoxIt - http://gregfranko.com/jquery.selectBoxIt.js/
		//function initSelectBoxIt(){
		//	if($('select').length > 0) {
		//		$("select").selectBoxIt({
		//			aggressiveChange: true,
		//			theme: 'default',
		//			autoWidth: false
		//		});
		//	}
		//}

		// jQuery placeholder - https://github.com/mathiasbynens/jquery-placeholder
		function initPlaceholder(){
			if($('body').is('.ie9, .lt-ie9')) {
				$('input, textarea').placeholder();
			}
		}

		// setupUITabs http://api.jqueryui.com/tabs/
		function initUITabs() {
			var $tabNav = $('.tab-container');
			if ($tabNav.length > 0) {
				$tabNav.tabs({
					create: function(){
						$(this).removeClass('no-js');
					}
				});
			}
		}

		// setupUIAccordion http://api.jqueryui.com/accordion/
		function initUIAccordion() {
			var $accordion = $('.accordion-container');
			if ($accordion.length > 0) {
				$accordion.each(function(){
					$(this).accordion({
						collapsible: true,
						animate: 'easeInOutExpo',
						icons: {
							"header": "icon-accordion-open",
							"activeHeader": "icon-accordion-close"
						},
						heightStyle: "content",
						create: function(){
							$(this).removeClass('no-js');
							//add hash targeting
							if(location.hash){
								var hash = window.location.hash;
								var targetHash = hash.substring(hash.lastIndexOf('#'), hash.length);
								var targetItem = $('a[href*='+ targetHash + ']');
								var offset = $(targetItem).offset().top;

								$accordion.find(targetItem).closest('h2').trigger('click');
								$('html, body').animate({scrollTop:offset}, 500);
							}
						}
					});
				});
			}
		}

		//setupSmoothScroll
		function initSmoothScroll() {
			var anchorLink = $('.anchor-link');
			if (anchorLink.length > 0) {
				anchorLink.on('click', function(event){
					event.preventDefault();
					var offset = $($(this).attr('href')).offset().top;
					$('html, body').animate({scrollTop:offset}, 500);
				});
			}
		}

		//Shadowbox - http://www.shadowbox-js.com/
		function initShadowbox() {
			var $target = $('a[rel*="shadowbox"]');
			var $video = $('a[rel*="shadowbox"].video');
			if ($target.length > 0) {
				window.Shadowbox.init();
			}
			if ($video.length > 0) {
				$video.attr('rel','shadowbox;width=720;height=480;');
			}
		}

		function initFormSteps() {

			var $doanteOptions = $(".donate-options input[type='radio']");
			var $otherAmount = $(".other-radio");
			$doanteOptions.on("click", function(){
				if ($otherAmount.is(':checked')){
					$(".other-input").show();
				} else if (!($otherAmount.is(':checked'))) {
					$(".other-input").hide().find('input').val("");
				}
			});

			//var $step1button = $('#step-1-button');
			//var $step2button = $('#step-2-button');
			//var $step3button = $('#step-3-button');
			//$doanteOptions.on('click', function(){
			//	if($doanteOptions.is(':checked')){
			//		$step1button.prop('disabled', false);
			//	} else {
			//		$step1button.prop('disabled', true);
			//	}
			//});
			//
			//var $message = $('#write-message');
			//$message.on('focus', function(){
			//	if($message.val()){
			//		$step2button.prop('disabled', false);
			//	} else {
			//		$step2button.prop('disabled', true);
			//	}
			//});
			//
			//var $requiredField = $('.required-field');
			//$requiredField.on('focus', function(){
			//	if($requiredField.val()){
			//		$step3button.prop('disabled', false);
			//	} else {
			//		$step3button.prop('disabled', true);
			//	}
			//});


		}

		function initToolTip(){
			var $toolTip = $('[data-toggle="tooltip"]');
			if($toolTip.length > 0) {
				$toolTip.tooltip();
			}
		}

		//init functions
		//initSelectBoxIt();
		initPlaceholder();
		initUIAccordion();
		initUITabs();
		initSmoothScroll();
		initShadowbox();
		initFormSteps();
		initToolTip();

	}());


});
