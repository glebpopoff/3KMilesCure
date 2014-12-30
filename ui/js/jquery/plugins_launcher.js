define(["jquery","jqueryui","selectboxit","jqcheckable", "jqFitHeaders"], function ($) {

    // these file contains an immediately executable function,
    // instead of returning an init function that needs to be run.
    // merely including it in require will execute its contents

    return (function ($) {

        // run link_buttons
        $(document).on('click', 'button[data-href]', function(e){
            e.preventDefault();

            window.location.href = $(this).attr('data-href');
        });

        var checkboxes = $('input[type="checkbox"]');
        for (var i = checkboxes.length - 1; i >= 0; i--) {
            $(checkboxes[i]).prettyCheckable();
        }

        //initialize selects
        $('select').selectBoxIt({
            autoWidth:false
        });

        $("#KJE-PAY_PERIOD")
            .bind({
                "change": function(ev, obj) {
                    KJE.calculateButtonAction();
                }
            });

        // handle home register button
        var $register = $(".register-button")
            , _homeDelayIncrement = 150
            , _homeDelay = 700;
        $register.hide().each(function(index){
            _homeDelay += _homeDelayIncrement;
            $(this).delay(_homeDelay).queue(function(){
                $(this).fadeIn(500).dequeue();
            });

        });

        //cloak
        var $deferredContainers = $(".container[ng-cloak], .container[data-ng-cloak]").not(".title")
            , _delayIncrement = 350
            , _delay = 1500;
        $deferredContainers.hide().each(function(index){
            $(this).delay(_delay).queue(function(){
                $(this).fadeIn(500).dequeue();
            });
            _delay += _delayIncrement;
        });


        // make CTA links open in new windows
        $(".takeaway-container a").attr("target","_blank");


        // takeaway cta click tracking
        $(".takeaway-container .cta").on('click','a', function(){
            var label = $(this).closest("a").text();
            var missionNameCTA = $(".title-bar h1").html().replace(/:\s+/, ':').replace(/\s+/g, '-').toLowerCase();
            missionNameCTA = missionNameCTA.substring(missionNameCTA.indexOf(":") + 1) + "-[" + label.replace(/\s+/g, '-') + "]";
            ga('primacy.send', 'event', 'final-mission-cta', 'click', missionNameCTA);
            ga('tiacref.send', 'event', 'final-mission-cta', 'click', missionNameCTA);
        });

        function isAddThis(classStr) {
            return classStr.indexOf("addthis") === 0;
        }

        if (!Array.prototype.filter) {
            Array.prototype.filter = function(fun/*, thisArg*/) {
                "use strict";

                if (this === void 0 || this === null) {
                    throw new TypeError();
                }

                var t = Object(this);
                var len = t.length >>> 0;
                if (typeof fun !== "function") {
                    throw new TypeError();
                }

                var res = [];
                var thisArg = arguments.length >= 2 ? arguments[1] : void 0;
                for (var i = 0; i < len; i++) {
                    if (i in t) {
                        var val = t[i];

                        // NOTE: Technically this should Object.defineProperty at
                        //       the next index, as push can be affected by
                        //       properties on Object.prototype and Array.prototype.
                        //       But that method's new, and collisions should be
                        //       rare, so use the more-compatible alternative.
                        if (fun.call(thisArg, val, i, t)) {
                            res.push(val);
                        }
                    }
                }

                return res;
            };
        }

        // click tracking for social buttons
        $(".addthis_toolbox").on('click','a', function(){
            var label = $(this).attr("class").split(" ").filter(isAddThis)[0];
            ga('primacy.send', 'social', label.substr(label.lastIndexOf("_") + 1), 'share', label);
            ga('tiaacref.send', 'social', label.substr(label.lastIndexOf("_") + 1), 'share', label);
        });

        //make closed onload info-reveal sections
        $(".info-reveal").addClass("closed");


        function makeHomePageBlueBoxesEqualHeight() {
            // make login page blue boxes of equal height since the 3rd one is user-controlled

            $('<div/>', {'class': 'scratch'}).appendTo('body');
            $('.container.content.home').clone().appendTo('.scratch');

            var maxHeight = 0,
                thisHeight = 0,
                $headImgContainerOrig = $(".head-image-container"),
                $headImgContainer = $(".scratch .head-image-container");

            $headImgContainer.each(function(){
                thisHeight = $(this).height();
                maxHeight = (thisHeight > maxHeight) ?  thisHeight : maxHeight;
            });
            $headImgContainer.css('min-height', maxHeight);
            $headImgContainerOrig.css('min-height', maxHeight);
            $('.scratch').remove();
        }

        // home, make headers fit into a single line
        var lengthOfWait = 2000;

        if ($(".home.content").hasClass("about")) {
            lengthOfWait = 2200;
        } else {
            lengthOfWait = 1500;
        }
        setTimeout(function(){
            $('.prize-container h1 > .fit-text-max').responsiveHeadlines({maxFontSize : 40});
            $('.prize-container h1 > .fit-text').responsiveHeadlines({maxFontSize : 90});
            makeHomePageBlueBoxesEqualHeight();
        }, lengthOfWait);

    })($);
});





































