(function (angular) {

    angular
        .module('DemoApp')
        .service('loginService', loginService);

        loginService.$inject = ['$rootScope', '$modal', 'notificationService', 'userService'];

        function loginService ($rootScope, $modal, notificationService, userService) {

            function assignCurrentUser (user) {
                userService.user.name = user.name;
                userService.user.loggedIn = true;
                userService.user.id = user.id;
                $rootScope.user = userService.user;
                $rootScope.$broadcast('savestate');
                return userService.user;
            }

            return function() {
                var instance = $modal.open({
                    templateUrl: 'views/login.html'
                });

                return instance.result.then(assignCurrentUser);
            };

        }
})(angular);
