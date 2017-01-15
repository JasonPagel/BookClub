(function (angular) {

    angular
        .module('DemoApp')
        .service('signupService', signupService);

        signupService.$inject = ['$rootScope', '$modal', 'notificationService'];

        function signupService($rootScope, $modal, notificationService) {

            function signupUser() {
                notificationService.success('Signup complete');
            }

            return function() {

                var instance = $modal.open({
                    templateUrl: 'views/signup.html'
                });

                return instance.result.then(signupUser);

            };

        }

})(angular);