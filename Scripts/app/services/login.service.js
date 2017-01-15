(function (angular) {

    angular
        .module('DemoApp')
        .service('loginService', loginService);
        
        loginService.$inject = ['$rootScope', '$modal', 'notificationService'];

        function loginService ($rootScope, $modal, notificationService) {

            function assignCurrentUser (user) {
                $rootScope.user = user;
                return user;
            }

            return function() {
                var instance = $modal.open({
                    templateUrl: 'views/login.html'
                });

                return instance.result.then(assignCurrentUser);
            };

        }
})(angular);