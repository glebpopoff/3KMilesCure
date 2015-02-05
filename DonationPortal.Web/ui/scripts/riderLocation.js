define(['async!http://maps.google.com/maps/api/js?sensor=false', 'underscore', 'jquery'], function (googleMaps, _, $) {

	var eventSlug = null;
	var riderSlug = null;

	function initialize() {
		// retrieve basic information about the event rider (map default location and zoom, and marker default location)
		$.getJSON('/api/v1/events/' + eventSlug + '/riders/' + riderSlug + '/').done(function (rider) {

			var mapOptions = {
				center: { lat: rider.MapLatitude, lng: rider.MapLongitude },
				zoom: rider.MapZoom,
				mapTypeId: google.maps.MapTypeId.ROADMAP,
				scrollwheel: false
			};

			var map = new google.maps.Map(document.getElementById('map-canvas'),
				mapOptions);

			var marker = new google.maps.Marker({
				position: { lat: rider.MarkerLatitude, lng: rider.MarkerLongitude },
				map: map
			});

			// retrieve the routes associated with this rider
			$.getJSON('/api/v1/events/' + eventSlug + '/riders/' + riderSlug + '/routes').done(function (routes) {

				var lines = [];

				// a rider may be riding on multiple routes throughout a race
				for (var i = 0; i < routes.length; i++) {

					var route = routes[i];

					// create a line for this route and draw it on the map
					var line = new google.maps.Polyline({
						path: _.map(route.Vertices, function (vertex) { return new google.maps.LatLng(vertex.Latitude, vertex.Longitude); }),
						geodisc: true,
						strokeColor: route.Color,
						strokeOpacity: 1.0,
						strokeWeight: 3
					});

					line.setMap(map);

					lines.push(line);
				}
			});

			// update the marker location with the rider's last known position.
			$.getJSON('/api/v1/events/' + eventSlug + '/riders/' + riderSlug + '/location').done(function (location) {

				marker.setPosition({lat: location.Latitude, lng: location.Longitude});

			});
		});
	}

	return {
		setup: function (event, rider) {

			eventSlug = event;
			riderSlug = rider;

			$(function() {
				initialize();
			});

		}
	};

});