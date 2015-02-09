define(["moment"], function (moment) {

	return function RiderTimer(rider) {

		var days = $(".elapsed .days .counter");
		var hours = $(".elapsed .hours .counter");
		var minutes = $(".elapsed .minutes .counter");

		var start = moment(rider.Start);
		var end = moment(rider.End);
		var now = moment();

		var duration = moment.duration(end.diff(start));

		// special case.  we don't show days in this situation.  just 24 hrs
		var exactly24Hours = duration.days() === 1 && duration.hours() === 0 && duration.minutes() === 0;

		var updateDisplay = function() {

			// after the event.
			if (now.isAfter(end)) {

				if (exactly24Hours) {
					hours.text('24');
					minutes.text('00');

					return;
				}

				if (duration.days() > 0) {
					days.text(duration.days());
				}

				hours.text(duration.hours());
				minutes.text(duration.minutes());

				return;
			}

			// event hasn't started yet
			if (start.isBefore(now)) {

			}

		};



		// start up an update every minute to change the counters
	};

});