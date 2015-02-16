define(["moment", "number"], function (moment, number) {

	/**
	 * Renders the UI with the given duration.
	 */
	var render = function (duration, exactly24Hours) {

		var days = $(".elapsed .days .counter");
		var hours = $(".elapsed .hours .counter");
		var minutes = $(".elapsed .minutes .counter");

		// special case.  show exactly 24 hrs, not 1 day.
		if (exactly24Hours) {
			hours.text('24');
			minutes.text('00');

			return;
		}

		if (duration.days() > 0) {
			days.text(number.pad(duration.days(), 2));
		}

		hours.text(number.pad(duration.hours()));
		minutes.text(number.pad(duration.minutes()));
	}
	
	/**
	 * Handles updates to the "time elapsed" rider event duration UI component.
	 */
	return function riderTimer(rider) {

		var riderStart = moment(rider.Start);
		var riderEnd = moment(rider.End);
		var riderDuration = moment.duration(riderEnd.diff(riderStart));

		// special case.  we don't show days in this situation.  just 24 hrs
		var exactly24Hours = riderDuration.days() === 1 && riderDuration.hours() === 0 && riderDuration.minutes() === 0;

		var updateDisplay = function () {

			// grab the current time
			var now = moment();

			// if the event is over, show the full event duration.  no need to refresh the time in the future
			if (now.isAfter(riderEnd)) {

				// otherwise, update the UI normally with the full event duration.
				render(riderDuration, exactly24Hours);

				// we have no further need to refresh the UI.  the event is over.
				return;
			}

			// if the event is currently underway, update the UI with the time since the event started.
			if (now.isAfter(riderStart)) {

				var elapsed = moment.duration(now.diff(riderStart));

				render(elapsed, false);
			}

			// calculate how long to wait until we refresh again.
			var timeUntilUpdate;

			// if the event hasn't started yet, wait until it starts.
			if (now.isBefore(riderStart)) {
				timeUntilUpdate = now.diff(riderStart);
			} else {
				// otherwise, wait until the next minute marker.
				timeUntilUpdate = 60000 - now.seconds() * 1000;
			}

			setTimeout(updateDisplay, timeUntilUpdate);
		};

		// kick off the UI update at least once.
		updateDisplay();
	};

});