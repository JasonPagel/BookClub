(function () {
    'use strict';

    angular
        .module('DemoApp')
        .config(globalConfig);

    globalConfig.$inject = ['$httpProvider'];

    function globalConfig($httpProvider) {

        // disable browser caching.
        $httpProvider.defaults.cache = false;

        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }

        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 03 Jan 2015 19:43:31 GMT';

        //intercept 401's and redirect to loginService
       $httpProvider.interceptors.push('myHttpInterceptor');

    }

    angular
        .module('DemoApp')
        .run(appSetup);

        appSetup.$inject = ['Analytics', '$rootScope', '$state', 'loginService'];

        function appSetup(Analytics, $rootScope, $state, loginService) {

            var sessionId = Analytics.GetSessionId();
            Analytics.setUser('DemoApp User');
            Analytics.setApp('DemoApp');
            Analytics.startTracking(sessionId);

            $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
                var requireLogin = toState.data.requireLogin;

                if (requireLogin && typeof $rootScope.user === 'undefined') {
                    event.preventDefault();
                    
                    loginService().then(function () {
                        return $state.go(toState.name, toParams);
                    })
                    .catch(function () {
                        return $state.go('welcome');
                    });
                }
            });
        }
})();