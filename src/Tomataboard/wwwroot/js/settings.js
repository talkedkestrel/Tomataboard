﻿(function () {
    "use strict";

    var app = angular.module("dashboard-app");

    app.directive("settings", function () {

        return {
            restrict: "E",
            replace: "true",

            templateUrl: '/views/settings.html',

            link: function(scope, element, attrs) {

            }
        };
    });

})();