define(function (require) {
	"use strict";

	var $ = require("jquery");
	require("jquery");

	(function() {

		function initT1Navigation(){
			log('init navigation');

			var $trigger = $('.t1-nav .has-menu'); // only items with class .has-menu will trigger
			var $menu = $('.dhtml-menu-container .collapsed-menu');
			var $close = $('.dhtml-menu-container .close-menu');
			if ($trigger.length > 0) {
				$trigger.on('click', function (e) {
					var $this = $(this);
					var target = $this.find('> a').data('target');
					if($trigger.is('.animating')){
						event.stopPropagation();
					}
					else if ($this.hasClass('active')){
						$this.removeClass('active');
						$(target).slideUp(function() {
							$(target).removeClass('menu-open');
						});
					} else if ($menu.is('.menu-open:visible')) {
						var $menuOpen = $menu.closest('.menu-open');
						$trigger.removeClass('active');
						$this.addClass('animating');
						$menuOpen.slideUp({
							complete: function (){
								$menu.removeClass('menu-open');
								$(target).slideDown(function () {
									$(target).addClass('menu-open');
									$trigger.removeClass('animating');
								});
								$this.addClass('active');
							}
						});
					}
					else {
						$this.addClass('animating');
						$(target).slideDown(function() {
							$(target).addClass('menu-open');
							$trigger.removeClass('animating');
						});
						$this.addClass('active');
					}
					e.preventDefault();
				});
				$close.on('click', function (e) {
					$menu.slideUp(function() {
						$menu.removeClass('menu-open');
						$trigger.removeClass('active');
					});
					e.preventDefault();
				});
			}

		}

		initT1Navigation();

	}());


});
