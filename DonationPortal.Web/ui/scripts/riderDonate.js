define(
    ['jquery', 'underscore', 'snapToRoute', 'async!http://maps.google.com/maps/api/js?sensor=false', 'jquery.serializeObject'],
    function ($, _, SnapToRoute, googleMaps, jquerySerializeObject) {

        // todo, determine these based on the event page we're showing
        var eventSlug = 'farmington';
        var riderSlug = 'kevin';

        function initialize() {

            // retrieve basic information about the event rider (map default location and zoom, and marker default location)
            $.getJSON('/api/v1/events/' + eventSlug + '/riders/' + riderSlug + '/').done(function (rider) {

                var mapOptions = {
                    center: { lat: rider.MapLatitude, lng: rider.MapLongitude },
                    zoom: rider.MapZoom,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                var map = new google.maps.Map(document.getElementById('map-canvas'),
                    mapOptions);

                var marker = new google.maps.Marker({
                    position: { lat: rider.MarkerLatitude, lng: rider.MarkerLongitude },
                    map: map,
                    draggable: true
                });

                google.maps.event.addListener(marker, 'dragend', function (e) {

                    document.getElementsByName('latitude')[0].value = e.latLng.lat();
                    document.getElementsByName('longitude')[0].value = e.latLng.lng();
                });

                // when the map loads, it fires projection_changed.
                // we need to wait until then for the snap to line functionality.
                google.maps.event.addListenerOnce(map, "projection_changed", function () {

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

                            // when the user clicks a line, move the donation marker there.
                            google.maps.event.addListener(lines[i], 'click', function (e) {

                                marker.setPosition(e.latLng);

                                document.getElementsByName('latitude')[0].value = e.latLng.lat();
                                document.getElementsByName('longitude')[0].value = e.latLng.lng();
                            });

                        }

                        // the map MUST have triggered the projection_changed event before starting to snap to route.
                        var oSnap = new SnapToRoute(map);

                        oSnap.init(map, marker, lines);
                    });
                });
            });
        }



        return {
            setup: function () {
                google.maps.event.addDomListener(window, 'load', initialize);

                $(function () {
                    $('form').submit(function (e) {

                        e.preventDefault();

                        var submission = $(this).serializeObject();

                        if (submission.presetDonation === 'other') {
                            submission.donationAmount = submission.otherDonation;
                        } else {
                            submission.donationAmount = submission.presetDonation;
                        }

                        delete submission.otherDonation;
                        delete submission.presentDonation;

                        $.post('/api/v1/events/' + eventSlug + '/riders/' + riderSlug + '/donations', submission).done(function (response) {

                            alert('thanks for your donation!');

                            window.location.reload();

                        });
                    });
                });
            }
        };
    });