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
       //$httpProvider.interceptors.push('myHttpInterceptor');

    }

    angular
        .module('DemoApp')
        .run(appSetup);

        appSetup.$inject = ['Analytics', '$rootScope', '$state', 'loginService', 'userService'];

        function appSetup(Analytics, $rootScope, $state, loginService, userService) {

            $rootScope.$broadcast('restorestate');

            var sessionId = Analytics.GetSessionId();
            Analytics.setUser('DemoApp User');
            Analytics.setApp('DemoApp');
            Analytics.startTracking(sessionId);

            $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
                var requireLogin = toState.data.requireLogin;

                if (requireLogin && (userService.user === undefined || userService.user.loggedIn === false)) {
                    event.preventDefault();

                    loginService().then(function () {
                        return $state.go(toState.name, toParams);
                    })
                    .catch(function () {
                        return $state.go('welcome');
                    });
                }

                //if (sessionStorage.restoreState == "true") {
                  $rootScope.$broadcast('restorestate'); // let everything know to restore state
                  //sessionStorage.restorestate = false;
                //}

            });

            //window.onbeforeunload = function (event) {
            //  alert('unloading');
            //  console.log('before unload');
            //    $rootScope.$broadcast('savestate');
            //};
        }
})();
