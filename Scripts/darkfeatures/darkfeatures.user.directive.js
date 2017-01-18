(function (angular) {
    'use strict';

    angular.module('DarkFeaturesService').directive('checkFeatureUser', checkFeatureUser);

    checkFeatureUser.$inject = ['darkfeatures'];

    function checkFeatureUser(darkfeatures) {
        return {
            restrict: 'A',
            link: function checkFeature(scope, element, attrs) {
                element.hide(); //hide element by default

                darkfeatures.getUserFlag(attrs.checkFeatureFirm).then(function (value) {
                    if (value) {
                        element.show(); 
                    }
                });
            }
        };
    }
})(angular);
