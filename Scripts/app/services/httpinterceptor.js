(function (angular) {

    angular
        .module('DemoApp')
        .factory('myHttpInterceptor', interceptor);

    interceptor.$inject = ['$timeout', '$q', '$injector', '$rootScope'];

     function interceptor ($timeout, $q, $injector, $rootScope) {
            var loginModal, $http, $state;

            // this trick must be done so that we don't receive
            // `Uncaught Error: [$injector:cdep] Circular dependency found`
            $timeout(function () {
                loginModal = $injector.get('loginService');
                $http = $injector.get('$http');
                $state = $injector.get('$state');
            });

            return {
                responseError: function (rejection) {
                    if (rejection.status !== 401) {
                        return $q.reject(rejection);
                    }

                    // holds unauthorized requests while the login form is open, allows them to reissue when closed.
                    function retryWait(config) {

                        if ($rootScope.loggingIn) {
                            $timeout(function () {
                                return retryWait(config);
                            }, 100);
                        } else {
                            return $q.reject(rejection);
                        }
                    }


                    var deferred = $q.defer();
                    $rootScope.user = undefined;

                    if ($rootScope.loggingIn !== true) {
                        // this is the first unauthorized on this call, show login and wait for it to resolve;
                        $rootScope.loggingIn = true;
                        loginModal()
                            .then(function () {
                                $rootScope.rejectedRequests = undefined;
                                deferred.resolve( $http(rejection.config) );
                            })
                            .catch(function () {
                                $rootScope.rejectedRequests = undefined;
                                $state.go('welcome');
                                deferred.reject(rejection);
                            });

                    } else {
                        // this isn't the first unauthorized on this call, wait for login and try again.
                        retryWait(rejection.config).then(function (config) {
                            deferred.resolve($http(rejection.config));
                        }, function (response) {
                          console.log('rejecting initial rety');
                          deferred.reject(response);
                        });
                    }

                    return deferred.promise;

                }
            };
        }

})(angular);
