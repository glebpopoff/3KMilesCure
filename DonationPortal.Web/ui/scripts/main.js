// Safe Console Polyfill
window.log = function () {
    'use strict';
    log.history = log.history || [];
    log.history.push(arguments);
    if (window.console) {
        console.log(Array.prototype.slice.call(arguments));
    }
};
require(['client.app'], function(App){
    'use strict';
    App.init();
});



