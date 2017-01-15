(function (angular) {
    'use strict';

    angular.module('DarkFeaturesService').directive('checkFeatureFirm', checkFeatureFirm);

    checkFeatureFirm.$inject = ['darkfeatures'];

    function checkFeatureFirm(darkfeatures) {
        return {
            restrict: 'A',
            link: function checkFeature(scope, element, attrs) {
                element.hide(); //hide element by default

                darkfeatures.getFirmFlag(attrs.checkFeatureFirm).then(function (value) {
                    if (value) { 
                        element.show() 
                    }
                });
            }
        };
    }
})(angular);