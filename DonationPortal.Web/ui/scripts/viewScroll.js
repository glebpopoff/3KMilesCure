define(['jquery'], function($) {

	return {
		scrollIntoView: function(element, duration) {
			
			if (duration === undefined) {
				duration = 500;
			}

			var offset = $(element).offset().top;
			$('html, body').animate({ scrollTop: offset }, duration);

		}
	};

});