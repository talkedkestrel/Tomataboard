(function () {
    "use strict";

    var app = angular.module("dashboard-app");

    app.directive("quote", function () {
        return {
            restrict: "E",
            replace: "true",

            templateUrl: '/app/dashboard/quote/quote.html',

            link: function(scope, element, attrs) {
                $(element).popover({
                    html: "true",
                    placement: "top",
                    trigger: "hover",
                    content: function () {
                        return $("#quote-popover").html();
                    }
                });
            }
        };
    });
})();