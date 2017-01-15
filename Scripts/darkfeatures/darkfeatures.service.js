
(function (angular) {
    'use strict';

    angular.module('DarkFeaturesService', ['AxcessHTTPService'])
        .factory('darkfeatures', darkfeatures);

    darkfeatures.$inject = ['$http', '$q', 'axcesshttp'];

    function darkfeatures($http, $q, axcesshttp) {

        var stuckUrl;

        return {
            getFirmFlag: getFirmFlag,
            getUserFlag: getUserFlag
        };

        function getFirmFlag(flag, defaultValue) {
            var defValue = defaultValue || false;

            var deferred = $q.defer();

            axcesshttp.get('api/firmfeature?flag=' + flag + '&defaultvalue=' + defValue).then(function(response) {
                deferred.resolve(response.data);
            }, function (response) {
                deferred.resolve(defValue);
            });

            return deferred.promise;
        }

        function getUserFlag(flag, defaultValue) {
            var defValue = defaultValue || false;

            var deferred = $q.defer();

            axcesshttp.get('api/userfeature?flag=' + flag + '&defaultvalue=' + defValue).then(function(response) {
                deferred.resolve(response.data);
            }, function (response) {
                deferred.resolve(defValue);
            });

            return deferred.promise;
        }
    }

})(angular);
